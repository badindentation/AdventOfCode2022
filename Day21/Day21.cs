namespace AdventOfCode2022.Day21;

class Day21 : Problem {

	public Day21(string input) : base(input) 
	{
	}

    internal class Node 
    {
        public bool Leaf { get; init; }
        public long Value { get; set; }

        public string Left { get; init; }
        public char Operator { get; init; }
        public string Right { get; init; }

        public Node(long value)
        {
            Leaf = true;
            Value = value;
        }
        public Node(string left, string right, char op)
        {
            Leaf = false;
            Left = left;
            Right = right;
            Operator = op;
        }
    }

    private static IDictionary<string, Node> Parse(string input)
    {
        string[] lines = input.TrimEnd().Split(Environment.NewLine);
       
        IDictionary<string, Node> rules = new Dictionary<string, Node>();
        foreach (string line in lines)
        {
            string[] parts = line.Split(":");
            string name = parts[0];
            string rule = parts[1].Trim();

            if (int.TryParse(rule, out int value))
            {
                rules.Add(name, new Node(value));
                continue;
            }
            else 
            {
                string[] ruleParts = rule.Split(" ");
                rules.Add(name, new Node(ruleParts[0], ruleParts[2], ruleParts[1][0]));
            }
        }
        return rules;
    }

    private long Recurse(string name, IDictionary<string,Node> rules)
    {
        Node node = rules[name];
        if (node.Leaf)
        {
            return node.Value;
        }
        else 
        {
            long left = Recurse(node.Left, rules);
            long right = Recurse(node.Right, rules);
            switch (node.Operator)
            {
                case '+':
                    return left + right;
                case '*':
                    return left * right;
                case '-':
                    return left - right;
                case '/':
                    return left / right;
                default:
                    throw new Exception("Should not be here");
            }
        }
        throw new Exception("Should not be here");
    }
    public override string SolvePart1()
    {
        return Recurse("root", Parse(input)).ToString();
    }

    private long Evaluate(string name, IDictionary<string,Node> rules, ref bool found)
    {
        if (name == "humn") 
        {
            found = true;
        }

        Node node = rules[name];
        if (node.Leaf)
        {
            return node.Value;
        }
 
        long left = Evaluate(node.Left, rules, ref found);
        long right = Evaluate(node.Right, rules, ref found);
        switch (node.Operator)
        {
            case '+':
                node.Value = left + right;
                return node.Value;
            case '*':
                node.Value = left * right;
                return node.Value;
            case '-':
                node.Value = left - right;
                return node.Value;
            case '/':
                node.Value = left / right;
                return node.Value;
            default:
                return -1;

        }
    }

    public long? Down(string name, long start, IDictionary<string,Node> rules) 
    {
        if (name == "humn") {
            return start;
        }
        Node node = rules[name];
        
        if (!node.Leaf)
        {
            switch (node.Operator)
            {
                // Addition and multiplication are right associative
                case '+':
                    return Down(node.Left, start - rules[node.Right].Value, rules) ?? Down(node.Right, start - rules[node.Left].Value, rules);
                case '*':
                    return Down(node.Left, start / rules[node.Right].Value, rules) ?? Down(node.Right, start / rules[node.Left].Value, rules);
                // Division and subtraction are left associative
                case '-':
                    return Down(node.Left, start + rules[node.Right].Value, rules) ?? Down(node.Right, rules[node.Left].Value - start, rules);
                case '/':
                    return Down(node.Left, start * rules[node.Right].Value, rules) ?? Down(node.Right, rules[node.Left].Value / start, rules);
                default:
                    return null;
            }
        }
        
        return null;
    }

    public override string SolvePart2()
    {
        IDictionary<string, Node> rules = Parse(input);
        Node root = rules["root"];
        Node left = rules[root.Left];
        Node right = rules[root.Right];

        bool foundLeft = false;
        bool foundRight = false;
        long leftEval = Evaluate(root.Left, rules, ref foundLeft);
        long rightEval = Evaluate(root.Right, rules, ref foundRight);


        long aim = !foundLeft ? leftEval : rightEval;
        // Console.WriteLine($"Left: {leftEval}, Right: {rightEval}, Aim: {aim}");
        string start = foundLeft ? root.Left : root.Right;

        long result = Down(start, aim, rules).Value;

        return result.ToString();
    }
}
