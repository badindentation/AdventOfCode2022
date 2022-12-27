using System.Reflection;

namespace AdventOfCode2022;

class Program
{

    static void Main(string[] args)
    {
        Type[] problemList = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => x.Name.StartsWith("Day"))
            .OrderBy(x => int.Parse(x.Name.Substring(3)))
            .ToArray();

        if (args.Length < 1)
        {
            ISet<int> doneDays = problemList.Select(x => int.Parse(x.Name.Substring(3))).Order().ToHashSet();
            Console.WriteLine("Please specify a day to run.");
            Console.WriteLine("Possible days:");
            for (int i = 0; i < 25; i++)
            {
                Console.Write(doneDays.Contains(i + 1) ? $"{(i + 1),2} " : "   ");
                if ((i + 1) % 5 == 0)
                {
                    Console.WriteLine();
                }
            }
            // Console.WriteLine("{0}", string.Join(", ", problemList.Select(x => int.Parse(x.Name.Substring(3))).Order()));
            return;
        }

        bool test = args.Length > 1 && (args[1] == "test" || args[1] == "t" || args[1] == "true");
        int day;

        if (args[0].Equals("all"))
        {
            foreach (Type type in problemList)
            {
                day = int.Parse(type.Name.Substring(3));
                TryRunProblem(day, problemList, test);
            }
            return;
        }

        day = int.Parse(args[0]);

        string customInput = args.Length >= 2 && !test ? args[1] : null;
        TryRunProblem(day, problemList, test, customInput);
    }

    private static void TryRunProblem(int day, Type[] problemList, bool test, string additionalInput = null)
    {
        string inputPath = test ? $"Day{day}/test.txt" : $"Day{day}/input.txt";

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

    private static void PrintProblemOutput(Problem problem)
    {
        Console.WriteLine(problem.GetType().Name);
        string part1 = problem.SolvePart1();
        string part2 = problem.SolvePart2();

        Console.WriteLine($"Part 1: {part1}");
        Console.WriteLine($"Part 2: {part2}");
    }

    internal struct Options
    {
        public bool DisplayOutput = false;
        public bool UsingTestInput = false;
        public bool RunAllProblems = false;

        public Options()
        { }
    }
}
