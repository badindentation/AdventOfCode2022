using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Day14;

class Day14 : Problem
{

    public Day14(string input) : base(input)
    {
    }


    internal struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            this.X = x; this.Y = y;
        }

        public override string ToString()
        {
            return $"[{X},{Y}]";
        }
    }

    internal class Map
    {
        char[,] grid;

        Point topLeft;
        Point botRight;

        public int Width { get; }
        public int Height { get; }

        private bool bottom;

        public Map(Point topLeft, Point topRight) : this(topLeft, topRight, false) { }

        public Map(Point topLeft, Point botRight, bool bottom)
        {
            this.bottom = bottom;

            this.topLeft = bottom ? new Point(topLeft.X - 500, topLeft.Y) : topLeft;
            this.botRight = bottom ? new Point(botRight.X + 500, botRight.Y + 2) : botRight;

            this.Width = this.botRight.X - this.topLeft.X + 1;
            this.Height = this.botRight.Y - this.topLeft.Y + 1;

            grid = new char[Width, Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    grid[x, y] = '.';
                }
            }

            if (bottom)
            {
                SetLine(new Point[] { new Point(this.topLeft.X, this.botRight.Y), this.botRight });
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"Width: {Width} Height: {Height} hasBottom: {bottom}\n");
            for (int y = 0; y < Height; y++)
            {
                //builder.Append(string.Format("{0,-2}", y));
                for (int x = 0; x < Width; x++)
                {
                    builder.Append(grid[x, y]);
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }

        public bool InBounds(Point p)
        {
            return (p.X >= topLeft.X && p.Y >= topLeft.Y && p.X <= botRight.X && p.Y <= botRight.Y);
        }

        private (int x, int y) Translate(Point p)
        {
            return (p.X - topLeft.X, p.Y - topLeft.Y);
        }

        public void SetGrid(Point p, char c)
        {
            (int x, int y) = Translate(p);
            grid[x, y] = c;
        }

        public char GetGrid(Point p)
        {
            (int x, int y) = Translate(p);
            return grid[x, y];
        }

        public void SetLine(Point[] line)
        {
            for (int i = 0; i < line.Length - 1; i++)
            {
                Point f = line[i];
                Point t = line[i + 1];

                if (f.X == t.X)
                {
                    int fy = Math.Min(f.Y, t.Y);
                    int ty = Math.Max(f.Y, t.Y);
                    // Vertical
                    for (int y = fy; y <= ty; y++)
                    {
                        SetGrid(new Point(f.X, y), '#');
                    }
                }
                else
                {
                    int fx = Math.Min(f.X, t.X);
                    int tx = Math.Max(f.X, t.X);
                    // Horizontal
                    for (int x = fx; x <= tx; x++)
                    {
                        SetGrid(new Point(x, f.Y), '#');
                    }
                }
            }
        }

        public void SetLines(IList<Point[]> lines)
        {
            foreach (Point[] line in lines)
            {
                SetLine(line);
            }
        }
    }

    private Point[] ParseLine(string line, Regex regex)
    {
        return regex.Matches(line)
            .Select(x => new Point(
                        int.Parse(x.Groups[1].ToString()),
                        int.Parse(x.Groups[2].ToString())))
            .ToArray();
    }

    private Map ParseMap(string input, bool hasBottom)
    {
        string[] lines = input.TrimEnd().Split(Environment.NewLine);

        Regex regex = new Regex("(\\d+),(\\d+)");

        Point topL = new Point(int.MaxValue, 0);
        Point botR = new Point(int.MinValue, int.MinValue);

        IList<Point[]> rocks = new List<Point[]>();
        foreach (string line in lines)
        {
            Point[] points = ParseLine(line, regex);

            foreach (Point p in points)
            {
                topL.X = Math.Min(topL.X, p.X);

                botR.X = Math.Max(botR.X, p.X);
                botR.Y = Math.Max(botR.Y, p.Y);
            }

            // Console.WriteLine(string.Join(" -> ", points));
            rocks.Add(points);
        }

        // Console.WriteLine($"Top Left: {topL}\nBot Right: {botR}");
        Map map = new Map(topL, botR, hasBottom);
        map.SetLines(rocks);

        return map;
    }

    public override string SolvePart1()
    {
        Map map = ParseMap(input, false);
        Console.WriteLine(map);

        int sandAtRest = 0;

        Point start = new Point(500, 0);
        Point cur = start;
        while (true)
        {
            if (!map.InBounds(cur))
            {
                break;
            }

            if (map.GetGrid(start) == 'o')
            {
                break;
            }

            if (cur.Y + 1 >= map.Height)
            {
                break;
            }

            Point next = new Point(cur.X, cur.Y + 1);
            char n = map.GetGrid(next);

            if (n == '.')
            {
                cur = next;
                continue;
            }

            if (n == 'o' || n == '#')
            {
                // Check left
                Point left = new Point(next.X - 1, next.Y);
                if (!map.InBounds(left))
                {
                    break;
                }
                char l = map.GetGrid(left);

                // Console.WriteLine($"Found bottom: at {next}: {l}{n}{r}");
                if (l == '.')
                {
                    cur = left;
                    continue;
                }

                Point right = new Point(next.X + 1, next.Y);
                if (!map.InBounds(right))
                {
                    break;
                }
                char r = map.GetGrid(right);
                if (r == '.')
                {
                    cur = right;
                    continue;
                }

                // Neither left or right free
                map.SetGrid(cur, 'o');
                sandAtRest++;
                cur = start;
            }
        }

        Console.WriteLine(map.ToString());
        return sandAtRest.ToString();
    }

    public override string SolvePart2()
    {
        Map map = ParseMap(input, true);
        Console.WriteLine(map);

        int sandAtRest = 0;

        Point start = new Point(500, 0);
        Point cur = start;

        while (true)
        {
            if (!map.InBounds(cur))
            {
                break;
            }

            if (map.GetGrid(start) == 'o')
            {
                break;
            }

            if (cur.Y + 1 >= map.Height)
            {
                break;
            }

            Point next = new Point(cur.X, cur.Y + 1);
            char n = map.GetGrid(next);

            if (n == '.')
            {
                cur = next;
                continue;
            }

            if (n == 'o' || n == '#')
            {
                // Check left
                Point left = new Point(next.X - 1, next.Y);
                if (!map.InBounds(left))
                {
                    break;
                }
                char l = map.GetGrid(left);

                // Console.WriteLine($"Found bottom: at {next}: {l}{n}{r}");
                if (l == '.')
                {
                    cur = left;
                    continue;
                }

                Point right = new Point(next.X + 1, next.Y);
                if (!map.InBounds(right))
                {
                    break;
                }
                char r = map.GetGrid(right);
                if (r == '.')
                {
                    cur = right;
                    continue;
                }

                // Neither left or right free
                map.SetGrid(cur, 'o');
                sandAtRest++;
                cur = start;
            }
        }

        Console.WriteLine(map);
        return sandAtRest.ToString();
    }
}
