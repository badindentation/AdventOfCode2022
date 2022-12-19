using System.Text.RegularExpressions;

namespace AdventOfCode2022.Day15;

class Day15 : Problem {

	public Day15(string input) : base(input) {}

    internal struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString() { return $"[{X},{Y}]"; }
    }

    private static int ManhattanDistance(Point a, Point b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }

    internal class Sensor {
        public Point position;
        public Point closest_beacon;
        public Sensor(Point position, Point closest_beacon) {
            this.position = position;
            this.closest_beacon = closest_beacon;
        }
    }

    private IList<Sensor> ParseInput(string input) {
        IList<Sensor> sensors = new List<Sensor>();
        string[] lines = input.TrimEnd().Split(Environment.NewLine);
        Regex regex = new Regex(@"x=(-?\d+), y=(-?\d+)");

        foreach (string line in lines) {
            MatchCollection matches = regex.Matches(line);
            Point sensorLocation = new Point(int.Parse(matches[0].Groups[1].Value), int.Parse(matches[0].Groups[2].Value));
            Point beaconLocation = new Point(int.Parse(matches[1].Groups[1].Value), int.Parse(matches[1].Groups[2].Value));
            sensors.Add(new Sensor(sensorLocation, beaconLocation));
            // Console.WriteLine($"Sensor at {sensorLocation} is closest to beacon at {beaconLocation}");
        }
        return sensors;
    }

    public (int, int) GetInterval(Point sensor, Point beacon, int y)
    {
        int line_diff = Math.Abs(y - sensor.Y);
        int beacon_diff = ManhattanDistance(sensor, beacon);
        int cover = beacon_diff - line_diff;
        return (sensor.X - cover, sensor.X + cover);
    }

    public override string SolvePart1()
    {
        IList<Sensor> sensors = ParseInput(input);
        int y = 4_000_000;

        IList<Sensor> validSensors = sensors.Where((Sensor sensor) => {
            int line_diff = Math.Abs(y - sensor.position.Y);
            int beacon_diff = ManhattanDistance(sensor.position, sensor.closest_beacon);
            return beacon_diff >= line_diff;
        }).ToList();

        // Sort by x coordinate and keep track of intervals length
        validSensors = validSensors.OrderBy((Sensor sensor) => sensor.position.X).ToList();

        IEnumerable<(int, int)> intervals = validSensors.Select((Sensor sensor) => {
            int line_diff = ManhattanDistance(sensor.position, new Point(sensor.position.X, y));
            int beacon_diff = ManhattanDistance(sensor.position, sensor.closest_beacon);

            int cover = beacon_diff - line_diff;
            return (sensor.position.X - cover , sensor.position.X + cover);
        });
        Console.WriteLine("Intervals: {0}", string.Join(", ", intervals));
            
        // For each sensor get it's manhattan distance to it's beacon and it's manhattan distance to the line
        List<(int, int)> joined = new List<(int, int)>();
        (int, int) newInterval = intervals.First();
        joined.Add(newInterval);
        foreach (var interval in intervals) {
            if (interval.Item1 <= newInterval.Item2) {
                newInterval = (newInterval.Item1, Math.Max(newInterval.Item2, interval.Item2));
                joined[joined.Count - 1] = newInterval;
            } else {
                newInterval = interval;
                joined.Add(newInterval);
            }
        }
        Console.WriteLine("Joined Intervals: {0}", string.Join(", ", joined));
        
        int sum = joined.Select(x => x.Item2 - x.Item1 + 1).Sum();

        return (sum - 1).ToString();
    }   

    private long CalculateTuningFrequency(Point p)
    {
        return (long)p.X * 4_000_000 + (long)p.Y;
    }
    public override string SolvePart2()
    {
        IList<Sensor> sensors = ParseInput(input);

        // Calculate every line between 0 and 4_000_000
        // If interval end if after 0 and < 4_000_000 then add to list
        
        for (int y = 0; y <= 4_000_000; y++)
        {
            List<(int, int)> intervals = new List<(int, int)>();
            foreach (Sensor sensor in sensors)
            {
                // Work out if the sensor is valid for this line
                int line_diff = Math.Abs(y - sensor.position.Y);
                int beacon_diff = ManhattanDistance(sensor.position, sensor.closest_beacon);
                // If the closest beacon is closer than the line just ignore this line for this beason.
                if (beacon_diff < line_diff) continue;

                // Get the interval for this line
                int cover = beacon_diff - line_diff;
                (int x, int y) interval = (sensor.position.X - cover, sensor.position.X + cover);
                intervals.Add(interval);
            }
            if (intervals.Count == 0) continue;

            intervals.Sort();
            List<(int, int)> joined = new List<(int, int)>();
            (int, int) newInterval = intervals.First();
            joined.Add(newInterval);
            foreach (var interval in intervals) {
                if (interval.Item1 <= newInterval.Item2) {
                    newInterval = (newInterval.Item1, Math.Max(newInterval.Item2, interval.Item2));
                    joined[joined.Count - 1] = newInterval;
                } else {
                    newInterval = interval;
                    joined.Add(newInterval);
                }
            }

            foreach (var full in joined) {
                if (full.Item1 > 0 && full.Item1 < 4_000_000) {
                    Point res = new Point(full.Item1 - 1, y);
                    Console.WriteLine(res);
                    return CalculateTuningFrequency(res).ToString();
                }
            }
        }
        return "";
    }
}
