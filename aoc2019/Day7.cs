using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;

namespace aoc2019
{
    public class Day7
    {
        public Day7()
        {
        }

        public int Part1()
        {
            List<int> results = new List<int>();

            List<int> values = new List<int> { 0, 1, 2, 3, 4 };
            var permutations = values.Permute();

            foreach (var permutation in permutations)
            {
                IntcodeComputer a = new IntcodeComputer(InputUtils.GetFileName(7));
                IntcodeComputer b = new IntcodeComputer(InputUtils.GetFileName(7));
                IntcodeComputer c = new IntcodeComputer(InputUtils.GetFileName(7));
                IntcodeComputer d = new IntcodeComputer(InputUtils.GetFileName(7));
                IntcodeComputer e = new IntcodeComputer(InputUtils.GetFileName(7));

                a.EnqueueInput(permutation.ToList()[0]);
                b.EnqueueInput(permutation.ToList()[1]);
                c.EnqueueInput(permutation.ToList()[2]);
                d.EnqueueInput(permutation.ToList()[3]);
                e.EnqueueInput(permutation.ToList()[4]);

                a.EnqueueInput(0);
                a.Run();
                b.EnqueueInput(a.GetAllOutput().First());
                b.Run();
                c.EnqueueInput(b.GetAllOutput().First());
                c.Run();
                d.EnqueueInput(c.GetAllOutput().First());
                d.Run();
                e.EnqueueInput(d.GetAllOutput().First());
                e.Run();

                results.Add((int)e.GetAllOutput().First());
            }

            return results.Max();
        }

        public int Part2()
        {
            List<int> results = new List<int>();

            List<int> values = new List<int> { 5, 6, 7, 8, 9 };
            var permutations = values.Permute();

            foreach (var permutation in permutations)
            {
                IntcodeComputer a = new IntcodeComputer(InputUtils.GetFileName(7));
                IntcodeComputer b = new IntcodeComputer(InputUtils.GetFileName(7));
                IntcodeComputer c = new IntcodeComputer(InputUtils.GetFileName(7));
                IntcodeComputer d = new IntcodeComputer(InputUtils.GetFileName(7));
                IntcodeComputer e = new IntcodeComputer(InputUtils.GetFileName(7));

                a.EnqueueInput(permutation.ToList()[0]);
                b.EnqueueInput(permutation.ToList()[1]);
                c.EnqueueInput(permutation.ToList()[2]);
                d.EnqueueInput(permutation.ToList()[3]);
                e.EnqueueInput(permutation.ToList()[4]);

                long eOutput = 0;

                while (!a.IsHalted && !b.IsHalted && !c.IsHalted && !d.IsHalted && !e.IsHalted)
                {
                    a.EnqueueInput(eOutput);
                    long aOutput = a.RunUntilNextOutput();

                    b.EnqueueInput(aOutput);
                    long bOutput = b.RunUntilNextOutput();

                    c.EnqueueInput(bOutput);
                    long cOutput = c.RunUntilNextOutput();

                    d.EnqueueInput(cOutput);
                    long dOutput = d.RunUntilNextOutput();

                    e.EnqueueInput(dOutput);
                    eOutput = e.RunUntilNextOutput();
                }

                results.Add((int)eOutput);
            }

            return results.Max();
        }
    }
}
