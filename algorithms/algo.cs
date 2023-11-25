
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace algorithms
{
    public delegate void SortingMethodDelegate(int[] array);

    public delegate int SearchMethodDelegate(int[] array, int target);

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

        public static void InsertionSort(int[] array)
        {
            for (int i = 1; i < array.Length; i++)
            {
                int key = array[i];
                int j = i - 1;

                while (j >= 0 && array[j] > key)
                {
                    array[j + 1] = array[j];
                    j = j - 1;
                }
                array[j + 1] = key;
            }
        }

        public static void SelectionSort(int[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (array[j] < array[minIndex])
                    {
                        minIndex = j;
                    }
                }
                Swap(array, i, minIndex);
            }
        }

        public static void BubbleSort(int[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        Swap(array, j, j + 1);
                    }
                }
            }
        }

        public static void MergeSort(int[] array)
        {
            if (array.Length <= 1)
                return;

            int mid = array.Length / 2;
            int[] left = new int[mid];
            int[] right = new int[array.Length - mid];

            for (int i = 0; i < mid; i++)
                left[i] = array[i];
            for (int i = mid; i < array.Length; i++)
                right[i - mid] = array[i];

            MergeSort(left);
            MergeSort(right);

            Merge(array, left, right);
        }

        public static void Merge(int[] array, int[] left, int[] right)
        {
            int leftIndex = 0, rightIndex = 0, arrayIndex = 0;

            while (leftIndex < left.Length && rightIndex < right.Length)
            {
                if (left[leftIndex] < right[rightIndex])
                {
                    array[arrayIndex] = left[leftIndex];
                    leftIndex++;
                }
                else
                {
                    array[arrayIndex] = right[rightIndex];
                    rightIndex++;
                }
                arrayIndex++;
            }

            while (leftIndex < left.Length)
            {
                array[arrayIndex] = left[leftIndex];
                leftIndex++;
                arrayIndex++;
            }

            while (rightIndex < right.Length)
            {
                array[arrayIndex] = right[rightIndex];
                rightIndex++;
                arrayIndex++;
            }
        }

        public static void QuickSort(int[] array)
        {
            QuickSort(array, 0, array.Length - 1);
        }

        public static void QuickSort(int[] array, int low, int high)
        {
            if (low < high)
            {
                int pivotIndex = Partition(array, low, high);

                QuickSort(array, low, pivotIndex - 1);
                QuickSort(array, pivotIndex + 1, high);
            }
        }

        public static int Partition(int[] array, int low, int high)
        {
            int pivot = array[high];
            int i = (low - 1);

            for (int j = low; j < high; j++)
            {
                if (array[j] < pivot)
                {
                    i++;
                    Swap(array, i, j);
                }
            }

            Swap(array, i + 1, high);
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

        public static async Task DisplayRunningTime<T>(int[] array, Func<int[], T> method, string description)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            T result = await Task.Run(() => method(array));

            stopwatch.Stop();
            double elapsedMilliseconds = stopwatch.Elapsed.TotalMilliseconds;

            // Check if the result is of type int, and adjust the output accordingly
            if (typeof(T) == typeof(int))
            {
                Console.WriteLine($"{description}: {elapsedMilliseconds} ms. Result: {result}");
            }
            else
            {
                Console.WriteLine($"{description}: {elapsedMilliseconds} ms");
            }
        }


    }
}
