using ApiProxy.Logic.ClearCode.Logic;

namespace ApiProxy.Logic.ClearCode;

public class NewCode
{
    public static void Main(string[] args)
    {
        var primesSettings = new PrimesSettings();
        var data = new PrimesData();

        var printPrimes = new PrintPrimes(primesSettings, data);
        printPrimes.Print();
    }
}