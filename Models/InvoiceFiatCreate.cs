using Newtonsoft.Json;

namespace PublicApi.Models
{
    /// <summary>
    /// Тело для запроса формирования счета на оплату в фиате
    /// </summary>
    public class InvoiceFiatCreate
    {
        [JsonProperty("redirect_url")]
        public string? RedirectUrl { get; set; }
        [JsonProperty("callback_url")]
        public string? CallbackUrl { get; set; }
        [JsonProperty("fiat")]
        public string? Fiat { get; set; }
        [JsonProperty("fiat_amount")]
        public string? FiatAmount { get; set; }
    }
}
