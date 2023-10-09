using ApiProxy.Logic.CQRS.Requests.Accounting;
using Microsoft.AspNetCore.Mvc;

namespace ApiProxy.Logic.CQRS.Handlers.Accounting;

public class AccountingSetCallbackRequestHandler : BaseAccountingHandler<AccountingSetCallbackRequest>
{
    public AccountingSetCallbackRequestHandler(IServiceLocator serviceLocator) : base(serviceLocator)
    {
    }

    public override IActionResult TryFunctor(AccountingSetCallbackRequest request)
    {
        var jwtCreator = ServiceLocator.Resolve<IJwtCreator>();
        var accounting = ServiceLocator.Resolve<Logic.Accounting>();

        var merchantName = jwtCreator.GetName(request.Context);
        accounting.SetCallback(request.CallbackUrl, merchantName);

        return Ok();
    }
}