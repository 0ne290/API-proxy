namespace ApiProxy.Logic.ClearCode.Logic;

public class PrimesSettings
{
    public PrimesSettings()
    {
        J = 1;
        K = 1;
        ORD = 2;
        SQUARE = 9;
    }

    public void InitPageNumber()
    {
        PAGENUMBER = 1;
    }
    public void InitPageOffset()
    {
        PAGENUMBER = 1;
    }

    public void InitN()
    {
        N = 2;
    }

    public void InitJPrime()
    {
        JPRIME = true;
    }

    public bool JPRIME;
    public int N;
    public int J;
    public int K;
    public int ORD;
    public int SQUARE;
    public int PAGENUMBER;
    public int PAGEOFFSET;

    public const int M = 1000;
    public const int RR = 50;
    public const int CC = 4;
    public const int ORDMAX = 30;
}