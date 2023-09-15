namespace ApiProxy.Logic.ClearCode.Logic
{
    public class PrimeNumberGenerator
    {
        public void Init(int ordMax, int m)
        {
            Mult = new int[ordMax + 1];
            P = new int[m + 1];
            P[1] = 2;
        }

        public int [] Calc(int m)
        {
            var prime = 1;
            var k = 1;
            while (k < m)
            {
                prime = GetPrimeNumber(prime);
                P[++k] = prime;
            }
            return P;
        }

        int GetPrimeNumber(int prime)
        {
            var ord = 2;
            var square = 9;
            var isPrime=false;

            do
            {
                prime += 2;
                if (prime == square)
                    square = GetSquare(prime, ref ord);
                else
                    isPrime = IsPrime(prime, ord);
            } while (!isPrime);

            return prime;
        }

        private bool IsPrime(int prime, int ord)
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

        int GetSquare(int prime, ref int ord)
        {
            ord++;
            Mult[ord - 1] = prime;
            return P[ord] * P[ord];
        }


        public int[] P { get; set; }
        public int[] Mult { get; set; }
    }
}
