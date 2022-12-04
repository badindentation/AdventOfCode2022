namespace AdventOfCode2022.Day4;
class Day4 : Problem
{
    public Day4(string input) : base(input)
    {
    }
    public override string SolvePart1()
    {
        string[] lines = input.Split(Environment.NewLine);
        int total = 0;
        foreach (string line in lines) {
            string[] parts = line.Split(",");
            int[] l = parts[0].Split("-").Select(x => int.Parse(x)).ToArray();
            int[] r = parts[1].Split("-").Select(x => int.Parse(x)).ToArray();

            if ((l[0] >= r[0] && l[1] <= r[1]) || (r[0] >= l[0] && r[1] <= l[1])) {
                total++;
            }
        }
        string result = total.ToString();
        return result;
    }

    public override string SolvePart2()
    {
        string[] lines = input.Split(Environment.NewLine);
        int total = 0;
        foreach (string line in lines) {
            string[] parts = line.Split(",");
            int[] l = parts[0].Split("-").Select(x => int.Parse(x)).ToArray();
            int[] r = parts[1].Split("-").Select(x => int.Parse(x)).ToArray();

            if ((l[0] >= r[0] && l[0] <= r[1]) || l[1] >= r[0] && l[1] <= r[1]) {
                total++;
            } else if (r[0] >= l[0] && r[0] <= l[1] || r[1] >= l[0] && r[1] <= l[1]) {
                total++;
            }
        }
        string result = total.ToString();
        return result;
    }
}