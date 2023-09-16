namespace ApiProxy.Logic.ClearCode.Logic
{
    public class PrimesData
    {
        public PrimesData(int ordMax, int m)
        {
            Ord = 2;
            M = m;
            Mult = new int[ordMax + 1];
            P = new int[m + 1];
            P[1] = 2;
        }

        public bool IsPrime(int prime)
        {
            var n = 2;
            var isPrime = true;
            while (n < Ord && isPrime)
            {
                while (Mult[n] < prime)
                    Mult[n] += 2 * P[n];
                if (Mult[n] == prime)
                    isPrime = false;
                n++;
            }
            return isPrime;
        }

        public int GetSquare(int prime)
        {
            Ord++;
            Mult[Ord - 1] = prime;
            return P[Ord] * P[Ord];
        }

        public void SetPrime(int k, int prime) => P[k] = prime;

        public int M { get; }
        public int[] P { get; }
        int Ord { get; set; }
        int[] Mult { get; }
    }
}
