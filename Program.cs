using System.Reflection;

namespace AdventOfCode2022;

class Program {
    static void Main(string[] args) {
        Type[] problemList = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.Name.StartsWith("Day")).ToArray();

        if (args.Length < 1) {
            Console.WriteLine("Please specify a day to run.");
            Console.WriteLine("Possible days:");
            Console.WriteLine("{0}", string.Join(", ", problemList.Select(x => x.Name.Substring(3)).Order()));
            return;
        }

        int day = int.Parse(args[0]);
        string inputPath = $"Day{day}/input.txt";

        if (!File.Exists(inputPath)) 
        {
            Console.WriteLine($"Could not read \'{inputPath}\'.");
            return;
        }

        string input = args.Length >= 2 ? args[1] : File.ReadAllText(inputPath);
        
        Type type = problemList.First(x => x.Name == $"Day{day}");

        if (Activator.CreateInstance(type, input) is Problem problem)
        {
            PrintProblemOutput(problem);
        } 
        else 
        {
            Console.WriteLine($"Instance of {type.Name} could not be created.");
        }
    }

    private static void PrintProblemOutput(Problem problem) {
        Console.WriteLine(problem.GetType().Name);
        
        string part1 = problem.SolvePart1();
        string part2 = problem.SolvePart2();

        Console.WriteLine($"\nPart 1: {part1}");
        Console.WriteLine($"Part 2: {part2}");
    }
}
