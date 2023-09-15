namespace ApiProxy.Logic.ClearCode.Logic;

public class PrimesSettings
{
    public PrimesSettings()
    {
        OrdMax = OrdMaxVal;
        M = MVal;

        J = 1;
        K = 1;
        Ord = 2;
        Square = 9;
    }

    public void InitPageNumber()
    {
        PageNumber = 1;
    }
    public void InitPageOffset()
    {
        PageNumber = 1;
    }



    public int OrdMax { get; set; }
    public int M { get; set; }
    public int J { get; set; }
    public int K { get; set; }
    public int Ord { get; set; }
    public int Square { get; set; }
    public int PageNumber { get; set; }
    public int PageOffset { get; set; }

    public static int MVal = 1000;
    public const int Rr = 50;
    public const int Cc = 4;
    public static int OrdMaxVal = 30;
}