using System.Collections.Concurrent;

namespace ApiProxy.Logic;

public class ServiceLocator : IServiceLocator
{
    private ServiceLocator()
    {
        Store = new ConcurrentDictionary<Type, ConcurrentDictionary<string, object>>
        { [typeof(IServiceLocator)] = new() { [""] = Lazy } };
        StoreDisposableObjects = new ConcurrentDictionary<Type, ConcurrentDictionary<Guid, object>>();
    }

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
        Store[typeof(TInterface)][key] = new Lazy<TInterface>(() => functor(Resolve<IServiceLocator>()), LazyThreadSafetyMode.ExecutionAndPublication);
        return this;
    }

    public IServiceLocator AddSingleton<TInterface>(Func<TInterface> functor, string key = "")
    {
        if (!Store.ContainsKey(typeof(TInterface)))
            Store[typeof(TInterface)] = new ConcurrentDictionary<string, object>();
        Store[typeof(TInterface)][key] = new Lazy<TInterface>(functor, LazyThreadSafetyMode.ExecutionAndPublication);
        return this;
    }

    public TInterface Resolve<TInterface>(string key = "", bool isLifeCycleManagement = true)
    {
        var value = Store[typeof(TInterface)][key] switch
        {
            Func<TInterface> => ((Func<TInterface>)Store[typeof(TInterface)][key])(),
            Func<IServiceLocator, TInterface> => ((Func<IServiceLocator, TInterface>)Store[typeof(TInterface)][key])(
                Resolve<IServiceLocator>()),
            _ => ((Lazy<TInterface>)Store[typeof(TInterface)][key]).Value
        };
        if (value is not IDisposable || !isLifeCycleManagement) return value;
        if (!StoreDisposableObjects.ContainsKey(typeof(TInterface)))
            StoreDisposableObjects[typeof(TInterface)] = new ConcurrentDictionary<Guid, object>();
        StoreDisposableObjects[typeof(TInterface)][Guid.NewGuid()] = value;
        return value;
    }

    public void Dispose()
    {
        if (StoreDisposableObjects.Count <= 0) return;
        StoreDisposableObjects.Keys.Select(key => StoreDisposableObjects[key]).Where(e => e.Count > 0)
            .SelectMany(e => e.ToList()).ToList().ForEach(e => ((IDisposable)e.Value).Dispose());
        StoreDisposableObjects.Clear();
        Store.Clear();
    }

    public static IServiceLocator Current => Lazy.Value;

    private static readonly Lazy<IServiceLocator> Lazy = new(() => new ServiceLocator(), LazyThreadSafetyMode.ExecutionAndPublication);
    private ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> Store { get; }
    private ConcurrentDictionary<Type, ConcurrentDictionary<Guid, object>> StoreDisposableObjects { get; }
}