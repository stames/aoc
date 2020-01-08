using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;
using System.Text;

namespace aoc2019
{
    public class Day11
    {
        HashSet<Point> seenPoints = new HashSet<Point>();

        // grid - false = black, true = white
        Dictionary<Point, bool> grid = new Dictionary<Point, bool>();

        public Day11()
        { 

        }

        public void RunComputer(int startingValue)
        {
            grid = new Dictionary<Point, bool>();

            int startingX = 0;
            int startingY = 0;

            IntcodeComputer computer = new IntcodeComputer(InputUtils.GetFileName(11));

            Point currentPoint = new Point(startingX, startingY);
            int currentX = 0;
            int currentY = 0;
            Direction direction = Direction.Up;

            while (true)
            {
                // get current color
                int inputColor = startingValue;
                if (grid.ContainsKey(currentPoint))
                {
                    inputColor = grid[currentPoint] ? 1 : 0;
                }

                computer.EnqueueInput(inputColor);
                long outputColor = computer.RunUntilNextOutput();

                grid[currentPoint] = outputColor == 0 ? false : true;

                // 0 == turn left, 1 == turn right
                long outputDirection = computer.RunUntilNextOutput();

                seenPoints.Add(currentPoint);

                // set the next point
                if (direction == Direction.Up)
                {
                    if (outputDirection == 0)
                    {
                        direction = Direction.Left;
                    }
                    else
                    {
                        direction = Direction.Right;
                    }
                }
                else if (direction == Direction.Right)
                {
                    if (outputDirection == 0)
                    {
                        direction = Direction.Up;
                    }
                    else
                    {
                        direction = Direction.Down;
                    }
                }
                else if (direction == Direction.Left)
                {
                    if (outputDirection == 0)
                    {
                        direction = Direction.Down;
                    }
                    else
                    {
                        direction = Direction.Up;
                    }
                }
                else
                {
                    if (outputDirection == 0)
                    {
                        direction = Direction.Right;
                    }
                    else
                    {
                        direction = Direction.Left;
                    }
                }

                switch (direction)
                {
                    case Direction.Up:
                        currentY -= 1;
                        break;
                    case Direction.Down:
                        currentY += 1;
                        break;
                    case Direction.Left:
                        currentX -= 1;
                        break;
                    case Direction.Right:
                        currentX += 1;
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                currentPoint = new Point(currentX, currentY);

                if (computer.IsHalted)
                {
                    break;
                }
            }
        }

        public int Part1()
        {
            RunComputer(0);
            return seenPoints.Count;
        }

        public int Part2()
        {
            RunComputer(1);

            GridUtils.DrawGenericGridToFile(
                grid,
                "/Users/jjacoby/testing/advent2019/day11out2.txt",
                p => p ? '#' : '.');
            
            return 0;
        }

        enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}
