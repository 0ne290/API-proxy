using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiProxy.Logic.CQRS.Requests.Accounting;

public class AccountingRegistrationRequest : IRequest<IActionResult>
{
    public AccountingRegistrationRequest(string? login, string? password, string? redirectUrl, string? callbackUrl)
    {
        Login = login;
        Password = password;
        RedirectUrl = redirectUrl;
        CallbackUrl = callbackUrl;
    }

    public string? Login { get; set; }
    public string? Password { get; set; }
    public string? RedirectUrl { get; set; }
    public string? CallbackUrl { get; set; }
}