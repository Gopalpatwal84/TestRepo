using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acom.IO;
using Acom.IO.Entities;
using Acom.IO.Json;
using Acom.IO.Strings;
using Acom.Redis;
using Mediation.Handlers;
using PowerBI.Entities;
using PowerBI.Jobs.SupportTopicsPump.Models;
using PowerBI.Messages.Refresh;
using Serilog;

namespace PowerBI.Jobs.SupportTopicsPump.Handlers
{
    public class RefreshSupportTopicsHandler : IHandleCommand<RefreshSupportTopicsCommand>
    {
        private readonly IRedisClient redisClient;
        private readonly ILogger logger;
        private readonly IRepository<Culture> cultureRepository;
        private readonly string apiUrl;
        private readonly string productId;

        public RefreshSupportTopicsHandler(
            IRedisClient redisClient,
            ILogger logger,
            IRepository<Culture> cultureRepository,
            Configuration configuration)
        {
            this.redisClient = redisClient;
            this.logger = logger;
            this.cultureRepository = cultureRepository;

            this.apiUrl = configuration.SupportServicesUrl;
            this.productId = configuration.ProductId;
        }

        public async Task Handle(RefreshSupportTopicsCommand command)
        {
            this.logger.Information("Retrieving default culture topics.");
            var defaultCulture = await this.GetTopicsByCulture(KnownCultures.Default);
            var problemTypes = this.MapTopics(defaultCulture);
            
            var cultures = await this.cultureRepository.GetAsync();
            foreach (var culture in cultures.Where(x => x.IsDisplayed))
            {
                if (culture.Slug != KnownCultures.Default)
                {
                    this.logger.Information("Retrieving {0} culture topics.", culture.Slug);
                    var cultureTopics = await this.GetTopicsByCulture(culture.Slug);
                    this.ParseAlternativeCultures(problemTypes, cultureTopics, culture.Slug);
                }
            }

            this.logger.Information("{0} topics fetched", problemTypes.Count);
            await this.redisClient.SetAllAsync(problemTypes.OrderBy(x => x.Id));

            this.logger.Verbose("Job done!");
        }

        private async Task<SupportTopic[]> GetTopicsByCulture(string culture)
        {
            var url = string.Format(this.apiUrl, culture, this.productId);
            var jsonRequest = new JsonGetRequest(new Uri(url));
            var jsonClient = new JsonRequestClient();

            return await jsonClient.ExecuteAsync<SupportTopic[]>(jsonRequest);
        }

        private List<ProblemType> MapTopics(IEnumerable<SupportTopic> topics)
        {
            return topics
                .Select(x => 
                new ProblemType
                {
                    Slug = string.Format("problem-type-{0}", x.Id.ToSlug()),
                    Id = x.Id,
                    Title = x.Name,
                    AlternateCultures = new Dictionary<string, string>(),
                    Categories = x.Subtopics.Select(y =>
                        new ProblemCategory
                        {
                            Slug = string.Format("problem-category-{0}", y.Id.ToSlug()),
                            Id = y.Id,
                            Title = y.Name,
                            AlternateCultures = new Dictionary<string, string>()
                        }).ToArray()
                }).ToList();
        }

        private void ParseAlternativeCultures(List<ProblemType> problemTypes, IEnumerable<SupportTopic> topics, string culture)
        {
            foreach (var problemType in problemTypes)
            {
                var topic = topics.FirstOrDefault(x => x.Id.Equals(problemType.Id));

                if (topic != null)
                {
                    problemType.AlternateCultures.Add(culture, topic.Name);

                    foreach (var category in problemType.Categories)
                    {
                        var subtopic = topic.Subtopics.FirstOrDefault(x => x.Id.Equals(category.Id));

                        if (subtopic != null)
                        {
                            category.AlternateCultures.Add(culture, subtopic.Name);
                        }
                    }
                }
            }
        }
    }
}
