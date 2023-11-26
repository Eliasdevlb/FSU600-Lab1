using Algorithms;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Benchmark;
using System.Linq;
using System.IO;


class Program
{

    static async Task Main(string[] args)
    {
        Console.WriteLine("Select mode of operation:");
        Console.WriteLine("1. Asynchronous");
        Console.WriteLine("2. Synchronous");

        bool useAsync = Console.ReadLine() == "1";

        while (true)
        {
            int choice = await ShowMenu();
            if (choice == 23)
            {
                break; // Exit the loop and end the program
            }

            await PerformOperation(choice, useAsync);
        }
    }

    static async Task<int> ShowMenu()
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
        Console.WriteLine("9. Test Binary Search - Best Case");
        Console.WriteLine("10. Test Binary Search - Average Case");
        Console.WriteLine("11. Test Binary Search - Worst Case");
        Console.WriteLine("12. Test Linear Search - Best Case");
        Console.WriteLine("13. Test Linear Search - Average Case");
        Console.WriteLine("14. Test Linear Search - Worst Case");
        Console.WriteLine("15. Test Lambda Search - Best Case");
        Console.WriteLine("16. Test Lambda Search - Average Case");
        Console.WriteLine("17. Test Lambda Search - Worst Case");
        Console.WriteLine("18. Process Employees File");
        Console.WriteLine("19. Run Automated Stopwatch Sort Performance Test");
        Console.WriteLine("20. Run Automated Stopwatch Search Performance Test");
        Console.WriteLine("21. Run Automated BechmarkDotNet Sort Performance Test");
        Console.WriteLine("22. Run Automated BechmarkDotNet Search Performance Test");
        Console.WriteLine("23. Exit");

