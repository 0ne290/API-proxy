using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiProxy.Logic.Database;
using ApiProxy.Logic.Models;

namespace ApiProxy.Logic
{
    public class Callback
    {
        private static HttpClient _httpClient = new HttpClient();
        private MyDbContext db;
        public Callback(string? connectionString)
        {
            MyDbContext db = MyDbContext.GetMyDbContext(connectionString);
        }
        public void Invoices(string? id, InvoiceCallback? invoiceCallback)
        {
            Database.Invoice invoice = new Database.Invoice() { InvoiceId = invoiceCallback.Id, MerchantGuid = id, InvoiceStatus = invoiceCallback.Status, InvoiceFiat = invoiceCallback.Fiat, InvoiceFiatAmount = invoiceCallback.FiatAmount };
            db.Invoices.Add(invoice);
            db.SaveChanges();
            Database.Merchant merchant = db.Merchants.Find(id);
            db.Dispose();
            Thread callback = new Thread(new ParameterizedThreadStart(Tools.SendCallback<InvoiceCallback>));
            callback.IsBackground = false;
            callback.Start(new object[3] { invoiceCallback, merchant.MerchantCallbackUrl, _httpClient });
        }
    }
}