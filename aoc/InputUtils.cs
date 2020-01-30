using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    public class InputUtils
    {
        public static string GetFileName(int year, int day)
        {
            string fileName = String.Format("/Users/jjacoby/testing/advent{0}/day{1}.txt", year, day);
            return fileName;
        }

        public static string GetFileName(int day)
        {
            string fileName = String.Format("/Users/jjacoby/testing/advent2019/day{0}.txt", day);
            return fileName;
        }

        public static string GetDayInputString(int year, int day)
        {
            return File.ReadAllText(GetFileName(year, day));
        }

        public static string GetDayInputString(int day)
        {
            return File.ReadAllText(GetFileName(day));
        }

        public static List<string> GetDayInputLines(int year, int day)
        {
            string fileName = GetFileName(year, day);
            return GetInputLines(fileName);
        }

        public static List<string> GetDayInputLines(int day)
        {
            string fileName = GetFileName(day);
            return GetInputLines(fileName);
        }

        public static List<string> GetInputLines(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            return new List<string>(lines);
        }

        public static Dictionary<Point, char> GetDayInputGrid(int day)
        {
            return GetInputGrid(GetFileName(day));
        }

        public static Dictionary<Point, char> GetDayInputGrid(int year, int day)
        {
            return GetInputGrid(GetFileName(year, day));
        }

        public static Dictionary<Point, char> GetInputGrid(string fileName)
        {
            Dictionary<Point, char> grid = new Dictionary<Point, char>();
            string[] lines = File.ReadAllLines(fileName);

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    grid[new Point(x, y)] = lines[y][x];
                }
            }

            return grid;
        }
    }
}
