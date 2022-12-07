namespace AdventOfCode2022.Day1;

class Day6 : Problem {

	public Day6(string input) : base(input) 
	{
	}

    public override string SolvePart1()
    {
        int windowSize = 4;
        IDictionary<char, int> set = new Dictionary<char, int>(windowSize);
        for (int i = 0; i + windowSize < input.Length; i++)
        {
            next:
            for (int j = 0; j < windowSize; j++)
            {
                if (set.ContainsKey(input[i + j]))
                {
                    i = set[input[i + j]] + 1;
                    set.Clear();
                    goto next;
                }
                set.Add(input[i + j], i + j);
            }
            if (set.Count == windowSize) 
            {
                return (i + windowSize).ToString();
            }
            set.Clear();
        }
        return "Not found";
    }

    public override string SolvePart2()
    {
        int windowSize = 14;
        IDictionary<char, int> set = new Dictionary<char, int>(windowSize);
        for (int i = 0; i + windowSize < input.Length; i++)
        {
            next:
            for (int j = 0; j < windowSize; j++)
            {
                if (set.ContainsKey(input[i + j]))
                {
                    i = set[input[i + j]] + 1;
                    set.Clear();
                    goto next;
                }
                set.Add(input[i + j], i + j);
            }
            if (set.Count == windowSize) 
            {
                return (i + windowSize).ToString();
            }
            set.Clear();
        }
        return "Not found";
    }
}

