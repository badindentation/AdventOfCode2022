namespace AdventOfCode2022.Day23;

class Day23 : Problem {

	public Day23(string input) : base(input) 
	{
	}

    class Map 
    {
        ISet<(int x, int y)> elves = new HashSet<(int x, int y)>(8192 * 4);

        (int x, int y) min;
        (int x, int y) max;

        (int x, int y) minForRound = (int.MaxValue, int.MaxValue);
        (int x, int y) maxForRound = (int.MinValue, int.MinValue);

        public Map(string input)
        {
            ParseInput(input);
        }

        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            for (int y = min.y; y <= max.y; y++)
            {
                builder.AppendFormat("{0,2} ", y);
                for (int x = min.x; x <= max.x; x++)
                {
                    if (elves.Contains((x, y)))
                    {
                        builder.Append('#');
                    }
                    else
                    {
                        builder.Append('.');
                    }
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }
        Queue<(Func<(int x, int y), bool>, (int nx, int ny))> queue = new Queue<(Func<(int x, int y), bool>, (int nx, int ny))>();

        private void ParseInput(string input)
        {
            string[] lines = input.TrimEnd().Split(Environment.NewLine);
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        if (y < min.y)
                            min.y = y;
                        if (y > max.y)
                            max.y = y;
                        if (x < min.x)
                            min.x = x;
                        if (x > max.x)
                            max.x = x;

                        elves.Add((x, y));
                    }
                }
            }
            Func<(int x, int y), bool> checkNorth = (elf) => !elves.Contains((elf.x, elf.y - 1)) && !elves.Contains((elf.x + 1, elf.y - 1)) && !elves.Contains((elf.x - 1, elf.y - 1));
            Func<(int x, int y), bool> checkSouth = (elf) => !elves.Contains((elf.x, elf.y + 1)) && !elves.Contains((elf.x + 1, elf.y + 1)) && !elves.Contains((elf.x - 1, elf.y + 1));
            Func<(int x, int y), bool> checkWest = (elf) => !elves.Contains((elf.x - 1, elf.y)) && !elves.Contains((elf.x - 1, elf.y + 1)) && !elves.Contains((elf.x - 1, elf.y - 1));
            Func<(int x, int y), bool> checkEast = (elf) => !elves.Contains((elf.x + 1, elf.y)) && !elves.Contains((elf.x + 1, elf.y + 1)) && !elves.Contains((elf.x + 1, elf.y - 1));
            queue.Enqueue((checkNorth, (0, -1)));
            queue.Enqueue((checkSouth, (0, 1)));
            queue.Enqueue((checkWest, (-1, 0)));
            queue.Enqueue((checkEast, (1, 0)));
        }

        public int CalculateEmpty()
        {
            int count = 0;
            for (int y = minForRound.y; y <= maxForRound.y; y++)
            {
                for (int x = minForRound.x; x <= maxForRound.x; x++)
                {
                    if (!elves.Contains((x, y)))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public bool MoveRound() 
        {
            IDictionary<(int x, int y), int> newElves = new Dictionary<(int x, int y), int>();
            IDictionary<(int x, int y), (int nx, int ny)> moves = new Dictionary<(int x, int y), (int nx, int ny)>();
            // First Generate moves for each elf
            minForRound = (int.MaxValue, int.MaxValue);
            maxForRound = (int.MinValue, int.MinValue);

            foreach (var elf in elves)
            {
                // Check every position around the elf
                int[] delta_x = new int[] { -1, 0, 1 };
                int[] delta_y = new int[] { -1, 0, 1 };
                bool spacious = true;
                for (int i = 0; i < delta_x.Length; i++)
                {
                    for (int j = 0; j < delta_y.Length; j++)
                    {
                        if (delta_x[i] == 0 && delta_y[j] == 0) continue;
                        var nx = elf.x + delta_x[i];
                        var ny = elf.y + delta_y[j];
                        if (elves.Contains((nx, ny))) {
                            spacious = false;
                        }
                    }
                }
                if (spacious) {
                    continue;
                }

                foreach (var (check, (dx, dy)) in queue)
                {
                    // Console.Write($"Checking {elf} for {nx}, {ny} - ");
                    if (check(elf))
                    {
                        var newPosition = (elf.x + dx, elf.y + dy);
                        if (newElves.ContainsKey(newPosition))
                        {
                            newElves[newPosition]++;
                        }
                        else
                        {
                            newElves[newPosition] = 1;     
                        }
                        moves[elf] = newPosition;
                        break;
                    }
                }
            }
            queue.Enqueue(queue.Dequeue());
            // Then move all elves
            int moveCount = 0;
            foreach (var elf in elves.ToList())
            {
                if (!moves.ContainsKey(elf)) continue;
                
                var currentPosition = elf;
                var nextPosition = moves[elf];
                if (newElves[nextPosition] == 1)
                {
                    moveCount++;
                    elves.Remove(currentPosition);
                    elves.Add(nextPosition);

                }
            }
            foreach (var elf in elves.ToList())
            {
                minForRound.x = Math.Min(minForRound.x, elf.x);
                maxForRound.x = Math.Max(maxForRound.x, elf.x);
                minForRound.y = Math.Min(minForRound.y, elf.y);
                maxForRound.y = Math.Max(maxForRound.y, elf.y);
            }
            min.x = Math.Min(min.x, minForRound.x);
            min.y = Math.Min(min.y, minForRound.y);
            max.x = Math.Max(max.x, maxForRound.x);
            max.y = Math.Max(max.y, maxForRound.y);

            return moveCount != 0;
        }
    }

    public override string SolvePart1()
    {
        Map map = new Map(input);
        int round = 1;
        // Console.WriteLine($"Round: {round}\n{map}");
        for (; round <= 10; round++)
        {
            map.MoveRound();
            // Console.WriteLine($"Round: {round}\n{map}");
        }
        return map.CalculateEmpty().ToString();
    }

    public override string SolvePart2()
    {
        Map map = new Map(input);
        int round = 1;
        while (map.MoveRound())
        {
            round++;
        }
        return round.ToString();
    }
}
