using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;

namespace aoc2019
{
    public class Day4
    {
        int min = 134792;
        int max = 675810;

        public Day4()
        {
        }

        public int Part1()
        {
            int count = 0;
            for (int i = min; i <= max; i++)
            {
                if (MatchesPattern(i, false))
                {
                    count++;
                }
            }
            return count;
        }

        public int Part2()
        {
            int count = 0;
            for (int i = min; i <= max; i++)
            {
                if (MatchesPattern(i, true))
                {
                    count++;
                }
            }
            return count;
        }

       
        private bool MatchesPattern(int value, bool requireExact)
        {
            // 2 adjacent digits are the same
            var chars = value.ToString().ToCharArray();
            if (CheckRepeat(value, requireExact))
            {
                // left to right, never decrease
                int v = 0;
                for (int c = 0; c < 6; c++)
                {
                    if (int.Parse(chars[c].ToString()) < v)
                    {
                        return false;
                    }

                    v = int.Parse(chars[c].ToString());
                }

                return true;
            }
            return false;
        }

        private bool CheckRepeat(int value, bool requireExact)
        {
            var chars = value.ToString().ToCharArray();
            if (chars[0] == chars[1] ||
                chars[1] == chars[2] ||
                chars[2] == chars[3] ||
                chars[3] == chars[4] ||
                chars[4] == chars[5])
            {
                if(!requireExact)
                {
                    // there are two or more repeating digits
                    return true;
                }

                bool isExactlyTwo = false;
                
                for (int c = 1; c < 6; c++)
                {
                    if ((chars[c] == chars[c - 1]) &&
                        (c == 1 || (chars[c] != chars[c - 2])) &&
                        (c == 5 || (chars[c] != chars[c + 1])))

                    {
                        isExactlyTwo = true;
                    }
                }

                return isExactlyTwo;
            }

            return false;
        }
    }
}
