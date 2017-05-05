namespace PowerBI.KuduServices.Models
{
    using System;

    using Newtonsoft.Json;

    public class KuduWebJobDescription
    {
        public string Status { get; set; }

        [JsonProperty("detailed_status")]
        public string DetailedStatus { get; set; }

        [JsonProperty("log_url")]
        public Uri LogUrl { get; set; }

        public string Name { get; set; }

        [JsonProperty("run_command")]
        public string RunCommand { get; set; }

        public Uri Url { get; set; }

        [JsonProperty("extra_info_url")]
        public Uri ExtraInfoUrl { get; set; }

        public string Type { get; set; }

        public string Error { get; set; }

        [JsonProperty("using_sdk")]
        public bool UsingSdk { get; set; }
    }
}
