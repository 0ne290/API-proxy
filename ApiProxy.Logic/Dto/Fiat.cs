using Newtonsoft.Json;

namespace ApiProxy.Logic.Dto
{
    public class Fiat : Currency
    {
        [JsonProperty("icon")]
        public string? Icon { get; set; }
    }
}