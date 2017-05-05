using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acom.IO.Entities;

namespace PowerBI.Entities
{
    public class Customer : IHaveSlug
    {
        public string Slug { get; set; }

        public string Name { get; set; }

        public string URL { get; set; }

        public string Logo { get; set; }

        public IEnumerable<string> Industries { get; set; }
    }
}
