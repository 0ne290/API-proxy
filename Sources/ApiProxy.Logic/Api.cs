using System.Text;
using System.Net;
using ApiProxy.Logic.Models;
using Newtonsoft.Json;
using System.Net.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;
using System.Linq.Expressions;
using ApiProxy.Logic.Database;

namespace ApiProxy.Logic
{
    public class Api
    {
        public Api(string apiUrl, string innerCallbackInvoices, string accessToken, string id, string connectionString)
        {
            ApiUrl = apiUrl;
            InnerCallbackInvoices = innerCallbackInvoices;
            AccessToken = accessToken;
            Id = id;

            MyDbContext db = MyDbContext.GetMyDbContext(connectionString);
            Database.Merchant? merchant = db.Merchants.Find(id);
            db.Dispose();
            RedirectUrl = merchant.MerchantRedirectUrl;
            CallbackUrl = merchant.MerchantCallbackUrl;
        }

        public List<Fiat>? Fiats(string fiatsUrl)
        {
            var resUrl = $"{ApiUrl}{fiatsUrl}";
            return Tools.SendRequest<List<Fiat>>(HttpMethod.Get, resUrl, AccessToken); 
        }

        public InvoiceResponse InvoicesCryptocurrency(string? nameCoin, decimal? amount, string invoicesUrl)
        {
            if (string.IsNullOrEmpty(nameCoin) || amount == null)
                throw new Exception("Set correct values");

            var resUrl = $"{ApiUrl}{invoicesUrl}";
            var body = new InvoiceCryptocurrencyCreate(RedirectUrl, $"{InnerCallbackInvoices}{Id}", nameCoin, $"{amount}");
            var content = body.ToStringContent();
            var invoice = Tools.SendRequest<StringContent, Models.Invoice>(content, HttpMethod.Post, resUrl, AccessToken);
            return InvoiceResponse.ToConvert(invoice, CallbackUrl);
        }

        public InvoiceResponse InvoicesFiat(string? nameFiat, decimal? amount, string invoicesUrl)
        {
            if (string.IsNullOrEmpty(nameFiat) || amount == null)
                throw new Exception("Set correct values");

            var resUrl = $"{ApiUrl}{invoicesUrl}";
            var body = new InvoiceFiatCreate(RedirectUrl, $"{InnerCallbackInvoices}{Id}", nameFiat, $"{amount}");
            var content = body.ToStringContent();
            var invoice = Tools.SendRequest<StringContent, Models.Invoice>(content, HttpMethod.Post, resUrl, AccessToken);
            return InvoiceResponse.ToConvert(invoice, CallbackUrl);
        }

        public string? ApiUrl { get; set; }
        public string? InnerCallbackInvoices { get; set; }
        public string? AccessToken { get; set; }
        public string? RedirectUrl { get; set; }
        public string? CallbackUrl { get; set; }
        public string? Id { get; set; }
    }
}