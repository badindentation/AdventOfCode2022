namespace AdventOfCode2022.Day9;

class Day9 : Problem {


	public Day9(string input) : base(input) 
	{
	}

    public override string SolvePart1()
    {
        (int, int) head = (0, 0);
        (int, int) head_prev = (0, 0);
        (int, int) tail = (0, 0);

        ISet<(int, int)> visited = new HashSet<(int, int)>();

        string[] instructions = input.TrimEnd().Split(Environment.NewLine);

        foreach (string instruction in instructions)
        {
            char direction = instruction[0];
            int distance = int.Parse(instruction.Substring(2));
            MoveDirection(direction, distance, ref head, ref head_prev, ref tail, visited);
        }

        return visited.Count.ToString();
    }


    private void MovePoint(char direction, ref (int, int) point) 
    {
        switch (direction) 
        {
            case 'U':
                point.Item2++;
                break;
            case 'D':
                point.Item2--;
                break;
            case 'R':
                point.Item1++;
                break;
            case 'L':
                point.Item1--;
                break;
        }
    }
    private void MoveDirection(char direction, int distance, ref (int, int) head, ref (int, int) head_prev, ref (int, int) tail, ISet<(int, int)> visited) 
    {
        for (int i = 0; i < distance; ++i)
        {
            head_prev = head;
            MovePoint(direction, ref head);
            if (Math.Abs(head.Item1 - tail.Item1) > 1 || Math.Abs(head.Item2 - tail.Item2) > 1) 
            {
                tail = head_prev;
                visited.Add(tail);
            }
        }
    }

    private void MoveUp(Point head, Point following)
    {
        if (Math.Abs(head.x - following.x) == 0 && Math.Abs(head.y - following.y) > 1)
        {
            following.y += head.y > following.y ? 1 : -1;
        }
        else if (Math.Abs(head.x - following.x) > 1 &&  Math.Abs(head.y - following.y) == 0)
        {
            following.x += head.x > following.x ? 1 : -1;
        }
        else if (Math.Abs(head.x - following.x) > 1 || Math.Abs(head.y - following.y) > 1)
        {
            following.y += head.y > following.y ? 1 : -1;
            following.x += head.x > following.x ? 1 : -1;
        }
    }


    private void MoveHead(Point[] rope, char direction)
    {
        switch (direction) 
        {
            case 'U':
                rope[0].y++;
                break;
            case 'D':
                rope[0].y--;
                break;
            case 'R':
                rope[0].x++;
                break;
            case 'L':
                rope[0].x--;
                break;
        }
        for (int i = 1; i < rope.Length; i++)
        {
            MoveUp(rope[i - 1], rope[i]);
        }
    }

    public override string SolvePart2()
    {
        Point[] rope = new Point[10];
        for (int i = 0; i < rope.Length; ++i)
        {
            rope[i] = new Point(0, 0);
        }
        Point tail = rope[rope.Length - 1];

        ISet<(int, int)> visited = new HashSet<(int, int)>();
        
        string[] instructions = input.TrimEnd().Split(Environment.NewLine);

        foreach (string instruction in instructions) 
        {
            char direction = instruction[0];
            int distance = int.Parse(instruction.Substring(2));
            
            for (int i = 0; i < distance; i++)
            {
                MoveHead(rope, direction);
                // Console.WriteLine("Tail: " + tail.x + ", " + tail.y);
                Console.WriteLine(visited.Count);
                
                (int, int) coords = (tail.x, tail.y);
                visited.Add(coords);
            }
        }
        return visited.Count.ToString();
    }


    class Point {
        public int x;
        public int y;

        public Point(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }
}

