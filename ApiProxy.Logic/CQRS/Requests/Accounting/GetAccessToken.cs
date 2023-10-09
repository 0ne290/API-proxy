using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProxy.Logic.CQRS.Requests.Accounting;

public class AccountingGetAccessTokenRequest : IRequest<IActionResult>
{
    public AccountingGetAccessTokenRequest(HttpContext context)
    {
        Context = context;
    }

    public HttpContext Context { get; set; }
}