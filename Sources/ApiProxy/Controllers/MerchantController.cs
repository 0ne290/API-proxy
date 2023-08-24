using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PublicApi.Models;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using static PublicApi.Models.DatabaseContext;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Drawing;
using System.Net;

namespace PublicApi.Controllers
{
    [Route("{controller}")]
    public class MerchantController : Controller
    {
        [HttpPost("{action}")]
        /// <summary>
        /// Регистрируем аккаунт мерчанта в системе и помещаем его в БД
        /// </summary>
        public IActionResult Registration([FromServices] MyDbContext db, string? login = null, string? password = null, string? redirectUrl = null, string? callbackUrl = null )
        {
            ApiProxy.Logic.Models.ResponseJson resJson = new ApiProxy.Logic.Models.ResponseJson();
            resJson.Response = "Request to merchant registration";
            resJson.Message = "Fail. Set correct values for required parameters";
            if (login == null || password == null || redirectUrl == null || callbackUrl == null)
                return new CustomJsonResult(resJson, HttpStatusCode.BadRequest);
            if (!Uri.IsWellFormedUriString(redirectUrl, UriKind.Absolute) || !Uri.IsWellFormedUriString(callbackUrl, UriKind.Absolute))
                return new CustomJsonResult(resJson, HttpStatusCode.BadRequest);

            resJson.Response = "Registration request";
            resJson.Message = "Fail. Enter another username or password";

            List<DatabaseContext.Merchant> merchants = db.Merchants.AsNoTracking().ToList();
            if (merchants != null)
                merchants = merchants.Where(m => Tools.ComputeSha256Hash(login + m.MerchantGuid + m.MerchantDate) == m.MerchantLogin || Tools.ComputeSha256Hash(password + m.MerchantGuid + m.MerchantDate) == m.MerchantPassword).ToList();
            
            if (merchants.Count > 0)
                return new CustomJsonResult(resJson, System.Net.HttpStatusCode.BadRequest);

            DatabaseContext.Merchant merchant = new DatabaseContext.Merchant() { MerchantLogin = login, MerchantPassword = password, MerchantRedirectUrl = redirectUrl, MerchantCallbackUrl = callbackUrl };
            db.Merchants.Add(merchant);
            db.SaveChanges();
            db.Dispose();

            resJson.Response = "Registration request";
            resJson.Message = "Completed";
            return new CustomJsonResult(resJson, System.Net.HttpStatusCode.OK);
        }
        [HttpPost("{action}")]
        /// <summary>
        /// Выдаем рефреш-токен мерчанта если логин-пароль верны
        /// </summary>
        public IActionResult GetRefreshToken([FromServices] MyDbContext db, string? login = null, string? password = null)
        {
            ApiProxy.Logic.Models.ResponseJson resJson = new ApiProxy.Logic.Models.ResponseJson();
            resJson.Response = "Request to get merchant refresh token";
            resJson.Message = "Fail. Set correct values for required parameters";
            if (login == null || password == null)
                return new CustomJsonResult(resJson, HttpStatusCode.BadRequest);

            DatabaseContext.Merchant? merchant = Tools.FindMerchant(db, login, password);
            db.Dispose();

            resJson.Response = "Merchant get refresh token request";
            resJson.Message = "Fail. Wrong login or password";
            if (merchant == null)
                return new CustomJsonResult(resJson, System.Net.HttpStatusCode.BadRequest);

            List<Claim> claims = new List<Claim> { new Claim("MerchantGuid", merchant.MerchantGuid), new Claim("IdentityRole", "RefreshToken") };
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(60)),
                signingCredentials: new SigningCredentials(AuthOptions.KEY, SecurityAlgorithms.HmacSha256)
            );

