using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;
using System.Text;

namespace aoc2018
{
    public class Day6
    {
        List<Point> points = new List<Point>();

        int minX;
        int maxX;
        int minY;
        int maxY;

        public Day6()
        {
            var lines = InputUtils.GetDayInputLines(2018, 6);
            //var lines = File.ReadAllLines("/Users/jjacoby/testing/advent2018/day6test.txt");

            foreach (var line in lines)
            {
                Point p = new Point(
                    int.Parse(line.Split(',')[0].Trim()),
                    int.Parse(line.Split(',')[1].Trim()));

                points.Add(p);
            }

            minX = points.Min(p => p.X);
            maxX = points.Max(p => p.X);
            minY = points.Min(p => p.Y);
            maxY = points.Max(p => p.Y);
        }

        public int Part1()
        {
            // coordinate from input -> number of points it's closest to
            Dictionary<Point, int> distances = new Dictionary<Point, int>();

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    Point p = new Point(x, y);

                    Point minPoint = new Point(-1, -1);
                    Dictionary<Point, int> temp = new Dictionary<Point, int>();
                    foreach (var c in points)
                    {
                        // check each input coordinate to see if it is closest
                        // to this point
                        int distance = Point.CalculateManhattanDistance(p, c);
                        temp.Add(c, distance);
                    }

                    int minOverall = temp.Values.Min();
                    if (temp.Values.Count(p => p == minOverall) == 1)
                    {
                        minPoint = temp.First(p => p.Value == minOverall).Key;
                        if (minPoint.X != minX && minPoint.X != maxX && minPoint.Y != minY && minPoint.Y != maxY)
                        {
                            if (distances.ContainsKey(minPoint))
                            {
                                distances[minPoint]++;
                            }
                            else
                            {
                                distances.Add(minPoint, 1);
                            }
                        }
                    }
                }
            }

            // trace around the grid and remove any points
            // who have min distances to these outer points
            // (indicating infinite)
            // minX - 1, minY - 1 through maxX + 1, minY - 1, horiz
            // minX - 1, minY - 1 through minX - 1, maxY + 1, vert
            // minX - 1, maxY + 1 through maxX + 1, maxY + 1, horiz
            // maxX + 1, minY - 1 through maxX + 1, maxY + 1, vert
            // 
            HashSet<Point> blacklist = new HashSet<Point>();
            int xp = 0;
            int yp = minY - 1;
            for (xp = minX - 1; xp <= maxX + 1; xp++)
            {
                blacklist.Add(new Point(xp, yp));
            }

            xp = minX - 1;
            for (yp = minY - 1; yp <= maxY + 1; yp++)
            {
                blacklist.Add(new Point(xp, yp));
            }

            yp = maxY + 1;
            for (xp = minX - 1; xp <= maxX + 1; xp++)
            {
                blacklist.Add(new Point(xp, yp));
            }

            xp = maxX + 1;
            for (yp = minY - 1; yp <= maxY + 1; yp++)
            {
                blacklist.Add(new Point(xp, yp));
            }

            foreach (var bp in blacklist)
            {
                // check what is the point with the shortest distance to this point
                int shortest = int.MaxValue;
                Point shortestPoint = new Point(0, 0);
                foreach (var c in points)
                {
                    int md = Point.CalculateManhattanDistance(c, bp);
                    if (md < shortest)
                    {
                        shortest = md;
                        shortestPoint = c;
                    }
                }

                if (distances.ContainsKey(shortestPoint))
                {
                    distances[shortestPoint] = 0;
                }
            }
            // largest is the point with the largest value in
            // the distances dictionary

            return distances.Values.Max();
        }

        public int Part2()
        {
            int count = 0;
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    Point p = new Point(x, y);
                    int totalDist = 0;
                    foreach(var c in points)
                    {
                        totalDist += Point.CalculateManhattanDistance(c, p);
                    }

                    if(totalDist < 10000)
                    {
                        count++;
                    }

                }
            }

            return count;
        }
    }
}
