namespace ApiProxy.Logic.ClearCode.Logic;

public class PrintPrimes
{
    public PrintPrimes(PrimesSettings settings)
    {
        Settings = settings;
    }

    public void Print(int[] p)
    {
        Settings.InitPageNumber();
        Settings.InitPageOffset();
        while (Settings.PageOffset <= Settings.M)
        {
            Console.WriteLine("The First " + Settings.M + " Prime Numbers --- Page " + Settings.PageNumber);
            Console.WriteLine("");
            for (var rowOffset = Settings.PageOffset; rowOffset < Settings.PageOffset + PrimesSettings.Rr; rowOffset++)
            {
                for (var c = 0; c < PrimesSettings.Cc; c++)
                    if (rowOffset + c * PrimesSettings.Rr <= Settings.M)
                        Console.WriteLine($"{p[rowOffset + c * PrimesSettings.Rr]:d10}");
                Console.WriteLine("");
            }
            Console.WriteLine("\f");
            Settings.PageNumber++;
            Settings.PageOffset += PrimesSettings.Rr * PrimesSettings.Cc;
        }
    }

    PrimesSettings Settings { get; }

}