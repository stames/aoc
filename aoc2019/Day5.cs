using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;

namespace aoc2019
{
    public class Day5
    {
        public Day5()
        {
        }

        public int Part1()
        {
            IntcodeComputer computer = new IntcodeComputer(InputUtils.GetFileName(5));
            computer.EnqueueInput(1);
            computer.Run();

            return (int)computer.GetAllOutput().Last();
        }

        public int Part2()
        {
            IntcodeComputer computer = new IntcodeComputer(InputUtils.GetFileName(5));
            computer.EnqueueInput(5);
            computer.Run();

            return (int)computer.GetAllOutput().Last();
        }
    }
}
