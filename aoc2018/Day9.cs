using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;

namespace aoc2018
{
    public class Day9
    {
        public Day9()
        {
        }

        public long Part1()
        {
            // 405 players; last marble is worth 71700 points
            return PlayMarbleGame(405, 71700);
        }

        public long Part2()
        {
			return PlayMarbleGame(405, 71700 * 100);
        }

        private long PlayMarbleGame(int playerCount, int lastMarble)
        {
            LinkedList<long> gameBoard = new LinkedList<long>();
            LinkedListNode<long> currentMarble = new LinkedListNode<long>(0);
            List<long> scores = new List<long>();
            scores.AddRange(Enumerable.Repeat(0L, playerCount + 1));

            gameBoard.AddLast(0);
            gameBoard.AddLast(1);

            int turn = 2;
            int currentElf = 0;
            currentMarble = gameBoard.Last;

            while(turn <= lastMarble)
            {
                if(turn % 23 == 0)
                {
                    currentElf = turn % playerCount;
                    if (currentElf == 0)
                    {
                        // actually the last one, e.g. 10 players
                        // and turn 10 is elf 10, not 0
                        currentElf = playerCount;
                    }

                    scores[currentElf] += turn;

                    // move current marble 7 to the left
                    for(int i = 0; i < 7; i++)
                    {
                        currentMarble = currentMarble.Previous ?? gameBoard.Last;
                    }

                    scores[currentElf] += currentMarble.Value;
                    currentMarble = currentMarble.Next ?? gameBoard.First;
                    gameBoard.Remove(currentMarble.Previous ?? gameBoard.Last);
                }
                else
                {
                    // move current 1 to the right, then insert
                    currentMarble = currentMarble.Next ?? gameBoard.First;

                    gameBoard.AddAfter(currentMarble, turn);
                    currentMarble = currentMarble.Next;
                }

                turn++;
            }

            return scores.Max();
        }
    }
}
