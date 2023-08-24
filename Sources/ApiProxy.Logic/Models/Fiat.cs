using Newtonsoft.Json;

namespace ApiProxy.Logic.Models
{
    public class Fiat : Currency
    {
        [JsonProperty("icon")]
        public string? Icon { get; set; }
    }
}
