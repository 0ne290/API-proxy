using ApiProxy.Logic.ClearCode.Interfaces;
using ApiProxy.Logic.Refactoring;

namespace ApiProxy.Logic.ClearCode;

public class NewCode : INewCode
{
    public NewCode(IServiceLocator serviceLocator)
    {
        ServiceLocator = serviceLocator;
    }

    public void Execute(string[] args)
    {
        var primeNumberGenerator = ServiceLocator.Resolve<IPrimeNumberGenerator>();
        var printPrimes = ServiceLocator.Resolve<IPrintPrimes>();

        var p = primeNumberGenerator.Calc(primesData);
        printPrimes.Print(p);
    }

    IServiceLocator ServiceLocator { get; }
}