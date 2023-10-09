namespace ApiProxy.Logic;

public interface IServiceLocator : IDisposable
{
    IServiceLocator Add<TInterface>(Func<IServiceLocator, TInterface> functor, string key = "");
    IServiceLocator Add<TInterface>(Func<TInterface> functor, string key = "");
    IServiceLocator AddSingleton<TInterface>(Func<IServiceLocator, TInterface> functor, string key = "");
    IServiceLocator AddSingleton<TInterface>(Func<TInterface> functor, string key = "");
    TInterface Resolve<TInterface>(string key = "", bool isLifeCycleManagement = true);
}