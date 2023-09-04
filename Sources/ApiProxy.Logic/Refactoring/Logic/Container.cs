namespace ApiProxy.Logic.Refactoring.Logic;

public class Container: IContainer
{
    public Container()
    {
        Store = new Dictionary<Type, object>
        {
            [typeof(IContainer)] = this
        };
    }

    public IContainer Add<TInterface, TImplementation>(TImplementation? implementation) where TImplementation : TInterface
    {
        Store[typeof(TInterface)] = implementation!;
        return this;
    }

    public TInterface Resolve<TInterface>()
    {
        return (TInterface)Store[typeof(TInterface)];
    }

    public static IContainer GetInstance() => Lazy.Value;

    static readonly Lazy<Container> Lazy = new(() => new Container());
    Dictionary<Type, object> Store { get; }
}