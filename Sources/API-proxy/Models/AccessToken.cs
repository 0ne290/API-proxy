using Newtonsoft.Json;

namespace PublicApi.Models
{
    /// <summary>
    /// Результат запроса на получение токена авторизации
    /// </summary>
    public class AccessTokenResponse
    {
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }
        [JsonProperty("expires_at")]
        public int? ExpiresAt { get; set; }
    }
}
