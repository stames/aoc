using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;
using System.Text;

namespace aoc2019
{
    public class Day17
    {
        public Day17()
        {
        }

        public int Part1()
        {
            IntcodeComputer computer = new IntcodeComputer(InputUtils.GetFileName(17));

            computer.Run();
            var initialOutput = computer.GetAllOutput();
            StringBuilder outputBuilder = new StringBuilder();
            if (initialOutput.Any())
            {
                foreach (var o in initialOutput)
                {
                    if ((char)o == '.' || (char)o == '#' || o == 10)
                    {
                        outputBuilder.Append(((char)o).ToString());
                    }
                }

                //Console.WriteLine(outputBuilder.ToString());
            }

            string inputGrid = outputBuilder.ToString();
            var lines = inputGrid.Split(Environment.NewLine).ToList();
            lines.RemoveAll(p => p.Equals(String.Empty));

            Dictionary<Point, char> grid = new Dictionary<Point, char>();

            for(int y = 0; y < lines.Count; y++)
            {
                for(int x = 0; x < lines[0].Length; x++)
                {
                    grid.Add(new Point(x, y), lines[y][x]);
                }
            }

            // count the number of crossings in the grid, and add
            // up the "score"
            int xMax = grid.Keys.Max(p => p.X);
            int yMax = grid.Keys.Max(p => p.Y);

            int score = 0;
            for(int x = 0; x <= xMax; x++)
            {
                for(int y = 0; y <= yMax; y++)
                {
                    Point thisPoint = new Point(x, y);
                    if(grid.ContainsKey(thisPoint) && grid[thisPoint] == '#')
                    {
                        if(grid.ContainsKey(thisPoint.PointInDirection(Direction.North))
                            && grid.ContainsKey(thisPoint.PointInDirection(Direction.East))
                            && grid.ContainsKey(thisPoint.PointInDirection(Direction.West))
                            && grid.ContainsKey(thisPoint.PointInDirection(Direction.South)))
                        {
                            if(grid[thisPoint.PointInDirection(Direction.North)] == '#'
                                && grid[thisPoint.PointInDirection(Direction.South)] == '#'
                                && grid[thisPoint.PointInDirection(Direction.East)] == '#'
                                && grid[thisPoint.PointInDirection(Direction.West)] == '#')
                            {
                                score += thisPoint.X * thisPoint.Y;
                            }
                        }
                    }
                }
            }

            return score;
        }

        public int Part2()
        {
            IntcodeComputer computer = new IntcodeComputer(InputUtils.GetFileName(17));
            computer.SetMemory(0, 2);

            List<int> input = new List<int>();

            // did this part by hand on pen and paper :)

            string mainRoutine = "B,C,C,A,B,C,A,B,C,A";
            foreach (char c in mainRoutine)
            {
                input.Add((int)c);
            }
            input.Add(10);

            // R = 82, L = 76
            // L,6,L,10,L,10,R,6
            string A = "L,6,L,10,L,10,R,6";
            foreach (char c in A)
            {
                input.Add((int)c);
            }
            input.Add(10);

            string B = "L,6,R,12,L,4,L,6";
            foreach (char c in B)
            {
                input.Add((int)c);
            }
            input.Add(10);

            string C = "R,6,L,6,R,12";
            foreach (char c in C)
            {
                input.Add((int)c);
            }
            input.Add(10);

            input.Add((int)'n');
            input.Add(10);

            StringBuilder outputBuilder = new StringBuilder();
            foreach (var i in input)
            {
                computer.EnqueueInput(i);
                computer.Run();
                var output = computer.GetAllOutput();
                if (output.Any())
                {
                    foreach (var o in output)
                    {
                        outputBuilder.Append(o.ToString() + ",");
                    }

                    //Console.WriteLine(outputBuilder.ToString());
                }
            }

            int result = int.Parse(outputBuilder.ToString().TrimEnd(',').Split(',').Last());
            return result;
        }
    }
}
