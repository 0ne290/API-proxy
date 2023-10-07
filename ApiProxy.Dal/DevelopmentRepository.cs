using System.Security.Cryptography;
using System.Text;
using ApiProxy.Logic.Boundaries;
using ApiProxy.Logic.Dto.Repository;

namespace ApiProxy.Dal
{
    public class DevelopmentRepository : IRepository
    {
		private static List<Merchant> _merchants = new List<Merchant>()
		{
			new Merchant("e7e72fbc-62b0-11ee-8c99-0242ac120002", (new DateTime(2023, 10, 4)).ToString(), "One290", "Amogus1337", "https://Test-site.com/Redirect-url", "https://Test-site.com/Callback-url"),
			new Merchant("5c34570a-62b1-11ee-8c99-0242ac120002", (new DateTime(2022, 4, 9)).ToString(), "Vasily", "1337Abobus", "https://Super-test-site.com/Super-redirect-url", "https://Super-test-site.com/Super-callback-url")
		};
		private static List<Invoice> _invoices = new List<Invoice>()
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
			return _merchants.Find(m => m.Guid == guid);
		}
		public Merchant? FindMerchant(string? login, string? password)
		{
			return _merchants.Find(m => ComputeSha256Hash(login + m.Guid + m.Date) == m.Login && ComputeSha256Hash(password + m.Guid + m.Date) == m.Password);
		}
		public bool ValidateLoginAndPassword(string? login, string? password)
		{
			Merchant? merchant = _merchants.Find(m => ComputeSha256Hash(login + m.Guid + m.Date) == m.Login || ComputeSha256Hash(password + m.Guid + m.Date) == m.Password);
			if (merchant == null)
				return true;
			else
				return false;
		}
		
		public void AddInvoice(Invoice invoice)
		{
			_invoices.Add(invoice);
		}
		public void AddMerchant(Merchant merchant)
		{
			_merchants.Add(merchant);
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