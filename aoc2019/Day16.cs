using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;
using System.Text;

namespace aoc2019
{
    public class Day16
    {
        public Day16()
        {
        }

        public int Part1()
        {
            string input = InputUtils.GetDayInputString(16);

            List<int> inputList = input.ToCharArray().Select(p => int.Parse(p.ToString())).ToList();

            List<int> newList = new List<int>();
            for(int phase = 0; phase < 100; phase++)
            {
                for(int i = 0; i < inputList.Count; i++)
                { 
                    // calculate position i in new list, which is
                    // the sum of all of the previous values
                    int sum = 0;
                    for(int j = 0; j < inputList.Count; j++)
                    {
                        sum += GetMultiplier(i + 1, j) * inputList[j];
                    }
                    newList.Add(Math.Abs(sum) % 10);
                }

                inputList = new List<int>(newList);
                newList.Clear();
            }

            StringBuilder sb = new StringBuilder();
            for (int o = 0; o < 8; o++)
            {
                sb.Append(inputList[o]);
            }

            return int.Parse(sb.ToString());
        }

        public int Part2()
        {
            string input = InputUtils.GetDayInputString(16);

            int offset = 5970417;

            List<int> oo = input.ToCharArray().Select(p => int.Parse(p.ToString())).ToList();
            List<int> originalInput = new List<int>();

            for (int c = 0; c < 10000; c++)
            {
                originalInput.AddRange(oo);
            }

            for (int phase = 0; phase < 100; phase++)
            {
                int sum = 0;
                // we only need to check the tail, and they are all 1s in
                // the repeating pattern at that point
                // from the end, we take each digit going backwards
                // and use it as part of the sum for the next digit,
                // because the repeating pattern makes for zeroes all the
                // way up to the point we are calculating
                for (int i = 1; i < originalInput.Count - offset + 1; i++)
                {
                    sum += originalInput[originalInput.Count - i];
                    sum = Math.Abs(sum) % 10;

                    // since we're going backwards, just replace the
                    // slot we just used with what we just computed
                    originalInput[originalInput.Count - i] = sum;
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int o = 0; o < 8; o++)
            {
                sb.Append(originalInput[offset + o]);
            }

            return int.Parse(sb.ToString());
        }

        int GetMultiplier(int repeater, int position)
        {
            // base pattern is 0, 1, 0, -1
            // e.g. index 2 = 0,0,1,1,0,0,-1,-1

            int totalLength = repeater * 4;

            // e.g. if repeater is 4
            // 0,0,0,0,1,1,1,1,0,0,0,0,-1,-1,-1,-1
            // then shift, so 
            // 0,0,0,1,1,1,1,0,0,0,0,-1,-1,-1,-1,0
            // so 0 from index 0 -> repeater - 2
            // 1 from index repeater - 1 --> 2*repeater - 2
            // 0 from index 2*repeater - 1 --> 3*repeater - 2
            // -1 from index 3*repeater - 1 --> totalLength - 2
            // 0 for index == totalLength - 1

            int index = position % totalLength;
            if (index <= repeater - 2)
            {
                return 0;
            }
            else if (index <= (2 * repeater - 2))
            {
                return 1;
            }
            else if (index <= (3 * repeater - 2))
            {
                return 0;
            }
            else if (index <= (4 * repeater - 2))
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
