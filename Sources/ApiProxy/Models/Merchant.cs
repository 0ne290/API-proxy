using Newtonsoft.Json;

namespace PublicApi.Models
{
    public class Merchant
    {
        [JsonProperty("title")]
        public string? Title { get; set; }
    }
}
