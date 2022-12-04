namespace AdventOfCode2022;

public abstract class Problem {
    protected string input;
    public Problem(string input) {
        this.input = input;
    }   
    public abstract string SolvePart1();
    public abstract string SolvePart2();
}
