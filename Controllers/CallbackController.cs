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

namespace PublicApi.Controllers
{
    [Route("{controller}")]
    public class CallbackController : Controller
    {
        private static HttpClient _httpClient = new HttpClient();
        private MyDbContext _db;
        public CallbackController(MyDbContext db)
        {
            _db = db;
        }
        [HttpPost("1f8976d4-149e-4aa0-89aa-e766d89cfc7d/{id}")]
        /// <summary>
        /// Принимаем коллбэки от Pay2Play и пересылаем их нашим мерчантам
        /// </summary>
        public IActionResult Invoices(string id)
        {
            InvoiceCallback? invoiceCallback;
            try
            {
                invoiceCallback = HttpContext.Request.ReadFromJsonAsync<InvoiceCallback>().Result;
            }
            catch
            {
                invoiceCallback = null;
            }
            if (invoiceCallback == null)
            {
                Console.WriteLine("!--FATAL_ERROR--   The ReadFromJsonAsync() method cannot deserialize the response body into an object of type InvoiceCallback. Most likely the Content-Type header is not set to application/json. Investigate the problem and find an alternative way to deserialize that doesn't depend on the header, or find and fix the error in the code if the header is correct (in this case, the format of the incoming JSON probably does not match the model)   --FATAL_ERROR--!");
                _db.Dispose();
                return Ok();
            }
            else
            {
                DatabaseContext.Invoice invoice = new DatabaseContext.Invoice() { InvoiceId = invoiceCallback.Id, MerchantGuid = id, InvoiceStatus = invoiceCallback.Status, InvoiceFiat = invoiceCallback.Fiat, InvoiceFiatAmount = invoiceCallback.FiatAmount };
                _db.Invoices.Add(invoice);
                _db.SaveChanges();
                DatabaseContext.Merchant merchant = _db.Merchants.Find(id);
                _db.Dispose();
                OkResult res = new OkResult();
                Thread callback = new Thread(new ParameterizedThreadStart(Tools.SendCallback<InvoiceCallback>));
                callback.IsBackground = false;
                callback.Start(new object[3] { invoiceCallback, merchant.MerchantCallbackUrl, _httpClient });
                return res;
            }
        }
    }
}
