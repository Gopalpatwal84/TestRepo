namespace PowerBI.Validation.Tests.Helpers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public static class ResourcesLoader
    {
        private static ConcurrentBag<string> errors = new ConcurrentBag<string>();

        public static IEnumerable<EmbeddedResource> GetAllResources(IEnumerable<string> cultures, IEnumerable<string> filesFilter)
        {
            var result = new ConcurrentBag<EmbeddedResource>();

            var resourceNames = GetResourceNames(typeof(PowerBI.Resources.Pages.Index), filesFilter);

            Parallel.ForEach(
                resourceNames, 
                resource =>
                {
                    var resourceName = SanitizeResourceName(resource);

                    var embeddedResource = new EmbeddedResource { Name = resourceName };

                    embeddedResource.ResourceEntries.AddRange(GetResourceEntries(typeof(PowerBI.Resources.Pages.Index), resourceName, cultures));

                    result.Add(embeddedResource);
                });

            if (errors.Any())
            {
                throw new Exception(string.Join(Environment.NewLine, errors));
            }

            return result.OrderBy(r => r.Name);
        }

        public static string GetFileNameFromResourceName(string resourceName)
        {
            var resourcesProjectName = "PowerBI.Resources";

            // PowerBI.Resources.Pages.Index => PowerBI.Resources/Pages/Index.resx
            return resourceName.Substring(0, resourcesProjectName.Length) + resourceName.Substring(resourcesProjectName.Length).Replace(".", "/") + ".resx";
        }

        private static string SanitizeResourceName(string resourceName)
        {
            var updatedResourceName = resourceName.Replace(".resources", string.Empty).Replace('-', '_');

            var lastSegment = updatedResourceName.Split('.').Last();
            int n;
            if (int.TryParse(lastSegment.Substring(0, 1), out n))
            {
                updatedResourceName = updatedResourceName.Replace("." + lastSegment, "._" + lastSegment);
            }

            return updatedResourceName;
        }

        /// <summary>
        /// Lists the Resources available starting with a certain pattern. For example "FAQ."
        /// </summary>
        /// <param name="type">Type to use to obtain the underlying Resources assembly</param>
        /// <param name="filesFilter">List of RESX paths relative to the Resources project. Used to filter the resource names obtained</param>
        /// <returns>List of resources name that match the pattern (as found in the assembly)</returns>
        private static IEnumerable<string> GetResourceNames(Type type, IEnumerable<string> filesFilter)
        {
            var asm = type.Assembly;

            var resourceNames = asm.GetManifestResourceNames();

            if ((filesFilter == null) || !filesFilter.Any())
            {
                return resourceNames.ToList();
            }

            var filteredResourceNames = new List<string>();

            foreach (var file in filesFilter)
            {
                var resxNamespace = file.Replace('\\', '.').Replace('/', '.').Replace(".resx", string.Empty);
                var resourceNamePattern = asm.GetName().Name + "." + resxNamespace.TrimStart('.').Trim();
                filteredResourceNames.AddRange(resourceNames.Where(n => n.StartsWith(resourceNamePattern, StringComparison.OrdinalIgnoreCase)));
            }

            return filteredResourceNames;
        }

        /// <summary>
        /// Obtains a dictionary with all string entries (key, value) within the specified resource
        /// </summary>
        /// <param name="type">Type to use to obtain the underlying Resources assembly</param>
        /// <param name="resourceName">Resource name to look for within the assembly</param>
        /// <param name="cultures">Cultures to localize the resources</param>
        /// <returns>Dictionary containing all string entries (key, value) within the specified resource</returns>
        private static IEnumerable<CultureResources> GetResourceEntries(Type type, string resourceName, IEnumerable<string> cultures)
        {
            var culturesResources = new List<CultureResources>();

            var asm = type.Assembly;

            Type resourceType = asm.GetType(resourceName, true);

            var cultureProperty = resourceType.GetProperty(
                "Culture",
                BindingFlags.Public | BindingFlags.Static | BindingFlags.SetProperty);

            if (cultureProperty == null)
            {
                errors.Add(string.Format("Resource file {0} is missing the 'Culture' property.", resourceName));
                return culturesResources;
            }

            PropertyInfo[] resourceProps = resourceType.GetProperties(
                BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty);

            foreach (var culture in cultures)
            {
                var cultureResources = new CultureResources { Culture = culture };

                cultureProperty.SetValue(null, new CultureInfo(culture));

                var stringType = typeof(string);
                foreach (PropertyInfo info in resourceProps)
                {
                    if (info.PropertyType == stringType)
                    {
                        string name = info.Name;
                        object value = info.GetValue(null, null);

                        if (value == null)
                        {
                            errors.Add(string.Format("Resource file {0} has no readable property {1}.", resourceName, name));
                            return culturesResources;
                        }

                        cultureResources.Entries.Add(name, value.ToString());
                    }
                }

                culturesResources.Add(cultureResources);
            }

            return culturesResources;
        }
    }
}
