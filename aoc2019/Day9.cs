using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;

namespace aoc2019
{
    public class Day9
    {
        public Day9()
        {
        }

        public long Part1()
        {
            IntcodeComputer computer = new IntcodeComputer(InputUtils.GetFileName(9));
            computer.EnqueueInput(1);
            computer.Run();

            return computer.GetAllOutput().Last();
        }

        public long Part2()
        {
            IntcodeComputer computer = new IntcodeComputer(InputUtils.GetFileName(9));
            computer.EnqueueInput(2);
            computer.Run();

            return computer.GetAllOutput().Last();
        }
    }
}
