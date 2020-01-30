using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;

namespace aoc2019
{
    public class Day23
    {
        public Day23()
        {
        }

        public long Part1()
        {
            Dictionary<int, IntcodeComputer> computers = new Dictionary<int, IntcodeComputer>();

            for (int i = 0; i < 50; i++)
            {
                IntcodeComputer c = new IntcodeComputer(InputUtils.GetFileName(23));
                c.EnqueueInput(i);
                c.EnqueueInput(-1);
                computers.Add(i, c);
            }

            while (true)
            {
                for (int i = 0; i < 50; i++)
                {
                    computers[i].Run();

                    var output = computers[i].GetAllOutput().ToList();
                    if (output.Count() % 3 != 0 || output.Count() == 0)
                    {
                        continue;
                    }

                    for (int j = 0; j < output.Count; j += 3)
                    {
                        if (output[j] == 255)
                        {
                            return output[j + 2];
                        }

                        computers[(int)output[j]].EnqueueInput(output[j + 1]);
                        computers[(int)output[j]].EnqueueInput(output[j + 2]);
                    }
                }
            }
        }

        public long Part2()
        {
            Dictionary<int, IntcodeComputer> computers = new Dictionary<int, IntcodeComputer>();

            for (int i = 0; i < 50; i++)
            {
                IntcodeComputer c = new IntcodeComputer(InputUtils.GetFileName(23));
                c.EnqueueInput(i);
                c.EnqueueInput(-1);
                computers.Add(i, c);
            }

            long natX = 0;
            long natY = 0;
            long oldNatY = 0;

            while (true)
            {
                bool networkIdle = true;
                for (int i = 0; i < 50; i++)
                {
                    computers[i].Run();

                    var output = computers[i].GetAllOutput().ToList();
                    if (output.Count() % 3 != 0 || output.Count() == 0)
                    {
                        continue;
                    }

                    networkIdle = false;
                    for (int j = 0; j < output.Count; j += 3)
                    {
                        if (output[j] == 255)
                        {
                            natX = output[j + 1];
                            natY = output[j + 2];
                            continue;
                        }

                        computers[(int)output[j]].EnqueueInput(output[j + 1]);
                        computers[(int)output[j]].EnqueueInput(output[j + 2]);
                    }
                }

                if (networkIdle)
                {
                    if (natY == oldNatY)
                    {
                        return natY;
                    }

                    computers[0].EnqueueInput(natX);
                    computers[0].EnqueueInput(natY);
                    oldNatY = natY;
                }
            }
        }
    }
}
