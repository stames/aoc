using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;
using System.Text.RegularExpressions;

namespace aoc2018
{
    public class Claim
    {
        public int ClaimId { get; set; }
        public Point UpperLeft { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class Day3
    {
        List<Claim> claims = new List<Claim>();

        public Day3()
        {
            var lines = InputUtils.GetDayInputLines(2018, 3);

            string pattern = @"#(\d+)\s@\s(\d+),(\d+):\s(\d+)x(\d+)";
            Regex r = new Regex(pattern);

            foreach (var line in lines)
            {
                Claim claim = new Claim();
                var match = r.Match(line);

                claim.ClaimId = int.Parse(match.Groups[1].Value);
                claim.UpperLeft = new Point(int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value));
                claim.Width = int.Parse(match.Groups[4].Value);
                claim.Height = int.Parse(match.Groups[5].Value);

                claims.Add(claim);
            }
        }

        public int Part1()
        {
            Dictionary<Point, int> allPoints = new Dictionary<Point, int>();

            foreach(var claim in claims)
            {
                for (int x = claim.UpperLeft.X; x < claim.UpperLeft.X + claim.Width; x++)
                {
                    for (int y = claim.UpperLeft.Y; y < claim.UpperLeft.Y + claim.Height; y++)
                    {
                        Point p = new Point(x, y);

                        if(!allPoints.ContainsKey(p))
                        {
                            allPoints.Add(p, 1);
                        }
                        else
                        {
                            allPoints[p]++;
                        }
                    }
                }
            }

            return allPoints.Values.Count(p => p >= 2);
        }

        bool ClaimsOverlap(Claim claim1, Claim claim2)
        {
            if(claim1.UpperLeft.X > claim2.UpperLeft.X + claim2.Width ||
                claim2.UpperLeft.X > claim1.UpperLeft.X + claim1.Width)
            {
                // claim1 is to the right of claim2
                // or claim2 is to the right of claim1
                return false;
            }

            if(claim1.UpperLeft.Y > claim2.UpperLeft.Y + claim2.Height ||
                claim2.UpperLeft.Y > claim1.UpperLeft.Y + claim1.Height)
            {
                // claim1 is below claim2
                // or claim2 is below claim1
                return false;
            }

            // otherwise they must overlap
            return true;
        }

        public int Part2()
        {
            foreach(var claim in claims)
            {
                bool found = false;
                foreach(var otherClaim in claims)
                {
                    if (claim.ClaimId == otherClaim.ClaimId)
                    {
                        // same claim
                        continue;
                    }

                    if (ClaimsOverlap(claim, otherClaim))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    // this is the one
                    return claim.ClaimId;
                }
            }

            return 0;
        }
    }
}
