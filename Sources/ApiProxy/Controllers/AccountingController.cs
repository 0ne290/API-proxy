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
using ApiProxy.Logic;
using Serilog;

namespace PublicApi.Controllers
{
    [Route("{controller}")]
    public class AccountingController : Controller
    {
        public AccountingController(IConfiguration appConfig)
        {
            _accounting = new Accounting(appConfig.GetConnectionString("MySql") ?? "server=localhost;user=root;password=!ZyXwV53412=;database=api-proxy;");
        }

        [HttpPost("{action}")]
        public IActionResult Registration([FromServices] MyDbContext db, string? login = null, string? password = null, string? redirectUrl = null, string? callbackUrl = null )
        {
            try
            {
                _accounting.Registration(login, password, redirectUrl, callbackUrl);

                return Ok();
            }
            catch (Exception exception)
            {
                if (exception.Message == "Set correct values for required parameters" || exception.Message == "Enter another username or password")
                    return new CustomJsonResult(new { Message = exception.Message }, HttpStatusCode.BadRequest);
                else
                {
                    Log.Fatal($"From Accounting/Registration. Message: {exception.Message}");
                    return StatusCode(500);
                }
            }
        }
        [HttpPost("{action}")]
        public IActionResult GetRefreshToken(string? login = null, string? password = null)
        {
            try
            {
                ApiProxy.Logic.Database.Merchant merchant = _accounting.GetMerchant(login, password);

                List<Claim> claims = new List<Claim> { new Claim("MerchantGuid", merchant.MerchantGuid), new Claim("IdentityRole", "RefreshToken") };
                JwtSecurityToken jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromDays(60)),
                    signingCredentials: new SigningCredentials(AuthOptions.KEY, SecurityAlgorithms.HmacSha256)
                );

                return Json(new { RefreshToken = new JwtSecurityTokenHandler().WriteToken(jwt) });
            }
            catch (Exception exception)
            {
                if (exception.Message == "Set correct values for required parameters" || exception.Message == "Wrong login or password")
                    return new CustomJsonResult(new { Message = exception.Message }, HttpStatusCode.BadRequest);
                else
                {
                    Log.Fatal($"From Accounting/GetRefreshToken. Message: {exception.Message}");
                    return StatusCode(500);
                }
            }
        }
        [HttpGet("{action}"), Authorize(Roles = "RefreshToken")]
        public IActionResult GetAccessToken([FromServices] MyDbContext db)
        {
            try
            {
                ClaimsIdentity merchantIdentity = (ClaimsIdentity)HttpContext.User.Identity;
                ApiProxy.Logic.Database.Merchant merchant = _accounting.GetMerchant(merchantIdentity.Name);

                List<Claim> claims = new List<Claim> { new Claim("MerchantGuid", merchant.MerchantGuid), new Claim("IdentityRole", "AccessToken") };
                JwtSecurityToken jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)),
                    signingCredentials: new SigningCredentials(AuthOptions.KEY, SecurityAlgorithms.HmacSha256)
                );

                return Json(new { AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt) });
            }
            catch (Exception exception)
            {
                if (exception.Message == "Account not found. It may have been deleted or its ID has been changed")
                    return new CustomJsonResult(new { Message = exception.Message }, HttpStatusCode.Unauthorized);
                else
                {
                    Log.Fatal($"From Accounting/GetAccessToken. Message: {exception.Message}");
                    return StatusCode(500);
                }
            }
        }
        [HttpPost("{action}"), Authorize(Roles = "RefreshToken")]
        public IActionResult SetRedirect(string? redirectUrl = null)
        {
            try
            {
                ClaimsIdentity merchantIdentity = (ClaimsIdentity)HttpContext.User.Identity;
                _accounting.SetRedirect(redirectUrl, merchantIdentity.Name);

                return Ok();
            }
            catch (Exception exception)
            {
                if (exception.Message == "Account not found. It may have been deleted or its ID has been changed")
                    return new CustomJsonResult(new { Message = exception.Message }, HttpStatusCode.Unauthorized);
                else if (exception.Message == "Set correct values for required parameters")
                    return new CustomJsonResult(new { Message = exception.Message }, HttpStatusCode.BadRequest);
                else
                {
                    Log.Fatal($"From Accounting/SetRedirect. Message: {exception.Message}");
                    return StatusCode(500);
                }
            }
        }
        [HttpPost("{action}"), Authorize(Roles = "RefreshToken")]
        public IActionResult SetCallback(string? callbackUrl = null)
        {
            try
            {
                ClaimsIdentity merchantIdentity = (ClaimsIdentity)HttpContext.User.Identity;
                _accounting.SetCallback(callbackUrl, merchantIdentity.Name);

                return Ok();
            }
            catch (Exception exception)
            {
                if (exception.Message == "Account not found. It may have been deleted or its ID has been changed")
                    return new CustomJsonResult(new { Message = exception.Message }, HttpStatusCode.Unauthorized);
                else if (exception.Message == "Set correct values for required parameters")
                    return new CustomJsonResult(new { Message = exception.Message }, HttpStatusCode.BadRequest);
                else
                {
                    Log.Fatal($"From Accounting/SetCallback. Message: {exception.Message}");
                    return StatusCode(500);
                }
            }
        }

        private Accounting _accounting;
    }
}
