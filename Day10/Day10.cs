using System.Text;

namespace AdventOfCode2022.Day10;

class Day10 : Problem {

	public Day10(string input) : base(input) 
	{
	}

    public override string SolvePart1()
    {
        string[] lines = input.TrimEnd().Split(Environment.NewLine);

        int X = 1;
        int cycle = 1;
        int signal_strength_sum = 0;
        foreach (string line in lines)
        {
            string[] split = line.Split();
            if (split.Length > 1) 
            {
                int amount = int.Parse(split[1]);
                cycle += 1;
                if (cycle % 40 == 20) 
                {
                    signal_strength_sum += X * cycle;
                }
                cycle += 1;
                X += amount;
            }
            else 
            {
                // No op
                cycle += 1;
            }

            if (cycle % 40 == 20) 
            {
                signal_strength_sum += X * cycle;
            }
        }
        return signal_strength_sum.ToString();
    }

    // Start Cycle
    // During Cycle -> Draw Pixel 
    // End Cycle -> Execute instruction

    private void DrawPixel(int x, int cycle, StringBuilder crt) 
    {
        int pixel = cycle - 1;

        if (pixel % 40 == 0) 
        {
            crt.Append("\n");
        }
        if (Math.Abs(x - (pixel % 40)) <= 1) 
        {
            crt.Append('#');
        }
        else 
        {
            crt.Append(' ');
        }
    }

    public override string SolvePart2()
    {
        string[] lines = input.TrimEnd().Split(Environment.NewLine);

        int X = 1;
        int cycle = 0;

        StringBuilder crt = new StringBuilder("\n");
        foreach (string line in lines)
        {
            string[] split = line.Split();
            if (split.Length > 1) 
            {
                int amount = int.Parse(split[1]);
                cycle += 1;
                DrawPixel(X, cycle, crt);
                cycle += 1;
                DrawPixel(X, cycle, crt);
                X += amount;
            }
            else 
            {
                cycle += 1;
                DrawPixel(X, cycle, crt);
            }
        }

        return crt.ToString();
    }
}
