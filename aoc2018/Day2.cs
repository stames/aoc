using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;

namespace aoc2018
{
    public class Day2
    {
        public Day2()
        {
        }

        public int Part1()
        {
            var lines = InputUtils.GetDayInputLines(2018, 2);

            int exactlyTwo = 0;
            int exactlyThree = 0;

            foreach(var line in lines)
            {
                bool found2 = false;
                bool found3 = false;

                var chars = line.ToCharArray();
                for(int i = 0; i < chars.Length; i++)
                {
                    if(chars.Count(p => p.Equals(chars[i])) == 2)
                    {
                        if (!found2)
                        {
                            exactlyTwo++;
                            found2 = true;
                        }
                    }
                    if (chars.Count(p => p.Equals(chars[i])) == 3)
                    {
                        if (!found3)
                        {
                            found3 = true;
                            exactlyThree++;
                        }
                    }
                }
            }

            return exactlyThree * exactlyTwo;
        }

        public int Part2()
        {
            var lines = InputUtils.GetDayInputLines(2018, 2);
            int len = lines[0].Length;

            foreach(var line in lines)
            {
                foreach(var inner in lines)
                {
                    if(line.Equals(inner))
                    {
                        // don't check itself
                        continue;
                    }

                    int same = 0;
                    for(int i = 0; i < len; i++)
                    {
                        if(line[i] == inner[i])
                        {
                            same++;
                        }
                    }

                    if(same == len - 1)
                    {
                        Console.WriteLine(line);
                        Console.WriteLine(inner);
                    }
                }
            }
            return 0;
        }
    }
}
