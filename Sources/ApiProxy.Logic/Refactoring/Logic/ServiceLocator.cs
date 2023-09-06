using System.Collections.Concurrent;

namespace ApiProxy.Logic.Refactoring.Logic;

public class ServiceLocator : IServiceLocator
{
    private ServiceLocator() => Store = new ConcurrentDictionary<Type, ConcurrentDictionary<string, object>>
        { [typeof(IServiceLocator)] = new() { [""] = this } };

    public IServiceLocator Add<TInterface>(Func<IServiceLocator, TInterface> functor, string key = "")
    {
        if (!Store.ContainsKey(typeof(TInterface)))
            Store[typeof(TInterface)] = new ConcurrentDictionary<string, object>();
        Store[typeof(TInterface)][key] = functor;
        return this;
    }

    public IServiceLocator Add<TInterface>(Func<TInterface> functor, string key = "")
    {
        if (!Store.ContainsKey(typeof(TInterface)))
            Store[typeof(TInterface)] = new ConcurrentDictionary<string, object>();
        Store[typeof(TInterface)][key] = functor;
        return this;
    }

    public IServiceLocator AddSingleton<TInterface>(Func<IServiceLocator, TInterface> functor, string key = "")
    {
        if (!Store.ContainsKey(typeof(TInterface)))
            Store[typeof(TInterface)] = new ConcurrentDictionary<string, object>();
        Store[typeof(TInterface)][key] = new Lazy<TInterface>(() => functor(this), true);
        return this;
    }

    public IServiceLocator AddSingleton<TInterface>(Func<TInterface> functor, string key = "")
    {
        if (!Store.ContainsKey(typeof(TInterface)))
            Store[typeof(TInterface)] = new ConcurrentDictionary<string, object>();
        Store[typeof(TInterface)][key] = new Lazy<TInterface>(functor, true);
        return this;
    }

    public TInterface Resolve<TInterface>(string key = "") => Store[typeof(TInterface)][key] switch
    {
        Func<TInterface> => ((Func<TInterface>)Store[typeof(TInterface)][key])(),
        Func<IServiceLocator, TInterface> => ((Func<IServiceLocator, TInterface>)Store[typeof(TInterface)][key])(this),
        _ => ((Lazy<TInterface>)Store[typeof(TInterface)][key]).Value
    };

    public static IServiceLocator GetInstance() => Lazy.Value;

    private static readonly Lazy<ServiceLocator> Lazy = new(() => new ServiceLocator(), true);
    private ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> Store { get; }
}