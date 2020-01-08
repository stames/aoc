using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;
using System.Diagnostics.CodeAnalysis;

namespace aoc2019
{
    public class GridState
    {
        public GridState OriginalLocation { get; set; }
        public Point Location { get; set; }
        public int Steps { get; set; }
        public int BitMask { get; set; }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            GridState that = (GridState)obj;

            return (this.BitMask == that.BitMask &&
                this.Location.Equals(that.Location));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Location.X.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Location.Y.GetHashCode();
                hashCode = (hashCode * 397) ^ BitMask.GetHashCode();
                return hashCode;
            }
        }
    }

    public class Day18
    {
        Dictionary<Point, char> grid;

        public Day18()
        {
            grid = InputUtils.GetDayInputGrid(18);
        }

        public int Part1()
        {
            GridState a = new GridState();
            GridState b = new GridState();

            Point robot = grid.Where(p => p.Value == '@').First().Key;
            GridState state = new GridState();
            state.Location = new Point(robot.X, robot.Y);
            HashSet<char> allKeys = new HashSet<char>();
            int allKeysBitMask = 0;
            for (char c = 'a'; c <= 'z'; c++)
            {
                allKeysBitMask |= CharToBit(c);
            }

            int path = GetOverallShortestPath(state, allKeysBitMask);

            return path;
        }

        public int Part2()
        {
            // break the grid into 4, and
            // find keys in each quadrant
            int northWestBitMask = GetBitMask(0, 0, 39, 39);
            int northEastBitMask = GetBitMask(41, 0, 80, 39);
            int southWestBitMask = GetBitMask(0, 41, 39, 80);
            int southEastBitMask = GetBitMask(41, 41, 80, 80);

            GridState nwStart = new GridState { Location = new Point(39, 39) };
            GridState neStart = new GridState { Location = new Point(41, 39) };
            GridState swStart = new GridState { Location = new Point(39, 41) };
            GridState seStart = new GridState { Location = new Point(41, 41) };

            int path1 = GetOverallShortestPath(nwStart, northWestBitMask);
            int path2 = GetOverallShortestPath(neStart, northEastBitMask);
            int path3 = GetOverallShortestPath(swStart, southWestBitMask);
            int path4 = GetOverallShortestPath(seStart, southEastBitMask);

            return path1 + path2 + path3 + path4;
        }

        private int GetBitMask(int minRow, int minCol, int maxRow, int maxCol)
        {
            int targetBitset = 0;
            for (int row = minRow; row <= maxRow; row++)
            {
                for (int col = minCol; col <= maxCol; col++)
                {
                    char c = grid[new Point(row, col)];
                    if (c >= 'a' && c <= 'z')
                    {
                        targetBitset |= CharToBit(c);
                    }

                }
            }

            return targetBitset;
        }

        private int GetOverallShortestPath(GridState startState, int keysBitmask)
        {
            // breadth-first search
            Queue<GridState> q = new Queue<GridState>();
            HashSet<GridState> visited = new HashSet<GridState>();

            q.Enqueue(startState);
            visited.Add(startState);

            while (q.Any())
            {
                GridState state = q.Dequeue();

                // if the state has reached all the requested keys,
                // we have gathered everything
                if (state.BitMask == keysBitmask)
                {
                    return state.Steps;
                }

                // visit each of the adjacent locations
                Visit(visited, q, state, -1, 0, keysBitmask);
                Visit(visited, q, state, 1, 0, keysBitmask);
                Visit(visited, q, state, 0, -1, keysBitmask);
                Visit(visited, q, state, 0, 1, keysBitmask);
            }

            throw new InvalidOperationException();
        }

        private void Visit(
            HashSet<GridState> visited,
            Queue<GridState> queue,
            GridState from,
            int xDelta,
            int yDelta,
            int keysBitmask)
        {
            GridState newState = MoveToNewState(from, xDelta, yDelta, keysBitmask);

            if (newState != null)
            {
                if (visited.Add(newState))
                {
                    queue.Enqueue(newState);
                }
            }
        }

        private GridState MoveToNewState(
            GridState state,
            int xDelta,
            int yDelta,
            int keysBitmask)
        {
            int newX = state.Location.X + xDelta;
            int newY = state.Location.Y + yDelta;

            var p = new Point(newX, newY);
            if (!grid.ContainsKey(p))
            {
                return null;
            }

            char c = grid[p];
            if (c == '#')
            {
                // hit a wall
                return null;
            }
            if (c >= 'A' && c <= 'Z')
            {
                // this is a door
                // key for this door is the associated lowercase letter
                char key = (char)(c + 32);
                int keybit = CharToBit(key);

                // if the key for this door is not in all the keys
                if ((keysBitmask & keybit) == 0)
                {
                    // this key is not in the keys we are looking for
                    // at all, so for part 2 it's in a different
                    // quadrant of the grid
                    return new GridState
                    {
                        Location = new Point(newX, newY),
                        OriginalLocation = state,
                        BitMask = state.BitMask,
                        Steps = state.Steps + 1
                    };
                }

                // if the key is in the state's found keys - found the key
                // for this door already so we can move there
                if ((state.BitMask & keybit) != 0)
                {
                    return new GridState
                    {
                        Location = new Point(newX, newY),
                        BitMask = state.BitMask,
                        OriginalLocation = state,
                        Steps = state.Steps + 1
                    };
                }

                // can't go here
                return null;
            }

            if (c >= 'a' && c <= 'z')
            {
                // we are at another key, pick it up
                // and move to this location
                return new GridState
                {
                    Location = new Point(newX, newY),
                    Steps = state.Steps + 1,
                    OriginalLocation = state,
                    BitMask = state.BitMask | CharToBit(c)
                };
            }

            // otherwise this is an empty location,
            // move there but don't update keys or anything
            return new GridState
            {
                Location = new Point(newX, newY),
                BitMask = state.BitMask,
                OriginalLocation = state,
                Steps = state.Steps + 1
            };
        }

        private int CharToBit(char c)
        {
            int index = c - 'a';
            return (1 << index);
        }
    }
}
