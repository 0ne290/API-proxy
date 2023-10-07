using Newtonsoft.Json;

namespace PublicApi.Models
{
    /// <summary>
    /// Тело для запроса формирования счета на оплату в крипте
    /// </summary>
    public class InvoiceCryptocurrencyCreate
    {
        [JsonProperty("redirect_url")]
        public string? RedirectUrl { get; set; }
        [JsonProperty("callback_url")]
        public string? CallbackUrl { get; set; }
        [JsonProperty("coin")]
        public string? Coin { get; set; }
        [JsonProperty("amount")]
        public string? Amount { get; set; }
    }
}
