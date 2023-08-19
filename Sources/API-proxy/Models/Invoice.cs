﻿using Newtonsoft.Json;

namespace PublicApi.Models
{
    /// <summary>
    /// Результат выполнения запроса на формирование счета на оплату
    /// </summary>
    public class Invoice
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
    }
}
