using System.Collections.Generic;
using PowerBI.Entities;

namespace PowerBI.Web.Models.Pages
{
    public class MobileIndexViewModel : ViewMetadataModel
    {
        public IEnumerable<AndroidApp> AndroidApps { get; set; }
    }
}