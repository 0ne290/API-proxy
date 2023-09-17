using ApiProxy.Logic.ClearCode.Interfaces;
using ApiProxy.Logic.Refactoring;

namespace ApiProxy.Logic.ClearCode.Logic;

public class PrimeNumberGenerator : IPrimeNumberGenerator
{
    public PrimeNumberGenerator(IServiceLocator serviceLocator)
    {
        ServiceLocator = serviceLocator;
    }

    public int [] Calc()
    {
        var data = ServiceLocator.Resolve<IPrimesData>();

        var prime = 1;
        var k = 1;
        var square = 9;

        while (k < data.M)
        { 
            var isPrime = false;
            do
            {
                prime += 2;
                if (prime == square) square = data.GetSquare(prime);
                else isPrime = data.IsPrime(prime);
            } while (!isPrime);

            data.SetPrime(++k, prime);
        }
        return data.P;
    }

    IServiceLocator ServiceLocator { get; }
}