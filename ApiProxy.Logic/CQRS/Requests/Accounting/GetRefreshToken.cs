using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiProxy.Logic.CQRS.Requests.Accounting;

public class AccountingGetRefreshTokenRequest: IRequest<IActionResult>
{
    public AccountingGetRefreshTokenRequest(string? login, string? password)
    {
        Login = login;
        Password = password;
    }

    public string? Login { get; set; }
    public string? Password { get; set; }
}