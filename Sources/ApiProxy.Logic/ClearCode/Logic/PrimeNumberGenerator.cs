namespace ApiProxy.Logic.ClearCode.Logic
{
    public class PrimeNumberGenerator
    {
        public PrimeNumberGenerator(PrimesData primesData)
        {
            PrimesData = primesData;
        }

        public int [] Calc(int m)
        {
            var prime = 1;
            var k = 1;
            while (k < m)
            {
                var ord = 2;
                var square = 9;
                var isPrime = false;

                do
                {
                    prime += 2;
                    if (prime == square) square = PrimesData.GetSquare(prime, ref ord);
                    else isPrime = PrimesData.IsPrime(prime, ord);
                } while (!isPrime);

                PrimesData.SetPrime(k++, prime);
            }
            return PrimesData.P;
        }

        PrimesData PrimesData { get; }
    }
}
