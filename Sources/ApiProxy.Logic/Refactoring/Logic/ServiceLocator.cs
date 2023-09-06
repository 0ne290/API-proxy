namespace ApiProxy.Logic.Refactoring.Logic;

public class ServiceLocator : IServiceLocator
{
    private ServiceLocator() => Store = new Dictionary<Type, object> { [typeof(IServiceLocator)] = this };

    public IServiceLocator Add<TInterface, TImplementation>() where TImplementation : TInterface, new()
    {
        Store[typeof(TInterface)] = new Lazy<TInterface>(new TImplementation());
        return this;
    }

    public IServiceLocator Add<TInterface, TImplementation>(TImplementation implementation) where TImplementation : TInterface
    {
        Store[typeof(TInterface)] = implementation!;
        return this;
    }

    public TInterface Resolve<TInterface>()
    {
        if (Store[typeof(TInterface)].GetType()==typeof(Lazy<TInterface>))
            return ((Lazy<TInterface>)Store[typeof(TInterface)]).Value;
        return (TInterface)Store[typeof(TInterface)];
    }

    public static IServiceLocator GetInstance() => Lazy.Value;

    private static readonly Lazy<ServiceLocator> Lazy = new(new ServiceLocator());
    private Dictionary<Type, object> Store { get; }
}