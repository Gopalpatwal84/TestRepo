namespace PowerBI.Web.Models
{
    public class Social
    {
        public Social()
        {
            this.DefaultImageRelativePath = "pictures/shared/social/social-default-image.png";
        }

        public string Domain { get; set; }

        public string Url { get; set; }

        public string CustomShareMessage { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string DefaultImageRelativePath { get; set; }

        public string VideoUrl { get; set; }
    }
}