using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProxy.Logic.CQRS.Requests.Accounting;

public class AccountingSetCallbackRequest : IRequest<IActionResult>
{
    public AccountingSetCallbackRequest(HttpContext context, string? callbackUrl)
    {
        Context = context;
        CallbackUrl = callbackUrl;
    }

    public HttpContext Context { get; set; }
    public string? CallbackUrl { get; set; }
}