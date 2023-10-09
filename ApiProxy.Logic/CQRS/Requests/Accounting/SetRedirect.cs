using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProxy.Logic.CQRS.Requests.Accounting;

public class AccountingSetRedirectRequest : IRequest<IActionResult>
{
    public AccountingSetRedirectRequest(HttpContext context, string? redirectUrl)
    {
        Context = context;
        RedirectUrl = redirectUrl;
    }

    public HttpContext Context { get; set; }
    public string? RedirectUrl { get; set; }
}