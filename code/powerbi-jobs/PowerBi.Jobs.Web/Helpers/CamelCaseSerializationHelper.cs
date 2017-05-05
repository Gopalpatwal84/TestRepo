namespace PowerBI.Jobs.Web.Helpers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public static class CamelCaseSerializationHelper
    {
        private static JsonSerializerSettings serializationSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

        public static string SerializeObject(object data)
        {
            return JsonConvert.SerializeObject(data, serializationSettings);
        }
    }
}