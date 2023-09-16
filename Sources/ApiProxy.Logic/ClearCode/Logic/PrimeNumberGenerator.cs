namespace ApiProxy.Logic.ClearCode.Logic
{
    public class PrimeNumberGenerator
    {
        public int [] Calc(PrimesData data)
        {
            var prime = 1;
            var k = 1;
            while (k < data.M)
            { 
                var square = 9;
                var isPrime = false;

                do
                {
                    prime += 2;
                    if (prime == square) square = data.GetSquare(prime);
                    else isPrime = data.IsPrime(prime);
                } while (!isPrime);

                data.SetPrime(k++, prime);
            }
            return data.P;
        }
    }
}
