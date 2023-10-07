using Newtonsoft.Json;

namespace ApiProxy.Logic.Dto
{
    public class Merchant
    {
        [JsonProperty("title")]
        public string? Title { get; set; }
    }
}