namespace AdventOfCode2022.Day1;

class Day1 : Problem {
	public Day1(string input) : base(input) 
	{
	}
	private static IEnumerable<int> ParseCalories(string raw)
	{
		string[] elves = raw.TrimEnd().Split(Environment.NewLine + Environment.NewLine);
		return elves.Select(elf => elf.Split(Environment.NewLine).Select(int.Parse).Sum()).ToList();
	}
    public override string SolvePart1()
    {
	    return ParseCalories(input).OrderDescending().First().ToString();
    }
    public override string SolvePart2()
    {
	    return ParseCalories(input).OrderDescending().Take(3).Sum().ToString();
    }
}