        if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > 23)
        {
            Console.WriteLine("Invalid input. Please enter a number between 1 and 23.");
            return await ShowMenu(); // Recursively call ShowMenu for invalid input.
        }

        return choice;
    }

    static async Task PerformOperation(int choice, bool useAsync)
    {
        int[] array;
        int target = 0; // Initialize target to 0
        int arraySize;

        // For sorting operations, prompt for array size
        if (choice >= 1 && choice <= 6)
        {
            Console.WriteLine("Enter the size of the array for sorting operations:");
            if (!int.TryParse(Console.ReadLine(), out arraySize) || arraySize <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a positive number.");
                return; 
            }
            array = Algo.Prepare(arraySize); // Prepare an array for sorting
        }
        else if(choice >= 7 && choice <= 8)
        {
            arraySize = 100000; // Default size for search operations
            array = Algo.Prepare(arraySize); // Prepare an array for searching
            Console.WriteLine("Enter the target number to search for:");
            if (!int.TryParse(Console.ReadLine(), out target) || target < 0 || target >= arraySize)
            {
                Console.WriteLine("Invalid input. Please enter a number between 0 and " + (arraySize - 1) + ".");
                return; 
            }
        }
        else {
            arraySize = 100000; // Default size for search operations
            array = Algo.Prepare(arraySize); // Prepare an array for searching
        }

        switch (choice)
        {
            case 1:
                await HandleSortOperation(array, Algo.BubbleSort, "Bubble Sort", useAsync);
                break;
            case 2:
                await HandleSortOperation(array, Algo.InsertionSort, "Insertion Sort", useAsync);
                break;
            case 3:
                await HandleSortOperation(array, Algo.SelectionSort, "Selection Sort", useAsync);
                break;
            case 4:
                await HandleSortOperation(array, Algo.MergeSort, "Merge Sort", useAsync);
                break;
            case 5:
                await HandleSortOperation(array, Algo.QuickSort, "Quick Sort", useAsync);
                break;
            case 6:
                await HandleSortOperation(array, Algo.SortByLambda, "Sort By Lambda", useAsync);
                break;
            case 7:
                await HandleSearchOperation(array, Algo.LinearSearch, "Linear Search", target, useAsync);
                break;
            case 8:
                await HandleSearchOperation(array, Algo.BinarySearch, "Binary Search", target, useAsync);
                break;
            case 9:
                target = array[array.Length / 2]; 
                await HandleSearchOperation(array, Algo.BinarySearch, "Binary Search - Best Case", target, useAsync);
                break;
            case 10:
                target = array[new Random().Next(0, array.Length)]; 
                await HandleSearchOperation(array, Algo.BinarySearch, "Binary Search - Average Case", target, useAsync);
                break;
            case 11: 
                target = array[array.Length - 1];
                await HandleSearchOperation(array, Algo.BinarySearch, "Binary Search - Worst Case", target, useAsync);
                break;
            case 12: 
                target = array[0]; 
                await HandleSearchOperation(array, Algo.LinearSearch, "Linear Search - Best Case", target, useAsync);
                break;
            case 13: 
                target = array[array.Length / 2]; 
                await HandleSearchOperation(array, Algo.LinearSearch, "Linear Search - Average Case", target, useAsync);
                break;
            case 14: 
                target = array[array.Length - 1]; 
                await HandleSearchOperation(array, Algo.LinearSearch, "Linear Search - Worst Case", target, useAsync);
                break;
            case 15: 
                target = array[0]; 
                await HandleSearchOperation(array, (arr, tgt) => Array.FindIndex(arr, item => item == tgt), "Lambda Search - Best Case", target, useAsync);
                break;
            case 16: 
                target = array[array.Length / 2]; 
                await HandleSearchOperation(array, (arr, tgt) => Array.FindIndex(arr, item => item == tgt), "Lambda Search - Average Case", target, useAsync);
                break;
            case 17:
                target = array[array.Length - 1];
                await HandleSearchOperation(array, (arr, tgt) => Array.FindIndex(arr, item => item == tgt), "Lambda Search - Worst Case", target, useAsync);
                break;
            case 18:
                await ProcessEmployeesFile();
                break;
            case 19:
                await RunAutomatedSortPerformanceTest(useAsync);
                break;
            case 20:
                await RunAutomatedSearchPerformanceTest(useAsync);
                break;
            case 21:
                BenchmarkRunner.Run<SortBenchmarkDotnet>();
                break;
            case 22:
                BenchmarkRunner.Run<SearchAlgorithmsBenchmark>();
                break;

        }
    }

    static async Task HandleSortOperation(int[] array, Func<int[], int[]> operation, string description, bool useAsync)
    {
        SortingAlgorithmDelegate sortDelegate = arr => operation(arr);

        if (useAsync)
        {
            await Algo.DisplayRunningTime(array, sortDelegate, description, useAsync);
        }
        else
        {
            Algo.DisplayRunningTime(array, sortDelegate, description, useAsync).GetAwaiter().GetResult();
        }
    }


    static async Task HandleSearchOperation(int[] array, Func<int[], int, int> searchOperation, string description, int target, bool useAsync)
    {
        SearchAlgorithmDelegate searchDelegate = (arr, tgt) => searchOperation(arr, tgt);

        if (useAsync)
        {
            await Algo.DisplayRunningTime(array, searchDelegate, description, useAsync, target);
        }
        else
        {
            Algo.DisplayRunningTime(array, searchDelegate, description, useAsync, target).GetAwaiter().GetResult();
        }
    }


    static async Task RunAutomatedSortPerformanceTest(bool useAsync)
    {
        var arraySizes = new int[] { 100, 5000, 100000, 1000000 };
        var sortingAlgorithms = new Dictionary<string, Func<int[], int[]>> {
            { "Bubble Sort", Algo.BubbleSort },
            { "Insertion Sort", Algo.InsertionSort },
            { "Selection Sort", Algo.SelectionSort },
            { "Merge Sort", Algo.MergeSort },
            { "Quick Sort", Algo.QuickSort },
            { "Sort By Lambda", Algo.SortByLambda }
        };

        foreach (int size in arraySizes)
        {
            Console.WriteLine($"Performance for array size {size}:");
            int[] testArray = Algo.Prepare(size);

            foreach (var algorithm in sortingAlgorithms)
            {
                SortingAlgorithmDelegate sortDelegate = arr => algorithm.Value(arr);
                Console.WriteLine($"Testing {algorithm.Key} for array size {size}");

                if (useAsync)
                {
                    await Algo.DisplayRunningTime(testArray, sortDelegate, algorithm.Key, useAsync);
                }
                else
                {
                    Algo.DisplayRunningTime(testArray, sortDelegate, algorithm.Key, useAsync).GetAwaiter().GetResult();
                }
            }
        }
    }

    static async Task RunAutomatedSearchPerformanceTest(bool useAsync)
    {
        // Prepare a random array
        int[] array = Algo.Prepare(10000000); // Adjust the array size as needed

        // Define targets for best, average, and worst case scenarios
        int bestCaseTarget = array[0]; // First element
        int averageCaseTarget = array[array.Length / 2]; // Middle element
        int worstCaseTarget = array[array.Length - 1]; // Last element

        // Perform Linear Search without sorting
        await HandleSearchOperation(array, Algo.LinearSearch, "Linear Search - Best Case", bestCaseTarget, useAsync);
        await HandleSearchOperation(array, Algo.LinearSearch, "Linear Search - Average Case", averageCaseTarget, useAsync);
        await HandleSearchOperation(array, Algo.LinearSearch, "Linear Search - Worst Case", worstCaseTarget, useAsync);


        // Sort the array once for Binary and Lambda Search
        Algo.MergeSort(array);

        // Run Binary and Lambda Search
        await HandleSearchOperation(array, Algo.BinarySearch, "Binary Search - Best Case", bestCaseTarget, useAsync);
        await HandleSearchOperation(array, Algo.BinarySearch, "Binary Search - Average Case", averageCaseTarget, useAsync);
        await HandleSearchOperation(array, Algo.BinarySearch, "Binary Search - Worst Case", worstCaseTarget, useAsync);
        await HandleSearchOperation(array, Algo.LambdaSearch, "Lambda Search - Best Case", bestCaseTarget, useAsync);
        await HandleSearchOperation(array, Algo.LambdaSearch, "Lambda Search - Average Case", averageCaseTarget, useAsync);
        await HandleSearchOperation(array, Algo.LambdaSearch, "Lambda Search - Worst Case", worstCaseTarget, useAsync);
    }



    public static List<Employee> FilterEmployeesByName(List<Employee> employees)
    {
        return employees.Where(employee => employee.GetName().Contains("an")).ToList();
    }

    public static List<string> MapEmployeesToNames(List<Employee> employees)
    {
        return employees.Select(employee => employee.GetName()).ToList();
    }

    public static int ReduceTotalYearsOfExperience(List<Employee> employees)
    {
        return employees.Sum(employee => employee.YearsOfExperience);
    }

    public static List<Employee> ParseEmployeeFile(string filePath)
    {
        var employees = new List<Employee>();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var parts = line.Split('|');
            if (parts.Length == 3)
            {
                var employee = new Employee();
                employee.SetName(parts[0].Trim());
                employee.SetDepartment(parts[1].Trim());
                employee.YearsOfExperience = int.Parse(parts[2].Trim());

                employees.Add(employee);
            }
        }

        return employees;
    }


    static Task ProcessEmployeesFile()
    {
        string filePath = @"..\..\Employees.txt"; // Relative path to the project

        try
        {
            var employees = ParseEmployeeFile(filePath);
            var filteredEmployees = FilterEmployeesByName(employees);
            var employeeNames = MapEmployeesToNames(employees);
            var totalExperience = ReduceTotalYearsOfExperience(employees);

            Console.WriteLine("Filtered Employees (contain 'an' in name):");
            foreach (var emp in filteredEmployees)
            {
                Console.WriteLine(emp.GetName());
            }

            Console.WriteLine("\nEmployee Names:");
            foreach (var name in employeeNames)
            {
                Console.WriteLine(name);
            }

            Console.WriteLine($"\nTotal Years of Experience: {totalExperience}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing file: {ex.Message}");
        }

        return Task.CompletedTask;
    }
}
