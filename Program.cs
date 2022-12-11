using System.Reflection;

namespace AdventOfCode2022;

class Program {
    static void Main(string[] args) {
        Type[] problemList = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => x.Name.StartsWith("Day"))
            .OrderBy(x => int.Parse(x.Name.Substring(3)))
            .ToArray();

        if (args.Length < 1) 
        {
            Console.WriteLine("Please specify a day to run.");
            Console.WriteLine("Possible days:");
            Console.WriteLine("{0}", string.Join(", ", problemList.Select(x => int.Parse(x.Name.Substring(3))).Order()));
            return;
        }

        int day;

        if (args[0].Equals("all")) 
        {
            foreach (Type type in problemList) 
            {
                day = int.Parse(type.Name.Substring(3));
                TryRunProblem(day, problemList);
            }
            return;
        }

        day = int.Parse(args[0]);

        string customInput = args.Length >= 2 ? args[1] : null; 
        TryRunProblem(day, problemList, customInput);
    }

    private static void TryRunProblem(int day, Type[] problemList, string additionalInput = null) 
    {
        string inputPath = $"Day{day}/input.txt";

        if (!File.Exists(inputPath)) 
        {
            Console.WriteLine($"Could not read \'{inputPath}\'.");
            return;
        }

        Type type = problemList.First(x => x.Name == $"Day{day}");
        string input = additionalInput == null ? File.ReadAllText(inputPath) : additionalInput;

        if (Activator.CreateInstance(type, input) is Problem problem)
        {
            PrintProblemOutput(problem);
        } 
        else 
        {
            Console.WriteLine($"Instance of {type.Name} could not be created.");
            return;
        }
    }

    private static void PrintProblemOutput(Problem problem) {
        Console.WriteLine(problem.GetType().Name);
        
        string part1 = problem.SolvePart1();
        string part2 = problem.SolvePart2();

        Console.WriteLine($"Part 1: {part1}");
        Console.WriteLine($"Part 2: {part2}");
        Console.WriteLine();
    }
}
