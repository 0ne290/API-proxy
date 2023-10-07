using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiProxy.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiProxy.UI.Controllers
{
    [Route("{controller}")]
    public class AccountingController : Controller
    {
		private Accounting _accounting;
		
		public AccountingController()
        {
            _accounting = ServiceLocator.Current.Resolve<Accounting>();
        }

        [HttpPost("{action}")]
        public IActionResult Registration(string? login = null, string? password = null, string? redirectUrl = null, string? callbackUrl = null )
        {
            try
            {
                _accounting.Registration(login, password, redirectUrl, callbackUrl);
                return Ok();
            }
            catch (Exception exception)
            {
                Tools.ErrorProcessing(exception);
				return BadRequest();
            }
        }
        [HttpPost("{action}")]
        public IActionResult GetRefreshToken(string? login = null, string? password = null)
        {
            try
            {
                var merchant = _accounting.GetMerchant(login, password);

                List<Claim> claims = new List<Claim> { new Claim("MerchantGuid", merchant.Guid), new Claim("IdentityRole", "RefreshToken") };
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
                Tools.ErrorProcessing(exception);
				return BadRequest();
            }
        }
        [HttpGet("{action}"), Authorize(Roles = "RefreshToken")]
        public IActionResult GetAccessToken()
        {
            try
            {
                ClaimsIdentity merchantIdentity = (ClaimsIdentity)HttpContext.User.Identity;
                var merchant = _accounting.GetMerchant(merchantIdentity.Name);

                List<Claim> claims = new List<Claim> { new Claim("MerchantGuid", merchant.Guid), new Claim("IdentityRole", "AccessToken") };
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
                Tools.ErrorProcessing(exception);
				return BadRequest();
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
                Tools.ErrorProcessing(exception);
				return BadRequest();
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
                Tools.ErrorProcessing(exception);
				return BadRequest();
            }
        }
    }
}
