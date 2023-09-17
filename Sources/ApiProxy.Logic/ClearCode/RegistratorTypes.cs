using ApiProxy.Logic.ClearCode.Interfaces;
using ApiProxy.Logic.ClearCode.Logic;
using ApiProxy.Logic.Refactoring;

namespace ApiProxy.Logic.ClearCode;

public class RegistratorTypes
{
    public void RegisterAllTypes(IServiceLocator serviceLocator)
    {
        serviceLocator.AddSingleton<IPrimesSettings>(() => new PrimesSettings())
            .AddSingleton<IPrimesData>(locator =>
            {
                var settings = locator.Resolve<IPrimesSettings>();
                return new PrimesData(settings);
            })
            .AddSingleton<IPrimeNumberGenerator>(locator => new PrimeNumberGenerator(locator))
            .AddSingleton<IPrintPrimes>(locator =>
            {
                var settings = locator.Resolve<IPrimesSettings>();
                return new PrintPrimes(settings);
            })
            .AddSingleton<IMain>(locator => new Main(locator));
    }
}