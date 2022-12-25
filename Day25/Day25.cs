using System.Text;

namespace AdventOfCode2022.Day25;

class Day25 : Problem {

	public Day25(string input) : base(input) 
	{
	}

    private int GetSign(char c) => c switch {
        '0' => 0,
        '1' => 1,
        '2' => 2,
        '-' => -1,
        '=' => -2,
        _ => 0
    };

    private string DecimalToSnafu(long l)
    {
        StringBuilder builder = new StringBuilder();
        int carry = 0;
        while (l != 0)
        {
            long place = l % 5;
            l /= 5;
            
            long x = place + carry;
            if (x >= 0 && x <= 2)
            {
                builder.Append(x);
                carry = 0;
            } 
            else if (x == 3)
            {
                builder.Append('=');
                carry = 1;
            }
            else if (x == 4)
            {
                builder.Append('-');
                carry = 1;
            }
            else if (x == 5)
            {
                builder.Append(0);
                carry = 1;
            }
        }
        StringBuilder result = new StringBuilder();
        foreach (char c in builder.ToString().Reverse())
        {
            result.Append(c);
        }
        return result.ToString();
    }

    private long SnafuToDecimal(string s)
    {
        long result = 0;
        long p = 1;
        foreach (char c in s.Reverse()) 
        {
            int sign = GetSign(c);
            result += sign * p;
            p *= 5;
        }
        return result;
    }
    public override string SolvePart1()
    {
        string[] lines = input.TrimEnd().Split(Environment.NewLine);
        long sum = lines.Select(x => SnafuToDecimal(x)).Sum();
        // Console.WriteLine(sum);
        return DecimalToSnafu(sum).ToString();
    }

    public override string SolvePart2()
    {
        return "Merry Christmas!";
    }
}
