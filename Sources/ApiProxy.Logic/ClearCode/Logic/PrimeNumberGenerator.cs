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
            var j = 1;
            var k = 1;
            while (k < m)
            {
                j = GetPrimeNumber(j);
                P[++k] = j;
            }
            return P;
        }

        int GetPrimeNumber(int j)
        {
            var ord = 2;
            var square = 9;
            bool jIsPrime;

            do
            {
                j += 2;
                var result = VerifyJEqualsSquare(ord, square, j);
                square = result.Item2;
                jIsPrime = FindPrimeNumber(result.Item1, j);
            } while (!jIsPrime);

            return j;
        }

        bool FindPrimeNumber(int ord, int j)
        {
            var n = 2;
            var isPrime = true;
            while (n < ord && isPrime)
            {
                while (Mult[n] < j)
                    Mult[n] += 2 * P[n];
                if (Mult[n] == j)
                    isPrime = false;
                n++;
            }
            return isPrime;
        }

        Tuple<int,int> VerifyJEqualsSquare(int ord, int square, int j)
        {
            if (j != square) return new Tuple<int, int>(ord, square);
            ord++;
            square = P[ord] * P[ord];
            Mult[ord - 1] = j;
            return new Tuple<int, int>(ord, square);
        }

        public int[] P { get; set; }
        public int[] Mult { get; set; }
    }
}
