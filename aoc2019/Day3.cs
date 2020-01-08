using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode;

namespace aoc2019
{
    public class Day3
    {
        List<int> wireLengths = new List<int>();
        List<int> distances = new List<int>();

        public Day3()
        {
            var input = InputUtils.GetDayInputLines(3);

            string wire1 = input[0];
            string wire2 = input[1];

            char[,] grid = new char[50000, 50000];
            grid[20000, 20000] = 'o'; // port

            List<string> w1list = wire1.Split(',').ToList();
            List<string> w2list = wire2.Split(',').ToList();

            Dictionary<Point, int> xDistance = new Dictionary<Point, int>();

            int currentX = 20000;
            int currentY = 20000;
            int wire1Length = 0;
            foreach (string w in w1list)
            {
                int newX = currentX;
                int newY = currentY;
                char direction = w[0];
                int length = int.Parse(w.Substring(1));

                switch (direction)
                {
                    case 'U':
                        newY = currentY + length;
                        for (int y = currentY + 1; y <= newY; y++)
                        {
                            //Console.WriteLine("{0}, {1}", currentX, y);
                            grid[currentX, y] = '1';
                            Point p = new Point(currentX, y);
                            if (!xDistance.ContainsKey(p))
                            {
                                xDistance.Add(p, wire1Length + y - currentY);
                            }
                        }
                        break;
                    case 'D':
                        newY = currentY - length;
                        for (int y = currentY - 1; y >= newY; y--)
                        {
                            //Console.WriteLine("{0}, {1}", currentX, y);
                            grid[currentX, y] = '1';
                            Point p = new Point(currentX, y);
                            if (!xDistance.ContainsKey(p))
                            {
                                xDistance.Add(p, wire1Length + currentY - y);
                            }
                        }
                        break;
                    case 'L':
                        newX = currentX - length;
                        for (int x = currentX - 1; x >= newX; x--)
                        {
                            //Console.WriteLine("{0}, {1}", x, currentY);
                            grid[x, currentY] = '1';
                            Point p = new Point(x, currentY);
                            if (!xDistance.ContainsKey(p))
                            {
                                xDistance.Add(p, wire1Length + currentX - x);
                            }
                        }
                        break;
                    case 'R':
                        newX = currentX + length;
                        for (int x = currentX + 1; x <= newX; x++)
                        {
                            //Console.WriteLine("{0}, {1}", x, currentY);
                            grid[x, currentY] = '1';
                            Point p = new Point(x, currentY);
                            if (!xDistance.ContainsKey(p))
                            {
                                xDistance.Add(p, wire1Length + x - currentX);
                            }
                        }
                        break;
                    default:
                        break;
                }

                wire1Length += length;
                currentX = newX;
                currentY = newY;
            }

            currentX = 20000;
            currentY = 20000;
            int wire2Length = 0;

            foreach (string w in w2list)
            {
                int newX = currentX;
                int newY = currentY;
                char direction = w[0];
                int length = int.Parse(w.Substring(1));

                switch (direction)
                {
                    case 'U':
                        newY = currentY + length;
                        for (int y = currentY + 1; y <= newY; y++)
                        {
                            if (grid[currentX, y] == '1')
                            {
                                grid[currentX, y] = 'X';
                                int distance = Point.CalculateManhattanDistance(new Point(currentX, y), new Point(20000, 20000));
                                distances.Add(distance);
                                if (xDistance.ContainsKey(new Point(currentX, y)))
                                { 
                                    wireLengths.Add(wire2Length + y - currentY + xDistance[new Point(currentX, y)]);
                                }

                            }
                            else
                            {
                                grid[currentX, y] = '2';
                            }
                        }
                        break;
                    case 'D':
                        newY = currentY - length;
                        for (int y = currentY - 1; y >= newY; y--)
                        {
                            if (grid[currentX, y] == '1')
                            {
                                grid[currentX, y] = 'X';
                                int distance = Point.CalculateManhattanDistance(new Point(currentX, y), new Point(20000, 20000));
                                distances.Add(distance);
                                if (xDistance.ContainsKey(new Point(currentX, y)))
                                {
                                    wireLengths.Add(wire2Length + currentY - y + xDistance[new Point(currentX, y)]);
                                }
                            }
                            else
                            {
                                grid[currentX, y] = '2';
                            }
                        }
                        break;
                    case 'L':
                        newX = currentX - length;
                        for (int x = currentX - 1; x >= newX; x--)
                        {
                            if (grid[x, currentY] == '1')
                            {
                                grid[x, currentY] = 'X';
                                int distance = Point.CalculateManhattanDistance(new Point(x, currentY), new Point(20000, 20000));
                                distances.Add(distance);
                                if (xDistance.ContainsKey(new Point(x, currentY)))
                                {
                                    wireLengths.Add(wire2Length + currentX - x + xDistance[new Point(x, currentY)]);
                                }
                            }
                            else
                            {
                                grid[x, currentY] = '2';
                            }
                        }
                        break;
                    case 'R':
                        newX = currentX + length;
                        for (int x = currentX + 1; x <= newX; x++)
                        {
                            if (grid[x, currentY] == '1')
                            {
                                int distance = Point.CalculateManhattanDistance(new Point(x, currentY), new Point(20000, 20000));
                                distances.Add(distance);
                                if (xDistance.ContainsKey(new Point(x, currentY)))
                                {
                                    wireLengths.Add(wire2Length + x - currentX + xDistance[new Point(x, currentY)]);
                                }
                                grid[x, currentY] = 'X';
                            }
                            else
                            {
                                grid[x, currentY] = '2';
                            }
                        }
                        break;
                    default:
                        break;
                }

                wire2Length += length;

                currentX = newX;
                currentY = newY;
            }
        }

        public int Part1()
        {
            return distances.Min();
        }

        public int Part2()
        {
            return wireLengths.Min();
        }
    }
}
