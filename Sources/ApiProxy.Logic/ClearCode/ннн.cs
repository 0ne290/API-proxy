// Было
public class PrintPrimes 
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




// Стало
public class PrimePrinter 
{
	public static void Main(string[] args) 
	{
		const int NUMBER_OF_PRIMES = 1000;
		int[] primes = PrimeGenerator.Generate(NUMBER_OF_PRIMES);
		const int ROWS_PER_PAGE = 50;
		const int COLUMNS_PER_PAGE = 4;
		RowColumnPagePrinter tablePrinter = new RowColumnPagePrinter(ROWS_PER_PAGE, COLUMNS_PER_PAGE, "The First " + NUMBER_OF_PRIMES + " Prime Numbers");
		tablePrinter.Print(primes);
	}
}

public class RowColumnPagePrinter 
{
	private int rowsPerPage;
	private int columnsPerPage;
	private int numbersPerPage;
	private string pageHeader;
	private PrintStream printStream;
	
	public RowColumnPagePrinter(int rowsPerPage, int columnsPerPage, string pageHeader) 
	{
		this.rowsPerPage = rowsPerPage;
		this.columnsPerPage = columnsPerPage;
		this.pageHeader = pageHeader;
		numbersPerPage = rowsPerPage * columnsPerPage;
		printStream = System.Out;
	}
	
	public void Print(int[] data) 
	{
		int pageNumber = 1;
		for (int firstIndexOnPage = 0; firstIndexOnPage < data.length; firstIndexOnPage += numbersPerPage) 
		{
			int lastIndexOnPage = Math.Min(firstIndexOnPage + numbersPerPage - 1, data.length - 1);
			PrintPageHeader(pageHeader, pageNumber);
			PrintPage(firstIndexOnPage, lastIndexOnPage, data);
			printStream.PrintLn("\f");
			pageNumber++;
		}
	}
	private void PrintPage(int firstIndexOnPage, int lastIndexOnPage, int[] data) 
	{
		int firstIndexOfLastRowOnPage = firstIndexOnPage + rowsPerPage - 1;
		for (int firstIndexInRow = firstIndexOnPage; firstIndexInRow <= firstIndexOfLastRowOnPage; firstIndexInRow++) 
		{
			printRow(firstIndexInRow, lastIndexOnPage, data);
			printStream.println("");
		}
	}
	private void PrintRow(int firstIndexInRow, int lastIndexOnPage, int[] data) 
	{
		for (int column = 0; column < columnsPerPage; column++) 
		{
			int index = firstIndexInRow + column * rowsPerPage;
			if (index <= lastIndexOnPage)
			printStream.format("%10d", data[index]);
		}
	}
	private void PrintPageHeader(string pageHeader, int pageNumber) 
	{
		printStream.println(pageHeader + " --- Page " + pageNumber);
		printStream.println("");
	}
	public void SetOutput(PrintStream printStream) 
	{
		this.printStream = printStream;
	}
}

public class PrimeGenerator 
{
	private static int[] primes;
	private static ArrayList<int> multiplesOfPrimeFactors;
	protected static int[] Generate(int n) 
	{
		primes = new int[n];
		multiplesOfPrimeFactors = new ArrayList<int>();
		Set2AsFirstPrime();
		CheckOddNumbersForSubsequentPrimes();
		return primes;
	}
	private static void Set2AsFirstPrime() 
	{
		primes[0] = 2;
		multiplesOfPrimeFactors.Add(2);
	}
	private static void CheckOddNumbersForSubsequentPrimes() 
	{
		int primeIndex = 1;
		for (int candidate = 3; primeIndex < primes.length; candidate += 2) 
		{
			if (IsPrime(candidate))
				primes[primeIndex++] = candidate;
		}
	}
	private static bool IsPrime(int candidate) 
	{
		if (IsLeastRelevantMultipleOfNextLargerPrimeFactor(candidate)) 
		{
			multiplesOfPrimeFactors.Add(candidate);
			return false;
		}
		return IsNotMultipleOfAnyPreviousPrimeFactor(candidate);
	}
	private static bool IsLeastRelevantMultipleOfNextLargerPrimeFactor(int candidate) 
	{
		int nextLargerPrimeFactor = primes[multiplesOfPrimeFactors.size()];
		int leastRelevantMultiple = nextLargerPrimeFactor * nextLargerPrimeFactor;
		return candidate == leastRelevantMultiple;
	}
	private static bool IsNotMultipleOfAnyPreviousPrimeFactor(int candidate) 
	{
		for (int n = 1; n < multiplesOfPrimeFactors.size(); n++) 
		{
			if (IsMultipleOfNthPrimeFactor(candidate, n))
				return false;
		}
		return true;
	}
	private static bool IsMultipleOfNthPrimeFactor(int candidate, int n) 
	{
		return candidate == SmallestOddNthMultipleNotLessThanCandidate(candidate, n);
	}
	private static int SmallestOddNthMultipleNotLessThanCandidate(int candidate, int n) 
	{
		int multiple = multiplesOfPrimeFactors.get(n);
		while (multiple < candidate)
			multiple += 2 * primes[n];
		multiplesOfPrimeFactors.set(n, multiple);
		return multiple;
	}
}