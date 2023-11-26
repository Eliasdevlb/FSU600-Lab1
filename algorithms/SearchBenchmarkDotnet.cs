using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Algorithms; 
public class SearchAlgorithmsBenchmark
{
    private int[] data;
    private int bestCaseTarget;
    private int averageCaseTarget;
    private int worstCaseTarget;

    [Params(100000)] // Only test array size of 100,000
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        // Prepare data array
        data = Algo.Prepare(N);

        // Define targets for best, average, and worst case scenarios
        bestCaseTarget = data[0]; // Assuming the array is sorted for binary search
        averageCaseTarget = data[N / 2];
        worstCaseTarget = data[N - 1];
    }

    [Benchmark]
    public void LinearSearchBestCase()
    {
        Algo.LinearSearch(data, bestCaseTarget);
    }

    [Benchmark]
    public void LinearSearchAverageCase()
    {
        Algo.LinearSearch(data, averageCaseTarget);
    }

    [Benchmark]
    public void LinearSearchWorstCase()
    {
        Algo.LinearSearch(data, worstCaseTarget);
    }

    [Benchmark]
    public void BinarySearchBestCase()
    {
        Algo.BinarySearch(data, bestCaseTarget); // Best case for Binary Search is the middle element
    }

    [Benchmark]
    public void BinarySearchAverageCase()
    {
        Algo.BinarySearch(data, averageCaseTarget);
    }

    [Benchmark]
    public void BinarySearchWorstCase()
    {
        Algo.BinarySearch(data, worstCaseTarget);
    }

    [Benchmark]
    public void LambdaSearchBestCase()
    {
        Algo.LambdaSearch(data, bestCaseTarget);
    }

    [Benchmark]
    public void LambdaSearchAverageCase()
    {
        Algo.LambdaSearch(data, averageCaseTarget);
    }

    [Benchmark]
    public void LambdaSearchWorstCase()
    {
        Algo.LambdaSearch(data, worstCaseTarget);
    }
}


