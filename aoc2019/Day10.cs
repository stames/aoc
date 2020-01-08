using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;

namespace aoc2019
{
    class AsteroidPair
    {
        public Point A { get; }
        public Point B { get; }

        public int XDistance
        {
            get { return B.X - A.X; }
        }

        public int YDistance
        {
            get { return B.Y - A.Y; }
        }
        
        public double NormalizedAngle
        {
            get
            {
                return Math.PI / 2 + Math.Atan2(YDistance, XDistance);
            }
        }

        public int TotalDistance
        {
            get
            {
                return XDistance * XDistance + YDistance * YDistance;
            }
        }

        public AsteroidPair(Point a, Point b)
        {
            this.A = a;
            this.B = b;
        }
    }

    public class Day10
    {
        const double EPSILON = 0.001;

        private List<Point> _asteroids = new List<Point>();
        private Point _monitoringStation;
        private int _max;

        public Day10()
        {
            var lines = InputUtils.GetDayInputLines(10);

            int y = 0;
            foreach (var line in lines)
            {
                for (int x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#')
                    {
                        _asteroids.Add(new Point(x, y));
                    }
                }
                y++;
            }

            // Check each asteroid pair, calculating the angle
            // between them. If the angles are the same, then one
            // blocks view of the other.
            foreach (Point source in _asteroids)
            {
                HashSet<double> angles = new HashSet<double>();
                foreach (Point target in _asteroids)
                {
                    angles.Add(new AsteroidPair(source, target).NormalizedAngle);
                }
                if (angles.Count > _max)
                {
                    _max = angles.Count;
                    _monitoringStation = source;
                }
            }
        }

        public int Part1()
        { 
            return _max;
        }

        public int Part2()
        {
            List<AsteroidPair> pairs = new List<AsteroidPair>(_max);

            foreach (Point asteroid in _asteroids)
            {
                // from the monitoring station to every other asteroid
                pairs.Add(new AsteroidPair(_monitoringStation, asteroid));
            }

            // order clockwise in a spiral going outward
            var orderedAsteroids =
                pairs.OrderBy(p => p.NormalizedAngle).ThenBy(p => p.TotalDistance).ToList();

            double currentAngle = -1;
            
            HashSet<int> vaporizedIndices = new HashSet<int>();

            // start at the top and move around
            var lastNegative = orderedAsteroids.Last(p => p.NormalizedAngle < 0);
            int startingIndex = orderedAsteroids.LastIndexOf(lastNegative);

            int index = startingIndex;

            while (true)
            {
                index++;

                // if we have already vaporized this one, keep going
                if (vaporizedIndices.Contains(index))
                {
                    continue;
                }

                AsteroidPair currentPair = orderedAsteroids[index % orderedAsteroids.Count];

                // if the one we are looking at isn't visible or
                // something closer was already hit
                if (Math.Abs(currentPair.NormalizedAngle - currentAngle) < EPSILON)
                {
                    continue;
                }

                // vaporize!
                currentAngle = currentPair.NormalizedAngle;
                vaporizedIndices.Add(index);

                if (vaporizedIndices.Count == 200)
                {
                    return 100 * currentPair.B.X + currentPair.B.Y;
                }
            }
        }
    }
}
