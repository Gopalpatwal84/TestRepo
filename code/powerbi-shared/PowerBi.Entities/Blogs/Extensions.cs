using System.Linq;
using Onyx.Contracts.Blog;

namespace PowerBI.Entities.Blogs
{
    public static class Extensions
    {
        public static BlogPost ToEntity(this SitePostContract postContract)
        {
            return new BlogPost
            {
                Author = postContract.Author?.ToEntity(),
                Categories = postContract.Categories.Select(x => new BlogTerm
                {
                    Name = x.Name,
                    Slug = x.Slug
                }).ToArray(),
                Tags = postContract.Tags.Select(x => new BlogTerm
                {
                    Name = x.Name,
                    Slug = x.Slug
                }).ToArray(),
                Summary = postContract.Summary,
                Slug = postContract.Slug,
                Title = postContract.Title,
                Updated = postContract.Updated,
                Published = postContract.Published,
            };
        }

        public static BlogAuthor ToEntity(this AuthorContract authorContract)
        {
            return new BlogAuthor
            {
                Name = authorContract.Name,
                Position = authorContract.Position,
                Slug = authorContract.Slug,
                ProfileImage = authorContract.Thumbnail,
            };
        }

        public static BlogCategory ToEntity(this TopicContract topicContract)
        {
            return new BlogCategory
            {
                Name = topicContract.Name,
                Slug = topicContract.Slug,
                Count = topicContract.NumUsages,
            };
        }
    }
}
