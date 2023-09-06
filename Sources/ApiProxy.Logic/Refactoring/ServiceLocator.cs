public interface IContainer
{
    IContainer Add<TInterface, TImplementation>(TImplementation implementation) where TImplementation : TInterface;
    TInterface Resolve<TInterface>();
}

public class Container : IContainer
{
    private Container() => Store = new Dictionary<Type, object> { [typeof(IContainer)] = new Lazy<Container>(this) };

    public IContainer Add<TInterface, TImplementation>(TImplementation implementation) where TImplementation : TInterface
    {
        Store[typeof(TInterface)] = new Lazy<TInterface>(implementation);
        return this;
    }

    public TInterface Resolve<TInterface>() => (TInterface)(((Lazy<TInterface>)(Store[typeof(TInterface)])).Value);

    public static IContainer GetInstance() => Lazy.Value;

    private static readonly Lazy<Container> Lazy = new Lazy<Container>();
    private Dictionary<Type, object> Store { get; }
}