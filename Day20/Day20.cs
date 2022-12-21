namespace AdventOfCode2022.Day20;

class Day20 : Problem {

	public Day20(string input) : base(input) 
	{
	}

    public int wrap(int index, int length)
    {
        while (index < 0) index += length;
        index %= length;
        return index;
    }

    private long Mix(int key, int times)
    {
        string[] lines = input.TrimEnd().Split(Environment.NewLine);

        // Items to be mixed in starting order.
        IList<long> items = lines.Select(x => long.Parse(x) * key).ToList();

        // Console.WriteLine(string.Join(", ", items));
        // Indexes of the items in the starting order.
        IList<long> list = Enumerable.Range(0, items.Count).Select(x => (long)x).ToList();

        for (int mixes = 0; mixes < times; mixes++) {
            for (int i = 0; i < items.Count; i++)
            {
                long val = items[i];
                int oldIndex = list.IndexOf(i);

                list.RemoveAt(oldIndex);

                long newIndex = (oldIndex + val);
                newIndex %= list.Count;
                while (newIndex < 0) newIndex += list.Count;
            
                list.Insert((int) newIndex, i);

                // Console.WriteLine($"{val} moves between {items[list[wrap(newIndex - 1, list.Count)]]} and {items[list[wrap(newIndex + 1, list.Count)]]}");
                // Console.WriteLine(string.Join(", ", list));
            }
        }
        

        IList<long> list2 = list.Select(x => items[(int) x]).ToList();
        // Console.WriteLine(string.Join(", ", list2));
        
        int zero_index = list.IndexOf(items.IndexOf(0));
        long a = items[(int) list[(zero_index + 1000) % list.Count]];
        long b = items[(int) list[(zero_index + 2000) % list.Count]];
        long c = items[(int) list[(zero_index + 3000) % list.Count]];
        long next = a + b + c;

        return next;
    }
    public override string SolvePart1()
    {
        long result = Mix(1, 1);
        return result.ToString();
    }

    public override string SolvePart2()
    {
        long result = Mix(811589153, 10);
        return result.ToString();
    }
}
