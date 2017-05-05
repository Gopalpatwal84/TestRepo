using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PowerBI.Validation.Tests.Helpers;

namespace PowerBI.Validation.Tests
{
    [TestClass]
    [DeploymentItem(@"..\..\..\PowerBI.Json\Data\Lefties\", "Lefties")]
    public class LeftNavFixture
    {
        private static readonly Dictionary<string, Dictionary<string, LeftySection>> s_leftyJsonFileNamesWithContents = new Dictionary<string, Dictionary<string, LeftySection>>();
        private static IEnumerable<EmbeddedResource> s_resxFiles;
        private static readonly ConcurrentBag<string> s_validationErrors = new ConcurrentBag<string>();

        private readonly List<string> _linkShortcuts = new List<string>
        {
            "article",
            "blog",
            "go",
            "msdn",
        };

        private const string LeftyJsonFilesFolder = @".\Lefties";

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            ParseLeftyJsonfiles();
            LoadResxFiles();

            if (s_validationErrors.Any())
            {
                // Manually calling the ClassCleanup before failing as it won't be executed if there are errors in ClassInitialize
                ClassCleanup();
                Assert.Fail(string.Join(Environment.NewLine, s_validationErrors));
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (s_validationErrors.Any() && IsRunningOnTeamCity())
            {
                var validationResults = string.Format(
                                        "The following errors were found when validating the left navigation files:{0}{0}{0}{1}",
                                        Environment.NewLine,
                                        string.Join(Environment.NewLine + Environment.NewLine, s_validationErrors));

                WriteValidationLog(validationResults);
            }
        }

        [TestMethod]
        [TestCategory("Validation")]
        public void ValidateJsonFilesAreValid()
        {
            Assert.IsTrue(s_leftyJsonFileNamesWithContents.Any());
        }

        [TestMethod]
        [TestCategory("Validation")]
        public void ValidateAreaKeys()
        {
            var errors = new List<string>();

            foreach (var leftyJson in s_leftyJsonFileNamesWithContents)
            {
                // Validate that the area's keys in the lefty json file start with "Area_"
                var invalidKeys = leftyJson.Value.Keys.Where(areaKey => !areaKey.StartsWith("Area_")).Select(areaKey => string.IsNullOrWhiteSpace(areaKey) ? "{empty}" : areaKey).ToList();

                if (invalidKeys.Any())
                {
                    errors.Add($"Not all of the area keys in the left navigation JSON file '{Path.GetFileName(leftyJson.Key)}' starts with 'Area_'. Invalid area keys: {string.Join(", ", invalidKeys)}.");
                }
            }

            if (errors.Any())
            {
                errors.ForEach(e => s_validationErrors.Add(e));
                Assert.Fail(string.Join(Environment.NewLine, errors));
            }
        }

        [TestMethod]
        [TestCategory("Validation")]
        public void ValidateLinkKeys()
        {
            var errors = new List<string>();

            foreach (var leftyJson in s_leftyJsonFileNamesWithContents)
            {
                // Validate that the link keys in the lefty json file start with "Link_"
                var invalidKeys = new List<string>();

                foreach (var links in leftyJson.Value.Values)
                {
                    var invalidLinks = links.Articles.Where(link => !link.Key.StartsWith("Link_")).Select(link => string.IsNullOrWhiteSpace(link.Key) ? "{empty}" : link.Key).ToList();
                    invalidKeys.AddRange(invalidLinks);
                }

                if (invalidKeys.Any())
                {
                    errors.Add($"Not all of the link keys in the left navigation JSON file '{Path.GetFileName(leftyJson.Key)}' starts with 'Link_'. Invalid link keys: {string.Join(", ", invalidKeys)}.");
                }
            }

            if (errors.Any())
            {
                errors.ForEach(e => s_validationErrors.Add(e));
                Assert.Fail(string.Join(Environment.NewLine, errors));
            }
        }

        [TestMethod]
        [TestCategory("Validation")]
        public void ValidateLinkValues()
        {
            var errors = new List<string>();

            foreach (var leftyJson in s_leftyJsonFileNamesWithContents)
            {
                var invalidEntries = new List<string>();

                foreach (var links in leftyJson.Value.Values)
                {
                    var invalidValues = links.Articles.Where(link => !this._linkShortcuts.Contains(link.Value.Split(':').First().Trim())).Select(link => string.IsNullOrWhiteSpace(link.Value) ? "{empty}" : link.Value).ToList();
                    invalidEntries.AddRange(invalidValues);
                }

                if (invalidEntries.Any())
                {
                    errors.Add($"Not all of the link values in the left navigation JSON file '{Path.GetFileName(leftyJson.Key)}' start with one of the allowed prefixes: '{string.Join(":','", _linkShortcuts)}:'. Invalid link values: {string.Join(", ", invalidEntries)}.");
                }
            }

            if (errors.Any())
            {
                errors.ForEach(e => s_validationErrors.Add(e));
                Assert.Fail(string.Join(Environment.NewLine, errors));
            }
        }

        [TestMethod]
        [TestCategory("Validation")]
        public void ValidateLinkUrls()
        {
            var errors = new List<string>();

            foreach (var leftyJson in s_leftyJsonFileNamesWithContents)
            {
                var invalidEntries = new List<KeyValuePair<string, KeyValuePair<string, string>>>();

                foreach (var links in leftyJson.Value.Values)
                {
                    foreach (var link in links.Articles)
                    {
                        if (link.Value.StartsWith("article:") && link.Value.EndsWith(".md"))
                        {
                            invalidEntries.Add(new KeyValuePair<string, KeyValuePair<string, string>>("Remove '.md' from article slug", link));
                        }

                        foreach (var supportedPrefix in _linkShortcuts)
                        {
                            if (link.Value.StartsWith(supportedPrefix + ": "))
                            {
                                invalidEntries.Add(new KeyValuePair<string, KeyValuePair<string, string>>("Remove space after '" + supportedPrefix + ":'", link));
                            }
                        }
                    }
                }

                errors.AddRange(invalidEntries.Select(entry => string.Format("{0}| Url: '{1}'| Key: {2} |File: {3}-------", entry.Key, entry.Value.Value, entry.Value.Key, Path.GetFileName(leftyJson.Key))));
            }

            if (errors.Any())
            {
                errors.ForEach(e => s_validationErrors.Add(e));
                Assert.Fail(string.Join(Environment.NewLine, errors));
            }
        }

        [TestMethod]
        [TestCategory("Validation")]
        public void ValidateLinkCasing()
        {
            var errors = new List<string>();

            foreach (var leftyJson in s_leftyJsonFileNamesWithContents)
            {
                var invalidLinks = new List<string>();

                var links = leftyJson.Value.Values.SelectMany(x => x.Articles.Values).Where(x => x.Contains("article:"));

                var invalidCasing = links
                            .Where(x => !x.Contains("#") && x != x.ToLower())
                            .Select(link => string.IsNullOrWhiteSpace(link) ? "{empty}" : link)
                            .ToList();

                var invalidCasingWithAnchorLinks = links
                            .Where(x => x.Contains("#"))
                            .Select(x => x.Substring(0, x.IndexOf('#')))
                            .Where(x => x != x.ToLower())
                            .Select(link => string.IsNullOrEmpty(link) ? "{empty}" : link)
                            .ToList();

                invalidCasing.AddRange(invalidCasingWithAnchorLinks);
                invalidLinks.AddRange(invalidCasing);

                if (invalidLinks.Any())
                {
                    errors.Add($"Not all link values in the left navigation JSON file '{Path.GetFileName(leftyJson.Key)}' have the correct casing, please ensure that all azure.microsoft.com links are lowercase. Invalid links: {string.Join(", ", invalidLinks)}");
                }
            }

            if (errors.Any())
            {
                errors.ForEach(e => s_validationErrors.Add(e));
                Assert.Fail(string.Join(Environment.NewLine, errors));
            }
        }

        [TestMethod]
        [TestCategory("Validation")]
        public void ValidateAllKeysInJsonExistsInResx()
        {
            var errors = new List<string>();

            foreach (var leftyJson in s_leftyJsonFileNamesWithContents)
            {
                var resxFile = GetResxFileByJsonFilePath(leftyJson.Key);

                if (resxFile == null)
                {
                    errors.Add($"The left navigation JSON file '{Path.GetFileName(leftyJson.Key)}' does not have a corresponding Resx file.");
                    continue;
                }

                var jsonKeysToProcess = leftyJson.Value.Keys.Union(leftyJson.Value.Values.SelectMany(link => link.Articles.Keys));
                var invalidKeys = new List<string>();

                foreach (var cultureResources in resxFile.ResourceEntries)
                {
                    invalidKeys.AddRange(jsonKeysToProcess.Where(jsonKey => !cultureResources.Entries.Keys.Distinct().Contains(jsonKey)).ToList());
                }

                if (invalidKeys.Any())
                {
                    errors.Add($"The following keys in the left navigation JSON file '{Path.GetFileName(leftyJson.Key)}' don't exist in it's corresponding Resx file '{resxFile.Name + ".resx"}': {string.Join(", ", invalidKeys)}.");
                }
            }

            if (errors.Any())
            {
                errors.ForEach(e => s_validationErrors.Add(e));
                Assert.Fail(string.Join(Environment.NewLine, errors));
            }
        }

        [TestMethod]
        [TestCategory("Validation")]
        public void ValidateAllKeysInResxExistInJson()
        {
            var errors = new List<string>();
            var whitelist = new[]
            {
                "Title"
            };

            foreach (var leftyJson in s_leftyJsonFileNamesWithContents)
            {
                var resxFile = GetResxFileByJsonFilePath(leftyJson.Key);

                if (resxFile == null)
                {
                    errors.Add($"The left navigation JSON file '{Path.GetFileName(leftyJson.Key)}' does not have a corresponding Resx file.");
                    continue;
                }

                var jsonKeysToProcess = leftyJson.Value.Keys.Union(leftyJson.Value.Values.SelectMany(link => link.Articles.Keys));
                var invalidKeys = new List<string>();

                foreach (var cultureResources in resxFile.ResourceEntries)
                {
                    invalidKeys.AddRange(cultureResources.Entries.Keys.Distinct().Where(resxKey => !jsonKeysToProcess.Contains(resxKey) && !whitelist.Contains(resxKey)).ToList());
                }

                if (invalidKeys.Any())
                {
                    errors.Add($"The following keys in the left navigation resx file '{resxFile.Name + ".resx"}' don't exist in it's corresponding Json file '{Path.GetFileName(leftyJson.Key)}': {string.Join(", ", invalidKeys)}.");
                }
            }

            if (errors.Any())
            {
                errors.ForEach(e => s_validationErrors.Add(e));
                Assert.Fail(string.Join(Environment.NewLine, errors));
            }
        }

        private static void LoadResxFiles()
        {
            try
            {
                // Parse resx files
                s_resxFiles = ResourcesLoader.GetAllResources(new[] { "en-us" }, new[] { "\\Shared\\Lefties" });
            }
            catch (Exception ex)
            {
                s_validationErrors.Add($"There was an error loading lefty Resx files. Details: {ex.Message}");
            }
        }

        private static void ParseLeftyJsonfiles()
        {
            var leftyJsonFilesFolder = Path.GetFullPath(LeftyJsonFilesFolder);

            // Check that the left navigations JSON folder path exists
            if (!Directory.Exists(leftyJsonFilesFolder))
            {
                s_validationErrors.Add($"The full path '{leftyJsonFilesFolder}' for the left navigations JSON files does not exist.");
            }
            else
            {
                var leftyJsonFilePaths = Directory.GetFiles(leftyJsonFilesFolder).ToList();

                // Deserialize all left navigation JSON and check for malformed files
                foreach (var leftyJsonFilePath in leftyJsonFilePaths)
                {
                    var jsonFileContent = File.ReadAllText(leftyJsonFilePath);

                    try
                    {
                        // Validate that JSON file is valid
                        var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, LeftySection>>(jsonFileContent);
                        s_leftyJsonFileNamesWithContents.Add(Path.GetFileName(leftyJsonFilePath), jsonObject);
                    }
                    catch (Exception ex)
                    {
                        s_validationErrors.Add($"Couldn't deserialize JSON file '{Path.GetFileName(leftyJsonFilePath)}'. Details: {ex.Message}");
                    }
                }
            }
        }

        private static void WriteValidationLog(string logText)
        {
            var logDir = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), Environment.GetEnvironmentVariable("BUILD_NUMBER") ?? string.Empty, @"ACOM.Validation.Tests\Logs\");
            var logFilename = typeof(LeftNavFixture) + ".log";

            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            File.WriteAllText(Path.Combine(logDir, logFilename), logText);
        }

        private static bool IsRunningOnTeamCity()
        {
            return Environment.GetEnvironmentVariable("TEAMCITY_VERSION") != null;
        }

        private EmbeddedResource GetResxFileByJsonFilePath(string jsonFilePath)
        {
            var jsonKey = Path.GetFileNameWithoutExtension(jsonFilePath).Split('.').First().ToLowerInvariant();

            return s_resxFiles.SingleOrDefault(x => x.Name == $"PowerBI.Resources.Shared.Lefties.{jsonKey.Replace("-", "_")}");
        }
    }

    public class LeftySection
    {
        public int VisibleArticles { get; set; }

        public Dictionary<string, string> Articles { get; set; }
    }
}
