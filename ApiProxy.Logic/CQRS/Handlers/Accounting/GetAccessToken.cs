using ApiProxy.Logic.CQRS.Requests.Accounting;
using Microsoft.AspNetCore.Mvc;

namespace ApiProxy.Logic.CQRS.Handlers.Accounting;

public class AccountingGetAccessTokenRequestHandler : BaseAccountingHandler<AccountingGetAccessTokenRequest>
{
    public AccountingGetAccessTokenRequestHandler(IServiceLocator serviceLocator) : base(serviceLocator)
    {
    }

    public override IActionResult TryFunctor(AccountingGetAccessTokenRequest request)
    {
        var jwtCreator = ServiceLocator.Resolve<IJwtCreator>();
        var accounting = ServiceLocator.Resolve<Logic.Accounting>();

        var merchantName = jwtCreator.GetName(request.Context);
        var merchant = accounting.GetMerchant(merchantName);
        var token = jwtCreator.GetAccessToken(merchant);

        return Json(token);
    }
}