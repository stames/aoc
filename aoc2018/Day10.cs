using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace aoc2018
{
    public class Day10
    {
        class Star
        {
            public Point Position { get; set; }
            public int VelocityX { get; set; }
            public int VelocityY { get; set; }
        }

        public Day10()
        {
        }

        public int Part1()
        {
            var lines = InputUtils.GetDayInputLines(2018, 10);

            Regex r = new Regex(@"position=<\s?(.+),\s?(.+)> velocity=<\s?(.+),\s?(.+)>");

            List<Star> stars = new List<Star>();

            foreach(var line in lines)
            {
                var m = r.Match(line);

                Star star = new Star();
                star.Position = new Point(int.Parse(m.Groups[1].Value.Trim()), int.Parse(m.Groups[2].Value.Trim()));
                star.VelocityX = int.Parse(m.Groups[3].Value.Trim());
                star.VelocityY = int.Parse(m.Groups[4].Value.Trim());

                stars.Add(star);
            }

            int time = 0;
            while(time < 1000000)
            {
                DrawStars(stars);

                // apply velocity
                foreach(var star in stars)
                {
                    star.Position.X += star.VelocityX;
                    star.Position.Y += star.VelocityY;
                }

                time++;
            }
            
            return 0;
        }

        private void DrawStars(List<Star> stars)
        {
            int minX = stars.Select(p => p.Position.X).Min();
            int minY = stars.Select(p => p.Position.Y).Min();
            int maxX = stars.Select(p => p.Position.X).Max();
            int maxY = stars.Select(p => p.Position.Y).Max();

            if(Math.Abs(maxX - minX) >= 200 || Math.Abs(maxY - minY) >= 100)
            {
                return;
            }

            string fileName = "/Users/jjacoby/testing/advent2018/day10out.txt";
            StringBuilder sb = new StringBuilder();

            for(int y = minY; y <= maxY; y++)
            {
                for(int x = minX; x <= maxX; x++)
                {
                    Point p = new Point(x, y);
                    if(stars.Any(s => s.Position.Equals(p)))
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

            File.WriteAllText(fileName, sb.ToString());
        }

        public int Part2()
        {
            return 0;
        }
    }
}
