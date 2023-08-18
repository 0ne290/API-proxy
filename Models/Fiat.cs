using Newtonsoft.Json;

namespace PublicApi.Models
{
    public class Fiat : Currency
    {
        [JsonProperty("icon")]
        public string? Icon { get; set; }
    }
}
