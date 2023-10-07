using System.Text;
using Newtonsoft.Json;

namespace ApiProxy.Logic.Dto
{
    public class InvoiceCryptocurrencyCreate
    {
        public InvoiceCryptocurrencyCreate(string? redirectUrl, string? callbackUrl, string? coin, string? amount)
        {
            RedirectUrl = redirectUrl;
            CallbackUrl = callbackUrl;
            Coin = coin;
            Amount = amount;
        }

        public StringContent ToStringContent()
        {
            var json = JsonConvert.SerializeObject(this);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

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