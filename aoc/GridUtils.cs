using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class GridUtils
    {
        public static void DrawGridToFile(Dictionary<Point, char> grid, string fileName)
        {
            int minX = grid.Keys.Min(p => p.X);
            int minY = grid.Keys.Min(p => p.Y);
            int maxX = grid.Keys.Max(p => p.X);
            int maxY = grid.Keys.Max(p => p.Y);

            StringBuilder sb = new StringBuilder();

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    Point p = new Point(x, y);
                    if (grid.ContainsKey(p))
                    {
                        sb.Append(grid[p]);
                    }
                }
                sb.AppendLine();
            }

            File.WriteAllText(fileName, sb.ToString());
        }

        public static void DrawGenericGridToFile<TValue>(Dictionary<Point, TValue> grid, string fileName, Func<TValue, char> charFunc)
        {
            int minX = grid.Keys.Min(p => p.X);
            int minY = grid.Keys.Min(p => p.Y);
            int maxX = grid.Keys.Max(p => p.X);
            int maxY = grid.Keys.Max(p => p.Y);

            StringBuilder sb = new StringBuilder();

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    Point p = new Point(x, y);
                    if (grid.ContainsKey(p))
                    {
                        sb.Append(charFunc(grid[p]));
                    }
                    else
                    {
                        sb.Append(" ");
                    }
                }
                sb.AppendLine();
            }

            File.WriteAllText(fileName, sb.ToString());
        }
    }
}
