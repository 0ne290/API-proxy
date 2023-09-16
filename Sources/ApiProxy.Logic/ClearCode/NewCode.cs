using ApiProxy.Logic.ClearCode.Logic;

namespace ApiProxy.Logic.ClearCode;

public class NewCode
{
    public static void Main(string[] args)
    {
        var primesSettings = new PrimesSettings();
        var primesData = new PrimesData(primesSettings.OrdMax, primesSettings.M);

        var primeNumberGenerator = new PrimeNumberGenerator();
        var printPrimes = new PrintPrimes(primesSettings);

        var p = primeNumberGenerator.Calc(primesData);
        printPrimes.Print(p);
    }
}