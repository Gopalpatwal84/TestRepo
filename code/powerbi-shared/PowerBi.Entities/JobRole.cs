using Acom.IO.Entities;

namespace PowerBI.Entities
{
    public class Jobrole : IHaveSlug
    {
        public string Slug { get; set; }
        public string LocKey { get; set; }
    }
}
