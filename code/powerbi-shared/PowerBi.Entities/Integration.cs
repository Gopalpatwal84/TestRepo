namespace PowerBI.Entities
{
    using Acom.IO.Entities;

    public class Integration : IHaveSlug, IHaveDetailPage
    {
        public string Slug { get; set; }

        public string NameLocKey { get; set; }

        public string TitleLocKey { get; set; }

        public string HeadingLocKey { get; set; }

        public string SubHeadingLocKey { get; set; }

        public string ContentLocKey { get; set; }

        public string SubContent1LocKey { get; set; }

        public string SubContent2LocKey { get; set; }

        public string SubContent3LocKey { get; set; }

        public string VideoUrl { get; set; }

        public string InfoHeadingLocKey { get; set; }

        public string InfoContentLocKey { get; set; }

        public string ConnectHeadingLocKey { get; set; }

        public string ConnectContentLocKey { get; set; }

        public string MetaKeywordsLocKey { get; set; }

        public string MetaDescriptionLocKey { get; set; }

        public string MetaTitleLocKey { get; set; }

        public string[] Tags { get; set; }

        public bool FromService { get; set; }

        public string ImageIcon
        {
            get
            {
                return string.Format("/pictures/shared/integrations/1x/{0}.png", this.Slug);
            }
        }

        public string ImageIcon2x
        {
            get
            {
                return string.Format("/pictures/shared/integrations/2x/{0}@2x.png", this.Slug);
            }
        }

        public string Url
        {
            get
            {
                return string.Format("/integrations/{0}/", this.Slug);
            }
        }
    }
}
