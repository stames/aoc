using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;

namespace aoc2018
{
    public class Day1
    {
        public Day1()
        {
        }

        public int Part1()
        {
            var lines = InputUtils.GetDayInputLines(2018, 1);

            int result = 0;
            foreach(var line in lines)
            {
                result += int.Parse(line);
            }

            return result;
        }

        public int Part2()
        {
            var lines = InputUtils.GetDayInputLines(2018, 1);

            int result = 0;
            HashSet<int> frequencies = new HashSet<int>();
            while (true)
            {
                foreach (var line in lines)
                {
                    result += int.Parse(line);

                    if (frequencies.Contains(result))
                    {
                        return result;
                    }
                    frequencies.Add(result);
                }
            }
        }
    }
}
