using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;
using System.Text;

namespace aoc2018
{
    public class Day14
    {
        public Day14()
        {
        }

        public int Part1()
        {
            int recipeCount = 793061;

            List<int> recipes = new List<int>();
            recipes.Add(3);
            recipes.Add(7);

            int elf1CurrentIndex = 0;
            int elf2CurrentIndex = 1;

            int round = 0;

            while (round < recipeCount + 10)
            {
                int result = recipes[elf1CurrentIndex] + recipes[elf2CurrentIndex];
                if (result < 10)
                {
                    recipes.Add(result);
                }
                else
                {
                    recipes.Add(result / 10);
                    recipes.Add(result % 10);
                }

                // move elf1 forward 1 + recipes[elf1CurrentIndex]
                // and elf2 forward 1 + recipes[elf2CurrentIndex]
                elf1CurrentIndex = (elf1CurrentIndex + 1 + recipes[elf1CurrentIndex]) % recipes.Count;
                elf2CurrentIndex = (elf2CurrentIndex + 1 + recipes[elf2CurrentIndex]) % recipes.Count;

                round++;
            }

            // write last 10
            for (int i = recipeCount; i < recipeCount + 10; i++)
            {
                Console.Write(recipes[i]);
            }
            Console.WriteLine();

            return 0;
        }

        public int Part2()
        {
            int recipeCount = 793061;
            int targetSize = recipeCount.ToString().Length;

            List<int> recipes = new List<int>();
            recipes.Add(3);
            recipes.Add(7);

            int elf1CurrentIndex = 0;
            int elf2CurrentIndex = 1;

            int round = 0;

            while (true)
            {
                if (MatchesEnd(recipes, targetSize, recipeCount))
                {
                    return recipes.Count - targetSize;
                }

                int result = recipes[elf1CurrentIndex] + recipes[elf2CurrentIndex];
                if (result < 10)
                {
                    recipes.Add(result);
                }
                else
                {
                    recipes.Add(result / 10);

                    // check after each addition, as it can match here too
                    if (MatchesEnd(recipes, targetSize, recipeCount))
                    {
                        return recipes.Count - targetSize;
                    }

                    recipes.Add(result % 10);
                }

                // move elf1 forward 1 + recipes[elf1CurrentIndex]
                elf1CurrentIndex = (elf1CurrentIndex + 1 + recipes[elf1CurrentIndex]) % recipes.Count;
                elf2CurrentIndex = (elf2CurrentIndex + 1 + recipes[elf2CurrentIndex]) % recipes.Count;

                round++;
            }
        }

        private bool MatchesEnd(List<int> recipes, int targetSize, int recipeCount)
        {
            if (recipes.Count <= 6)
            {
                return false;
            }

            var lastPortion = recipes.Skip(recipes.Count - targetSize).Take(targetSize).ToList();

            bool match = true;
            for (int i = 0; i < targetSize; i++)
            {
                if (lastPortion[i] != (recipeCount / (int)Math.Pow(10, targetSize - i - 1)) % 10)
                {
                    match = false;
                }
            }

            return match;
        }
    }
}
