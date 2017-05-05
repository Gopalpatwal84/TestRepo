using System.Linq;
using Acom.Search.Queue.Indexing;
using PowerBI.Entities.Blogs;
using PowerBI.Search.Documents;

namespace PowerBI.Jobs.BlogPump.Features
{
    public static class IndexItem
    {
        public static IndexItemCommand ToIndexItem(this BlogPost blogPost, IndexAction action, bool priority)
        {
            var entry = new BlogEntry(blogPost.Url, blogPost.Slug)
            {
                Title = blogPost.Title,
                Summary = blogPost.Summary,
                Categories = blogPost.Categories.Select(c => c.Slug).ToArray(),
                Tags = blogPost.Tags.Select(t => t.Slug).ToArray(),
                Featured = blogPost.Featured,
                Created = blogPost.Published,
                Modified = blogPost.Published,
            };

            if (blogPost.Author != null)
            {
                entry.PublisherSlug = blogPost.Author.Slug;
                entry.PublisherName = blogPost.Author.Name;
                entry.PublisherPosition = blogPost.Author.Position;
                entry.PublisherProfileImage = blogPost.Author.ProfileImage;
            }

            var indexItemCommand = new IndexItemCommand(entry, action);

            if (priority)
            {
                indexItemCommand.Priority = SearchIndexPriority.High;
            }

            return indexItemCommand;
        }
    }
}
