using System.Reflection;

namespace AdventOfCode2022;
class Program {
    static void Main(string[] args) {

        Type[] problemList = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.Name.StartsWith("Day")).ToArray();
        if (args.Length > 0) {
            int day = int.Parse(args[0]);
            string inputPath = $"day{day}/input.txt";
            if (File.Exists(inputPath)) {
                string input = File.ReadAllText(inputPath);
                Type type = problemList.First(x => x.Name == $"Day{day}");
                if (Activator.CreateInstance(type, input) is Problem problem) {
                    PrintProblemOutput(problem);
                } else {
                    Console.WriteLine($"Instance of {type.Name} could not be created");
                }
            } else {
                Console.WriteLine($"Input file for day {day} not found");
            }
        } else {
            Console.WriteLine("No day specified");
            Console.WriteLine("Possible days:");
            Console.WriteLine("{0}", string.Join(", ", problemList.Select(x => x.Name.Substring(3)).Order()));
        }
    }
    private static void PrintProblemOutput(Problem problem) {
        Console.WriteLine(problem.GetType().Name);
        Console.WriteLine("Part 1: " + problem.SolvePart1());
        Console.WriteLine("Part 2: " + problem.SolvePart2());
    }
}
