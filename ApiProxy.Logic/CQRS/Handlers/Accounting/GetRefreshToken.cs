using ApiProxy.Logic.CQRS.Requests.Accounting;
using Microsoft.AspNetCore.Mvc;

namespace ApiProxy.Logic.CQRS.Handlers.Accounting;

public class AccountingGetRefreshTokenRequestHandler:BaseAccountingHandler<AccountingGetRefreshTokenRequest>
{
    public AccountingGetRefreshTokenRequestHandler(IServiceLocator serviceLocator) : base(serviceLocator)
    {
    }

    public override IActionResult TryFunctor(AccountingGetRefreshTokenRequest request)
    {
        var jwtCreator = ServiceLocator.Resolve<IJwtCreator>();
        var accounting = ServiceLocator.Resolve<Logic.Accounting>();

        var merchant = accounting.GetMerchant(request.Login, request.Password);
        var token = jwtCreator.GetRefreshToken(merchant);

        return Json(token);
    }
}