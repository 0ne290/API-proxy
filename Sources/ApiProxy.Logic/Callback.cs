using System.Net;
using System.Text;
using ApiProxy.Logic.Database;
using ApiProxy.Logic.Models;
using Newtonsoft.Json;

namespace ApiProxy.Logic
{
    public class Callback
    {
        private static HttpClient _httpClient = new HttpClient();
        private MyDbContext db;
        public Callback(string connectionString)
        {
            db = MyDbContext.GetMyDbContext(connectionString);
        }
        public void Invoices(string id, InvoiceCallback invoiceCallback)
        {
            Database.Invoice invoice = new Database.Invoice
            {
                InvoiceId = invoiceCallback.Id, MerchantGuid = id, InvoiceStatus = invoiceCallback.Status,
                InvoiceFiat = invoiceCallback.Fiat, InvoiceFiatAmount = invoiceCallback.FiatAmount
            };

            db.Invoices.Add(invoice);
            db.SaveChanges();
            var merchant = db.Merchants.Find(id);
            db.Dispose();

            var callback = new Thread(SendCallback)
            {
                IsBackground = false
            };

            callback.Start(new object[3] { invoiceCallback, merchant.MerchantCallbackUrl, _httpClient });
        }

        public void SendCallback(object? args)
        {
            object[] argList = (object[])args;
            HttpClient httpClient = (HttpClient)argList[2];
            int i = 0;
            using var request = new HttpRequestMessage(HttpMethod.Post, (string)argList[1]);
            request.Headers.Remove("Accept");
            request.Headers.Add("Accept", "application/json");
            request.Content = new StringContent(JsonConvert.SerializeObject((InvoiceCallback)argList[0]), Encoding.UTF8, "application/json");
            var response = httpClient.Send(request);
            while (i < 12 && response.StatusCode != HttpStatusCode.OK)
            {
                response.Dispose();
                Thread.Sleep(300000);
                i++;
                response = httpClient.Send(request);
            }
            response.Dispose();
        }
    }
}