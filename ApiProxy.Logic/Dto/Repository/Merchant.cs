using System.Security.Cryptography;
using System.Text;

namespace ApiProxy.Logic.Dto.Repository
{
    public class Merchant
    {
        private string? _login, _password;
		
        public string? Guid { get; private set; }
        public string? Date { get; private set; }
        public string? Login
        {
            get
            {
                return _login;
            }
            set
            {
                _login = ComputeSha256Hash(value + Guid + Date);
            }
        }
        public string? Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = ComputeSha256Hash(value + Guid + Date);
            }
        }
        public string? RedirectUrl { get; set; }
        public string? CallbackUrl { get; set; }

        public Merchant(string? guid, string? date, string? login, string? password, string? redirectUrl, string? callbackUrl)
        {
            Guid = guid;
            Date = date;
            Login = login;
            Password = password;
            RedirectUrl = redirectUrl;
            CallbackUrl = callbackUrl;
        }

        public Merchant()
        {
            Guid = System.Guid.NewGuid().ToString();
            Date = DateTime.UtcNow.ToString();
        }

        private static string ComputeSha256Hash(string rawData)
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
    }
}
