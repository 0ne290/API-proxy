namespace ApiProxy.Logic.ClearCode.Logic
{
    public class PrimesData
    {
        public PrimesData(int ordMax, int m)
        {
            Mult = new int[ordMax + 1];
            P = new int[m + 1];
            P[1] = 2;
        }

        public bool IsPrime(int prime, int ord)
        {
            var n = 2;
            var isPrime = true;
            while (n < ord && isPrime)
            {
                while (Mult[n] < prime)
                    Mult[n] += 2 * P[n];
                if (Mult[n] == prime)
                    isPrime = false;
                n++;
            }
            return isPrime;
        }

        public int GetSquare(int prime, ref int ord)
        {
            ord++;
            Mult[ord - 1] = prime;
            return P[ord] * P[ord];
        }

        public void SetPrime(int k, int prime)
        {
            P[k] = prime;
        }

        public int[] P { get; }
        int[] Mult { get; }
    }
}
