using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;

namespace aoc2019
{
    public class Day19
    {
        public Day19()
        {
        }

        public int Part1()
        {
            Dictionary<Point, char> grid = new Dictionary<Point, char>();

            int count = 0;

            IntcodeComputer computer = new IntcodeComputer(InputUtils.GetFileName(19));

            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    computer.Reboot();

                    // give x, y as input
                    computer.EnqueueInput(x);
                    computer.EnqueueInput(y);

                    computer.Run();
                    var output = computer.GetAllOutput().ToList();

                    Point p = new Point(x, y);
                    if (output.Any() && output.Last() == 1)
                    {
                        grid.Add(p, '#');
                        count++;
                    }
                    else
                    {
                        grid.Add(p, '.');
                    }
                }
            }

            return count;
        }

        public int Part2()
        {
            Dictionary<Point, char> grid = new Dictionary<Point, char>();

            IntcodeComputer computer = new IntcodeComputer(InputUtils.GetFileName(19));

            for (int y = 0; y < 2000; y++)
            {
                bool foundHash = false;
                for (int x = Math.Max(0, y - 30); x < 2000; x++)
                {
                    computer.Reboot();

                    // give x, y as input
                    computer.EnqueueInput(x);
                    computer.EnqueueInput(y);

                    computer.Run();
                    var output = computer.GetAllOutput().ToList();

                    Point p = new Point(x, y);
                    if (output.Any() && output.Last() == 1)
                    {
                        grid.Add(p, '#');
                        foundHash = true;
                    }
                    else
                    {
                        grid.Add(p, '.');
                        if (foundHash)
                        {
                            // tractor beam is solid, no more hashes
                            // come if there has already been one and now
                            // comes a dot
                            break;
                        }
                    }
                }
            }

            for (int x = 0; x < 2000; x++)
            {
                for (int y = 0; y < 2000; y++)
                {
                    Point point = new Point(x, y);
                    if (grid.ContainsKey(point) && grid[point] == '#')
                    {
                        bool ok = true;
                        for (int xdelta = 1; xdelta <= 99; xdelta++)
                        {
                            Point p = new Point(x + xdelta, y);
                            if (!grid.ContainsKey(p) || grid[p] == '.')
                            {
                                ok = false;
                                break;
                            }
                        }
                        for (int ydelta = 1; ydelta <= 99; ydelta++)
                        {
                            Point p = new Point(x, y + ydelta);
                            if (!grid.ContainsKey(p) || grid[p] == '.')
                            {
                                ok = false;
                                break;
                            }
                        }
                        if (ok)
                        {
                            return x * 10000 + y;
                        }
                    }
                }
            }

            return 0;
        }
    }
}
