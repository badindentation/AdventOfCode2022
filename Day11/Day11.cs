using System.Text.RegularExpressions;

namespace AdventOfCode2022.Day11;

class Day11 : Problem {

	public Day11(string input) : base(input) 
	{
	}

    internal class Monkey {
        public List<long> Items { get; set; }
        public Func<long, long> Operation { get; set; }
        public int Divisor { get; set; }
        public int IfTrueIndex { get; set; }
        public int IfFalseIndex { get; set; }
        public long Inspected { get; set; }
    }

    private IList<Monkey> ParseMonkeys(string[] monkeys) {
        IList<Monkey> monkeyList = new List<Monkey>();

        foreach (string info in monkeys) 
        {
            Monkey monkey = new Monkey();
            string[] lines = info.Split(Environment.NewLine);
            
            // First get items
            monkey.Items = lines[1].Split(':', ',').Skip(1).Select(x => long.Parse(x.Trim())).ToList();
            // Console.WriteLine("Items: {0}", String.Format("{0}", string.Join(", ", monkey.Items)));
            
            // Then get operation
            Match match = Regex.Match(lines[2], @"([\+\*]) ([\w\d]+)");
            string op = match.Groups[1].ToString();
            string value = match.Groups[2].ToString();
            // Console.WriteLine("Operation: {0}", op);
            // Console.WriteLine("Value: {0}", value);
            
            if (value != "old") 
            {
                monkey.Operation = op switch {
                    "+" => x => x + long.Parse(value),
                    "*" => x => x * long.Parse(value),
                    _ => throw new Exception("Invalid operation")
                };
            } 
            else 
            {
                monkey.Operation = op switch {
                    "+" => x => x + x,
                    "*" => x => x * x,
                    _ => throw new Exception("Invalid operation")
                };
            }

            // Then get divisor
            monkey.Divisor = int.Parse(Regex.Match(lines[3], "divisible by (\\d+)").Groups[1].ToString());
            // Console.WriteLine("Divisor: {0}", monkey.Divisor);

            // Then get if true and if false
            monkey.IfTrueIndex = int.Parse(Regex.Match(lines[4], "If true: throw to monkey (\\d+)").Groups[1].ToString());
            // Console.WriteLine("IfTrueIndex: {0}", monkey.IfTrueIndex);
            monkey.IfFalseIndex = int.Parse(Regex.Match(lines[5], "If false: throw to monkey (\\d+)").Groups[1].ToString());
            // Console.WriteLine("IfFalseIndex: {0}", monkey.IfFalseIndex);
            // Console.WriteLine("-----------------------");

            monkey.Inspected = 0;
            monkeyList.Add(monkey);
        }

        return monkeyList;
    }

    public override string SolvePart1()
    {
        string[] monkeys = input.TrimEnd().Split(Environment.NewLine + Environment.NewLine);
        
        IList<Monkey> monkeyList = ParseMonkeys(monkeys);

        int rounds = 20;
        for (int i = 1; i <= rounds; ++i) 
        {
            foreach (var monkey in monkeyList) {
                foreach (var item in monkey.Items.ToList()) {
                    monkey.Items.Remove(item);
                    long worry = monkey.Operation(item);
                    worry /= 3;
                    if (worry % monkey.Divisor == 0) {
                        monkeyList[monkey.IfTrueIndex].Items.Add(worry);
                    } else {
                        monkeyList[monkey.IfFalseIndex].Items.Add(worry);
                    }
                    monkey.Inspected++;
                }
            }
        }

        var x = monkeyList.Select(x => x.Inspected).OrderDescending().Take(2).Aggregate((x, y) => x * y);
        return x.ToString();
    }

    public override string SolvePart2()
    {        
        string[] monkeys = input.TrimEnd().Split(Environment.NewLine + Environment.NewLine);
        IList<Monkey> monkeyList = ParseMonkeys(monkeys);

        int divisorProduct = monkeyList.Select(x => x.Divisor).Aggregate((x, y) => x * y);
        int rounds = 10_000;
        for (int i = 1; i <= rounds; ++i) 
        {
            foreach (var monkey in monkeyList) {
                foreach (var item in monkey.Items.ToList()) {
                    monkey.Items.Remove(item);
                    long worry = monkey.Operation(item);
                    // worry /= 3;
                    worry %= divisorProduct;
                    if (worry % monkey.Divisor == 0) {
                        monkeyList[monkey.IfTrueIndex].Items.Add(worry);
                    } else {
                        monkeyList[monkey.IfFalseIndex].Items.Add(worry);
                    }
                    monkey.Inspected++;
                }
            }
        }

        var x = monkeyList.Select(x => x.Inspected).OrderDescending().Take(2).Aggregate((x, y) => x * y);
        return x.ToString();
    }
}
