using BenchmarkDotNet.Attributes;
using Algorithms;

public class SortBenchmarkDotnet
{
    private int[] data;

    [Params(100, 5000, 100000, 1000000)] // Array sizes to test
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        // Prepare data array
        data = Algo.Prepare(N);
    }

    [Benchmark]
    public int[] BubbleSortBenchmark()
    {
        return Algo.BubbleSort(data);
    }

    [Benchmark]
    public int[] InsertionSortBenchmark()
    {
        return Algo.InsertionSort(data);
    }

    [Benchmark]
    public int[] SelectionSortBenchmark()
    {
        return Algo.SelectionSort(data);
    }

    [Benchmark]
    public int[] MergeSortBenchmark()
    {
        return Algo.MergeSort(data);
    }

    [Benchmark]
    public int[] QuickSortBenchmark()
    {
        return Algo.QuickSort(data);
    }

    [Benchmark]
    public int[] SortByLambdaBenchmark()
    {
        return Algo.SortByLambda(data);
    }
}