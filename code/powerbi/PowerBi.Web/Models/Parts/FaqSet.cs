using System.Collections.Generic;
using System.Web;

namespace PowerBI.Web.Models.Parts
{
    public class FaqSet
    {
        public FaqSet(IDictionary<string, IHtmlString> faqList)
        {
            this.Faqs = faqList;
        }

        public IDictionary<string, IHtmlString> Faqs { get; set; }
    }
}