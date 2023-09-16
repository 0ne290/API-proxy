namespace ApiProxy.Logic.ClearCode.Logic;

public class PrintPrimes
{
    public PrintPrimes(PrimesSettings settings)
    {
        M = settings.M;
        Rr = settings.Rr;
        Cc = settings.Cc;
    }

    public void Print(int[] p)
    {
        var pageNumber = 1;
        var pageOffset = 1;
        var addingForPageOffset = Rr * Cc;

        while (pageOffset <= M)
        {
            Console.WriteLine("The First " + M + " Prime Numbers --- Page " + pageNumber);
            Console.WriteLine(string.Empty);
            for (var rowOffset = pageOffset; rowOffset < pageOffset + Rr; rowOffset++)
            {
                for (var c = 0; c < Cc; c++)
                    if (rowOffset + c * Rr <= M)
                        Console.WriteLine($"{p[rowOffset + c * Rr]:d10}");
                Console.WriteLine(string.Empty);
            }
            Console.WriteLine("\f");
            pageNumber++;
            pageOffset += addingForPageOffset;
        }
    }

    public int M { get; set; }
    public int Rr { get; set; }
    public int Cc { get; set; }
}