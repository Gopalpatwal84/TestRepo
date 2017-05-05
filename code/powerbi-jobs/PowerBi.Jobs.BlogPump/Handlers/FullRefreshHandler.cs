using System;
using System.Linq;
using System.Threading.Tasks;
using Acom.IO.Collections;
using Acom.Redis;
using Acom.Search.Queue.Indexing;
using Mediation;
using Mediation.Handlers;
using Mediation.Webjobs.Storage;
using Onyx.Client;
using Onyx.Client.Requests.Blog.PowerBI;
using PowerBI.Entities.Blogs;
using PowerBI.Jobs.BlogPump.Features;
using PowerBI.Jobs.CommonModules;
using PowerBI.Messages.Models;
using PowerBI.Messages.Refresh;
using Serilog;

namespace PowerBI.Jobs.BlogPump.Handlers
{
    public class FullRefreshHandler : IHandleCommand<RefreshFullBlogCommand>
    {
        private readonly IWebjobStorageRepository<BlogPost> _postStorageRepository;
        private readonly IWebjobStorageRepository<BlogAuthor> _authorStorageRepository;
        private readonly IWebjobStorageRepository<BlogCategory> _topicStorageRepository;
        private readonly IWebjobStorageRepository<FeaturedBlogPost> _featuredPostStorageRepository;
        private readonly IWebjobStorageRepository<BlogContent> _contentStorageRepository;
        private readonly IRedisClient _redisConnection;
        private readonly MediatorBroadcaster _broadcaster;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IOnyxRequestClient _onyxRequestClient;

        public FullRefreshHandler(
            IWebjobStorageRepository<BlogPost> postStorageRepository,
            IWebjobStorageRepository<BlogAuthor> authorStorageRepository,
            IWebjobStorageRepository<BlogCategory> topicStorageRepository,
            IWebjobStorageRepository<FeaturedBlogPost> featuredPostStorageRepository,
            IWebjobStorageRepository<BlogContent> contentStorageRepository,
            IRedisClient redisConnection,
            MediatorBroadcaster broadcaster,
            ILogger logger,
            IMediator mediator,
            IOnyxRequestClient onyxRequestClient)
        {
            _postStorageRepository = postStorageRepository;
            _authorStorageRepository = authorStorageRepository;
            _topicStorageRepository = topicStorageRepository;
            _featuredPostStorageRepository = featuredPostStorageRepository;
            _contentStorageRepository = contentStorageRepository;
            _redisConnection = redisConnection;
            _broadcaster = broadcaster;
            _logger = logger;
            _mediator = mediator;
            _onyxRequestClient = onyxRequestClient;
        }

        public async Task Handle(RefreshFullBlogCommand command)
        {
            if (command.ReadSource == ReadSource.Remote)
            {
                Exception delayException = null;
                try
                {
                    var postsData = await _onyxRequestClient.GetAllPowerBiBlogPosts();
                    var authorsData = await _onyxRequestClient.GetAllPowerBiBlogAuthors();
                    var topicsData = await _onyxRequestClient.GetAllPowerBiBlogTopics();
                    var featuredData = await _onyxRequestClient.GetFeaturedPowerBiBlogPost();

                    _logger.Information("Uploading blog posts metadata to Storage. Count: {PowerBiBlogPostsCount}", postsData.Length);
                    await _postStorageRepository.SaveResults(postsData.Select(x => x.ToEntity()));

                    _logger.Information("Uploading blog posts authors to Storage. Count: {PowerBiBlogAuthorsCount}", authorsData.Length);
                    await _authorStorageRepository.SaveResults(authorsData.Select(x => x.ToEntity()));

                    _logger.Information("Uploading blog posts topics to Storage. Count: {PowerBiBlogTopicsCount}", topicsData.Length);
                    await _topicStorageRepository.SaveResults(topicsData.Select(x => x.ToEntity()));

                    _logger.Verbose("Uploading the featured blog post to Storage.");
                    await _featuredPostStorageRepository.SaveResults(new[]
                    {
                        new FeaturedBlogPost
                        {
                            Post = featuredData.ToEntity(),
                        }
                    });

                    _logger.Verbose("Uploading the blog content to Storage.");
                    await _contentStorageRepository.SaveResults(postsData.Select(x =>
                        new BlogContent
                        {
                            Slug = x.Slug,
                            Content = x.Content,
                        }));

                }
                catch (Exception exception)
                {
                    delayException = exception;
                }

                await _broadcaster.Send(new RefreshFullBlogCommand
                {
                    ReadSource = ReadSource.Storage,
                });

                if (delayException != null) throw delayException;
            }
            else
            {
                var priority = command.IsFromCacheRefreshApi();
                var posts = await _postStorageRepository.LoadResults();

                await _redisConnection.SetAllAsync(posts.OrderByDescending(p => p.Published));
                await _redisConnection.SetAllAsync((await _contentStorageRepository.LoadResults()).OrderBy(c => c.Slug));
                await _redisConnection.SetAllAsync((await _authorStorageRepository.LoadResults()).OrderBy(x => x.Name));
                await _redisConnection.SetAllAsync((await _topicStorageRepository.LoadResults()).OrderBy(x => x.Name));
                await _redisConnection.SetAllAsync((await _featuredPostStorageRepository.LoadResults()).OrderBy(x => x.Slug));

                _logger.Information("Hydrated redis from {ReadSource}", command.ReadSource);

                _logger.Verbose("Queuing messages to update the Azure Search index...");
                await posts
                    .OrderByDescending(x => x.Published)
                    .ForEachAsync(post => _mediator.Send(post.ToIndexItem(IndexAction.UpdateOrInsert, priority)));
            }

            _logger.Verbose("Job done!");
        }
    }
}
