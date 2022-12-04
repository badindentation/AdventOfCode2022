namespace AdventOfCode2022.Day1;

class Day1 : Problem {

	public Day1(string input) : base(input) 
	{
	}
    public override string SolvePart1()
    {
        string[] elves = input.Split("\n\n");
		IList<int> calories = new List<int>();
		foreach (string elf in elves) {
			calories.Add(elf.Split("\n").Select(x => int.Parse(x)).Sum());
		}
		return calories.OrderDescending().First().ToString();
    }
    public override string SolvePart2()
    {
        string[] elves = input.Split("\n\n");
		IList<int> calories = new List<int>();
		foreach (string elf in elves) {
			calories.Add(elf.Split("\n").Select(x => int.Parse(x)).Sum());
		}
		return calories.OrderDescending().Take(3).Sum().ToString();
    }
}
