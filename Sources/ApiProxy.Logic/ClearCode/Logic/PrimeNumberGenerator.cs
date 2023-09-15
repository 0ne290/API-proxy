namespace ApiProxy.Logic.ClearCode.Logic
{
    public class PrimeNumberGenerator
    {
        public PrimeNumberGenerator(PrimesSettings settings)
        {
            Settings = settings;
        }

        public void Init()
        {
            M = Settings.M;
            OrdMax = Settings.OrdMax;
            K=Settings.K;
            J=Settings.J;
            Square = Settings.Square;
            Ord = Settings.Ord;

            Mult = new int[OrdMax + 1];
            P = new int[M + 1];
            P[1] = 2;
        }

        public int [] Calc()
        {
            while (K < M)
            {
                GetPrimeNumber();
                K++;
                P[K] = J;
            }
            return P;
        }

        void GetPrimeNumber()
        {
            bool jIsPrime;
            do
            {
                J += 2;
                VerifyJEqualsSquare();
                jIsPrime = FindPrimeNumber();
            } while (!jIsPrime);
        }

        bool FindPrimeNumber()
        {
            var n = 2;
            var isPrime = true;
            while (n < Ord && isPrime)
            {
                while (Mult[n] < J)
                    Mult[n] += 2 * P[n];
                if (Mult[n] == J)
                    isPrime = false;
                n++;
            }
            return isPrime;
        }

        private void VerifyJEqualsSquare()
        {
            if (J != Square) return;
            Ord++;
            Square = P[Ord] * P[Ord];
            Mult[Ord - 1] = J;
        }

        public int M { get; set; }
        public int OrdMax { get; set; }
        public int K { get; set; }
        public int J { get; set; }
        public int Square { get; set; }
        public int Ord { get; set; }
        public int[] P { get; set; }
        public int[] Mult { get; set; }

        PrimesSettings Settings { get; }
    }
}
