using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Day16;

class Day16 : Problem {

	public Day16(string input) : base(input) 
	{
	}

    private int DFS(int cur, int time, int set, int[,] distances, IList<int> flowRates, int depth, int[] unopened, string[] names, bool elephant=false)
    {
        if (time < 1 && elephant) 
        {
            int other = DFS(52, 26, set, distances, flowRates, depth, unopened, names, false);
            // Console.WriteLine("Elephant: " + other);
            return other;
        }
        if (time < 1) return 0;

        int flow = flowRates[cur];
        int pressure = (time) * flow;

        int maxRest = 0;
        for (int x = set; x != 0; x &= (x - 1))
        {
            int tz = BitOperations.TrailingZeroCount(x);
            int setOff = set & ~(1 << tz);
            int un = unopened[tz];
            int rest = DFS(un, time - 1 - (distances[cur, un]), setOff, distances, flowRates, depth + 1, unopened, names, elephant);

            if (rest > maxRest) 
            {
                maxRest = rest;
            }
            
        }
        int result = (maxRest + pressure);
        return result;
    }
    public override string SolvePart1()
    {    
        Regex regex = new Regex(@"Valve ([A-Z]{2}) has flow rate=(\d+);.*valves? (.*)");

        string[] lines = input.TrimEnd().Split(Environment.NewLine);

        int N = lines.Length;
        int[] flowRates = new int[N];
        string[] names = new string[N];
        IList<string>[] neighbours = new List<string>[N];

        IDictionary<string, int> indexes = new Dictionary<string, int>();

        int count = 0;
        foreach (Match match in regex.Matches(input))
        {
            string name = match.Groups[1].Value;
            int flowRate = int.Parse(match.Groups[2].Value);
            IList<string> tunnels = match.Groups[3].Value.Split(", ").Select(x => x.Trim()).ToList();

            names[count] = name;
            flowRates[count] = flowRate;
            neighbours[count] = tunnels;
            indexes[name] = count;
            count++;
        }

        // Compute distances between all nodes
        int[,] distances = new int[N, N];

        for (int r = 0; r < N; r++)
        {
            for (int c = 0; c < N; c++)
            {
                distances[r, c] = short.MaxValue;
            }
            distances[r, r] = 0;
            foreach (string neighbour in neighbours[r])
            {
                int ni = indexes[neighbour];
                distances[r, ni] = 1;
                distances[ni, r] = 1;
            }
        }

        // Floyd-Warshall
        for (int k = 0; k < N; k++)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    distances[i,j] = Math.Min(distances[i,j], distances[i,k] + distances[k,j]);
                }
            }
        }

        int[] unopened = flowRates.Select((flow, i) => (flow, i)).Where(t => t.flow > 0).Select(t => t.i).ToArray();
        int set = (1 << unopened.Length) - 1;
        // int res = BFS(indexes["AA"], unopened, distances, flowRates);
        int res = DFS(indexes["AA"], 30, set, distances, flowRates, 0, unopened, names);
        return res.ToString();
    }

    private void PrintGrid() 
    {
                // Console.Write("   ");
        // for (int i = 0; i < distances.GetLength(0); i++)
        // {
        //     Console.Write($"{names[i]} ");
        // }
        // Console.WriteLine();

        // for (int i = 0; i < distances.GetLength(0); i++)
        // {
        //     Console.Write($"{names[i]} ");
        //     for (int j = 0; j < distances.GetLength(1); j++)
        //     {
        //         Console.Write($"{distances[i,j],2} ");
        //     }
        //     Console.WriteLine();
        // }
        // for (int i = 0; i < distances.GetLength(0); i++) {
        //     Console.Write("---");
        // }
        // Console.WriteLine("--");
        // // Unopened 0 - N mapping to indexes for valves with flow rate > 0


        // Console.Write("   ");
        // for (int i = 0; i < unopened.Length; i++)
        // {
        //     Console.Write($"{names[unopened[i]]} ");
        // }
        // Console.WriteLine();

        // for (int i = 0; i < unopened.Length; i++)
        // {
        //     Console.Write($"{names[unopened[i]]} ");
        //     // Console.WriteLine($"{names[unopened[i]]} {unopened[i],-2} {flowRates[unopened[i]], -2}");
        //     for (int j = 0; j < unopened.Length; j++) {
        //         Console.Write($"{distances[unopened[i], unopened[j]],2} ");
        //     }
        //     Console.WriteLine();
        // }
    }

    public override string SolvePart2()
    {
        Regex regex = new Regex(@"Valve ([A-Z]{2}) has flow rate=(\d+);.*valves? (.*)");

        string[] lines = input.TrimEnd().Split(Environment.NewLine);

        int N = lines.Length;
        int[] flowRates = new int[N];
        string[] names = new string[N];
        IList<string>[] neighbours = new List<string>[N];

        IDictionary<string, int> indexes = new Dictionary<string, int>();

        int count = 0;
        foreach (Match match in regex.Matches(input))
        {
            string name = match.Groups[1].Value;
            int flowRate = int.Parse(match.Groups[2].Value);
            IList<string> tunnels = match.Groups[3].Value.Split(", ").Select(x => x.Trim()).ToList();

            names[count] = name;
            flowRates[count] = flowRate;
            neighbours[count] = tunnels;
            indexes[name] = count;
            count++;
        }

        // Compute distances between all nodes
        int[,] distances = new int[N, N];

        for (int r = 0; r < N; r++)
        {
            for (int c = 0; c < N; c++)
            {
                distances[r, c] = short.MaxValue;
            }
            distances[r, r] = 0;
            foreach (string neighbour in neighbours[r])
            {
                int ni = indexes[neighbour];
                distances[r, ni] = 1;
                distances[ni, r] = 1;
            }
        }

        // Floyd-Warshall
        for (int k = 0; k < N; k++)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    distances[i,j] = Math.Min(distances[i,j], distances[i,k] + distances[k,j]);
                }
            }
        }

        int[] unopened = flowRates.Select((flow, i) => (flow, i)).Where(t => t.flow > 0).Select(t => t.i).ToArray();
        int set = (1 << unopened.Length) - 1;
        // int res = BFS(indexes["AA"], unopened, distances, flowRates);
        int res = DFS(indexes["AA"], 26, set, distances, flowRates, 0, unopened, names, true);
        return res.ToString();
    }
}
