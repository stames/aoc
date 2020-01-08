using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;
using System.Text;

namespace aoc2019
{
    public class Day8
    {
        List<string> layers = new List<string>();
        int width = 25;
        int height = 6;

        public Day8()
        {
            string input = File.ReadAllText("/Users/jjacoby/testing/advent2019/day8.txt");

            int i = 0;
            while (i < input.Length)
            {
                var str = input.Skip(i).Take(width * height).ToArray();
                layers.Add(new string(str));
                i += (width * height);
            }
        }

        public int Part1()
        {
            // layer with fewest 0 digits
            int zeroCount = int.MaxValue;
            string finalLayer = string.Empty;
            foreach (var l in layers)
            {
                int z = l.ToArray().Count(p => p == '0');
                if (z < zeroCount)
                {
                    zeroCount = z;
                    finalLayer = l;
                }
            }

            int ones = finalLayer.ToArray().Count(p => p == '1');
            int twos = finalLayer.ToArray().Count(p => p == '2');

            return ones * twos;
        }

        public int Part2()
        {
            // final image
            int[,] image = new int[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    // all possible values in layers order
                    var list = new List<int>();
                    foreach (var layer in layers)
                    {
                        list.Add(int.Parse(layer[(j * width) + i].ToString()));
                    }

                    image[i, j] = GetFinalValue(list);
                }
            }

            DrawImage(image);
            return 0;
        }
       
        private void DrawImage(int[,] image)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (image[j, i] != 0)
                    {
                        sb.Append(".");
                    }
                    else
                    {
                        sb.Append(" ");
                    }
                }
                sb.AppendLine();
            }

            File.WriteAllText("/Users/jjacoby/testing/advent2019/day8out2.txt", sb.ToString());
        }

        private int GetFinalValue(List<int> ints)
        {
            // 0 = black
            // 1 = white
            // 2 = transparent
            // first layer in front, last layer in back
            return ints.First(p => p != 2);
        }
    }
}
