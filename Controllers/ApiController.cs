using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PublicApi.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using System.Drawing;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using System.Timers;
using Microsoft.Extensions.Configuration.Json;
using System.Text;

namespace PublicApi.Controllers
{
    [Route("{controller}"), Authorize(Roles = "AccessToken")]
    public class ApiController : Controller
    {
        private const string _apiUrl = "https://api.staging.pay2play.cash";
        private const string _innerCallbackInvoices = "https://MyDomain/Callback/1f8976d4-149e-4aa0-89aa-e766d89cfc7d/";

        private static string? _refreshToken;
        private static string? _accessToken;
		private static System.Timers.Timer _timer;
        private static IConfiguration _appConfig = null;
        private static HttpClient _httpClient = new HttpClient();
        private static object _locker = new object();

        private string? _redirectUrl;
        private string? _callbackUrl;
        private string? _id;
        private DatabaseContext.MyDbContext _db;

        public ApiController(IConfiguration appConfig, DatabaseContext.MyDbContext db)
        {
            _appConfig = appConfig;
            _db = db;
        }
		
		static ApiController()
		{
			_timer = new System.Timers.Timer(TimeSpan.FromMinutes(50));
			_timer.Elapsed += RefreshAccessToken;
			_timer.AutoReset = true;
            RefreshAccessToken(null, null);
			_timer.Start();
		}

        public static void RefreshAccessToken(object? sender, ElapsedEventArgs e)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, _apiUrl + "/v1/auth/access-token");
            _refreshToken = _appConfig?["RefreshToken"] ?? "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIxMjQxMWYyNS1kN2IzLTQ1ZTQtOGJhNC0yNmUyZGFkMzU3N2MiLCJpYXQiOjE2OTA5NzUxMTQsImlzcyI6ImNvcmUtaWQiLCJzdWIiOiJ1c2VyIiwidWlkIjoxNTcwNywidmVyIjoxLCJyZXMiOlsxXSwidHlwIjoxLCJzY29wZXMiOlsiYXV0aDphY3Rpb24iLCJjb3JlOnJlYWQiLCJleGNoYW5nZTphY3Rpb24iLCJleGNoYW5nZTpyZWFkIiwibWVyY2hhbnQ6YWN0aW9uIiwibWVyY2hhbnQ6cmVhZCIsIm5vdGlmaWNhdGlvbnM6YWN0aW9uIiwibm90aWZpY2F0aW9uczpyZWFkIiwicGF5b3V0czphY3Rpb24iLCJwYXlvdXRzOnJlYWQiLCJwcm9maWxlOmFjdGlvbiIsInByb2ZpbGU6cmVhZCIsIndhbGxldDphY3Rpb24iLCJ3YWxsZXQ6cmVhZCJdLCJpc18yZmFfZGlzYWJsZWQiOmZhbHNlLCJuYW1lIjoiTWF4QXBpVG9rZW4iLCJpc19kaXNhYmxlX29ubGluZSI6dHJ1ZX0.K0OxFBt4SgrAGCNlGrAQ2krbBtfr1eM45Ph_MsMcuOEzRu1fZHCCL9O59EpdMzHkU72pj3E8G9tWiTPblZFsEw";
            request.Headers.Add("Authorization", "Bearer " + _refreshToken);
            using var response = _httpClient.Send(request);
            AccessTokenResponse? resJson = JsonConvert.DeserializeObject<AccessTokenResponse>(response.Content.ReadAsStringAsync().Result);
            if (resJson == null)
            {
                lock (_locker)
                {
                    Console.WriteLine("!--FATAL ERROR--   Failed to get access token. Most likely, the appsettings.json file contains an incorrect refresh token. Immediately replace the refresh token value with the correct one!!!   --FATAL ERROR--!");
                }
            }
            else
                _accessToken = resJson.AccessToken;
        }

        public void Init()
        {
            _id = ((ClaimsIdentity)(HttpContext.User.Identity)).Name;
            DatabaseContext.Merchant? merchant = _db.Merchants.Find(_id);
            _db.Dispose();
            _redirectUrl = merchant.MerchantRedirectUrl;
            _callbackUrl = merchant.MerchantCallbackUrl;
        }

        [HttpGet("{action}")]
        public IActionResult Fiats()
        {
            Init();
            HttpStatusCode code;
            string error;
            string resUrl = _apiUrl + "/v1/fiats";
            List<Fiat>? res = Tools.SendRequest<List<Fiat>>(_httpClient, HttpMethod.Get, resUrl, out error, _accessToken, out code);
            ResponseJson resJson = new ResponseJson();
            resJson.Response = res;
            resJson.Message = error;
            return new CustomJsonResult(resJson, code);
        }
        [HttpPost("{action}")]
        /// <summary>
        /// Формируем счет на оплату в крипте
        /// </summary>
        public IActionResult InvoicesCryptocurrency(string? pCoin = null, int? pAmount = null)
        {
            ResponseJson resJson = new ResponseJson();
            resJson.Response = "Request to create an invoice in cryptocurrency";
            resJson.Message = "Fail. Set correct values for required parameters";
            if (pCoin == null || pAmount == null)
                return new CustomJsonResult(resJson, HttpStatusCode.BadRequest);
            Init();
            HttpStatusCode code;
            string error;
            string resUrl = _apiUrl + "/v1/invoices";
            InvoiceCryptocurrencyCreate body = new InvoiceCryptocurrencyCreate() { RedirectUrl = _redirectUrl, CallbackUrl = _innerCallbackInvoices + _id, Coin = pCoin, Amount = pAmount.ToString() };
            Invoice? res = Tools.SendRequest<StringContent, Invoice>(_httpClient, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"), HttpMethod.Post, resUrl, out error, _accessToken, out code);
            resJson.Response = (InvoiceResponse)res;
            ((InvoiceResponse)resJson.Response).CallbackUrl = _callbackUrl;
            resJson.Message = error;
            return new CustomJsonResult(resJson, code);
        }
        [HttpPost("{action}")]
        /// <summary>
        /// Формируем счет на оплату в фиате
        /// </summary>
        public IActionResult InvoicesFiat(string? pFiat = null, int? pAmount = null)
        {
            ResponseJson resJson = new ResponseJson();
            resJson.Response = "Request to create an invoice in fiat";
            resJson.Message = "Fail. Set correct values for required parameters";
            if (pFiat == null || pAmount == null)
                return new CustomJsonResult(resJson, HttpStatusCode.BadRequest);
            Init();
            HttpStatusCode code;
            string error;
            string resUrl = _apiUrl + "/v1/invoices";
            InvoiceFiatCreate body = new InvoiceFiatCreate() { RedirectUrl = _redirectUrl, CallbackUrl = _innerCallbackInvoices + _id, Fiat = pFiat, FiatAmount = pAmount.ToString() };
            Invoice? res = Tools.SendRequest<StringContent, Invoice>(_httpClient, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"), HttpMethod.Post, resUrl, out error, _accessToken, out code);
            resJson.Response = (InvoiceResponse)res;
            ((InvoiceResponse)resJson.Response).CallbackUrl = _callbackUrl;
            resJson.Message = error;
            return new CustomJsonResult(resJson, code);
        }
    }
}