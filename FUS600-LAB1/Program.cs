using System.Diagnostics;
using algorithms;
using FUS600_LAB1;

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
            if (choice == 12)
            {
                break; // Exit the loop and end the program
            }

            int[] array = Algo.Prepare(100000); // Prepare an array for sorting/searching
            int target = new Random().Next(0, 100000); // Random target for search operations

            await PerformOperation(choice, array, target, useAsync);
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
        Console.WriteLine("9. Run Automated Performance Test");
        Console.WriteLine("10. Run Automated Search Test");
        Console.WriteLine("11. Process Employees File");
        Console.WriteLine("12. Exit");

        if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > 12)
        {
            Console.WriteLine("Invalid input. Please enter a number between 1 and 12.");
            return await ShowMenu(); // Recursively call ShowMenu for invalid input.
        }

        return choice;
    }

    static async Task PerformOperation(int choice, int[] array, int target, bool useAsync)
    {
        switch (choice)
        {
            case 1:
                await HandleOperation(array, Algo.BubbleSort, "Bubble Sort", useAsync);
                break;
            case 2:
                await HandleOperation(array, Algo.InsertionSort, "Insertion Sort", useAsync);
                break;
            case 3:
                await HandleOperation(array, Algo.SelectionSort, "Selection Sort", useAsync);
                break;
            case 4:
                await HandleOperation(array, Algo.MergeSort, "Merge Sort", useAsync);
                break;
            case 5:
                await HandleOperation(array, Algo.QuickSort, "Quick Sort", useAsync);
                break;
            case 6:
                await HandleOperation(array, arr => Algo.SortByLambda(arr), "Sort By Lambda", useAsync);
                break;
            case 7:
                await HandleSearchOperation(array, num => Algo.LinearSearch(num, target), "Linear Search", useAsync);
                break;
            case 8:
                await HandleSearchOperation(array, num => Algo.BinarySearch(num, target), "Binary Search", useAsync);
                break;
            case 9:
                await RunAutomatedPerformanceTest(useAsync);
                break;
            case 10:
                await RunSearchComparison(useAsync);
                break;
            case 11:
                await ProcessEmployeesFile();
                break;
        }
    }

    static async Task HandleOperation(int[] array, Action<int[]> operation, string description, bool useAsync)
    {
        var task = Algo.DisplayRunningTime(array, arr => { operation(arr); return (object)null; }, description);

        if (useAsync)
        {
            await task;
        }
        else
        {
            task.Wait(); // This will block until the task is completed
        }
    }

    static async Task HandleSearchOperation(int[] array, Func<int[], int> searchOperation, string description, bool useAsync)
    {
        var task = Algo.DisplayRunningTime(array, arr => searchOperation(arr), description);

        if (useAsync)
        {
            await task;
        }
        else
        {
            task.Wait(); // This will block until the task is completed
        }
    }

    static async Task RunAutomatedPerformanceTest(bool useAsync)
    {
        var arraySizes = new int[] { 100, 5000, 100000, 1000000 };
        var sortingAlgorithms = new Dictionary<string, Action<int[]>> {
        { "Bubble Sort", Algo.BubbleSort },
        { "Insertion Sort", Algo.InsertionSort },
        { "Selection Sort", Algo.SelectionSort },
        { "Merge Sort", Algo.MergeSort },
        { "Quick Sort", Algo.QuickSort },
        { "Sort By Lambda", arr => Algo.SortByLambda(arr) }
    };

        foreach (int size in arraySizes)
        {
            Console.WriteLine($"Performance for array size {size}:");
            int[] testArray = Algo.Prepare(size);

            var tasks = new List<Task>();
            foreach (var algorithm in sortingAlgorithms)
            {
                int[] tempArray = (int[])testArray.Clone();
                tasks.Add(RunSortAndDisplayAsync(tempArray, algorithm.Key, algorithm.Value, useAsync));
            }

            await Task.WhenAll(tasks);
        }
    }

    static async Task RunSortAndDisplayAsync(int[] array, string algorithmName, Action<int[]> sortingAlgorithm, bool useAsync)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        if (useAsync)
        {
            await Task.Run(() => sortingAlgorithm(array));
        }
        else
        {
            sortingAlgorithm(array);
        }

        stopwatch.Stop();
        Console.WriteLine($"{algorithmName}: {stopwatch.Elapsed.TotalMilliseconds} ms");
    }


    static async Task RunSearchComparison(bool useAsync)
    {
        // Prepare a random array
        int[] array = Algo.Prepare(10000000); // Adjust the array size as needed

        // Define targets for best, average, and worst case scenarios
        int bestCaseTarget = array[0]; // First element
        int averageCaseTarget = array[array.Length / 2]; // Middle element
        int worstCaseTarget = array[array.Length - 1]; // Last element

        // Perform Linear Search without sorting
        if (useAsync)
        {
            await Algo.DisplayRunningTime(array, arr => Algo.LinearSearch(arr, bestCaseTarget), "Linear Search - Best Case");
            await Algo.DisplayRunningTime(array, arr => Algo.LinearSearch(arr, averageCaseTarget), "Linear Search - Average Case");
            await Algo.DisplayRunningTime(array, arr => Algo.LinearSearch(arr, worstCaseTarget), "Linear Search - Worst Case");
        }
        else
        {
            Algo.DisplayRunningTime(array, arr => Algo.LinearSearch(arr, bestCaseTarget), "Linear Search - Best Case").GetAwaiter().GetResult(); ;
            Algo.DisplayRunningTime(array, arr => Algo.LinearSearch(arr, averageCaseTarget), "Linear Search - Average Case").GetAwaiter().GetResult(); ;
            Algo.DisplayRunningTime(array, arr => Algo.LinearSearch(arr, worstCaseTarget), "Linear Search - Worst Case").GetAwaiter().GetResult(); ;
        }

        // Sort the array once for Binary and Lambda Search
        int[] sortedArray = (int[])array.Clone();
        Algo.MergeSort(sortedArray);

        // Perform Binary and Lambda Search
        if (useAsync)
        {
            await Algo.DisplayRunningTime(sortedArray, arr => Algo.BinarySearch(arr, bestCaseTarget), "Binary Search - Best Case");
            await Algo.DisplayRunningTime(sortedArray, arr => Algo.BinarySearch(arr, averageCaseTarget), "Binary Search - Average Case");
            await Algo.DisplayRunningTime(sortedArray, arr => Algo.BinarySearch(arr, worstCaseTarget), "Binary Search - Worst Case");
            await Algo.DisplayRunningTime(sortedArray, arr => Algo.LambdaSearch(arr, bestCaseTarget), "Lambda Search - Best Case");
            await Algo.DisplayRunningTime(sortedArray, arr => Algo.LambdaSearch(arr, averageCaseTarget), "Lambda Search - Average Case");
            await Algo.DisplayRunningTime(sortedArray, arr => Algo.LambdaSearch(arr, worstCaseTarget), "Lambda Search - Worst Case");
        }
        else
        {
            Algo.DisplayRunningTime(sortedArray, arr => Algo.BinarySearch(arr, bestCaseTarget), "Binary Search - Best Case").GetAwaiter().GetResult(); ;
            Algo.DisplayRunningTime(sortedArray, arr => Algo.BinarySearch(arr, averageCaseTarget), "Binary Search - Average Case").GetAwaiter().GetResult(); ;
            Algo.DisplayRunningTime(sortedArray, arr => Algo.BinarySearch(arr, worstCaseTarget), "Binary Search - Worst Case").GetAwaiter().GetResult(); ;
            Algo.DisplayRunningTime(sortedArray, arr => Algo.LambdaSearch(arr, bestCaseTarget), "Lambda Search - Best Case").GetAwaiter().GetResult(); ;
            Algo.DisplayRunningTime(sortedArray, arr => Algo.LambdaSearch(arr, averageCaseTarget), "Lambda Search - Average Case").GetAwaiter().GetResult(); ;
            Algo.DisplayRunningTime(sortedArray, arr => Algo.LambdaSearch(arr, worstCaseTarget), "Lambda Search - Worst Case").GetAwaiter().GetResult(); ;
        }
    }

    public static List<Employee> FilterEmployeesByName(List<Employee> employees)
    {
        return employees.Where(employee => employee.Name.Contains("an")).ToList();
    }

    public static List<string> MapEmployeesToNames(List<Employee> employees)
    {
        return employees.Select(employee => employee.Name).ToList();
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
                employees.Add(new Employee
                {
                    Name = parts[0].Trim(),
                    Department = parts[1].Trim(),
                    YearsOfExperience = int.Parse(parts[2].Trim())
                });
            }
        }

        return employees;
    }

    static Task ProcessEmployeesFile()
    {
        // Assuming the file path is known
        string filePath = @"..\..\..\Employees.txt"; // Change this to the actual file path

        try
        {
            var employees = ParseEmployeeFile(filePath);
            var filteredEmployees = FilterEmployeesByName(employees);
            var employeeNames = MapEmployeesToNames(employees);
            var totalExperience = ReduceTotalYearsOfExperience(employees);

            Console.WriteLine("Filtered Employees (contain 'an' in name):");
            foreach (var emp in filteredEmployees)
            {
                Console.WriteLine(emp.Name);
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
