namespace AdventOfCode2022.Day2;

class Day2 : Problem {

	public Day2(string input) : base(input) 
	{
	}
    public override string SolvePart1()
    {
		// Rock, Paper, Scissors
		// A, B, C
		// X, Y, Z
        string[] games = input.Split("\n");
		
		int score = 0;
		foreach (string game in games) {
			int move1 = game[0] - 'A';
			int move2 = game[2] - 'X';

			// Only need to calculate wins and draws
			if (move1 == move2) {
				score += 3; // Draw
			} else if ((move1 + 1 ) % 3 == move2) {
				score += 6; // Win
			}

			score += (move2 + 1);
		}
		return score.ToString();
    }
    public override string SolvePart2()
    {
        // Rock, Paper, Scissors
		// A, B, C
		// Loss, Draw, Win
		// X, Y, Z
        string[] games = input.Split("\n");
		
		int score = 0;
		foreach (string game in games) {
			int move1 = game[0] - 'A';
			int res = game[2] - 'X';

			// Loss 0, Draw 1, Win 2
			// Loss +2, Draw +0, Win +1
			int move2 = (move1 + res + 2) % 3;
			score += (res * 3);
			score += (move2 + 1);
		}
		return score.ToString();
    }
}
