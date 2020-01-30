using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;

namespace aoc2019
{
    public class Day24
    {
        Dictionary<int, Dictionary<Point, char>> levels = new Dictionary<int, Dictionary<Point, char>>();

        public Day24()
        {
        }

        public int Part1()
        {
            var lines = InputUtils.GetDayInputLines(24);

            Dictionary<Point, char> grid = new Dictionary<Point, char>();
            for (int y = 0; y < lines.Count; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    grid[new Point(x, y)] = lines[y][x];
                }
            }

            HashSet<int> diversityRatings = new HashSet<int>();

            int originalRating = CalculateRating(grid);
            diversityRatings.Add(originalRating);

            int minute = 0;

            while (true)
            {
                Dictionary<Point, char> tempGrid = new Dictionary<Point, char>();
                for (int y = 0; y < 5; y++)
                {
                    for (int x = 0; x < 5; x++)
                    {
                        Point p = new Point(x, y);

                        Point left = new Point(x - 1, y);
                        Point right = new Point(x + 1, y);
                        Point up = new Point(x, y - 1);
                        Point down = new Point(x, y + 1);

                        int isLeft = grid.ContainsKey(left) && grid[left] == '#' ? 1 : 0;
                        int isRight = grid.ContainsKey(right) && grid[right] == '#' ? 1 : 0;
                        int isUp = grid.ContainsKey(up) && grid[up] == '#' ? 1 : 0;
                        int isDown = grid.ContainsKey(down) && grid[down] == '#' ? 1 : 0;

                        int bugCount = isLeft + isRight + isUp + isDown;

                        char c = grid[p];

                        // if it's a bug
                        if (c == '#')
                        {
                            // A bug dies (becoming an empty space) unless there is exactly one bug adjacent to it.
                            if (bugCount == 1)
                            {
                                tempGrid[p] = '#';
                            }
                            else
                            {
                                tempGrid[p] = '.';
                            }
                        }
                        else
                        {
                            // An empty space becomes infested with a bug if exactly one or two bugs are adjacent to it.
                            if ((bugCount == 1) ||
                                (bugCount == 2))
                            {
                                tempGrid[p] = '#';
                            }
                            else
                            {
                                tempGrid[p] = '.';
                            }

                        }
                    }
                }

                int rating = CalculateRating(tempGrid);
                if (diversityRatings.Contains(rating))
                {
                    return rating;
                }

                diversityRatings.Add(rating);

                grid = new Dictionary<Point, char>(tempGrid);
                minute++;
            }
        }

        public int Part2()
        {
            var lines = InputUtils.GetDayInputLines(24);

            Dictionary<Point, char> originalGrid = new Dictionary<Point, char>();

            Dictionary<int, Dictionary<Point, char>> tempLevels = new Dictionary<int, Dictionary<Point, char>>();

            for (int y = 0; y < lines.Count; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    originalGrid[new Point(x, y)] = lines[y][x];
                }
            }

            levels.Add(0, originalGrid);

            for (int i = 1; i < 500; i++)
            {
                Dictionary<Point, char> level = new Dictionary<Point, char>();
                for (int y = 0; y < lines.Count; y++)
                {
                    for (int x = 0; x < lines[y].Length; x++)
                    {
                        level[new Point(x, y)] = '.';
                    }
                }

                levels.Add(i, level);
            }

            for (int i = -1; i > -500; i--)
            {
                Dictionary<Point, char> level = new Dictionary<Point, char>();
                for (int y = 0; y < lines.Count; y++)
                {
                    for (int x = 0; x < lines[y].Length; x++)
                    {
                        level[new Point(x, y)] = '.';
                    }
                }

                levels.Add(i, level);
            }

            int minute = 0;
            tempLevels = new Dictionary<int, Dictionary<Point, char>>(levels);

