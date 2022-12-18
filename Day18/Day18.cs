namespace AdventOfCode2022.Day18;

class Day18 : Problem {

	public Day18(string input) : base(input) 
	{
	}

    private bool[,,] ParseDroplets(string input)
    {
        string[] lines = input.TrimEnd().Split(Environment.NewLine);
        bool[,,] grid = new bool[30, 30, 30];

        int total = 0;
        int offset = 5;
        foreach (string line in lines)
        {
            string[] parts = line.Split(",");
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            int z = int.Parse(parts[2]);
            grid[x + offset, y + offset, z + offset] = true;
            total++;
        }
        return grid;
    }
    public override string SolvePart1()
    {
        bool[,,] grid = ParseDroplets(input);
 
        int count = 0;
        for (int x = 1; x < grid.GetLength(0) - 1; x++)
        {
            for (int y = 1; y < grid.GetLength(1) - 1; y++)
            {
                for (int z = 1; z < grid.GetLength(2) - 1; z++)
                {
                    if (!grid[x,y,z]) continue;

                    if (!grid[x + 1, y, z]) count++;
                    if (!grid[x - 1, y, z]) count++;
                    if (!grid[x, y + 1, z]) count++;
                    if (!grid[x, y - 1, z]) count++;
                    if (!grid[x, y, z + 1]) count++;
                    if (!grid[x, y, z - 1]) count++;
                }
            }
        }
        return count.ToString();
    }

    public void AddToQueue((int x, int y, int z) pos, bool[,,] visited, Queue<(int,int,int)> queue)
    {
        if (pos.x < 0 || pos.x >= visited.GetLength(0) || pos.y < 0 || pos.y >= visited.GetLength(1) || pos.z < 0 || pos.z >= visited.GetLength(2)) return;
        if (visited[pos.x, pos.y, pos.z]) return;
        queue.Enqueue(pos);
    }


    // Extremely rushed BFS, did this in 10 minutes.
    private int bfs(int x, int y, int z, bool[,,] grid, bool[,,] visited) {
        int count = 0;
        
        Queue<(int, int, int)> q = new Queue<(int, int, int)>();
        q.Enqueue((x, y, z));
        while (q.Count > 0) {
            (int, int, int) current = q.Dequeue();
            if (visited[current.Item1, current.Item2, current.Item3]) continue;
            
            if (grid[current.Item1, current.Item2, current.Item3]) {
                count++;
                continue;
            }
            visited[current.Item1, current.Item2, current.Item3] = true;

            AddToQueue((current.Item1 + 1, current.Item2, current.Item3), visited, q);
            AddToQueue((current.Item1 - 1, current.Item2, current.Item3), visited, q);
            AddToQueue((current.Item1, current.Item2 + 1, current.Item3), visited, q);
            AddToQueue((current.Item1, current.Item2 - 1, current.Item3), visited, q);
            AddToQueue((current.Item1, current.Item2, current.Item3 + 1), visited, q);
            AddToQueue((current.Item1, current.Item2, current.Item3 - 1), visited, q);
        }
        return count;
    }

    public override string SolvePart2()
    {
        bool[,,] grid = ParseDroplets(input);

        // Need to flood fill air pockets and count sides that are not air
        int result = 0;
        bool[,,] visited = new bool[30,30,30];

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int z = 0; z < grid.GetLength(2); z++)
                {
                    if (grid[x,y,z] || visited[x,y,z]) continue;
                    int t = bfs(x,y,z, grid, visited);
                    result = Math.Max(t,result);
                }
            }
        }

        return result.ToString();
    }
}
