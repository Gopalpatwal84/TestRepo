namespace BusinessPlatform.Web.Infrastructure.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Acom.IO;
    using Acom.IO.Entities;
    using Entities;

    public class CultureHelper : CultureHelperBase
    {
        public override ICulture[] GetCultures()
        {
            return DependencyResolver.Current.GetService<IRepository<Culture>>().GetAsync().Result.ToArray();
        }

        protected override Dictionary<string, Func<string, bool, string>> CreateLinkShortcutDictionary()
        {
            return new Dictionary<string, Func<string, bool, string>>
            {
                { "msdn", (value, isFullyQualified)  => $"https://msdn.microsoft.com/library/{value}.aspx" },
                { "go", (value, isFullyQualified) => $"https://go.microsoft.com/fwlink/?LinkId={value}&clcid={GetCurrentClcid()}" },
            };
        }
    }
}