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
using ApiProxy.Logic;
using System.Data;
using Serilog;

namespace PublicApi.Controllers
{
    [Route("{controller}"), Authorize(Roles = "AccessToken")]
    public class ApiController : Controller
    {
        private static string? _accessToken;
		private static System.Timers.Timer _timer;
        private static IConfiguration _appConfig = null;
        private static HttpClient _httpClient = new HttpClient();

        private Api _api;

        public ApiController(IConfiguration appConfig)
        {
            _appConfig = appConfig;
        }
		
		static ApiController()
		{
			_timer = new System.Timers.Timer(TimeSpan.FromMinutes(50));
			_timer.Elapsed += UpdateSettings;
			_timer.AutoReset = true;
            UpdateSettings(null, null);
			_timer.Start();
		}

        public static void UpdateSettings(object? sender, ElapsedEventArgs e)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, (_appConfig?["Api:RootUrl"] ?? "https://api.staging.pay2play.cash") + (_appConfig?["Api:GetAccessTokenUrl"] ?? "/v1/auth/access-token"));
            string refreshToken = _appConfig?["Api:RefreshToken"] ?? "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIxMjQxMWYyNS1kN2IzLTQ1ZTQtOGJhNC0yNmUyZGFkMzU3N2MiLCJpYXQiOjE2OTA5NzUxMTQsImlzcyI6ImNvcmUtaWQiLCJzdWIiOiJ1c2VyIiwidWlkIjoxNTcwNywidmVyIjoxLCJyZXMiOlsxXSwidHlwIjoxLCJzY29wZXMiOlsiYXV0aDphY3Rpb24iLCJjb3JlOnJlYWQiLCJleGNoYW5nZTphY3Rpb24iLCJleGNoYW5nZTpyZWFkIiwibWVyY2hhbnQ6YWN0aW9uIiwibWVyY2hhbnQ6cmVhZCIsIm5vdGlmaWNhdGlvbnM6YWN0aW9uIiwibm90aWZpY2F0aW9uczpyZWFkIiwicGF5b3V0czphY3Rpb24iLCJwYXlvdXRzOnJlYWQiLCJwcm9maWxlOmFjdGlvbiIsInByb2ZpbGU6cmVhZCIsIndhbGxldDphY3Rpb24iLCJ3YWxsZXQ6cmVhZCJdLCJpc18yZmFfZGlzYWJsZWQiOmZhbHNlLCJuYW1lIjoiTWF4QXBpVG9rZW4iLCJpc19kaXNhYmxlX29ubGluZSI6dHJ1ZX0.K0OxFBt4SgrAGCNlGrAQ2krbBtfr1eM45Ph_MsMcuOEzRu1fZHCCL9O59EpdMzHkU72pj3E8G9tWiTPblZFsEw";
            request.Headers.Add("Authorization", "Bearer " + refreshToken);
            using var response = _httpClient.Send(request);
            AccessTokenResponse? resJson = JsonConvert.DeserializeObject<AccessTokenResponse>(response.Content.ReadAsStringAsync().Result);
            if (resJson == null)
				Log.Fatal("From Api/UpdateSettings. Failed to get access token. Most likely, the appsettings.json file contains an incorrect refresh token. Immediately replace the refresh token value with the correct one");
            else
                _accessToken = resJson.AccessToken;
        }

        public void Init()
        {
            string? id = ((ClaimsIdentity)(HttpContext.User.Identity)).Name;
            _api = new Api
            (
                _appConfig?["Api:RootUrl"] ?? "https://api.staging.pay2play.cash",
                _appConfig?["Api:InnerCallbackInvoicesUrl"] ?? "https://MyDomain/Callback/1f8976d4-149e-4aa0-89aa-e766d89cfc7d/",
                _accessToken,
                id,
                _appConfig.GetConnectionString("MySql") ?? "server=localhost;user=root;password=!ZyXwV53412=;database=api-proxy;"
            );
        }

        [HttpGet("{action}")]
        public IActionResult Fiats()
        {
            Init();
            try
            {
                var res = _api.Fiats(_appConfig?["Api:FiatsUrl"] ?? "/v1/fiats");
                return Json(res);
            }
            catch (Exception exception)
            {
                if (exception.Message == "ErrorProcessingException")
                {
                    Log.Error($"From Api/Fiats. StatusCode: {(HttpStatusCode)exception.Data["StatusCode"]}; Message: {(string)exception.Data["MessageFromTheApiServer"]}");
                    return new CustomJsonResult(new { Message = (string)exception.Data["MessageForClient"] }, (HttpStatusCode)exception.Data["StatusCode"]);
                }
                else
                {
                    Log.Fatal($"From Api/Fiats. Message: {exception.Message}");
                    return StatusCode(500);
                }
            }
        }
        [HttpPost("{action}")]
        public IActionResult InvoicesCryptocurrency(string? nameCoin = null, decimal? amount = null)
        {
            Init();
            try
            {
                var res = _api.InvoicesCryptocurrency(nameCoin, amount, _appConfig?["Api:InvoicesUrl"] ?? "/v1/invoices");
                return Json(res);
            }
            catch (Exception exception)
            {
                if (exception.Message == "ErrorProcessingException")
                {
                    Log.Error($"From Api/InvoicesCryptocurrency. StatusCode: {(HttpStatusCode)exception.Data["StatusCode"]}; Message: {(string)exception.Data["MessageFromTheApiServer"]}");
                    return new CustomJsonResult(new { Message = (string)exception.Data["MessageForClient"] }, (HttpStatusCode)exception.Data["StatusCode"]);
                }
                else if (exception.Message == "Set correct values")
                    return new CustomJsonResult(new { Message = exception.Message }, HttpStatusCode.BadRequest);
                else
                {
                    Log.Fatal($"From Api/InvoicesCryptocurrency. Message: {exception.Message}");
                    return StatusCode(500);
                }
            }
        }
        [HttpPost("{action}")]
        public IActionResult InvoicesFiat(string? nameFiat = null, decimal? amount = null)
        {
            Init();
            try
            {
                var res = _api.InvoicesFiat(nameFiat, amount, _appConfig?["Api:InvoicesUrl"] ?? "/v1/invoices"); ;
                return Json(res);
            }
            catch (Exception exception)
            {
                if (exception.Message == "ErrorProcessingException")
                {
                    Log.Error($"From Api/Fiats. StatusCode: {(HttpStatusCode)exception.Data["StatusCode"]}; Message: {(string)exception.Data["MessageFromTheApiServer"]}");
                    return new CustomJsonResult(new { Message = (string)exception.Data["MessageForClient"] }, (HttpStatusCode)exception.Data["StatusCode"]);
                }
                else if (exception.Message == "Set correct values")
                    return new CustomJsonResult(new { Message = exception.Message }, HttpStatusCode.BadRequest);
                else
                {
                    Log.Fatal($"From Api/Fiats. Message: {exception.Message}");
                    return StatusCode(500);
                }
            }
        }
    }
}