using System;

namespace aoc2019
{
    class Program
    {
        static void Main(string[] args)
        {
            var day = new Day24();

            //day.Test();

            long part1 = day.Part1();
            Console.WriteLine("Part 1: {0}", part1);

            long part2 = day.Part2();
            Console.WriteLine("Part 2: {0}", part2);

            Console.WriteLine("done");
        }
    }
}
