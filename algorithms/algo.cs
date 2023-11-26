
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace Algorithms
{
    public delegate int[] SortingAlgorithmDelegate(int[] array);
    public delegate int SearchAlgorithmDelegate(int[] array, int target);

    public class Algo
    {
        public static void Swap(int[] myArray, int m, int n)
        {
            int temp = myArray[m];
            myArray[m] = myArray[n];
            myArray[n] = temp;
        }

        public static void Randomize(int[] array)
        {
            Random rand = new Random();
            int size = array.Length;
            for (int i = 0; i < size; i++)
            {
                array[i] = rand.Next(0, 10 * size);
            }
        }


        public static int[] Prepare(int n)
        {
            int[] array = new int[n];
            Randomize(array);
            return array;
        }

        public static int[] InsertionSort(int[] array)
        {
            int[] result = (int[])array.Clone();
            for (int i = 1; i < result.Length; i++)
            {
                int key = result[i];
                int j = i - 1;
                while (j >= 0 && result[j] > key)
                {
                    result[j + 1] = result[j];
                    j--;
                }
                result[j + 1] = key;
            }
            return result;
        }


        public static int[] SelectionSort(int[] array)
        {
            int[] result = (int[])array.Clone();
            for (int i = 0; i < result.Length - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < result.Length; j++)
                {
                    if (result[j] < result[minIndex])
                    {
                        minIndex = j;
                    }
                }
                int temp = result[minIndex];
                result[minIndex] = result[i];
                result[i] = temp;
            }
            return result;
        }


        public static int[] BubbleSort(int[] array)
        {
            int[] result = (int[])array.Clone();
            for (int i = 0; i < result.Length - 1; i++)
            {
                for (int j = 0; j < result.Length - i - 1; j++)
                {
                    if (result[j] > result[j + 1])
                    {
                        int temp = result[j];
                        result[j] = result[j + 1];
                        result[j + 1] = temp;
                    }
                }
            }
            return result;
        }


        public static int[] MergeSort(int[] array)
        {
            if (array.Length <= 1)
                return array;

            int mid = array.Length / 2;
            int[] left = MergeSort(array.Take(mid).ToArray());
            int[] right = MergeSort(array.Skip(mid).ToArray());

            return Merge(left, right);
        }

        private static int[] Merge(int[] left, int[] right)
        {
            int[] result = new int[left.Length + right.Length];
            int i = 0, j = 0, k = 0;
            while (i < left.Length && j < right.Length)
            {
                if (left[i] < right[j])
                {
                    result[k++] = left[i++];
                }
                else
                {
                    result[k++] = right[j++];
                }
            }
            while (i < left.Length)
            {
                result[k++] = left[i++];
            }
            while (j < right.Length)
            {
                result[k++] = right[j++];
            }
            return result;
        }


        public static int[] QuickSort(int[] array)
        {
            int[] result = (int[])array.Clone();
            QuickSortRecursive(result, 0, result.Length - 1);
            return result;
        }

        private static void QuickSortRecursive(int[] array, int low, int high)
        {
            if (low < high)
            {
                int pivot = Partition(array, low, high);
                QuickSortRecursive(array, low, pivot - 1);
                QuickSortRecursive(array, pivot + 1, high);
            }
        }

        private static int Partition(int[] array, int low, int high)
        {
            int pivot = array[high];
            int i = (low - 1);
            for (int j = low; j < high; j++)
            {
                if (array[j] < pivot)
                {
                    i++;
                    int temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }
            int temp1 = array[i + 1];
            array[i + 1] = array[high];
            array[high] = temp1;
            return i + 1;
        }


        public static int[] SortByLambda(int[] array)
        {
            int[] newArray = (int[])array.Clone();

            Array.Sort(newArray, (x, y) => x.CompareTo(y));

            return newArray;
        }

        public static int LinearSearch(int[] array, int target)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == target)
                {
                    return i;
                }
            }
            return -1;
        }
        public static int BinarySearch(int[] array, int target)
        {
            int left = 0;
            int right = array.Length - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;

                if (array[mid] == target)
                {
                    return mid;
                }
                else if (array[mid] < target)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }
            return -1;
        }

        public static int LambdaSearch(int[] array, int target)
        {
            return Array.FindIndex(array, item => item == target);
        }

        public static async Task DisplayRunningTime(int[] array, Delegate algorithmMethod, string description, bool useAsync, int? target = null)
        {
            Stopwatch stopwatch = new Stopwatch();

            Action executeAlgorithm = () =>
            {
                if (algorithmMethod is SortingAlgorithmDelegate sortingMethod)
                {
                    sortingMethod(array); // Invoke sorting algorithm
                }
                else if (algorithmMethod is SearchAlgorithmDelegate searchMethod && target.HasValue)
                {
                    searchMethod(array, target.Value); // Invoke search algorithm
                }
            };

            stopwatch.Start();
            if (useAsync)
            {
                await Task.Run(executeAlgorithm);
            }
            else
            {
                executeAlgorithm();
            }
            stopwatch.Stop();

            Console.WriteLine($"{description}: Running time is {stopwatch.Elapsed.TotalMilliseconds} ms");
        }

    }
}
