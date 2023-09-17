using ApiProxy.Logic.ClearCode.Interfaces;

namespace ApiProxy.Logic.ClearCode.Logic;

public class PrimesData : IPrimesData
{
    public PrimesData(IPrimesSettings settings)
    {
        Ord = 2;
        M = settings.M;
        
        Mult = new int[settings.OrdMax + 1];
        P = new int[M + 1];
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
            if (Mult[n] == prime) isPrime = false;
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