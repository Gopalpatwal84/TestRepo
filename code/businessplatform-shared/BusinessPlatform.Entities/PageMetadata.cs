namespace BusinessPlatform.Entities
{
    using System.Collections.Generic;
    using System.IO;
    using System.Resources;
    using Acom.IO.Entities;

    public class PageMetadata : IHaveSlug
    {
        private string slug;

        public string Slug
        {
            get
            {
                return this.Url.Trim('/').Replace("/", "--");
            }

            set
            {
                this.slug = value;
            }
        }

        public string ViewPath { get; set; }

        public string Url { get; set; }

        public string PrimaryResxFile { get; set; }

        public IEnumerable<string> AdditionalResxFiles { get; set; }

        public IEnumerable<string> AvailableCultures { get; set; }

        public string ViewType { get; set; }

        public string ResxMetaPageTitle { get; set; }

        public string ResxMetaDescription { get; set; }

        public string ResxMetaKeywords { get; set; }

        public void SetMetaValuesFromPrimaryResx()
        {
            var resourcesAssembly = typeof(Resources.Pages.Index).Assembly;

            if (string.IsNullOrWhiteSpace(this.PrimaryResxFile))
            {
                return;
            }

            try
            {
                var resourceName = "BusinessPlatform.Resources." + Path.ChangeExtension(this.PrimaryResxFile, string.Empty).Replace("\\", ".").TrimEnd('.');

                for (var i = 0; i < 10; i++)
                {
                    resourceName = resourceName.Replace("." + i, "._" + i);
                }

                var rm = new ResourceManager(resourceName, resourcesAssembly)
                {
                    IgnoreCase = true
                };

                this.ResxMetaPageTitle = rm.GetString("Meta_PageTitle") ?? string.Empty;
                this.ResxMetaDescription = rm.GetString("Meta_Description") ?? string.Empty;
                this.ResxMetaKeywords = rm.GetString("Meta_Keywords") ?? string.Empty;
            }
            catch
            {
                this.ResxMetaPageTitle = "Error reading resource";
                this.ResxMetaDescription = "Error reading resource";
                this.ResxMetaKeywords = "Error reading resource";
            }
        }
    }
}
