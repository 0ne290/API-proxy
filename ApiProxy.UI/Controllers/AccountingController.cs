using ApiProxy.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiProxy.UI.Controllers;

[Route("{controller}")]
public class AccountingController : Controller
{
		
    public AccountingController(IServiceLocator serviceLocator)
    {
        ServiceLocator = serviceLocator;
    }

    [HttpPost("{action}")]
    public IActionResult Registration(string? login = null, string? password = null, string? redirectUrl = null, string? callbackUrl = null ) =>
        RequestCommandHandling(() =>
        {
            var accounting = ServiceLocator.Resolve<Accounting>();
            accounting.Registration(login, password, redirectUrl, callbackUrl);
            return Ok();
        });

    [HttpPost("{action}")]
    public IActionResult GetRefreshToken(string? login = null, string? password = null) =>
        RequestCommandHandling(() =>
        {
            var jwtCreator = ServiceLocator.Resolve<IJwtCreator>();
            var accounting = ServiceLocator.Resolve<Accounting>();

            var merchant = accounting.GetMerchant(login, password);
            var token = jwtCreator.GetRefreshToken(merchant);

            return Json(token);
        });

    [HttpGet("{action}"), Authorize(Roles = "RefreshToken")]
    public IActionResult GetAccessToken() =>
        RequestCommandHandling(() =>
        {
            var jwtCreator = ServiceLocator.Resolve<IJwtCreator>();
            var accounting = ServiceLocator.Resolve<Accounting>();

            var merchantName = jwtCreator.GetName(HttpContext);
            var merchant = accounting.GetMerchant(merchantName);
            var token = jwtCreator.GetAccessToken(merchant);

            return Json(token);
        });

    [HttpPost("{action}"), Authorize(Roles = "RefreshToken")]
    public IActionResult SetRedirect(string? redirectUrl = null) =>
        RequestCommandHandling(() =>
        {
            var jwtCreator = ServiceLocator.Resolve<IJwtCreator>();
            var accounting = ServiceLocator.Resolve<Accounting>();

            var merchantName = jwtCreator.GetName(HttpContext);
            accounting.SetRedirect(redirectUrl, merchantName);

            return Ok();
        });

    [HttpPost("{action}"), Authorize(Roles = "RefreshToken")]
    public IActionResult SetCallback(string? callbackUrl = null) =>
        RequestCommandHandling(() =>
        {
            var jwtCreator = ServiceLocator.Resolve<IJwtCreator>();
            var accounting = ServiceLocator.Resolve<Accounting>();

            var merchantName = jwtCreator.GetName(HttpContext);
            accounting.SetCallback(callbackUrl, merchantName);

            return Ok();
        });

    IActionResult RequestCommandHandling(Func<IActionResult> functor)
    {
        try
        {
            return functor();
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