using System;
using algorithms;
using System.Diagnostics;
using FUS600_LAB1;

class Program
{
    private static bool useAsync;

    static async Task Main()
    {

        Console.WriteLine("Select mode of operation:");
        Console.WriteLine("1. Asynchronous");
        Console.WriteLine("2. Synchronous");

        useAsync = Convert.ToInt32(Console.ReadLine()) == 1;

        while (true)
        {
            Console.WriteLine("Select an operation:");
            Console.WriteLine("1. Bubble Sort");
            Console.WriteLine("2. Insertion Sort");
            Console.WriteLine("3. Selection Sort");
            Console.WriteLine("4. Merge Sort");
            Console.WriteLine("5. Quick Sort");
            Console.WriteLine("6. Sort By Lambda");
            Console.WriteLine("7. Linear Search");
            Console.WriteLine("8. Binary Search");
            Console.WriteLine("9. Run Automated Performance Test");
            Console.WriteLine("10. Run Automated Search Test");
            Console.WriteLine("11 Process Employees File");
            Console.WriteLine("12. Exit");

            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > 11)
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 12.");
                continue;
            }

            switch (choice)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    await HandleSorting(choice);
                    break;
                case 7:
                case 8:
                    await HandleSearching(choice);
                    break;
                case 9:
                    RunPerformanceTest();
                    break;
                case 10:
                    RunSearchTest();
                    break;
                case 11:
                    PerformEmployeeOperations();
                    break;
                case 12:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid selection. Please try again.");
                    break;
            }
        }
    }

    static async Task HandleSorting(int choice)
    {
        SortingMethodDelegate sortMethod = GetSortingMethod(choice);
        if (sortMethod == null)
        {
            Console.WriteLine("Invalid sorting selection.");
            return;
        }

        Console.WriteLine("Enter the size of the array:");
        if (!int.TryParse(Console.ReadLine(), out int size) || size <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a positive integer.");
            return;
        }

        int[] array = algo.Prepare(size);
        await DisplayRunningTime(array, (arr, _) => { sortMethod(arr); return 0; });

    }

    static async Task HandleSearching(int choice)
    {
        int[] array = algo.Prepare(100000);
        algo.MergeSort(array); // Sorting the array before searching

        Console.WriteLine("Enter the target value to search for:");
        if (!int.TryParse(Console.ReadLine(), out int target))
        {
            Console.WriteLine("Invalid input. Please enter an integer.");
            return;
        }

        SearchMethodDelegate searchMethod = choice == 7 ? algo.LinearSearch : algo.BinarySearch;
        await DisplayRunningTime(array, (arr, _) => searchMethod(arr, target));
    }

    static SortingMethodDelegate GetSortingMethod(int choice)
    {
        switch (choice)
        {
            case 1: return algo.BubbleSort;
            case 2: return algo.InsertionSort;
            case 3: return algo.SelectionSort;
            case 4: return algo.MergeSort;
            case 5: return algo.QuickSort;
            case 6: return array => { algo.SortByLambda(array); };
            default:return array => { /* No operation */ };
        }
    }

    static void RunPerformanceTest()
    {
        int[] sizes = { 100, 5000, 100000, 1000000 };
        string[] algorithms = { "Insertion Sort", "Selection Sort", "Bubble Sort", "Merge Sort", "Quick Sort", "Using Lambda" };
        SortingMethodDelegate[] methods = { algo.InsertionSort, algo.SelectionSort, algo.BubbleSort, algo.MergeSort, algo.QuickSort, array => { algo.SortByLambda(array); } };

        // Print table header
        Console.Write("{0,-16}", "Algorithm");
        foreach (int size in sizes)
        {
            Console.Write("{0,-15}", $"n={size}");
        }
        Console.WriteLine();

        for (int i = 0; i < algorithms.Length; i++)
        {
            Console.Write("{0,-16}", algorithms[i]);

            foreach (int size in sizes)
            {
                int[] array = algo.Prepare(size);
                double durationMilliseconds = MeasureDuration(array, methods[i]);

                Console.Write("{0,-15}", $"{durationMilliseconds:F2} ms");
            }
            Console.WriteLine();
        }
    }

    static void RunSearchTest()
    {
        int[] array = algo.Prepare(100000);
        algo.MergeSort(array); 

        int firstElement = array[0];
        int middleElement = array[array.Length / 2];
        int lastElement = array[array.Length - 1];

        Console.WriteLine("{0,-15} {1,-15} {2,-15} {3,-15}", "", "Best case", "Average case", "Worst case");

        Console.Write("{0,-15} ", "Linear Search");
        PrintSearchTimes(array, firstElement, middleElement, lastElement, algo.LinearSearch);

        Console.Write("{0,-15} ", "Binary Search");
        PrintSearchTimes(array, firstElement, middleElement, lastElement, algo.BinarySearch);

        Console.Write("{0,-15} ", "Using Lambda");
        PrintSearchTimes(array, firstElement, middleElement, lastElement, (arr, target) => algo.SortByLambda(arr).ToList().IndexOf(target));
    }

    static void PrintSearchTimes(int[] array, int firstElement, int middleElement, int lastElement, SearchMethodDelegate searchMethod)
    {
        Console.Write("{0,-15} ", MeasureSearchTime(array, firstElement, searchMethod));
        Console.Write("{0,-15} ", MeasureSearchTime(array, middleElement, searchMethod));
        Console.WriteLine("{0,-15} ", MeasureSearchTime(array, lastElement, searchMethod));
    }

    static double MeasureSearchTime(int[] array, int target, SearchMethodDelegate searchMethod)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        searchMethod(array, target);
        stopwatch.Stop();
        return stopwatch.Elapsed.TotalMilliseconds;
    }

    static double MeasureDuration(int[] array, SortingMethodDelegate sortingMethod)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        sortingMethod(array);
        stopwatch.Stop();
        return stopwatch.Elapsed.TotalMilliseconds;
    }

    static async Task DisplayRunningTime(int[] array, Func<int[], object, int> operation, object parameter = null)
    {
        string resultText = "";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        int result = useAsync ? await Task.Run(() => operation(array, parameter)) : operation(array, parameter);

        resultText = result >= 0 ? $"Operation result: {result}" : "Operation completed";
        stopwatch.Stop();
        Console.WriteLine($"{resultText}. Elapsed Time: {stopwatch.Elapsed.TotalMilliseconds} ms");
    }

    static void PerformEmployeeOperations()
    {
        List<Employee> allEmployees = new List<Employee>();
        Console.WriteLine("Please enter the path to the Employees.txt file:");
        string filePath = Console.ReadLine();
        try
        {
             allEmployees = File.ReadAllLines(filePath)
                                                     .Select(record => record.Split('|'))
                                                     .Select(fields => new Employee
                                                     {
                                                         Name = fields[0],
                                                         Department = fields[1],
                                                         YearsOfExperience = int.Parse(fields[2])
                                                     })
                                                     .ToList();
        }catch (Exception)
        {
            Console.WriteLine("Invalid file path or file not found.");
        }
       

        // Find employees with 'an' in their names
        var employeesWithAn = allEmployees.Where(emp => emp.Name.Contains("an")).ToList();
        Console.WriteLine("\nEmployees with 'an' in their names:");
        foreach (var emp in employeesWithAn)
        {
            Console.WriteLine(emp.Name);
        }

        // Extract names from employee records
        var listOfNames = allEmployees.Select(emp => emp.Name).ToList();
        Console.WriteLine("\nList of Employee Names:");
        foreach (var individualName in listOfNames)
        {
            Console.WriteLine(individualName);
        }

        // Sum up the total years of experience
        var experienceSum = allEmployees.Sum(emp => emp.YearsOfExperience);
        Console.WriteLine($"\nCombined Total Years of Experience: {experienceSum}");
    }



}
