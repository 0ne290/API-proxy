using System.Text;
using System.Net;
using ApiProxy.Logic.Models;
using Newtonsoft.Json;
using System.Net.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;

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
            try
            {
                var resUrl = $"{ApiUrl}{fiatsUrl}";
                var res = Tools.SendRequest<List<Fiat>>(HttpMethod.Get, resUrl, AccessToken);
                return res;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        /// <summary>
        /// Формируем счет на оплату в крипте
        /// </summary>
        public InvoiceResponse InvoicesCryptocurrency(string? pCoin, int? pAmount, string invoicesUrl)
        {
            if (pCoin == null || pAmount == null)
                throw new Exception("Set correct values");
            try
            {
                var resUrl = $"{ApiUrl}{invoicesUrl}";
                var body = new InvoiceCryptocurrencyCreate() { RedirectUrl = RedirectUrl, CallbackUrl = InnerCallbackInvoices + Id, Coin = pCoin, Amount = pAmount.ToString() };
                var res = (InvoiceResponse)Tools.SendRequest<StringContent, Invoice>(new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"), HttpMethod.Post, resUrl, AccessToken);
                res.CallbackUrl = CallbackUrl;
                return res;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        /// <summary>
        /// Формируем счет на оплату в фиате
        /// </summary>
        public InvoiceResponse InvoicesFiat(string? pFiat, int? pAmount, string invoicesUrl)
        {
            if (pFiat == null || pAmount == null)
                throw new Exception("Set correct values");
            try
            {
                var resUrl = $"{ApiUrl}{invoicesUrl}";
                var body = new InvoiceFiatCreate() { RedirectUrl = RedirectUrl, CallbackUrl = InnerCallbackInvoices + Id, Fiat = pFiat, FiatAmount = pAmount.ToString() };
                var res = (InvoiceResponse)Tools.SendRequest<StringContent, Invoice>(new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"), HttpMethod.Post, resUrl, AccessToken);
                res.CallbackUrl = CallbackUrl;
                return res;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string? ApiUrl { get; set; }
        public string? InnerCallbackInvoices { get; set; }
        public string? AccessToken { get; set; }
        public string? RedirectUrl { get; set; }
        public string? CallbackUrl { get; set; }
        public string? Id { get; set; }
    }
}