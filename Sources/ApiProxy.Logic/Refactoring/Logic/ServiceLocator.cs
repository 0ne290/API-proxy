using System.Collections.Concurrent;

namespace ApiProxy.Logic.Refactoring.Logic;

public class ServiceLocator : IServiceLocator
{
    private ServiceLocator() => Store = new ConcurrentDictionary<Type, ConcurrentDictionary<string, object>>
        { [typeof(IServiceLocator)] = new() { [""] = this } };

    public IServiceLocator Add<TInterface, TImplementation>(string key="") where TImplementation : TInterface, new()
    {
        if (!Store.ContainsKey(typeof(TInterface)))
            Store[typeof(TInterface)] = new ConcurrentDictionary<string, object>();
        Store[typeof(TInterface)][key] = new Lazy<TInterface>(new TImplementation());
        return this;
    }

    public IServiceLocator Add<TInterface, TImplementation>(TImplementation implementation, string key="") where TImplementation : TInterface
    {
        if (!Store.ContainsKey(typeof(TInterface)))
            Store[typeof(TInterface)] = new ConcurrentDictionary<string, object>();
        Store[typeof(TInterface)][key] = implementation!;
        return this;
    }

    public TInterface Resolve<TInterface>(string key="")
    {
        if (Store[typeof(TInterface)][key].GetType()==typeof(Lazy<TInterface>))
            return ((Lazy<TInterface>)Store[typeof(TInterface)][key]).Value;
        return (TInterface)Store[typeof(TInterface)][key];
    }

    public static IServiceLocator GetInstance() => Lazy.Value;

    private static readonly Lazy<ServiceLocator> Lazy = new(() => new ServiceLocator(), true);
    private ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> Store { get; }
}