using ApiProxy.Logic.ClearCode.Interfaces;
using ApiProxy.Logic.Refactoring;

namespace ApiProxy.Logic.ClearCode.Logic;

public class Main : IMain
{
    public Main(IServiceLocator serviceLocator)
    {
        ServiceLocator = serviceLocator;
    }

    public void Execute(string[] args)
    {
        var primeNumberGenerator = ServiceLocator.Resolve<IPrimeNumberGenerator>();
        var printPrimes = ServiceLocator.Resolve<IPrintPrimes>();

        var p = primeNumberGenerator.Calc();
        printPrimes.Print(p);
    }

    IServiceLocator ServiceLocator { get; }
}