using Newtonsoft.Json;

namespace PublicApi.Models
{
    /// <summary>
    /// Ответ, отправляемый нашим мерчантам. Создается в результате урезания ответа от
    /// Pay2Play
    /// </summary>
    public class InvoiceResponse
    {
        [JsonProperty("id")]
        public string? Id { get; set; }
        [JsonProperty("status")]
        public string? Status { get; set; }
        [JsonProperty("type")]
        public string? Type { get; set; }
        [JsonProperty("created_at")]
        public string? CreatedAt { get; set; }
        [JsonProperty("amount")]
        public string? Amount { get; set; }
        [JsonProperty("coin")]
        public string? Coin { get; set; }
        [JsonProperty("fiat")]
        public string? Fiat { get; set; }
        [JsonProperty("fiat_amount")]
        public string? FiatAmount { get; set; }
        [JsonProperty("payment_system")]
        public string? PaymentSystem { get; set; }
        [JsonProperty("lang_id")]
        public string? LangId { get; set; }
        [JsonProperty("country_code")]
        public string? CountryCode { get; set; }
        [JsonProperty("callback_url")]
        public string? CallbackUrl { get; set; }
        [JsonProperty("redirect_url")]
        public string? RedirectUrl { get; set; }
        [JsonProperty("payment_url")]
        public string? PaymentUrl { get; set; }
        [JsonProperty("merchant")]
        public Merchant? Merchant { get; set; }
        [JsonProperty("expires_at")]
        public string? ExpiresAt { get; set; }
        [JsonProperty("fee")]
        public string? Fee { get; set; }
        public static explicit operator InvoiceResponse(Invoice invoice)
        {
            return new InvoiceResponse()
            {
                Id = invoice.Id, Status = invoice.Status, Type = invoice.Type,
                CreatedAt = invoice.CreatedAt, Amount = invoice.Amount, Coin = invoice.Coin,
                Fiat = invoice.Fiat, FiatAmount = invoice.FiatAmount, PaymentSystem = invoice.PaymentSystem,
                LangId = invoice.LangId, CountryCode = invoice.CountryCode, CallbackUrl = invoice.CallbackUrl,
                RedirectUrl = invoice.RedirectUrl, PaymentUrl = invoice.PaymentUrl, Merchant = invoice.Merchant,
                ExpiresAt = invoice.ExpiresAt
            };
        }
    }
}
