using System;
using System.Diagnostics;

namespace aoc2018
{
    class Program
    {
        static void Main(string[] args)
        {
            var day = new Day17();

            Stopwatch sw = Stopwatch.StartNew();
            long part1 = day.Part1();
            sw.Stop();
            Console.WriteLine("Part 1: {0}, {1}ms", part1, sw.ElapsedMilliseconds);

            sw.Reset();
            sw.Start();
            long part2 = day.Part2();
            sw.Stop();
            Console.WriteLine("Part 2: {0}, {1}ms", part2, sw.ElapsedMilliseconds);

            Console.WriteLine("done");
        }
    }
}
