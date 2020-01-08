using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;

namespace aoc2019
{
    class Portal
    {
        public string Name;
        public Point Location;
        public Point OutputLocation;
    }

    class Day20GridState
    {
        public Point Point { get; set; }
        public int Steps { get; set; }
        public Day20GridState FromState { get; set; }
        public int Level { get; set; }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            Day20GridState that = (Day20GridState)obj;

            return this.Point.Equals(that.Point)
                && this.Level == that.Level;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Point.X.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Point.Y.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Level;

                return hashCode;
            }
        }
    }

    public class Day20
    {
        Dictionary<Point, char> grid = new Dictionary<Point, char>();
        HashSet<Portal> portals = new HashSet<Portal>();

        public Day20()
        {
            var lines = InputUtils.GetDayInputLines(20);

            // hard-coded rows that the portals are on
            // in my input  ¯\_(ツ)_/¯

            for (int i = 0; i < lines[2].Length; i++)
            {
                if (lines[2][i] == '.')
                {
                    // portal
                    Portal p = new Portal();
                    p.Name = lines[0][i].ToString() + lines[1][i].ToString();
                    p.Location = new Point(i, 1);
                    p.OutputLocation = new Point(i, 2);
                    portals.Add(p);
                }
            }

            int y = 26;
            for (int i = 0; i < lines[y].Length; i++)
            {
                if (lines[y][i] == '.' && char.IsLetter(lines[y + 1][i]))
                {
                    // portal
                    Portal p = new Portal();
                    p.Name = lines[y + 1][i].ToString() + lines[y + 2][i].ToString();
                    p.Location = new Point(i, y + 1);
                    p.OutputLocation = new Point(i, y);
                    portals.Add(p);
                }
            }

            y = 80;
            for (int i = 0; i < lines[y].Length; i++)
            {
                if (lines[y][i] == '.' && char.IsLetter(lines[y - 1][i]))
                {
                    // portal
                    Portal p = new Portal();
                    p.Name = lines[y - 2][i].ToString() + lines[y - 1][i].ToString();
                    p.Location = new Point(i, y - 1);
                    p.OutputLocation = new Point(i, y);
                    portals.Add(p);
                }
            }

            y = 104;
            for (int i = 0; i < lines[y].Length; i++)
            {
                if (lines[y][i] == '.' && char.IsLetter(lines[y + 1][i]))
                {
                    // portal
                    Portal p = new Portal();
                    p.Name = lines[y + 1][i].ToString() + lines[y + 2][i].ToString();
                    p.Location = new Point(i, y + 1);
                    p.OutputLocation = new Point(i, y);
                    portals.Add(p);
                }
            }

            for (int i = 0; i < lines.Count; i++)
            {
                if (char.IsLetter(lines[i][0]))
                {
                    Portal p = new Portal();
                    p.Name = lines[i][0].ToString() + lines[i][1].ToString();
                    p.Location = new Point(1, i);
                    p.OutputLocation = new Point(2, i);
                    portals.Add(p);
                }

                int len = lines[i].Length;
                if (char.IsLetter(lines[i][len - 1]))
                {
                    Portal p = new Portal();
                    p.Name = lines[i][len - 2].ToString() + lines[i][len - 1].ToString();
                    p.Location = new Point(len - 2, i);
                    p.OutputLocation = new Point(len - 3, i);
                    portals.Add(p);
                }

                len = 29;
                if (char.IsLetter(lines[i][len - 1]))
                {
                    Portal p = new Portal();
                    p.Name = lines[i][len - 2].ToString() + lines[i][len - 1].ToString();
                    p.Location = new Point(len - 2, i);
                    p.OutputLocation = new Point(len - 3, i);
                    portals.Add(p);
                }

                int s = 78;
                if (char.IsLetter(lines[i][s]))
                {
                    Portal p = new Portal();
                    p.Name = lines[i][s].ToString() + lines[i][s + 1].ToString();
                    p.Location = new Point(s + 1, i);
                    p.OutputLocation = new Point(s + 2, i);
                    portals.Add(p);
                }
            }

            for (int yy = 0; yy < lines.Count; yy++)
            {
                for (int x = 0; x < lines[yy].Length; x++)
                {
                    Point p = new Point(x, yy);
                    grid[p] = lines[yy][x];
                }
            }
        }

        public int Part1()
        {
            // AA portal
            Point start = portals.First(p => p.Name.Equals("AA")).OutputLocation;

            int part1Distance = GetDistanceBfs(new Day20GridState { Point = start }, false);
            return part1Distance;
        }

        public int Part2()
        {
            // AA portal
            Point start = portals.First(p => p.Name.Equals("AA")).OutputLocation;

            int distance = GetDistanceBfs(new Day20GridState { Point = start }, true);
            return distance;
        }

        private int CalculateLevel(Point p)
        {
            // if we go through a portal on the outside of the
            // torus, we go "down" a level
            int maxX = grid.Keys.Max(p => p.X);
            int maxY = grid.Keys.Max(p => p.Y);

            if (p.X == 2 || p.Y == 2 || p.X == maxX - 2 || p.Y == maxY - 2)
            {
                return -1;
            }

            // otherwise we go "up" a level
            return 1;
        }

        private int GetDistanceBfs(Day20GridState start, bool useLevels)
        {
            Queue<Day20GridState> q = new Queue<Day20GridState>();
            HashSet<Day20GridState> visited = new HashSet<Day20GridState>();

            q.Enqueue(start);
            visited.Add(start);

            while (q.Any())
            {
                Day20GridState state = q.Dequeue();

                var thisPortal = portals.FirstOrDefault(p => p.OutputLocation.Equals(state.Point));
                string portalName = (thisPortal == null ? null : thisPortal.Name);

                // we have reached a portal
                if (portalName != null)
                {
                    if (state.Level == 0 && portalName == "ZZ")
                    {
                        // this is the exit to the maze
                        return state.Steps;
                    }

                    // get portals with this name -- there will be two
                    // unless it's AA. If it's one of the others, we
                    // need to queue up a location that is the other
                    // portal
                    var allPortals = portals.Where(p => p.Name.Equals(portalName));
                    foreach (var p in allPortals)
                    {
                        // if it's not the one we're already on
                        if (!p.OutputLocation.Equals(state.Point))
                        {
                            int newLevel = 0;
                            if (useLevels)
                            {
                                newLevel = state.Level + CalculateLevel(state.Point);
                            }

                            if (!useLevels || (useLevels && newLevel >= 0))
                            {
                                // teleport!
                                Day20GridState newState = new Day20GridState
                                {
                                    Point = p.OutputLocation,
                                    Steps = state.Steps + 1,
                                    FromState = state,
                                    Level = newLevel
                                };

                                if (visited.Add(newState))
                                {
                                    q.Enqueue(newState);
                                }
                            }
                        }
                    }
                }

                Visit(q, visited, state, -1, 0);
                Visit(q, visited, state, 1, 0);
                Visit(q, visited, state, 0, -1);
                Visit(q, visited, state, 0, 1);
            }

            throw new InvalidOperationException();
        }

        private void Visit(
            Queue<Day20GridState> q,
            HashSet<Day20GridState> visited,
            Day20GridState state,
            int xDelta,
            int yDelta)
        {
            Day20GridState newState = null;

            int newX = state.Point.X + xDelta;
            int newY = state.Point.Y + yDelta;

            var p = new Point(newX, newY);

            if (!grid.ContainsKey(p))
            {
                // not in the grid, can't move here
                newState = null;
            }
            else
            {
                char c = grid[p];
                if (c == '.')
                {
                    // movable space
                    newState = new Day20GridState
                    {
                        FromState = state,
                        Steps = state.Steps + 1,
                        Point = new Point(newX, newY),
                        Level = state.Level
                    };
                }
                else
                {
                    // hit a wall
                    newState = null;
                }
            }

            if (newState != null)
            {
                if (visited.Add(newState))
                {
                    q.Enqueue(newState);
                }
            }
        }
    }
}
