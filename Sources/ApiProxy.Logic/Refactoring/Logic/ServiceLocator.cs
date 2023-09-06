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

    public TInterface Resolve<TInterface>(string key="")
    {
        switch (Store[typeof(TInterface)][key])
        {
            case Func<TInterface>:
            {
                var functor = (Func<TInterface>)Store[typeof(TInterface)][key];
                return functor();
            }
            case Func<IServiceLocator, TInterface>:
            {
                var functor = (Func<IServiceLocator, TInterface>)Store[typeof(TInterface)][key];
                return functor(this);
            }
            default:
            {
                var lazy = (Lazy<TInterface>)Store[typeof(TInterface)][key];
                return lazy.Value;
            }
        }
    }

    public static IServiceLocator GetInstance() => Lazy.Value;

    private static readonly Lazy<ServiceLocator> Lazy = new(() => new ServiceLocator(), true);
    private ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> Store { get; }
}