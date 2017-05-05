using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Acom.IO;
using Acom.IO.Entities;
using PowerBI.Entities;

namespace PowerBI.Web.Infrastructure.Helpers
{
    public class CultureHelper : CultureHelperBase
    {
        public override ICulture[] GetCultures()
        {
            return DependencyResolver.Current.GetService<IRepository<Culture>>().GetAsync().Result.ToArray<ICulture>();
        }

        protected override Dictionary<string, Func<string, bool, string>> CreateLinkShortcutDictionary()
        {
            return new Dictionary<string, Func<string, bool, string>>
            {
                { "blog", (value, isFullyQualified) => LocalizeUrl($"/blog/{value}/", isFullyQualified)},
                { "article", (value, isFullyQualified) => LocalizeUrl($"/documentation/{value}/", isFullyQualified) },
                { "go", (value, isFullyQualified) => $"https://go.microsoft.com/fwlink/?LinkId={value}&clcid={GetCurrentClcid()}" },
                { "msdn", (value, isFullyQualified) => $"https://msdn.microsoft.com/library/{value}.aspx"},
            };
        }
    }
}