using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiProxy.Logic;
using System.Security.Claims;

namespace ApiProxy.UI.Controllers;

[Route("{controller}"), Authorize(Roles = "AccessToken")]
public class ApiController : Controller
{
    private static IConfiguration _appConfig;
    private static HttpClient _httpClient;
    private IServiceLocator ServiceLocator { get; }

    private Api _api;

    public ApiController(IConfiguration appConfig, IServiceLocator serviceLocator)
    {
        ServiceLocator = serviceLocator;
        _httpClient = ServiceLocator.Resolve<HttpClient>();
        _api = ServiceLocator.Resolve<Api>();
        _appConfig = appConfig;
    }

    [HttpGet("{action}")]
    public IActionResult Fiats()
    {
        try
        {
            var res = _api.Fiats(_appConfig?["Api:FiatsUrl"] ?? "/v1/fiats");
            return Json(res);
        }
        catch (Exception exception)
        {
            var tools = ServiceLocator.Resolve<ITools>();
            tools.ErrorProcessing(exception);
            return BadRequest();
        }
    }
    [HttpPost("{action}")]
    public IActionResult InvoicesCryptocurrency(string? nameCoin = null, decimal? amount = null)
    {
        try
        {
            var res = _api.InvoicesCryptocurrency(nameCoin, amount, _appConfig?["Api:InvoicesUrl"] ?? "/v1/invoices", ((ClaimsIdentity)(HttpContext.User.Identity)).Name);
            return Json(res);
        }
        catch (Exception exception)
        {
            var tools = ServiceLocator.Resolve<ITools>();
            tools.ErrorProcessing(exception);
            return BadRequest();
        }
    }
    [HttpPost("{action}")]
    public IActionResult InvoicesFiat(string? nameFiat = null, decimal? amount = null)
    {
        try
        {
            var res = _api.InvoicesFiat(nameFiat, amount, _appConfig?["Api:InvoicesUrl"] ?? "/v1/invoices", ((ClaimsIdentity)(HttpContext.User.Identity)).Name); ;
            return Json(res);
        }
        catch (Exception exception)
        {
            var tools = ServiceLocator.Resolve<ITools>();
            tools.ErrorProcessing(exception);
            return BadRequest();
        }
    }
}