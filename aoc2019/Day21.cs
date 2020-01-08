using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;
using System.Text;

namespace aoc2019
{
    public class Day21
    {
        public Day21()
        {
        }

        public long Part1()
        {
            IntcodeComputer computer = new IntcodeComputer(InputUtils.GetFileName(21));

            computer.Run();
            var initialOutput = computer.GetAllOutput().ToList();
            if (initialOutput.Any())
            {
                StringBuilder outputBuilder = new StringBuilder();
                foreach (var o in initialOutput)
                {
                    outputBuilder.Append((char)o);
                }

                Console.WriteLine(outputBuilder.ToString());
            }

            List<int> input = new List<int>();
            List<string> program = new List<string>();

            program.Add("NOT A J");
            program.Add("OR J T");
            program.Add("NOT B J");
            program.Add("OR T J");
            program.Add("NOT C T");
            program.Add("OR T J");
            program.Add("AND D J");
            program.Add("WALK");

            foreach (string s in program)
            {
                foreach (char c in s)
                {
                    input.Add((int)c);
                }
                input.Add(10);
            }

            foreach (var i in input)
            {
                computer.EnqueueInput(i);
                computer.Run();
                var output = computer.GetAllOutput().ToList();
                if (output.Any())
                {
                    return output.Last();
                }
            }

            return 0;
        }

        public long Part2()
        {
            IntcodeComputer computer = new IntcodeComputer(InputUtils.GetFileName(21));

            computer.Run();
            var initialOutput = computer.GetAllOutput().ToList();
            if (initialOutput.Any())
            {
                StringBuilder outputBuilder = new StringBuilder();
                foreach (var o in initialOutput)
                {
                    outputBuilder.Append((char)o);
                }

                Console.WriteLine(outputBuilder.ToString());
            }

            List<int> input = new List<int>();
            List<string> program = new List<string>();

            program.Add("OR E J");
            program.Add("OR H J");
            program.Add("AND D J");
            program.Add("OR A T");
            program.Add("AND B T");
            program.Add("AND C T");
            program.Add("NOT T T");
            program.Add("AND T J");
            program.Add("RUN");

            foreach (string s in program)
            {
                foreach (char c in s)
                {
                    input.Add((int)c);
                }
                input.Add(10);
            }

            foreach (var i in input)
            {
                computer.EnqueueInput(i);
                computer.Run();
                var output = computer.GetAllOutput().ToList();
                if (output.Any())
                {
                    return output.Last();
                }
            }

            return 0;
        }
    }
}
