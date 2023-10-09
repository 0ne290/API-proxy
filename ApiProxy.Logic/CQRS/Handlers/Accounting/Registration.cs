using ApiProxy.Logic.CQRS.Requests.Accounting;
using Microsoft.AspNetCore.Mvc;

namespace ApiProxy.Logic.CQRS.Handlers.Accounting;

public class AccountingRegistrationRequestHandler : BaseAccountingHandler<AccountingRegistrationRequest>
{
    public AccountingRegistrationRequestHandler(IServiceLocator serviceLocator) : base(serviceLocator)
    {
    }

    public override IActionResult TryFunctor(AccountingRegistrationRequest request)
    {
        var accounting = ServiceLocator.Resolve<Logic.Accounting>();
        accounting.Registration(request.Login, request.Password, request.RedirectUrl, request.CallbackUrl);
        return Ok();
    }
}