using Newtonsoft.Json;

namespace ApiProxy.Logic.Dto
{
    public class AccessTokenResponse
    {
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }
        [JsonProperty("expires_at")]
        public int? ExpiresAt { get; set; }
    }
}