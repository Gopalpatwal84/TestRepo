using System.Collections.Generic;
using System.Resources;
using Acom.IO.Entities;

namespace PowerBI.Entities
{
    public class Lefty : IHaveSlug
    {
        public string Slug { get; set; }
        public Dictionary<string, LeftySection> Maps { get; set; }
        public ResourceManager ResourceManager { get; set; }
    }
}
