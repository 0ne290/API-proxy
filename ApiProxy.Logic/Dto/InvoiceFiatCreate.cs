using System.Text;
using Newtonsoft.Json;

namespace ApiProxy.Logic.Dto
{
    public class InvoiceFiatCreate
    {
		public InvoiceFiatCreate(string? redirectUrl, string? callbackUrl, string? fiat, string? fiatAmount)
        {
            RedirectUrl = redirectUrl;
            CallbackUrl = callbackUrl;
            Fiat = fiat;
			FiatAmount = fiatAmount;
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
        [JsonProperty("fiat")]
        public string? Fiat { get; set; }
        [JsonProperty("fiat_amount")]
        public string? FiatAmount { get; set; }
    }
}