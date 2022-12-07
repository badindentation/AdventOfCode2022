// #define DISPLAY

namespace AdventOfCode2022.Day1;
class Day7 : Problem {

	public Day7(string input) : base(input) 
	{
	}

    private Node GenerateFilesystemTree(string input) 
    {
        Node rootNode = new Node() {
            Name = "/",
            IsDirectory = true,
        };

        Node cur = null;

        string[] lines = input.Split(Environment.NewLine);
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (line.StartsWith("$ cd")) 
            {
                string[] split = line.Split(" ");
                string snd = split[2];
                if (snd.StartsWith("/")) {
                    cur = rootNode;
                } else if (snd.StartsWith("..")) {
                    cur = cur.Parent;
                } else {
                    cur = cur.Children[snd];
                }
            } 
            else if (line.StartsWith("$ ls"))
            {
                int j = i + 1;
                while (j < lines.Length && !(lines[j].StartsWith("$"))) 
                {
                    line = lines[j];
                    string[] split = line.Split(" ");
                    string name = split[1];
                    Node file = new Node() {
                        Name = name,
                        Parent = cur,
                        IsDirectory = true,
                    };

                    if (!split[0].Equals("dir")) {
                        file.Size = int.Parse(split[0]);    
                        file.IsDirectory = false;
                    } 

                    cur.Children.Add(name, file);
                    j++;
                }
                i = j - 1;
            }
        }
        return rootNode;
    }
    public override string SolvePart1()
    {
        Node rootNode = GenerateFilesystemTree(input);

        long total = 0;
        long done = recurse(rootNode, ref total, 0);

        return done.ToString();
    }

    public long recurse(Node node, ref long total, int depth) {
        foreach (Node child in node.Children.Values) 
        {
            // Run with dotnet run --configuration DISPLAY to see the pretty tree structure.
#if DISPLAY
            for (int i = 0; i < depth - 1; i++) {
                Console.Write("┃    ");
            }
            Console.Write("┠━━━━");
            Console.WriteLine("{0} {1} {2}", child.IsDirectory ? "▢" : "━", child.Name, child.Size);
#endif

            if (child.IsDirectory) 
            {
                recurse(child, ref total, depth + 1);
                if (child.Size <= 100_000) {
                    total += child.Size;
                }
            }
        }
        return total;
    }

    // Leetcode is occasionally useful...
    public IEnumerable<Node> GetDirectories(Node node) {
        if (node == null) {
            yield return null;
        }

        Stack<Node> stack = new Stack<Node>();
        stack.Push(node);

        while (stack.Count != 0) 
        {
            Node cur = stack.Pop();
            yield return cur;
            foreach (Node child in cur.Children.Values) 
            {
                if (child.IsDirectory) {
                    stack.Push(child);
                }
            }
        }
    }

    public override string SolvePart2()
    {
        long totalSpace = 70_000_000;
        long spaceNeeded = 30_000_000;

        Node rootNode = GenerateFilesystemTree(input);

        long takenSpace = rootNode.Size;
        long spaceLeft = totalSpace - takenSpace;

        IEnumerable<Node> directories = GetDirectories(rootNode)
            .Where(x => (spaceLeft + x.Size >= spaceNeeded))
            .OrderByDescending(x => x.Size);

#if DISPLAY  
        foreach (Node node in directories) {
            Console.WriteLine(node);
        }
#endif

        Node result = directories.MinBy(x => x.Size);
    
        return result.Size.ToString();
    }

    internal class Node {
        public string Name { get; set; }
        private long? size;
        public long Size { 
            get {
                if (!IsDirectory || size.HasValue) {
                    return size.Value;
                }
                size = Children.Values.Select(x => x.Size).Sum();
                return size.Value;
            }
            set => size = value;
        }
        public bool IsDirectory { get; set;}
        public Node Parent { get; set; }
        public IDictionary<string, Node> Children { get; set; } = new Dictionary<string, Node>();

        public override string ToString()
        {
            return $"{Name} {Size}";
        }
    }
}
