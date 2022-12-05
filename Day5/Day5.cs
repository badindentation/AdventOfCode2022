using System.Text;

namespace AdventOfCode2022.Day5;
class Day5 : Problem
{
    public Day5(string input) : base(input) { }
    public override string SolvePart1()
    {
        string[] parts = input.Split(Environment.NewLine + Environment.NewLine);
        string stackInput = parts[0];
        string[] lines = stackInput.Split(Environment.NewLine);


        Stack<char>[] stacks = new Stack<char>[9];
        for (int i = 0; i < stacks.Length; i++) {
            stacks[i] = new Stack<char>(9);
        }
        for (int line = lines.Length - 2; line >= 0; line--) {
            for (int stack = 0; stack < stacks.Length; stack++) {
                char c = lines[line][4 * stack + 1];
                if (c >= 'A' && c <= 'Z') {
                    stacks[stack].Push(c);
                }
            }
        }
        string[] moves = parts[1].Split(Environment.NewLine);
        foreach (string move in moves) {
            string[] nums = move.Split(" ");
            int amount = int.Parse(nums[1]);
            int from = int.Parse(nums[3]);
            int to = int.Parse(nums[5]);
            for (int i = 1; i <= amount; i++) {
                stacks[to-1].Push(stacks[from-1].Pop());
            }
        }

        StringBuilder sb = new StringBuilder();
        foreach (var stack in stacks) {
            sb.Append(stack.Pop());
        }
        
        return sb.ToString();
    }

    public override string SolvePart2()
    {
        string[] parts = input.Split(Environment.NewLine + Environment.NewLine);
        string stackInput = parts[0];
        string[] lines = stackInput.Split(Environment.NewLine);


        Stack<char>[] stacks = new Stack<char>[9];
        for (int i = 0; i < stacks.Length; i++) {
            stacks[i] = new Stack<char>(9);
        }
        for (int line = lines.Length - 2; line >= 0; line--) {
            for (int stack = 0; stack < stacks.Length; stack++) {
                char c = lines[line][4 * stack + 1];
                if (c >= 'A' && c <= 'Z') {
                    stacks[stack].Push(c);
                }
            }
        }
        string[] moves = parts[1].Split(Environment.NewLine);
        foreach (string move in moves) {
            string[] nums = move.Split(" ");
            int amount = int.Parse(nums[1]);
            int from = int.Parse(nums[3]);
            int to = int.Parse(nums[5]);
            // Just use a temp stack for part 2
            Stack<char> tmp = new Stack<char>();
            for (int i = 1; i <= amount; i++) {
                tmp.Push(stacks[from-1].Pop());
            }
            for (int i = 1; i <= amount; i++) {
                stacks[to-1].Push(tmp.Pop());
            }
        }

        StringBuilder sb = new StringBuilder();
        foreach (var stack in stacks) {
            sb.Append(stack.Pop());
        }
        
        return sb.ToString();
    }
}
