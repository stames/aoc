using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;
using System.Text;

namespace aoc2019
{
    public class Day25
    {
        public Day25()
        {
        }

        public int Part1()
        {
            IntcodeComputer computer = new IntcodeComputer("/Users/jjacoby/testing/advent2019/day25.txt");

            computer.Run();
            var initialOutput = computer.GetAllOutput();
            if (initialOutput.Any())
            {
                StringBuilder outputBuilder = new StringBuilder();
                foreach (var o in initialOutput)
                {
                    outputBuilder.Append(((char)o).ToString());
                }

                Console.WriteLine(outputBuilder.ToString());
            }

            List<int> input = new List<int>();

            List<string> program = new List<string>();
            program.Add("north");
            program.Add("east");
            program.Add("take coin");
            program.Add("west");
            program.Add("south");
            program.Add("south");
            program.Add("take food ration");
            program.Add("west");
            program.Add("take sand");
            program.Add("north");
            program.Add("north");
            program.Add("east");
            program.Add("take astrolabe");
            program.Add("west");
            program.Add("south");
            program.Add("south");
            program.Add("east");
            program.Add("north");
            program.Add("east");
            program.Add("take cake");
            program.Add("east");
            program.Add("east");
            program.Add("east");

            foreach (string s in program)
            {
                foreach (char c in s)
                {
                    computer.EnqueueInput((long)c);
                }
                computer.EnqueueInput(10);

                //computer.NextInput = i;
                computer.Run();
                var output = computer.GetAllOutput();
                if (output.Any())
                {
                    StringBuilder outputBuilder = new StringBuilder();
                    foreach (var o in output)
                    {
                        outputBuilder.Append(((char)o).ToString());
                    }

                    Console.WriteLine(outputBuilder.ToString());
                }
            }

            while (true)
            {
                string textInput = Console.ReadLine();

                foreach (char c in textInput)
                {
                    computer.EnqueueInput((long)c);
                }
                computer.EnqueueInput(10);

                //computer.NextInput = i;
                computer.Run();
                var output = computer.GetAllOutput();
                if (output.Any())
                {
                    StringBuilder outputBuilder = new StringBuilder();
                    foreach (var o in output)
                    {
                        outputBuilder.Append(((char)o).ToString());
                    }

                    Console.WriteLine(outputBuilder.ToString());
                }

                if (computer.IsHalted)
                {
                    return 0;
                }

                input.Clear();
            }
        }

        public int Part2()
        {
            return 0;
        }
    }
}
