namespace AdventOfCode2022.Day8;

class Day8 : Problem {

	public Day8(string input) : base(input) 
	{
	}

    public override string SolvePart1()
    {
        string[] lines = input.Split(Environment.NewLine);
        int width = lines[0].Length;
        int height = lines.Length;
        int[,] grid = new int[height, width];
        
        for (int y = 0; y < lines.Length; y++) {
            string line = lines[y];
            for (int x = 0; x < line.Length; x++) {
                grid[y,x] = line[x] - '0';
            }
        }

#if DISPLAY
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                Console.Write(grid[y,x]);
            }
            Console.WriteLine();
        }
#endif

        int[,] top = new int[height,width];
        for (int x = 0; x < width; x++) {
            top[0,x] = grid[0,x];
            for (int y = 1; y < height; y++) {
                top[y,x] = Math.Max(grid[y,x], top[y-1,x]);
            }
        }

        int[,] bot = new int[height,width];
        for (int x = 0; x < width; x++) {
            bot[height - 1,x] = grid[height - 1,x];
            for (int y = height - 2; y >= 0; y--) {
                bot[y,x] = Math.Max(grid[y,x], bot[y+1,x]);
            }
        }

        int[,] left = new int[height,width];
        for (int y = 0; y < height; y++) {
            left[y,0] = grid[y,0];
            for (int x = 1; x < width; x++) {
                left[y,x] = Math.Max(grid[y,x], left[y,x-1]);
            }
        }

        int[,] right = new int[height,width];
        for (int y = 0; y < height; y++) {
            right[y,width - 1] = grid[y,width - 1];
            for (int x = width - 2; x >= 0; x--) {
                right[y,x] = Math.Max(grid[y,x], right[y,x+1]);
            }
        }

        int start = width * 2 + height * 2 - 4;
        for (int x = 1; x < width - 1; x++) {
           for (int y = 1; y < height - 1; y++) {
               int h = grid[y,x];
               if (h > top[y - 1, x] || h > bot[y + 1, x] || h > left[y, x - 1] || h > right[y, x + 1]) {
                   start++;
               }
           }
        }
#if DISPLAY
        Console.WriteLine("Width: " + width + " Height: " + height);
        Console.WriteLine("Total: " + width * height);
#endif
        return start.ToString();
    }

    public override string SolvePart2()
    {
        string[] lines = input.Split(Environment.NewLine);
        int width = lines[0].Length;
        int height = lines.Length;
        int[,] grid = new int[height, width];
        
        for (int y = 0; y < lines.Length; y++) {
            string line = lines[y];
            for (int x = 0; x < line.Length; x++) {
                grid[y,x] = line[x] - '0';
            }
        }

#if DISPLAY
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                Console.Write(grid[y,x]);
            }
            Console.WriteLine();
        }
#endif

        int[,] top = new int[height,width];
        for (int x = 0; x < width; x++) {
            top[0,x] = grid[0,x];
            for (int y = 1; y < height; y++) {
                top[y,x] = Math.Max(grid[y,x], top[y-1,x]);
            }
        }

        int[,] bot = new int[height,width];
        for (int x = 0; x < width; x++) {
            bot[height - 1,x] = grid[height - 1,x];
            for (int y = height - 2; y >= 0; y--) {
                bot[y,x] = Math.Max(grid[y,x], bot[y+1,x]);
            }
        }

        int[,] left = new int[height,width];
        for (int y = 0; y < height; y++) {
            left[y,0] = grid[y,0];
            for (int x = 1; x < width; x++) {
                left[y,x] = Math.Max(grid[y,x], left[y,x-1]);
            }
        }

        int[,] right = new int[height,width];
        for (int y = 0; y < height; y++) {
            right[y,width - 1] = grid[y,width - 1];
            for (int x = width - 2; x >= 0; x--) {
                right[y,x] = Math.Max(grid[y,x], right[y,x+1]);
            }
        }

        int start = width * 2 + height * 2 - 4;

        long max_score = 0;
        for (int x = 1; x < width - 1; x++) {
           for (int y = 1; y < height - 1; y++) {
               int h = grid[y,x];

               int vy = y - 1;
               int up_count = 0;
               while (vy >= 0) {
                   up_count++;
                   if (grid[vy,x] >= h) {
                       break;
                   }
                   vy--;
               }

               int down_count = 0;
               vy = y + 1;
               while (vy <= height - 1) {
                   down_count++;
                   if (grid[vy,x] >= h) {
                       break;
                   }
                   vy++;
               }

               int left_count = 0;
               int vx = x - 1;
               while (vx >= 0) {
                   left_count++;
                   if (grid[y,vx] >= h) {
                       break;
                   }
                   vx--;
               }

               int right_count = 0;
               vx = x + 1;
               while (vx <= height - 1) {
                   right_count++;
                   if (grid[y,vx] >= h) {
                       break;
                   }
                   vx++;
               }

               max_score = Math.Max(max_score, up_count * down_count * left_count * right_count);
           }
        }
#if DISPLAY
        Console.WriteLine("Width: " + width + " Height: " + height);
        Console.WriteLine("Total: " + width * height);
#endif

        return max_score.ToString();
    }
}
