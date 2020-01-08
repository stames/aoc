using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;
using System.Text;

namespace aoc2019
{
    enum GridLocation
    {
        Empty,
        Wall,
        Oxygen,
        Robot
    }

    public class Day15
    {
        Dictionary<Point, GridLocation> grid = new Dictionary<Point, GridLocation>();

        int oxygenX = 0;
        int oxygenY = 0;

        Point ox;

        public Day15()
        {
            IntcodeComputer computer = new IntcodeComputer(InputUtils.GetFileName(15));

            Point robot = new Point(0, 0);
            grid[new Point(0, 0)] = GridLocation.Robot;

            int currentX = 0;
            int currentY = 0;

            for (int rep = 0; rep < 2000000; rep++)
            {
                // try each movement

                int nextValue = new Random().Next(1, 5);
                Direction nextDirection = (Direction)nextValue;
                computer.EnqueueInput(nextValue);
                computer.Run();
                long outputCode = computer.GetAllOutput().First();

                int xDelta = 0;
                int yDelta = 0;
                switch (nextDirection)
                {
                    case Direction.North:
                        yDelta = 1;
                        break;
                    case Direction.South:
                        yDelta = -1;
                        break;
                    case Direction.West:
                        xDelta = -1;
                        break;
                    case Direction.East:
                        xDelta = 1;
                        break;
                }

                if (outputCode == 0)
                {
                    // wall
                    Point p = new Point(currentX + xDelta, currentY + yDelta);
                    grid[p] = GridLocation.Wall;
                }
                else if (outputCode == 1)
                {
                    // robot moved 
                    grid[robot] = GridLocation.Empty;
                    currentY += yDelta;
                    currentX += xDelta;

                    robot.Y += yDelta;
                    robot.X += xDelta;
                    Point rp = new Point(robot.X, robot.Y);
                    if (!grid.ContainsKey(rp) || grid[rp] != GridLocation.Oxygen)
                    {
                        grid[rp] = GridLocation.Robot;
                    }
                }
                else if (outputCode == 2)
                {
                    Point p = new Point(currentX + xDelta, currentY + yDelta);
                    grid[p] = GridLocation.Oxygen;

                    currentY += yDelta;
                    currentX += xDelta;

                    robot.Y += yDelta;
                    robot.X += xDelta;

                    oxygenX = p.X;
                    oxygenY = p.Y;
                }
            }

            ox = new Point(oxygenX, oxygenY);
            grid[ox] = GridLocation.Oxygen;
        }

        public int Part1()
        {
            //DrawBoard();

            // shortest distance from ox to start
            int shortest = GetShortestDistance(ox, new Point(0, 0));
            return shortest;
        }

        public int Part2()
        {
            int minutes = 0;
            int previousOxygen = 0;

            while (true)
            {
                if (grid.Count(p => p.Value == GridLocation.Empty) == 0)
                {
                    break;
                }

                // spread oxygen to all adjacent empty
                var oxygen = grid.Where(p => p.Value == GridLocation.Oxygen).ToList();

                if (oxygen.Count == previousOxygen)
                {
                    break;
                }

                previousOxygen = oxygen.Count;

                foreach (var o in oxygen)
                {
                    // spread to adjacent
                    if (grid.ContainsKey(new Point(o.Key.X - 1, o.Key.Y)) && grid[new Point(o.Key.X - 1, o.Key.Y)] == GridLocation.Empty)
                    {
                        grid[new Point(o.Key.X - 1, o.Key.Y)] = GridLocation.Oxygen;
                    }

                    if (grid.ContainsKey(new Point(o.Key.X + 1, o.Key.Y)) && grid[new Point(o.Key.X + 1, o.Key.Y)] == GridLocation.Empty)
                    {
                        grid[new Point(o.Key.X + 1, o.Key.Y)] = GridLocation.Oxygen;
                    }

                    if (grid.ContainsKey(new Point(o.Key.X, o.Key.Y - 1)) && grid[new Point(o.Key.X, o.Key.Y - 1)] == GridLocation.Empty)
                    {
                        grid[new Point(o.Key.X, o.Key.Y - 1)] = GridLocation.Oxygen;
                    }

                    if (grid.ContainsKey(new Point(o.Key.X, o.Key.Y + 1)) && grid[new Point(o.Key.X, o.Key.Y + 1)] == GridLocation.Empty)
                    {
                        grid[new Point(o.Key.X, o.Key.Y + 1)] = GridLocation.Oxygen;
                    }
                }

                minutes++;
            }

            return minutes - 1;
        }

