using ApiProxy.Logic.ClearCode.Logic;

namespace ApiProxy.Logic.ClearCode;

public class NewCode
{
    public static void Main(string[] args)
    {
        var primesSettings = new PrimesSettings();

        var primeNumberGenerator = new PrimeNumberGenerator();
        var printPrimes = new PrintPrimes(primesSettings);

        primeNumberGenerator.Init(primesSettings.OrdMax, primesSettings.M);
        var p = primeNumberGenerator.Calc(primesSettings.M);
        printPrimes.Print(p);
    }
}