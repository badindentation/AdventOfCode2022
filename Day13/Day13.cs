using System.Text.Json.Nodes;

namespace AdventOfCode2022.Day13;

class Day13 : Problem
{
    public Day13(string input) : base(input)
    {
    }

    internal bool IsCorrectOrder(string aString, string bString)
    {
        JsonNode aNode = JsonNode.Parse(aString);
        JsonNode bNode = JsonNode.Parse(bString);

        // For nullable case
        return CompareNodes(aNode, bNode) == true ? true : false;
    }

    public bool? CompareNodes(JsonNode n1, JsonNode n2)
    {
        // Beautiful...
        Func<bool?> f = (n1, n2) switch
        {
            (JsonValue val1, JsonValue val2) => () =>
            {
                int l = val1.GetValue<int>();
                int r = val2.GetValue<int>();
                return l == r ? null : l < r;
            }
            ,
            (JsonNode val1, JsonNode val2) => () =>
            {
                if (val1 is not JsonArray l)
                {
                    int z = val1.GetValue<int>();
                    l = new JsonArray(z);
                }

                if (val2 is not JsonArray r)
                {
                    int z = val2.GetValue<int>();
                    r = new JsonArray(z);
                }

                for (int i = 0; i < Math.Min(l.Count, r.Count); ++i)
                {
                    bool? result = CompareNodes(l[i], r[i]);
                    if (result.HasValue) return result;
                }

                if (l.Count != r.Count)
                {
                    return l.Count < r.Count;
                }

                return null;
            }
            ,
        };
        return f();
    }

    public override string SolvePart1()
    {
        string[] pairs = input.TrimEnd().Split(Environment.NewLine + Environment.NewLine);

        int result = 0;
        for (int i = 1; i <= pairs.Length; ++i)
        {
            string[] pair_couple = pairs[i - 1].Split("\n");
            if (IsCorrectOrder(pair_couple[0], pair_couple[1]))
            {
                result += i;
            }
        }

        return result.ToString();
    }

    public override string SolvePart2()
    {
        string[] pairs = input.TrimEnd().Split(Environment.NewLine + Environment.NewLine);

        int result = 0;

        JsonNode div1 = JsonNode.Parse("[[2]]");
        JsonNode div2 = JsonNode.Parse("[[6]]");

        List<JsonNode> allNodes = new List<JsonNode>()
        {
            div1,
            div2,
        };

        for (int i = 1; i <= pairs.Length; ++i)
        {
            string[] pair_couple = pairs[i - 1].Split("\n");
            allNodes.Add(JsonNode.Parse(pair_couple[0]));
            allNodes.Add(JsonNode.Parse(pair_couple[1]));
        }
        allNodes.Sort((a, b) => CompareNodes(a, b) == true ? -1 : 1);

        return ((allNodes.IndexOf(div1) + 1) * (allNodes.IndexOf(div2) + 1)).ToString();
    }
}
