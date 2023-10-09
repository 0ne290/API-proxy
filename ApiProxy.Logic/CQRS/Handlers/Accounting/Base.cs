using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiProxy.Logic.CQRS.Handlers.Accounting;

public abstract class BaseAccountingHandler<T> : IRequestHandler<T, IActionResult> where T : IRequest<IActionResult>
{
    protected BaseAccountingHandler(IServiceLocator serviceLocator)
    {
        ServiceLocator = serviceLocator;
    }

    public async Task<IActionResult> Handle(T request, CancellationToken cancellationToken)
    {
        try
        {
            var result = TryFunctor(request);
            return await Task.FromResult(result);
        }
        catch (Exception exception)
        {
            var tools = ServiceLocator.Resolve<ITools>();
            tools.ErrorProcessing(exception);
            return await Task.FromResult(new BadRequestResult());
        }
    }

    public abstract IActionResult TryFunctor(T request);

    protected OkResult Ok() => new();

    protected virtual JsonResult Json(object? data) => new(data);

    protected IServiceLocator ServiceLocator { get; set; }
}