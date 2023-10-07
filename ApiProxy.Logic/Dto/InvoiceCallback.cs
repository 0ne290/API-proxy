using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ApiProxy.Logic.Dto
{
    public class InvoiceCallback
    {
        [JsonProperty("id")]
        public string? Id { get; set; }
        [JsonProperty("status")]
        public string? Status { get; set; }
        [JsonProperty("fiat")]
        public string? Fiat { get; set; }
        [JsonProperty("fiat_amount")]
        public string? FiatAmount { get; set; }
    }
}