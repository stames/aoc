using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;

namespace aoc2019
{
    public class Day6
    {
        HashSet<string> nodes = new HashSet<string>();
        List<Tuple<string, string>> orbits = new List<Tuple<string, string>>();
        List<string> inputList;

        public Day6()
        {
            inputList = InputUtils.GetDayInputLines(6);

            // orbits, e.g. abc)def
            // def orbits abc
            foreach (var i in inputList)
            {
                string inner = i.Split(')')[0];
                string outer = i.Split(')')[1];
                nodes.Add(inner);
                nodes.Add(outer);

                orbits.Add(Tuple.Create(inner, outer));
            }
        }

        public int Part1()
        {
            int totalLength = 0;
            foreach (var node in nodes)
            {
                // calculate length to COM
                totalLength += CalculateLength(node);
            }

            return totalLength;
        }

        public int Part2()
        {
            List<string> pathFromYou = new List<string>();

            var you = orbits.Where(p => p.Item2 == "YOU").First();
            while (true)
            {
                pathFromYou.Add(you.Item1);
                you = orbits.Where(p => p.Item2 == you.Item1).FirstOrDefault();
                if (you == null)
                {
                    break;
                }
            }

            List<string> pathFromSan = new List<string>();

            var san = orbits.Where(p => p.Item2 == "SAN").First();
            while (true)
            {
                pathFromSan.Add(san.Item1);
                san = orbits.Where(p => p.Item2 == san.Item1).FirstOrDefault();
                if (san == null)
                {
                    break;
                }
            }

            pathFromYou.Reverse();
            pathFromSan.Reverse();

            // find overlap
            int j = 0;
            while (pathFromSan[j] == pathFromYou[j])
            {
                j++;
            }

            string divergenceNode = pathFromSan[j - 1];

            // distance from YOU to divergence + SAN to divergence
            int distanceFromYou = 0;
            var next = orbits.Where(p => p.Item2 == "YOU").First();
            while (true)
            {
                distanceFromYou++;
                next = orbits.Where(p => p.Item2 == next.Item1).FirstOrDefault();
                if (next == null || next.Item1 == divergenceNode)
                {
                    break;
                }
            }

            int distanceFromSan = 0;
            var nextSan = orbits.Where(p => p.Item2 == "SAN").First();
            while (true)
            {
                distanceFromSan++;
                nextSan = orbits.Where(p => p.Item2 == nextSan.Item1).FirstOrDefault();
                if (nextSan == null || nextSan.Item1 == divergenceNode)
                {
                    break;
                }
            }

            return distanceFromSan + distanceFromYou;
        }

        private int CalculateLength(string node)
        {
            // get nodes that 'node' orbits
            var innerNodes = orbits.Where(p => p.Item2 == node).Select(p => p.Item1).ToList();

            int ret = 0;
            foreach (var n in innerNodes)
            {
                ret += 1 + CalculateLength(n);
            }

            return ret;
        }
    }
}
