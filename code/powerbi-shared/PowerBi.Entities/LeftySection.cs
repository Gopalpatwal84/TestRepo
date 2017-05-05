using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBI.Entities
{
    public class LeftySection
    {
        public int VisibleArticles { get; set; }

        public Dictionary<string, string> Articles { get; set; }
    }
}
