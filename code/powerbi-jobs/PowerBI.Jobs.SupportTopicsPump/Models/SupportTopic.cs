using System.Collections.Generic;

namespace PowerBI.Jobs.SupportTopicsPump.Models
{
    public class SupportTopic
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<SupportTopic> Subtopics { get; set; }
    }
}