            while (true)
            {
                for (int levelIndex = -498; levelIndex < 499; levelIndex++)
                {
                    Dictionary<Point, char> grid = levels[levelIndex];
                    Dictionary<Point, char> tempGrid = new Dictionary<Point, char>();
                    for (int y = 0; y < 5; y++)
                    {
                        for (int x = 0; x < 5; x++)
                        {
                            Point p = new Point(x, y);

                            List<char> neighbors = new List<char>();

                            // if a point is on the edge, get from the next level
                            if (x == 0)
                            {
                                // left
                                // higher level location 1,2
                                neighbors.Add(levels[levelIndex + 1][new Point(1, 2)]);
                            }
                            if (y == 0)
                            {
                                // top
                                neighbors.Add(levels[levelIndex + 1][new Point(2, 1)]);
                            }
                            if (x == 4)
                            {
                                // right
                                neighbors.Add(levels[levelIndex + 1][new Point(3, 2)]);
                            }
                            if (y == 4)
                            {
                                // bottom
                                neighbors.Add(levels[levelIndex + 1][new Point(2, 3)]);
                            }

                            // inner neighbours
                            if (x == 2 && y == 1)
                            {
                                // add the top row of the next level
                                for (int xx = 0; xx < 5; xx++)
                                {
                                    neighbors.Add(levels[levelIndex - 1][new Point(xx, 0)]);
                                }
                            }
                            if (x == 2 && y == 3)
                            {
                                // add the bottom row of the next level
                                for (int xx = 0; xx < 5; xx++)
                                {
                                    neighbors.Add(levels[levelIndex - 1][new Point(xx, 4)]);
                                }
                            }
                            if (x == 1 && y == 2)
                            {
                                // add the left column of the next level
                                for (int yy = 0; yy < 5; yy++)
                                {
                                    neighbors.Add(levels[levelIndex - 1][new Point(0, yy)]);
                                }
                            }
                            if (x == 3 && y == 2)
                            {
                                // add the right column of the next level
                                for (int yy = 0; yy < 5; yy++)
                                {
                                    neighbors.Add(levels[levelIndex - 1][new Point(4, yy)]);
                                }
                            }

                            // regular points
                            Point left = new Point(x - 1, y);
                            Point right = new Point(x + 1, y);
                            Point up = new Point(x, y - 1);
                            Point down = new Point(x, y + 1);

                            int isLeft = grid.ContainsKey(left) && grid[left] == '#' ? 1 : 0;
                            int isRight = grid.ContainsKey(right) && grid[right] == '#' ? 1 : 0;
                            int isUp = grid.ContainsKey(up) && grid[up] == '#' ? 1 : 0;
                            int isDown = grid.ContainsKey(down) && grid[down] == '#' ? 1 : 0;

                            int bugCount = neighbors.Count(p => p == '#');

                            if (x == 2 && y == 2)
                            {
                                // nothing
                            }
                            else
                            {
                                bugCount += isLeft + isRight + isUp + isDown;
                            }

                            char c = grid[p];

                            // if it's a bug
                            if (c == '#')
                            {
                                // A bug dies (becoming an empty space) unless there is exactly one bug adjacent to it.
                                if (bugCount == 1)
                                {
                                    tempGrid[p] = '#';
                                }
                                else
                                {
                                    tempGrid[p] = '.';
                                }
                            }
                            else
                            {
                                // An empty space becomes infested with a bug if exactly one or two bugs are adjacent to it.
                                if ((bugCount == 1) ||
                                    (bugCount == 2))
                                {
                                    tempGrid[p] = '#';
                                }
                                else
                                {
                                    tempGrid[p] = '.';
                                }

                            }
                        }
                    }

                    tempLevels[levelIndex] = tempGrid;
                }

                levels = new Dictionary<int, Dictionary<Point, char>>(tempLevels);

                minute++;

                if (minute == 200)
                {
                    // count bugs
                    int cnt = 0;
                    foreach (var l in levels)
                    {

                        for (int y = 0; y < 5; y++)
                        {
                            for (int x = 0; x < 5; x++)
                            {
                                if (l.Value[new Point(x, y)] == '#')
                                {
                                    cnt++;
                                }
                            }
                        }

                    }

                    return cnt;
                }
            }
        }

        private int CalculateRating(Dictionary<Point, char> grid)
        {
            int rating = 0;
            int ratingTemp = 1;
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    if (grid[new Point(x, y)] == '#')
                    {
                        rating += ratingTemp;
                    }

                    ratingTemp *= 2;
                }
            }

            return rating;
        }
    }
}
