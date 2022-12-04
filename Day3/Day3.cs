
namespace AdventOfCode2022.Day3;
class Day3 : Problem {
    public Day3(string input) : base(input)
    {
    }

    int GetPriority(char c) {
        switch (c) {
            case >= 'A' and <= 'Z':
                return (c - 'A') + 27;
            case >= 'a' and <= 'z':
                return (c - 'a') + 1;
        }
        return 0;
    }

    public override string SolvePart1() {
        string[] lines = input.Split(Environment.NewLine);

        int total = 0;
        bool[] set;
        foreach (string line in lines) {
            set = new bool[128];
            for (int i = 0; i < line.Length/2; i++) {
                set[line[i] - 'A'] = true;
            }
            for (int i = line.Length/2; i < line.Length; i++) {
                if (set[line[i] - 'A']) {
                    if (line[i] >= 'A' && line[i] <= 'Z') {
                        total += line[i] - 'A' + 27;
                    } else {
                        total += line[i] - 'a' + 1;
                    }
                    break;
                }
            }
        }
        return total.ToString();
    }

    public override string SolvePart2()
    {
        string[] lines = input.Split(Environment.NewLine);
        int total = 0;
        for (int i = 0; i < lines.Length; i += 3) {
            ISet<char> set = new HashSet<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmonpqrstuvwxyz");
            for (int j = 0; j < 3; j++) {
               set.IntersectWith(lines[i + j]);
               if (set.Count <= 1) {
                   break;
               }
            }
            // Write first element
            total += GetPriority(set.First());
        }
        return total.ToString();
    }
}