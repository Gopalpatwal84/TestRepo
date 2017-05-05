using System;
using System.Collections.Generic;
using Acom.IO.Entities;

namespace PowerBI.Entities
{
    [Serializable]
    public class Culture : ICulture
    {
        public string Slug { get; set; }
        public string DefaultCurrency { get; set; }
        public bool IsDisplayed { get; set; }
        public IEnumerable<string> FuzzyMatches { get; set; }

        public string FallbackCulture { get; set; }

        public IEnumerable<string> AcceptLanguages { get; set; }

        public string FallbackCultureBasedOn { get; set; }

        public CultureResxLocalizationSupport ResxLocalizationSupport { get; set; }
    }
}
