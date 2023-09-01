using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PublicApi.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using System.Net.Http;
using static PublicApi.Models.DatabaseContext;
using System;
using Serilog;
using ApiProxy.Logic;

namespace PublicApi.Controllers
{
    [Route("{controller}")]
    public class CallbackController : Controller
    {
        private static HttpClient _httpClient = new HttpClient();

        [HttpPost("1f8976d4-149e-4aa0-89aa-e766d89cfc7d/{id}")]
        public IActionResult Invoices([FromServices] IConfiguration appConfig, string id)
        {
            try
            {
                ApiProxy.Logic.Models.InvoiceCallback? invoiceCallback = HttpContext.Request.ReadFromJsonAsync<ApiProxy.Logic.Models.InvoiceCallback>().Result;
                if (invoiceCallback == null )
                {
                    Log.Fatal("From Callback/Invoices. The ReadFromJsonAsync() method cannot deserialize the response body into an object of type InvoiceCallback. Most likely the Content-Type header is not set to application/json. Investigate the problem and find an alternative way to deserialize that doesn't depend on the header, or find and fix the error in the code if the header is correct (in this case, the format of the incoming JSON probably does not match the model)");
                    return Ok();
                }
                var callback = new Callback(appConfig.GetConnectionString("MySql") ?? "server=localhost;user=root;password=!ZyXwV53412=;database=api-proxy;");
                callback.Invoices(id, invoiceCallback);
                return Ok();
            }
            catch
            {
                Log.Fatal("From Callback/Invoices. The ReadFromJsonAsync() method cannot deserialize the response body into an object of type InvoiceCallback. Most likely the Content-Type header is not set to application/json. Investigate the problem and find an alternative way to deserialize that doesn't depend on the header, or find and fix the error in the code if the header is correct (in this case, the format of the incoming JSON probably does not match the model)");
                return Ok();
            }
        }
    }
}
