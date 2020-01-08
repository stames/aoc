using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;

namespace aoc2019
{
    enum BoardTile
    {
        Empty = 0,
        Wall,
        Block,
        HorizontalPaddle,
        Ball
    }

    public class Day13
    {
        public Day13()
        {
        }

        public int Part1()
        {
            Dictionary<Point, BoardTile> tiles = new Dictionary<Point, BoardTile>();

            IntcodeComputer computer = new IntcodeComputer(InputUtils.GetFileName(13));

            computer.Run();

            var output = computer.GetAllOutput().ToList();
            int i = 0;
            while(i < output.Count)
            {
                int x = (int)output[i];
                int y = (int)output[i + 1];
                BoardTile tile = (BoardTile)output[i + 2];

                tiles.Add(new Point(x, y), tile);

                i += 3;
            }

            return tiles.Count(p => p.Value == BoardTile.Block);
        }

        public long Part2()
        {
            Dictionary<Point, BoardTile> tiles = new Dictionary<Point, BoardTile>();

            IntcodeComputer computer = new IntcodeComputer(InputUtils.GetFileName(13));
            computer.SetMemory(0, 2);

            bool seenPaddle = false;
            bool seenBall = false;

            List<long> scores = new List<long>();

            while (!computer.IsHalted)
            {
                var paddle = tiles.FirstOrDefault(p => p.Value == BoardTile.HorizontalPaddle);
                var ball = tiles.FirstOrDefault(p => p.Value == BoardTile.Ball);

                seenPaddle = paddle.Key != null;
                seenBall = ball.Key != null;

                if (seenPaddle && seenBall)
                {
                    int paddleX = paddle.Key != null ? paddle.Key.X : 0;
                    int ballX = ball.Key != null ? ball.Key.X : 0;

                    int input = 0;

                    if (paddleX < ballX)
                    {
                        // move right
                        input = 1;
                    }
                    else if (ballX < paddleX)
                    {
                        // move left
                        input = -1;
                    }

                    computer.EnqueueInput(input);
                }

                // get the new board
                computer.Run();
                List<long> results = computer.GetAllOutput().ToList();

                int i = 0;
                while (i < results.Count - 2)
                {
                    long result0 = results[i];
                    long result1 = results[i + 1];
                    long result2 = results[i + 2];

                    if (result0 == -1 && result1 == 0)
                    {
                        // score
                        scores.Add(result2);
                    }
                    else
                    {
                        Point p = new Point((int)result0, (int)result1);
                        tiles[p] = (BoardTile)(int)result2;
                    }

                    i += 3;
                }
            }

            return scores.Last();
        }
    }
}
