using ApiProxy.Logic.ClearCode.Interfaces;

namespace ApiProxy.Logic.ClearCode.Logic;

public class PrintPrimes : IPrintPrimes
{
    public PrintPrimes(IPrimesSettings settings)
    {
        M = settings.M;
        Rr = settings.Rr;
        Cc = settings.Cc;
        AddingForPageOffset = Rr * Cc;
    }

    public void Print(int[] primeNumbers)
    {
        PrimeNumbers = primeNumbers;
        PageNumber = 0;
        PageOffset = 1;

        while (PageOffset <= M)
            PrintPage();
    }

    void PrintPage()
    {
        PageNumber++;
        Console.WriteLine($"The First {M} Prime Numbers --- Page {PageNumber}");
        Console.WriteLine(string.Empty);
        for (var rowOffset = PageOffset; rowOffset < PageOffset + Rr; rowOffset++)
        {
            PrintRow(rowOffset);
            Console.WriteLine(string.Empty);
        }
        Console.WriteLine("\f");
        PageOffset += AddingForPageOffset;
    }

    void PrintRow(int rowOffset)
    {
        for (var c = 0; c < Cc; c++)
            if (rowOffset + c * Rr <= M)
                Console.Write($"{PrimeNumbers[rowOffset + c * Rr]:d10}");
    }

    int M { get; }
    int Rr { get; }
    int Cc { get; }
    int[] PrimeNumbers { get; set; } = null!;
    int PageNumber { get; set; }
    int PageOffset { get; set; }
    int AddingForPageOffset { get; }
}