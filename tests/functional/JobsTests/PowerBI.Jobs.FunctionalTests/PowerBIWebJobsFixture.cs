namespace PowerBI.Jobs.FunctionalTests
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PowerBI.AzureManagementServices;
    using PowerBI.KuduServices;
    using PowerBI.KuduServices.Models;

    [TestClass]
    public class PowerBIWebJobsFixture
    {
        private const string RunningStatus = "Running";

        [TestMethod]
        public async Task AllWebJobsMustBeInRunningState()
        {
            // Arrange
            var webSiteService = new WebSiteManagementService(ConfigurationManager.AppSettings["SubscriptionId"], ConfigurationManager.AppSettings["CertificateThumbprint"]);

            var errorMessages = new ConcurrentBag<string>();

            var webJosbNames = new List<string>(ConfigurationManager.AppSettings["WebJobNames"].Split(',').Select(w => w.Trim()));
            var webSitesNames = new List<string>(ConfigurationManager.AppSettings["WebSiteNames"].Split(',').Select(w => w.Trim()));

            // Act
            var publishProfiles = (await webSiteService.GetWebSitePublishProfileAsync(webSitesNames)).ToList();

            foreach (var webSite in webSitesNames)
            {
                if (!publishProfiles.Any(p => string.Equals(p.WebSiteName, webSite)))
                {
                    errorMessages.Add(string.Format("Could not obtain publish profile for site '{0}'", webSite));
                }
            }

            if (errorMessages.Any())
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("There were issues while retrieving the publish profiles. Details:");

                foreach (var message in errorMessages)
                {
                    stringBuilder.AppendLine(message);
                }

                throw new AssertFailedException(stringBuilder.ToString());
            }

            Parallel.ForEach(
                publishProfiles,
                publishProfile =>
                {
                    try
                    {
                        var kuduCredentials = new KuduCredentials { UserName = publishProfile.UserName, UserPassword = publishProfile.UserPassword };
                        var kuduService = new KuduService(publishProfile.WebSiteName, kuduCredentials);

                        var webJobs = kuduService.GetWebJobsAsync().Result.ToList();

                        foreach (var webJobName in webJosbNames)
                        {
                            if (webJobs.Any(w => string.Equals(w.Name, webJobName)))
                            {
                                var webJob = webJobs.First(w => string.Equals(w.Name, webJobName));

                                if (!string.Equals(webJob.Status, RunningStatus))
                                {
                                    errorMessages.Add(string.Format("The WebJob '{0}' in site '{1}' is in state '{2}'", webJob.Name, publishProfile.WebSiteName, webJob.Status));
                                }
                            }
                            else
                            {
                                errorMessages.Add(string.Format("The WebJob '{0}' was not found in site '{1}'", webJobName, publishProfile.WebSiteName));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessages.Add(string.Format("There was an error while trying get the list of WebJobs in site '{0}':{1}", publishProfile.WebSiteName, ex.Message));
                    }
                });

            // Assert
            if (errorMessages.Any())
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(string.Format("There were issues in {0} WebJobs. Details:", errorMessages.Count));

                foreach (var message in errorMessages)
                {
                    stringBuilder.AppendLine(message);
                }

                throw new AssertFailedException(stringBuilder.ToString());
            }
        }
    }
}