        private int GetShortestDistance(Point a, Point b, Point previous = null)
        {
            if (a.Equals(b))
            {
                return 0;
            }

            // if it can only move one direction, go there,
            // also don't want to go back the way we came from
            int walls = 0;
            List<Direction> possibleDirections = new List<Direction>();

            Point eastPoint = a.PointInDirection(Direction.East);
            Point westPoint = a.PointInDirection(Direction.West);
            Point southPoint = a.PointInDirection(Direction.South);
            Point northPoint = a.PointInDirection(Direction.North);

            if (!grid.ContainsKey(eastPoint) || grid[eastPoint] == GridLocation.Wall || (previous != null && previous.Equals(eastPoint)))
            {
                walls++;
            }
            else
            {
                possibleDirections.Add(Direction.East);
            }

            if (!grid.ContainsKey(westPoint) || grid[westPoint] == GridLocation.Wall || (previous != null && previous.Equals(westPoint)))
            {
                walls++;
            }
            else
            {
                possibleDirections.Add(Direction.West);
            }

            if (!grid.ContainsKey(northPoint) || grid[northPoint] == GridLocation.Wall || (previous != null && previous.Equals(northPoint)))
            {
                walls++;
            }
            else
            {
                possibleDirections.Add(Direction.North);
            }

            if (!grid.ContainsKey(southPoint) || grid[southPoint] == GridLocation.Wall || (previous != null && previous.Equals(southPoint)))
            {
                walls++;
            }
            else
            {
                possibleDirections.Add(Direction.South);
            }

            if (walls == 4)
            {
                // can't go anywhere, infinite
                return int.MaxValue - 1000;
            }

            // minimum of paths
            List<int> possiblePaths = new List<int>();
            foreach (Direction d in possibleDirections)
            {
                switch (d)
                {
                    case Direction.North:
                        possiblePaths.Add(1 + GetShortestDistance(northPoint, b, a));
                        break;
                    case Direction.South:
                        possiblePaths.Add(1 + GetShortestDistance(southPoint, b, a));
                        break;
                    case Direction.West:
                        possiblePaths.Add(1 + GetShortestDistance(westPoint, b, a));
                        break;
                    case Direction.East:
                        possiblePaths.Add(1 + GetShortestDistance(eastPoint, b, a));
                        break;
                }
            }

            return possiblePaths.Min();
        }

        private void DrawBoard()
        {
            int minX = grid.Keys.Min(p => p.X);
            int minY = grid.Keys.Min(p => p.Y);
            int maxX = grid.Keys.Max(p => p.X);
            int maxY = grid.Keys.Max(p => p.Y);

            StringBuilder sb = new StringBuilder();

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    Point p = new Point(x, y);
                    if (grid.ContainsKey(p))
                    {
                        if (p.X == 0 && p.Y == 0)
                        {
                            sb.Append("X");
                        }
                        else if (p.X == oxygenX && p.Y == oxygenY)
                        {
                            sb.Append("o");
                        }
                        else
                        {
                            switch (grid[p])
                            {
                                case GridLocation.Empty:
                                    sb.Append(".");
                                    break;
                                case GridLocation.Wall:
                                    sb.Append("#");
                                    break;
                                case GridLocation.Oxygen:
                                    sb.Append("o");
                                    break;
                                case GridLocation.Robot:
                                    sb.Append("D");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        sb.Append(" ");
                    }

                }
                sb.AppendLine();
            }

            File.WriteAllText("/Users/jjacoby/testing/advent2019/day15out.txt", sb.ToString());

        }
    }
}
