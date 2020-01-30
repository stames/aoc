using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;

namespace aoc2018
{
    public class Day11
    {
        public Day11()
        {
        }

        public int Part1()
        {
            int serialNumber = 1308;

            int highPower = 0;
            Point highScore = new Point(0, 0);

            for (int x = 1; x <= 300; x++)
            {
                for (int y = 1; y <= 300; y++)
                {
                    Point p = new Point(x, y);

                    // 3x3 grid from this point
                    int maxX = x + 2;
                    int maxY = y + 2;

                    if (maxX <= 300 && maxY <= 300)
                    {
                        // in the grid
                        List<Point> points = new List<Point>();
                        points.Add(p);
                        points.Add(new Point(p.X, p.Y + 1));
                        points.Add(new Point(p.X, p.Y + 2));

                        points.Add(new Point(p.X + 1, p.Y));
                        points.Add(new Point(p.X + 1, p.Y + 1));
                        points.Add(new Point(p.X + 1, p.Y + 2));

                        points.Add(new Point(p.X + 2, p.Y));
                        points.Add(new Point(p.X + 2, p.Y + 1));
                        points.Add(new Point(p.X + 2, p.Y + 2));

                        int powerLevel = 0;
                        foreach (var point in points)
                        {
                            powerLevel += GetPowerLevel(point, serialNumber);
                        }

                        if (powerLevel > highPower)
                        {
                            highPower = powerLevel;
                            highScore = p;
                        }
                    }
                }
            }

            Console.WriteLine(highScore);
            return 0;
        }

        public int Part2()
        {
            int serialNumber = 1308;

            int highPower = 0;
            Point highScore = new Point(0, 0);
            int highSize = 0;
            Dictionary<Point, int> powers = new Dictionary<Point, int>();

            for (int x = 1; x <= 300; x++)
            {
                for (int y = 1; y <= 300; y++)
                {
                    Point p = new Point(x, y);
                    powers.Add(p, GetPowerLevel(p, serialNumber));
                }
            }

            for (int x = 1; x <= 300; x++)
            {
                for (int y = 1; y <= 300; y++)
                {
                    Point p = new Point(x, y);
                    Console.WriteLine("Checking {0}", p);

                    // NxN grid from this point
                    int previousSum = powers[p];

                    // NxN grid sum is:
                    // (N-1)x(N-1) grid sum + line N plus column N sums
                    for (int gridSize = 1; gridSize <= 300; gridSize++)
                    {
                        int maxX = x + gridSize;
                        int maxY = y + gridSize;

                        int powerLevel = previousSum;
                        if (powers.ContainsKey(new Point(maxX, maxY)))
                        {
                            // in the grid
                            HashSet<Point> points = new HashSet<Point>();

                            // add all additional points
                            // (x + 1), y --> (x + 1), maxY
                            // x, y + 1 --> maxX, y + 1
                            for(int tempY = y; tempY <= maxY; tempY++)
                            {
                                points.Add(new Point(maxX, tempY));
                            }
                            for(int tempX = x; tempX <= maxX; tempX++)
                            {
                                points.Add(new Point(tempX, maxY));
                            }

                            foreach (var point in points)
                            {
                                powerLevel += powers[point];
                            }

                            if (powerLevel > highPower)
                            {
                                highPower = powerLevel;
                                highScore = p;
                                highSize = gridSize + 1;
                            }

                            previousSum = powerLevel;
                        }
                    }
                }
            }

            Console.WriteLine(highScore + ", " + highSize + ", power = " + highPower);
            return 0;
        }

        private int GetPowerLevel(Point location, int serialNumber)
        {
            int rackId = location.X + 10;
            int powerLevel = rackId * location.Y;
            powerLevel += serialNumber;
            powerLevel *= rackId;

            // keep only hundreds digit
            powerLevel /= 100;
            powerLevel = powerLevel % 10;
            powerLevel -= 5;

            return powerLevel;
        }
    }
}