            resJson.Response = new JwtSecurityTokenHandler().WriteToken(jwt);
            resJson.Message = "Your refresh token";
            return new CustomJsonResult(resJson, System.Net.HttpStatusCode.OK);
        }
        [HttpGet("{action}"), Authorize(Roles = "RefreshToken")]
        /// <summary>
        /// Выдаем аксесс-токен мерчанта если логин-пароль верны
        /// </summary>
        public IActionResult GetAccessToken([FromServices] MyDbContext db)
        {
            ClaimsIdentity merchantIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            DatabaseContext.Merchant? merchant = db.Merchants.Find(merchantIdentity.Name);
            db.Dispose();

            ApiProxy.Logic.Models.ResponseJson resJson = new ApiProxy.Logic.Models.ResponseJson();
            resJson.Response = "Merchant get access token request";
            resJson.Message = "Fail. Refresh token not found";
            if (merchant == null)
                return new CustomJsonResult(resJson, System.Net.HttpStatusCode.Unauthorized);

            List<Claim> claims = new List<Claim> { new Claim("MerchantGuid", merchant.MerchantGuid), new Claim("IdentityRole", "AccessToken") };
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)),
                signingCredentials: new SigningCredentials(AuthOptions.KEY, SecurityAlgorithms.HmacSha256)
            );

            resJson.Response = new JwtSecurityTokenHandler().WriteToken(jwt);
            resJson.Message = "Your access token";
            return new CustomJsonResult(resJson, System.Net.HttpStatusCode.OK);
        }
        [HttpPost("{action}"), Authorize(Roles = "RefreshToken")]
        /// <summary>
        /// Устанавливаем редирект мерчанта
        /// </summary>
        public IActionResult SetRedirect([FromServices] MyDbContext db, string? redirectUrl = null)
        {
            ApiProxy.Logic.Models.ResponseJson resJson = new ApiProxy.Logic.Models.ResponseJson();
            resJson.Response = "Request to set merchant redirect URL";
            resJson.Message = "Fail. Set correct values for required parameters";
            if (redirectUrl == null)
                return new CustomJsonResult(resJson, HttpStatusCode.BadRequest);
            if (!Uri.IsWellFormedUriString(redirectUrl, UriKind.Absolute))
                return new CustomJsonResult(resJson, HttpStatusCode.BadRequest);

            ClaimsIdentity merchantIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            DatabaseContext.Merchant? merchant = db.Merchants.Find(merchantIdentity.Name);

            resJson.Response = "Merchant set redirect request";
            resJson.Message = "Fail. Refresh token not found";
            if (merchant == null)
                return new CustomJsonResult(resJson, System.Net.HttpStatusCode.Unauthorized);

            merchant.MerchantRedirectUrl = redirectUrl;
            db.SaveChanges();
            db.Dispose();
            resJson.Message = "Complete";
            return new CustomJsonResult(resJson, System.Net.HttpStatusCode.OK);
        }
        [HttpPost("{action}"), Authorize(Roles = "RefreshToken")]
        /// <summary>
        /// Устанавливаем редирект мерчанта
        /// </summary>
        public IActionResult SetCallback([FromServices] MyDbContext db, string? callbackUrl = null)
        {
            ApiProxy.Logic.Models.ResponseJson resJson = new ApiProxy.Logic.Models.ResponseJson();
            resJson.Response = "Request to set merchant callback URL";
            resJson.Message = "Fail. Set correct values for required parameters";
            if (callbackUrl == null)
                return new CustomJsonResult(resJson, HttpStatusCode.BadRequest);
            if (!Uri.IsWellFormedUriString(callbackUrl, UriKind.Absolute))
                return new CustomJsonResult(resJson, HttpStatusCode.BadRequest);

            ClaimsIdentity merchantIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            DatabaseContext.Merchant? merchant = db.Merchants.Find(merchantIdentity.Name);

            resJson.Response = "Merchant set callback request";
            resJson.Message = "Fail. Refresh token not found";
            if (merchant == null)
                return new CustomJsonResult(resJson, System.Net.HttpStatusCode.Unauthorized);

            merchant.MerchantCallbackUrl = callbackUrl;
            db.SaveChanges();
            db.Dispose();
            resJson.Message = "Complete";
            return new CustomJsonResult(resJson, System.Net.HttpStatusCode.OK);
        }
    }
}
