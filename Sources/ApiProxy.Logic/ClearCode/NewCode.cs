using ApiProxy.Logic.ClearCode.Logic;

namespace ApiProxy.Logic.ClearCode;

public class NewCode
{
    public static void Main(string[] args)
    {
        var primesSettings = new PrimesSettings();

        var primeNumberGenerator = new PrimeNumberGenerator(primesSettings);
        var printPrimes = new PrintPrimes(primesSettings);

        primeNumberGenerator.Init();
        var p = primeNumberGenerator.Calc();
        printPrimes.Print(p);
    }
}