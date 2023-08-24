using Newtonsoft.Json;

namespace ApiProxy.Logic.Models
{
    public class Merchant
    {
        [JsonProperty("title")]
        public string? Title { get; set; }
    }
}
