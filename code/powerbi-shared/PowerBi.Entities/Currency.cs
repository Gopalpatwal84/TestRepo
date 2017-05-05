using Acom.IO.Entities;

namespace PowerBI.Entities
{
    public class Currency : IHaveSlug
    {
        public string Slug { get; set; }
        public string LocKey { get; set; }
        public decimal Conversion { get; set; }
        public string Glyph { get; set; }
        public bool InvertGlyph { get; set; }
    }
}
