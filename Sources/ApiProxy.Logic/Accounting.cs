using ApiProxy.Logic.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Data;
using System.Net.Http;
using Serilog;

namespace ApiProxy.Logic
{
    public class Accounting
    {
        public Accounting(string connectionString)
        {
            _db = MyDbContext.GetMyDbContext(connectionString);
        }

        public void Registration(string? login = null, string? password = null, string? redirectUrl = null, string? callbackUrl = null)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(redirectUrl) || string.IsNullOrEmpty(callbackUrl))
                throw new Exception("Set correct values for required parameters");
            if (!Uri.IsWellFormedUriString(redirectUrl, UriKind.Absolute) || !Uri.IsWellFormedUriString(callbackUrl, UriKind.Absolute))
                throw new Exception("Set correct values for required parameters");

            List<Database.Merchant> merchants = _db.Merchants.AsNoTracking().ToList();
            if (merchants != null)
                merchants = merchants.Where(m => Tools.ComputeSha256Hash(login + m.MerchantGuid + m.MerchantDate) == m.MerchantLogin || Tools.ComputeSha256Hash(password + m.MerchantGuid + m.MerchantDate) == m.MerchantPassword).ToList();

            if (merchants.Count > 0)
                throw new Exception("Enter another username or password");

            Database.Merchant merchant = new Database.Merchant() { MerchantLogin = login, MerchantPassword = password, MerchantRedirectUrl = redirectUrl, MerchantCallbackUrl = callbackUrl };
            _db.Merchants.Add(merchant);
            _db.SaveChanges();
            _db.Dispose();
        }
        public Database.Merchant GetMerchant(string? login = null, string? password = null)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                throw new Exception("Set correct values for required parameters");

            Database.Merchant? merchant = Tools.FindMerchant(_db, login, password);
            _db.Dispose();

            if (merchant == null)
                throw new Exception("Wrong login or password");

            return merchant;
        }
        public Database.Merchant GetMerchant(string id)
        {
            Database.Merchant? merchant = _db.Merchants.Find(id);
            _db.Dispose();

            if (merchant == null)
                throw new Exception("Account not found. It may have been deleted or its ID has been changed");

            return merchant;
        }
        public void SetRedirect(string? redirectUrl, string id)
        {
            if (string.IsNullOrEmpty(redirectUrl))
                throw new Exception("Set correct values for required parameters");
            if (!Uri.IsWellFormedUriString(redirectUrl, UriKind.Absolute))
                throw new Exception("Set correct values for required parameters");

            Database.Merchant? merchant = _db.Merchants.Find(id);

            if (merchant == null)
                throw new Exception("Account not found. It may have been deleted or its ID has been changed");

            merchant.MerchantRedirectUrl = redirectUrl;
            _db.SaveChanges();
            _db.Dispose();
        }
        public void SetCallback(string? callbackUrl, string id)
        {
            if (string.IsNullOrEmpty(callbackUrl))
                throw new Exception("Set correct values for required parameters");
            if (!Uri.IsWellFormedUriString(callbackUrl, UriKind.Absolute))
                throw new Exception("Set correct values for required parameters");

            Database.Merchant? merchant = _db.Merchants.Find(id);

            if (merchant == null)
                throw new Exception("Account not found. It may have been deleted or its ID has been changed");

            merchant.MerchantCallbackUrl = callbackUrl;
            _db.SaveChanges();
            _db.Dispose();
        }

        private MyDbContext _db;
    }
}
