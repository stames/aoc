using System;
using System.Linq;
using AdventOfCode;

namespace aoc2019
{
    public class Day2
    {
        public Day2()
        {
        }

        public int Part1()
        {
            IntcodeComputer computer = new IntcodeComputer(InputUtils.GetFileName(2));

            computer.SetMemory(1, 12);
            computer.Run();
            return (int)computer.GetValueAtAddress(0);
        }

        public int Part2()
        {
            long desiredOutput = 19690720;

            IntcodeComputer computer = new IntcodeComputer(InputUtils.GetFileName(2));

            for (int noun = 0; noun < 100; noun++)
            {
                for (int verb = 0; verb < 100; verb++)
                {
                    computer.Reboot();
                    computer.SetMemory(1, noun);
                    computer.SetMemory(2, verb);

                    computer.Run();

                    if(computer.GetValueAtAddress(0) == desiredOutput)
                    {
                        return 100 * noun + verb;
                    }
                }
            }

            return 0;
        }
    }
}
