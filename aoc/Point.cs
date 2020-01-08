using System;

namespace AdventOfCode
{
    public enum Direction
    {
        North = 1,
        South,
        West,
        East,
        Northeast,
        Northwest,
        Southeast,
        Southwest
    }

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.X.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Y.GetHashCode();
                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            Point other = (Point)obj;
            return other.X == this.X && other.Y == this.Y;
        }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return String.Format("({0},{1})", X, Y);
        }

        public Point PointInDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return new Point(X, Y - 1);
                case Direction.South:
                    return new Point(X, Y + 1);
                case Direction.East:
                    return new Point(X + 1, Y);
                case Direction.West:
                    return new Point(X - 1, Y);
                case Direction.Northeast:
                    return new Point(X + 1, Y - 1);
                case Direction.Northwest:
                    return new Point(X - 1, Y - 1);
                case Direction.Southeast:
                    return new Point(X + 1, Y + 1);
                case Direction.Southwest:
                    return new Point(X - 1, Y + 1);
            }

            throw new InvalidOperationException("Bad direction");
        }

        public static int CalculateManhattanDistance(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }
    }
}
