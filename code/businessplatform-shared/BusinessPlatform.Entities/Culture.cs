namespace BusinessPlatform.Entities
{
    using System;
    using System.Collections.Generic;
    using Acom.IO.Entities;

    [Serializable]
    public class Culture : ICulture
    {
        public string Slug { get; set; }

        public IEnumerable<string> FuzzyMatches { get; set; }

        public bool IsDisplayed { get; set; }

        public string FallbackCulture { get; set; }

        public IEnumerable<string> AcceptLanguages { get; }

        public string FallbackCultureBasedOn { get; set; }

        public CultureResxLocalizationSupport ResxLocalizationSupport { get; set; }
    }
}
