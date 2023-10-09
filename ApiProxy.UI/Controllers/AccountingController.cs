using ApiProxy.Logic.CQRS.Requests.Accounting;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiProxy.UI.Controllers;

[Route("{controller}")]
public class AccountingController : Controller
{
    public AccountingController(IMediator mediator)
    {
        Mediator = mediator;
    }

    [HttpPost("{action}")]
    public async Task<IActionResult> Registration([FromBody]AccountingRegistrationRequest request) => await Mediator.Send(request);


    [HttpPost("{action}")]
    public async Task<IActionResult> GetRefreshToken([FromBody]AccountingGetRefreshTokenRequest request) => await Mediator.Send(request);


    [HttpGet("{action}"), Authorize(Roles = "RefreshToken")]
    public async Task<IActionResult> GetAccessToken()
    {
        var request= new AccountingGetAccessTokenRequest(HttpContext);
        return await Mediator.Send(request);
    }

    [HttpPost("{action}"), Authorize(Roles = "RefreshToken")]
    public async Task<IActionResult> SetRedirect(string? redirectUrl = null)
    {
        var request = new AccountingSetRedirectRequest(HttpContext, redirectUrl);
        return await Mediator.Send(request);
    }

    [HttpPost("{action}"), Authorize(Roles = "RefreshToken")]
    public async Task<IActionResult> SetCallback(string? callbackUrl = null)
    {
        var request = new AccountingSetCallbackRequest(HttpContext, callbackUrl);
        return await Mediator.Send(request);

    }

    IMediator Mediator { get; }
}