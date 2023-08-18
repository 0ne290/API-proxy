using Newtonsoft.Json;

namespace PublicApi.Models
{
    public class Currency
    {
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("full_name")]
        public string? FullName { get; set; }
        [JsonProperty("decimals")]
        public int? Decimals { get; set; }
    }
}
