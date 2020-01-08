using System;
using System.Linq;
using AdventOfCode;

namespace aoc2019
{
    public class Day1
    {
        public Day1()
        {
        }

        public int Part1()
        {
            var lines = InputUtils.GetDayInputLines(1).Select(p => int.Parse(p));

            int total = 0;
            foreach(int line in lines)
            {
                total += (line / 3) - 2;
            }

            return total;
        }

        public int Part2()
        {
            int total = 0;

            var lines = InputUtils.GetDayInputLines(1).Select(p => int.Parse(p));

            foreach (int line in lines)
            {
                int mass = line;
                while (mass >= 0)
                {
                    int result = mass / 3;
                    result -= 2;

                    if (result > 0)
                    {
                        total += result;
                    }

                    mass = result;
                }
            }

            return total;
        }
    }
}
