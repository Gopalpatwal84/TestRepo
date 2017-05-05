using Acom.IO.Entities;

namespace PowerBI.Entities
{
    public class AndroidApp : IHaveSlug
    {
        public string Slug { get; set; }

        public string LocKey { get; set; }

        public string DownloadUrl { get; set; }

        public string SvgIconSlug { get; set; }
    }
}
