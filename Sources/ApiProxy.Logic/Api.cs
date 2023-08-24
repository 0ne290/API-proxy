using System.Text;
using System.Net;
using ApiProxy.Logic.Models;
using Newtonsoft.Json;

namespace ApiProxy.Logic
{
    public class Api
    {
        public Api(string? apiUrl, string? innerCallbackInvoices, string? accessToken, string? redirectUrl, string? callbackUrl, string? id)
        {
            ApiUrl = apiUrl;
            InnerCallbackInvoices = innerCallbackInvoices;
            AccessToken = accessToken;
            RedirectUrl = redirectUrl;
            CallbackUrl = callbackUrl;
            Id = id;
        }

        public List<Fiat>? Fiats(string fiatsUrl)
        {
            var resUrl = $"{ApiUrl}{fiatsUrl}";
            var res = Tools.SendRequest<List<Fiat>>(HttpMethod.Get, resUrl, out var error, AccessToken, out var code);
            if (code != HttpStatusCode.OK) throw new Exception($"http code [{code}], message [{error}]");
            return res;
        }
        /// <summary>
        /// Формируем счет на оплату в крипте
        /// </summary>
        public bool InvoicesCryptocurrency(string? pCoin, int? pAmount, string invoicesUrl, out ResponseJson resJson, out HttpStatusCode code)
        {
            resJson = new ResponseJson();
            resJson.Response = "Request to create an invoice in cryptocurrency";
            resJson.Message = "Fail. Set correct values for required parameters";
            code = HttpStatusCode.BadRequest;
            if (pCoin == null || pAmount == null)
                return false;
            string error;
            string resUrl = ApiUrl + invoicesUrl;
            InvoiceCryptocurrencyCreate body = new InvoiceCryptocurrencyCreate() { RedirectUrl = RedirectUrl, CallbackUrl = InnerCallbackInvoices + Id, Coin = pCoin, Amount = pAmount.ToString() };
            Invoice? res = Tools.SendRequest<StringContent, Invoice>(new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"), HttpMethod.Post, resUrl, out error, AccessToken, out code);
            resJson.Response = (InvoiceResponse)res;
            ((InvoiceResponse)resJson.Response).CallbackUrl = CallbackUrl;
            resJson.Message = error;
            return true;
        }
        /// <summary>
        /// Формируем счет на оплату в фиате
        /// </summary>
        public bool InvoicesFiat(string? pFiat, int? pAmount, string invoicesUrl, out ResponseJson resJson, out HttpStatusCode code)
        {
            resJson = new ResponseJson();
            resJson.Response = "Request to create an invoice in fiat";
            resJson.Message = "Fail. Set correct values for required parameters";
            code = HttpStatusCode.BadRequest;
            if (pFiat == null || pAmount == null)
                return false;
            string error;
            string resUrl = ApiUrl + invoicesUrl;
            InvoiceFiatCreate body = new InvoiceFiatCreate() { RedirectUrl = RedirectUrl, CallbackUrl = InnerCallbackInvoices + Id, Fiat = pFiat, FiatAmount = pAmount.ToString() };
            Invoice? res = Tools.SendRequest<StringContent, Invoice>(new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"), HttpMethod.Post, resUrl, out error, AccessToken, out code);
            resJson.Response = (InvoiceResponse)res;
            ((InvoiceResponse)resJson.Response).CallbackUrl = CallbackUrl;
            resJson.Message = error;
            return true;
        }

        public string? ApiUrl { get; set; }
        public string? InnerCallbackInvoices { get; set; }
        public string? AccessToken { get; set; }
        public string? RedirectUrl { get; set; }
        public string? CallbackUrl { get; set; }
        public string? Id { get; set; }
    }
}