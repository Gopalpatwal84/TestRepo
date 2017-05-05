using System.Collections.Generic;
using Acom.Search.Client.Documents;

namespace PowerBI.Search.Documents
{
    public abstract class PowerBiEntry : Entry
    {
        public static string GuidedLearningTag = "isGuidedLearning";

        protected PowerBiEntry(string url, string slug) : base(url, slug)
        {
        }

        protected PowerBiEntry()
        {
        }

        public string ExternalId { get { return Get<string>(); } set { SetValue(value); } }

        public string TitleCanonical { get { return Get<string>(); } set { SetValue(value); } }

        public IEnumerable<string> Countries { get { return Get<IEnumerable<string>>(); } set { SetValue(value); } }

        public IEnumerable<string> Cultures { get { return Get<IEnumerable<string>>(); } set { SetValue(value); } }

        public IEnumerable<string> Industries { get { return Get<IEnumerable<string>>(); } set { SetValue(value); } }

        public IEnumerable<string> Departments { get { return Get<IEnumerable<string>>(); } set { SetValue(value); } }
    }
}