namespace ApiProxy.Logic.ClearCode
{
    public class OldCode
    {
        public static void Main(string[] args)
        {
            const int M = 1000;
            const int RR = 50;
            const int CC = 4;
            const int WW = 10;
            const int ORDMAX = 30;
            int[] P = new int[M + 1];
            int PAGENUMBER;
            int PAGEOFFSET;
            int ROWOFFSET;
            int C;
            int J;
            int K;
            bool JPRIME;
            int ORD;
            int SQUARE;
            int N;
            int[] MULT = new int[ORDMAX + 1];
            J = 1;
            K = 1;
            P[1] = 2;
            ORD = 2;
            SQUARE = 9;
            while (K < M)
            {
                do
                {
                    J = J + 2;
                    if (J == SQUARE)
                    {
                        ORD = ORD + 1;
                        SQUARE = P[ORD] * P[ORD];
                        MULT[ORD - 1] = J;
                    }
                    N = 2;
                    JPRIME = true;
                    while (N < ORD && JPRIME)
                    {
                        while (MULT[N] < J)
                            MULT[N] = MULT[N] + P[N] + P[N];
                        if (MULT[N] == J)
                            JPRIME = false;
                        N = N + 1;
                    }
                } while (!JPRIME);
                K = K + 1;
                P[K] = J;
            }
            PAGENUMBER = 1;
            PAGEOFFSET = 1;
            while (PAGEOFFSET <= M)
            {
                Console.WriteLine("The First " + M + " Prime Numbers --- Page " + PAGENUMBER);
                Console.WriteLine("");
                for (ROWOFFSET = PAGEOFFSET; ROWOFFSET < PAGEOFFSET + RR; ROWOFFSET++)
                {
                    for (C = 0; C < CC; C++)
                        if (ROWOFFSET + C * RR <= M)
                            Console.WriteLine("%10d", P[ROWOFFSET + C * RR]);
                    Console.WriteLine("");
                }
                Console.WriteLine("\f");
                PAGENUMBER = PAGENUMBER + 1;
                PAGEOFFSET = PAGEOFFSET + RR * CC;
            }
        }
    }
}
