using Newtonsoft.Json;

namespace PowerBI.Jobs.CommunityPump.Models
{
    public class LithiumApiResponse<T>
    {
        public string Status { get; set; }

        public string Message { get; set; }

        [JsonProperty("http_code")]
        public int HttpCode { get; set; }

        public T Data { get; set; }
    }
}
