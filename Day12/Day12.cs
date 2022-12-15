namespace AdventOfCode2022.Day12;

class Day12 : Problem
{
    public Day12(string input) : base(input)
    {
    }

    internal struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }

    internal class Map
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        public char[,] Grid { get; set; }
    }

    internal Map ParseMap(string all_lines)
    {
        string[] lines = all_lines.TrimEnd().Split(Environment.NewLine);
        int width = lines[0].Length;
        int height = lines.Length;

        char[,] grid = new char[width, height];

        Point start = new Point(0, 0);
        Point end = new Point(0, 0);

        for (int y = 0; y < lines.Length; ++y)
        {
            string line = lines[y];
            for (int x = 0; x < line.Length; ++x)
            {
                grid[x, y] = line[x];
                switch (line[x])
                {
                    case 'S':
                        start = new Point(x, y);
                        break;
                    case 'E':
                        end = new Point(x, y);
                        break;
                }
            }

        }

        return new Map
        {
            Start = start,
            End = end,
            Grid = grid,
        };
    }

    public IEnumerable<Point> GetNeighbours(Point p, char[,] grid)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };
        for (int i = 0; i < 4; ++i)
        {
            int nx = p.X + dx[i];
            int ny = p.Y + dy[i];
            if (nx >= 0 && nx < width && ny >= 0 && ny < height)
            {
                yield return new Point(nx, ny);
            }
        }
    }

    private char GetElevation(char c)
    {
        switch (c)
        {
            case 'S':
                return 'a';
            case 'E':
                return 'z';
            default:
                return c;
        }
    }

    public override string SolvePart1()
    {
        Map map = ParseMap(input);

        // Do BFS
        var queue = new Queue<Point>();
        queue.Enqueue(map.Start);

        IDictionary<Point, int> distances = new Dictionary<Point, int>();
        distances[map.Start] = 0;

        Point end = new Point(-1, -1);
        while (queue.Count != 0)
        {
            // Get current point
            Point p = queue.Dequeue();
            if (map.Grid[p.X, p.Y] == 'E')
            {
                end = new Point(p.X, p.Y);
                break;
            }
            char cur = GetElevation(map.Grid[p.X, p.Y]);
            foreach (Point neighbour in GetNeighbours(p, map.Grid))
            {
                if (distances.ContainsKey(neighbour)) continue;

                char next = GetElevation(map.Grid[neighbour.X, neighbour.Y]);
                if (next - cur > 1) continue;

                queue.Enqueue(neighbour);
                distances.Add(neighbour, distances[p] + 1);
            }
        }
        return distances[end].ToString();
    }

    public override string SolvePart2()
    {
        Map map = ParseMap(input);

        var queue = new Queue<Point>();
        queue.Enqueue(map.End);

        IDictionary<Point, int> distances = new Dictionary<Point, int>();
        distances[map.End] = 0;

        Point end = new Point(-1, -1);
        while (queue.Count != 0)
        {
            // Get current point
            Point p = queue.Dequeue();

            char val = map.Grid[p.X, p.Y];
            if (val == 'S' || val == 'a')
            {
                end = new Point(p.X, p.Y);
                break;
            }
            char cur = GetElevation(map.Grid[p.X, p.Y]);
            foreach (Point neighbour in GetNeighbours(p, map.Grid))
            {
                if (distances.ContainsKey(neighbour)) continue;

                char next = GetElevation(map.Grid[neighbour.X, neighbour.Y]);
                if (cur - next > 1) continue;

                queue.Enqueue(neighbour);
                distances.Add(neighbour, distances[p] + 1);
            }
        }
        return distances[end].ToString();
    }
}
