namespace ApiProxy.Logic.ClearCode.Logic
{
    public class PrimesData
    {
        public PrimesData()
        {
            MULT = new int[PrimesSettings.ORDMAX + 1];
            P = new int[PrimesSettings.M + 1];
            P[1] = 2;
        }

        public int[] MULT;
        public int[] P;
    }
}
