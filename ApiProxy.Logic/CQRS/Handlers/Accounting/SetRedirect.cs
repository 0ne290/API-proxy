using ApiProxy.Logic.CQRS.Requests.Accounting;
using Microsoft.AspNetCore.Mvc;

namespace ApiProxy.Logic.CQRS.Handlers.Accounting;

public class AccountingSetRedirectRequestHandler : BaseAccountingHandler<AccountingSetRedirectRequest>
{
    public AccountingSetRedirectRequestHandler(IServiceLocator serviceLocator) : base(serviceLocator)
    {
    }

    public override IActionResult TryFunctor(AccountingSetRedirectRequest request)
    {
        var jwtCreator = ServiceLocator.Resolve<IJwtCreator>();
        var accounting = ServiceLocator.Resolve<Logic.Accounting>();

        var merchantName = jwtCreator.GetName(request.Context);
        accounting.SetRedirect(request.RedirectUrl, merchantName);

        return Ok();
    }
}