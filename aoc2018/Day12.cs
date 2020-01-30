using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;
using System.Text;

namespace aoc2018
{
    public class Day12
    {

        public Day12()
        {
        }

        public long Part1()
        {
            var lines = InputUtils.GetDayInputLines(2018, 12);

            // line 0 is initial state
            string initialState = lines[0].Split(':')[1].Trim();
            int initialLength = initialState.Length;

            List<(string input, string output)> rules = new List<(string input, string output)>();

            for(int i = 2; i < lines.Count; i++)
            {
                rules.Add((lines[i].Split("=>")[0].Trim(), lines[i].Split("=>")[1].Trim()));
            }

            long generation = 0;
            Dictionary<int, bool> plants = new Dictionary<int, bool>();
            for(int i = -25; i < initialLength + 25; i++)
            {
                plants.Add(i, false);
            }
            for(int i = 0; i < initialLength; i++)
            {
                plants[i] = initialState[i] == '#';
            }

            while (generation < 20)
            {
                // apply rules
                Dictionary<int, bool> temp = new Dictionary<int, bool>();

                // check each location against the pattern
                // should location i have a plant
                for (int i = -25; i < initialLength + 1000; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    for(int j = i - 2; j <= i + 2; j++)
                    {
                        if(plants.ContainsKey(j) && plants[j])
                        {
                            sb.Append("#");
                        }
                        else
                        {
                            sb.Append(".");
                        }
                    }

                    if(rules.First(p => p.input.Equals(sb.ToString(), StringComparison.OrdinalIgnoreCase)).output == "#")
                    {
                        temp.Add(i, true);
                    }
                    else
                    {
                        temp.Add(i, false);
                    }
                }

                plants = new Dictionary<int, bool>(temp);
                generation++;
            }

            long sum = 0;
            foreach(var plant in plants)
            {
                if(plant.Value)
                {
                    sum += plant.Key;
                }
            }

            return sum;
        }

        public long Part2()
        {
            // use part 1 to draw out 1000 generations, find pattern
            // and offsets, then populate here.

            // after 164 generations, starts at pot 78
            // after 165 generations, starts at pot 79 (-86)
            // after 50000000000 generations, starts at pot 49 999 999 914
            long startingPot = 50000000000 - 86;
            string state = "###.#....###...###....###......###....###....###....###....###.......###....###......###" +
                "...###........###......###...........###......###....###...###...###.......###...###...###...###";

            long sum = 0;
            for(int i = 0; i < state.Length; i++)
            {
                if(state[i] == '#')
                {
                    sum += startingPot + i;
                }
            }

            return sum;
        }
    }
}
