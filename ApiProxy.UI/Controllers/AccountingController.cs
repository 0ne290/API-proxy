using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiProxy.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiProxy.UI.Controllers;

[Route("{controller}")]
public class AccountingController : Controller
{
		
    public AccountingController(IServiceLocator serviceLocator)
    {
        ServiceLocator = serviceLocator;
    }

    [HttpPost("{action}")]
    public IActionResult Registration(string? login = null, string? password = null, string? redirectUrl = null, string? callbackUrl = null )
    {
        try
        {
            var accounting = ServiceLocator.Resolve<Accounting>();
            accounting.Registration(login, password, redirectUrl, callbackUrl);
            return Ok();
        }
        catch (Exception exception)
        {
            var tools = ServiceLocator.Resolve<ITools>();
            tools.ErrorProcessing(exception);
            return BadRequest();
        }
    }

    [HttpPost("{action}")]
    public IActionResult GetRefreshToken(string? login = null, string? password = null)
    {
        try
        {
            var jwtCreator = ServiceLocator.Resolve<JwtCreator>();
            var accounting = ServiceLocator.Resolve<Accounting>();
            
            var merchant = accounting.GetMerchant(login, password);
            var token = jwtCreator.Create(merchant.Guid);


            return Json(new { RefreshToken = new JwtSecurityTokenHandler().WriteToken(jwt) });
        }
        catch (Exception exception)
        {
            var tools = ServiceLocator.Resolve<ITools>();
            tools.ErrorProcessing(exception);
            return BadRequest();
        }
    }

    [HttpGet("{action}"), Authorize(Roles = "RefreshToken")]
    public IActionResult GetAccessToken()
    {
        try
        {
            var accounting = ServiceLocator.Resolve<Accounting>();
            ClaimsIdentity merchantIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            var merchant = accounting.GetMerchant(merchantIdentity.Name);

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
            var tools = ServiceLocator.Resolve<ITools>();
            tools.ErrorProcessing(exception);
            return BadRequest();
        }
    }
    [HttpPost("{action}"), Authorize(Roles = "RefreshToken")]
    public IActionResult SetRedirect(string? redirectUrl = null)
    {
        try
        {
            var accounting = ServiceLocator.Resolve<Accounting>();
            ClaimsIdentity merchantIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            accounting.SetRedirect(redirectUrl, merchantIdentity.Name);

            return Ok();
        }
        catch (Exception exception)
        {
            var tools = ServiceLocator.Resolve<ITools>();
            tools.ErrorProcessing(exception);
            return BadRequest();
        }
    }
    [HttpPost("{action}"), Authorize(Roles = "RefreshToken")]
    public IActionResult SetCallback(string? callbackUrl = null)
    {
        try
        {
            var accounting = ServiceLocator.Resolve<Accounting>();
            ClaimsIdentity merchantIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            accounting.SetCallback(callbackUrl, merchantIdentity.Name);

            return Ok();
        }
        catch (Exception exception)
        {
            var tools = ServiceLocator.Resolve<ITools>();
            tools.ErrorProcessing(exception);
            return BadRequest();
        }

    }

    IServiceLocator ServiceLocator { get; }
}