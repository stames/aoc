using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;
using System.Text;

namespace aoc2018
{
    public class Day17
    {
        public Day17()
        {
        }

        public int Part1()
        {
            var lines = InputUtils.GetDayInputLines(2018, 17);

            Point spring = new Point(500, 0);

            List<Point> clayPoints = new List<Point>();

            foreach (var line in lines)
            {
                // e.g. vertical line
                // x=484, y=1286..1289
                // or horizontal line
                // y=289, x=475..560
                bool vertical = line.StartsWith("x=");

                int firstValue = int.Parse(line.Split('=')[1].Split(',')[0]);
                int min = int.Parse(line.Split('=')[2].Split("..")[0]);
                int max = int.Parse(line.Split('=')[2].Split("..")[1]);

                for(int i = min; i <= max; i++)
                {
                    if(vertical)
                    {
                        clayPoints.Add(new Point(firstValue, i));
                    }
                    else
                    {
                        clayPoints.Add(new Point(i, firstValue));
                    }
                }
            }

            Draw(clayPoints.ToHashSet());

            return 0;
        }

        public int Part2()
        {
            return 0;
        }

        private void Draw(HashSet<Point> clayPoints)
        {
            int minX = clayPoints.Min(p => p.X);
            int maxX = clayPoints.Max(p => p.X);

            int minY = 0;
            int maxY = clayPoints.Max(p => p.Y);

            StringBuilder sb = new StringBuilder();

            for(int y = minY; y <= maxY; y++)
            {
                for(int x = minX; x <= maxX; x++)
                {
                    if(x == 500 && y == 0)
                    {
                        sb.Append("+");
                    }
                    else if(clayPoints.Contains(new Point(x, y)))
                    {
                        sb.Append("#");
                    }
                    else
                    {
                        sb.Append(".");
                    }
                }
                sb.AppendLine();
            }

            File.WriteAllText("/Users/jjacoby/testing/advent2018/day17out.txt", sb.ToString());
        }
    }
}
