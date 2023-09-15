namespace ApiProxy.Logic.ClearCode.Logic;

public class PrintPrimes
{
    public PrintPrimes(PrimesSettings settings, PrimesData data)
    {
        Settings = settings;
        Data = data;
    }

    public void Print()
    {
        while (Settings.K < PrimesSettings.M)
        {
            do
            {
                Settings.J += 2;
                if (Settings.J == Settings.SQUARE)
                {
                    Settings.ORD++;
                    Settings.SQUARE = Data.P[Settings.ORD] * Data.P[Settings.ORD];
                    Data.MULT[Settings.ORD - 1] = Settings.J;
                }
                Settings.InitN();
                Settings.InitJPrime();
                while (Settings.N < Settings.ORD && Settings.JPRIME)
                {
                    while (Data.MULT[Settings.N] < Settings.J)
                        Data.MULT[Settings.N] = Data.MULT[Settings.N] + Data.P[Settings.N] + Data.P[Settings.N];
                    if (Data.MULT[Settings.N] == Settings.J)
                        Settings.JPRIME = false;
                    Settings.N += 1;
                }
            } while (!Settings.JPRIME);
            Settings.K++;
            Data.P[Settings.K] = Settings.J;
        }

        Settings.InitPageNumber();
        Settings.InitPageOffset();
        while (Settings.PAGEOFFSET <= PrimesSettings.M)
        {
            Console.WriteLine("The First " + PrimesSettings.M + " Prime Numbers --- Page " + Settings.PAGENUMBER);
            Console.WriteLine("");
            for (var ROWOFFSET = Settings.PAGEOFFSET; ROWOFFSET < Settings.PAGEOFFSET + PrimesSettings.RR; ROWOFFSET++)
            {
                for (var C = 0; C < PrimesSettings.CC; C++)
                    if (ROWOFFSET + C * PrimesSettings.RR <= PrimesSettings.M)
                        Console.WriteLine($"{Data.P[ROWOFFSET + C * PrimesSettings.RR]:d10}");
                Console.WriteLine("");
            }
            Console.WriteLine("\f");
            Settings.PAGENUMBER++;
            Settings.PAGEOFFSET += PrimesSettings.RR * PrimesSettings.CC;
        }
    }

    PrimesData Data { get; }
    PrimesSettings Settings { get; }

}