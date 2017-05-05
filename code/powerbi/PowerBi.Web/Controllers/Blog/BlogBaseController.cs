using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Acom.IO;
using Acom.Mvc.Helpers;
using Acom.Search.Client;
using PowerBI.Entities.Blogs;
using PowerBI.Search;
using PowerBI.Search.Documents;
using PowerBI.Web.Models.Pages.Blog;
using PowerBI.Web.Models.Parts.Blog;
using Serilog;

namespace PowerBI.Web.Controllers.Blog
{
    public class BlogBaseController : BaseController
    {
        protected const int PageSize = 10;

        private readonly ISearchClient _searchClient;
        private readonly IRepository<BlogCategory> _blogCategoriesRepository;
        private IEnumerable<BlogCategory> _categories;

        protected IRepository<BlogPost> BlogPostsRepository { get; private set; }

        protected BlogBaseController(
            IRepository<BlogPost> blogPostsRepository,
            IRepository<BlogCategory> blogCategoriesRepository,
            ISearchClient searchClient)
        {
            BlogPostsRepository = blogPostsRepository;
            _blogCategoriesRepository = blogCategoriesRepository;
            _searchClient = searchClient;
        }

        public async Task<IEnumerable<BlogCategory>> GetCategories()
        {
            if (_categories == null)
            {
                _categories = await _blogCategoriesRepository.GetAsync();
                _categories = _categories?.OrderBy(t => t?.Name);
            }

            return _categories;
        }

        protected async Task<BlogSearchResult> ExecuteSearch(Func<Task<BlogSearchResult>> searchFunction,
                                                             bool throwException = false, [CallerMemberName] string callerName = "")
        {
            try
            {
                return await searchFunction();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "BLOG AZURE SEARCH REQUEST : {Name} -> {callerName}", this.GetType().Name, callerName);

                if (throwException)
                {
                    throw;
                }
            }

            return BlogSearchResult.ErrorResult;
        }

        protected async Task<BlogSearchResult> FromOrderedSet(int page = 1)
        {
            var results = await this.BlogPostsRepository.GetRangeAsync(PageSize * (page - 1), PageSize);

            if (results == null)
            {
                return BlogSearchResult.ErrorResult;
            }

            return new BlogSearchResult
            {
                TotalCount = results.TotalCount,
                Results = results.Data.Select(p => new BlogPostModel
                {
                    Slug = p.Slug,
                    Summary = p.Summary,
                    Author = p.Author,
                    Published = p.Published,
                    Title = p.Title,
                    Url = p.Url,
                    IsFeatured = p.Featured,
                    Categories = p.Categories.Select(c => c.Slug).ToArray(),
                    Tags = p.Tags.Select(c => c.Slug).ToArray()
                })
            };
        }

        protected async Task<BlogSearchResult> BlogSearchInRange(int pageNumber, DateTime startDate, DateTime endDate)
        {
            var searchBuilder = this.CreateSearchQuery(pageNumber);
            searchBuilder.FilterByProperty(x => x.Created, (DateTimeOffset)startDate, SearchIndexOperator.IsGreaterOrEqual);
            searchBuilder.FilterByProperty(x => x.Created, (DateTimeOffset)endDate, SearchIndexOperator.IsLessOrEqual);

            var searchResponse = await searchBuilder.SearchAsync();

            return new BlogSearchResult
            {
                Results = searchResponse.Results.Select(r => this.CreateModel(r.Document)).ToArray(),
                TotalCount = Convert.ToInt32(searchResponse.Count)
            };
        }

        protected async Task<BlogSearchResult> BlogSearchWithFilterByTermInCollection(int pageNumber, Expression<Func<BlogEntry, IEnumerable<string>>> collection, string term)
        {
            var searchBuilder = this.CreateSearchQuery(pageNumber);
            searchBuilder.AnyTermInCollection(collection, term);

            var searchResponse = await searchBuilder.SearchAsync();

            return new BlogSearchResult
            {
                Results = searchResponse.Results.Select(r => this.CreateModel(r.Document)).ToArray(),
                TotalCount = Convert.ToInt32(searchResponse.Count)
            };
        }

        protected async Task<BlogSearchResult> BlogSearchWithFilterByProperty(int pageNumber, Expression<Func<BlogEntry, object>> propertySelector, string value)
        {
            var searchBuilder = this.CreateSearchQuery(pageNumber);
            searchBuilder.FilterByProperty(propertySelector, value);

            var searchResponse = await searchBuilder.SearchAsync();

            if (searchResponse == null)
                return BlogSearchResult.ErrorResult;

            return new BlogSearchResult
            {
                Results = searchResponse.Results.Select(r => this.CreateModel(r.Document)).ToArray(),
                TotalCount = searchResponse.Count
            };
        }

        protected PowerBiQueryBuilder<BlogEntry> CreateSearchQuery(int pageNumber)
        {
            var searcher = _searchClient.CreateQuery<BlogEntry>()
                .IncludeTotalResultCount()
                .OrderByPropertyDescending(x => x.Created);

            if (pageNumber > 0)
            {
                searcher
                    .Skip(PageSize * (pageNumber - 1))
                    .Top(PageSize);
            }
            else
            {
                searcher.Top(PageSize);
            }

            return searcher;
        }

        protected BlogPostModel CreateModel(BlogEntry entry)
        {
            return new BlogPostModel
            {
                Slug = entry.Slug,
                Url = entry.Url,
                IsFeatured = entry.Featured,
                Published = entry.Created.DateTime.ToPacificTime(),
                Author = new BlogAuthor
                {
                    Name = entry.PublisherName,
                    Position = entry.PublisherPosition,
                    ProfileImage = entry.PublisherProfileImage,
                    Slug = entry.PublisherSlug,
                },
                Summary = entry.Summary,
                Tags = entry.Tags.ToArray(),
                Title = entry.Title,
                Categories = entry.Categories.ToArray()
            };
        }
    }
}