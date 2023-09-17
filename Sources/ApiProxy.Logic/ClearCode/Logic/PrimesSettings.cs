using ApiProxy.Logic.ClearCode.Interfaces;

namespace ApiProxy.Logic.ClearCode.Logic;

public class PrimesSettings : IPrimesSettings
{
    public PrimesSettings()
    {
        M = MVal;
        OrdMax = OrdMaxVal;
        Rr = RrVal;
        Cc = CcVal;
    }

    public int M { get; set; }
    public int OrdMax { get; set; }
    public int Rr { get; set; }
    public int Cc { get; set; }

    static int MVal = 1000;
    static int OrdMaxVal = 30;
    static int RrVal = 50;
    static int CcVal = 4;
}