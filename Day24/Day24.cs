using System.Text;

namespace AdventOfCode2022.Day24;

class Day24 : Problem {

	public Day24(string input) : base(input) 
	{
	}

    public ISet<(int x, int y)> NextTurn(IList<((int x, int y), char)> blizzards, int width, int height)
    {
        ISet<(int x, int y)> next = new HashSet<(int x, int y)>(blizzards.Count);

        foreach (var kvp in blizzards.ToList()) 
        {
            blizzards.Remove(kvp);
            (int x, int y) pos = kvp.Item1;
            (int x, int y) newPos = pos;
            switch (kvp.Item2)
            {
                case 'v':
                    newPos.y = (newPos.y + 1) == height - 1 ? 1 : newPos.y + 1;
                    break;
                case '>':
                    newPos.x = (newPos.x + 1) == width - 1 ? 1 : newPos.x + 1;
                    break;
                case '<':
                    newPos.x = (newPos.x - 1 == 0) ? width - 2 : newPos.x - 1;
                    break;
                case '^':
                    newPos.y = (newPos.y - 1) == 0 ? height - 2 : newPos.y - 1;
                    break;
                default:
                    throw new Exception("Invalid blizzard type");
            }
            blizzards.Add((newPos, kvp.Item2));
            next.Add(newPos);
        }

        return next;
    }

    private static string ShowBlizzards(IList<((int x, int y), char)> blizzards, ISet<(int x, int y)> walls, int width, int height)
    {
        StringBuilder builder = new StringBuilder();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (walls.Contains((x, y)))
                {
                    builder.Append('#');
                    continue;
                }
                int count = blizzards.Count(b => b.Item1 == (x, y));
                if (count == 0)
                {
                    builder.Append('.');
                } 
                else if (count == 1)
                {
                    builder.Append(blizzards.First(b => b.Item1 == (x, y)).Item2);
                }
                else
                {
                    builder.Append(count);
                }
            }
            builder.AppendLine();
        }
        return builder.ToString();
    }

    public int BFS(IList<((int x, int y), char)> blizzards, ISet<(int x, int y)> walls, int width, int height, (int x, int y) start, (int x, int y) end)
    {
        int steps = 0;  
        ISet<(int x, int y)> queue = new HashSet<(int x, int y)>();
        queue.Add(start);

        while (queue.Count > 0)
        {
            if (queue.Contains(end))
            {
                return steps;
            }

            var positions = queue.ToList();
            queue.Clear();
            // Console.WriteLine(ShowBlizzards(blizzards, walls, width, height));

            ISet<(int x, int y)> blocked = NextTurn(blizzards, width, height);
            int size = queue.Count;

            // Console.WriteLine($"Positions: {string.Join(",",positions)}");
            // Console.WriteLine($"Blizzards: {string.Join(",",blocked)}");
            foreach (var pos in positions)
            {       
       
                foreach (var dir in new (int x, int y)[] { (0, 1), (0, -1), (1, 0), (-1, 0), (0, 0) })
                {
                    (int x, int y) newPos = (pos.x + dir.x, pos.y + dir.y);

                    if (walls.Contains(newPos) || blocked.Contains(newPos))
                    {
                        continue;
                    }
                    queue.Add(newPos);
                }
     
            }
            steps++;    
        }
        return steps;
    }

    public override string SolvePart1()
    {
        string[] lines = input.TrimEnd().Split(Environment.NewLine);

        int width = lines[0].Length;
        int height = lines.Length;

        IList<((int x, int y), char)> initial_blizzard_positions = new List<((int x, int y), char)>();
        ISet<(int x, int y)> walls = new HashSet<(int x, int y)>();


        (int x, int y) start = (lines[0].Select((c, i) => (c, i)).First(x => x.c == '.').i, 0);
        (int x, int y) end = (lines[^1].Select((c, i) => (c, i)).First(x => x.c == '.').i, lines.Length - 1);

        // Console.WriteLine($"Start: {start}, End: {end}");
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if ("<>^v".Contains(lines[y][x]))
                {
                    initial_blizzard_positions.Add(((x, y), lines[y][x]));
                } 
                else if (lines[y][x] == '#')
                {
                    walls.Add((x, y));
                }
            }
        }
        walls.Add((start.x, start.y - 1));
        Console.WriteLine($"Initial: {initial_blizzard_positions.Count}");
        // Console.WriteLine(string.Join(", ", initial_blizzard_positions.Select(x => $"{x.Item1} {x.Item2}")));
        return BFS(initial_blizzard_positions, walls, width, height, start, end).ToString();
    }

    public override string SolvePart2()
    {
        string[] lines = input.TrimEnd().Split(Environment.NewLine);

        int width = lines[0].Length;
        int height = lines.Length;

        IList<((int x, int y), char)> initial_blizzard_positions = new List<((int x, int y), char)>();
        ISet<(int x, int y)> walls = new HashSet<(int x, int y)>();

        (int x, int y) start = (lines[0].Select((c, i) => (c, i)).First(x => x.c == '.').i, 0);
        (int x, int y) end = (lines[^1].Select((c, i) => (c, i)).First(x => x.c == '.').i, lines.Length - 1);

        // Console.WriteLine($"Start: {start}, End: {end}");
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if ("<>^v".Contains(lines[y][x]))
                {
                    initial_blizzard_positions.Add(((x, y), lines[y][x]));
                } 
                else if (lines[y][x] == '#')
                {
                    walls.Add((x, y));
                }
            }
        }
        walls.Add((start.x, start.y - 1));
        walls.Add((end.x, end.y + 1));

        int initial = BFS(initial_blizzard_positions, walls, width, height, start, end);
        int back = BFS(initial_blizzard_positions, walls, width, height, end, start);
        int back_with_snacks = BFS(initial_blizzard_positions, walls, width, height, start, end);
        // Console.WriteLine($"Intial: {initial}, Back: {back}, Back with snacks: {back_with_snacks}");
        return (initial + back + back_with_snacks).ToString();
    }
}
