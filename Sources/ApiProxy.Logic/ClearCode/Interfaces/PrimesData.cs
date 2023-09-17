namespace ApiProxy.Logic.ClearCode.Interfaces;

public interface IPrimesData
{
    bool IsPrime(int prime);
    int GetSquare(int prime);
    void SetPrime(int k, int prime);
    int M { get; }
    int[] P { get; }
}