namespace PowerBI.AzureManagementServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PowerBI.AzureManagementServices.Models;

    public interface IWebSiteManagementService
    {
        Task<IEnumerable<WebSitePublishProfile>> GetWebSitePublishProfileAsync(IEnumerable<string> webSiteNames);
    }
}
