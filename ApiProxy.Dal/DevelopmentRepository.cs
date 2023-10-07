using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using ApiProxy.Logic.Boundaries;
using ApiProxy.Logic.Dto.Repository;

namespace ApiProxy.Dal
{
    public class DevelopmentRepository : IRepository
    {
		private static readonly List<Merchant> Merchants = new()
		{
			new Merchant("e7e72fbc-62b0-11ee-8c99-0242ac120002", $"{new DateTime(2023, 10, 4)}", "One290", "Amogus1337", "https://Test-site.com/Redirect-url", "https://Test-site.com/Callback-url"),
			new Merchant("5c34570a-62b1-11ee-8c99-0242ac120002", $"{new DateTime(2022, 4, 9)}", "Vasily", "1337Abobus", "https://Super-test-site.com/Super-redirect-url", "https://Super-test-site.com/Super-callback-url")
		};
		private static readonly List<Invoice> Invoices = new()
		{
			new Invoice(1, "e7e72fbc-62b0-11ee-8c99-0242ac120002", "COMPLETED", "usdt", 3.75),
			new Invoice(2, "5c34570a-62b1-11ee-8c99-0242ac120002", "FAIL", "rub", 1.5)
		};
		
		public string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
		
		public Merchant? FindMerchant(string? guid)
        {
			return Merchants.Find(m => m.Guid == guid);
		}
		public Merchant? FindMerchant(string? login, string? password)
		{
			return Merchants.Find(m => ComputeSha256Hash(login + m.Guid + m.Date) == m.Login && ComputeSha256Hash(password + m.Guid + m.Date) == m.Password);
		}
		public bool ValidateLoginAndPassword(string? login, string? password)
        {
            var merchant = Merchants.Find(m => ComputeSha256Hash(login + m.Guid + m.Date) == m.Login || ComputeSha256Hash(password + m.Guid + m.Date) == m.Password);
            return merchant == null;
        }
		
		public void AddInvoice(Invoice invoice)
		{
			Invoices.Add(invoice);
		}
		public void AddMerchant(Merchant merchant)
		{
			Merchants.Add(merchant);
		}
		
		public void SetRedirectUrl(string? id, string? redirectUrl)
		{
			FindMerchant(id).RedirectUrl = redirectUrl;
		}
		public void SetCallbackUrl(string? id, string? callbackUrl)
		{
			FindMerchant(id).CallbackUrl = callbackUrl;
		}
    }
}