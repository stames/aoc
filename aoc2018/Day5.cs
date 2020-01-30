using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;
using System.Text;

namespace aoc2018
{
    public class Day5
    {
        public Day5()
        {
        }

        public int Part1()
        {
            string input = InputUtils.GetDayInputString(2018, 5);

            return CollapsePolymer(input);
        }

        public int Part2()
        {
            string input = InputUtils.GetDayInputString(2018, 5);
            var letters = Enumerable.Range((int)'a', 26).Select(p => (char)p).ToList();

            int smallest = int.MaxValue;
            foreach(var letter in letters)
            {
                // remove letter
                string newInput = input.Replace(letter.ToString(), String.Empty);
                newInput = newInput.Replace(char.ToUpper(letter).ToString(), String.Empty);

                int collapsed = CollapsePolymer(newInput);
                if(collapsed < smallest)
                {
                    smallest = collapsed;
                }
            }

            return smallest;
        }

        private int CollapsePolymer(string input)
        {
            int i = 0;
            while (i < input.Length - 1)
            {
                if (Math.Abs((int)input[i] - (int)input[i + 1]) == 32)
                {
                    // these react (lowercase and uppercase of the
                    // same letter), remove them and recalculate the grid
                    // from two back
                    input = input.Remove(i, 2);
                    i -= 2;
                    if (i < 0)
                    {
                        i = 0;
                    }
                }
                else
                {
                    i++;
                }
            }

            return input.Length;
        }
    }
}
