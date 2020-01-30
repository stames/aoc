using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;
using System.Text;

namespace aoc2018
{
    public class Day13
    {
        private class Track
        {
            public Point TopLeft { get; set; }
            public Point TopRight { get; set; }
            public Point BottomLeft { get; set; }
            public Point BottomRight { get; set; }

            public bool PointOnTopLine(Point p)
            {
                return (p.Y == TopLeft.Y && p.X >= TopLeft.X && p.X <= TopRight.X);
            }

            public bool PointOnBottomLine(Point p)
            {
                return (p.Y == BottomLeft.Y && p.X >= BottomLeft.X && p.X <= BottomRight.X);
            }

            public bool PointOnLeftLine(Point p)
            {
                return (p.X == TopLeft.X && p.Y >= TopLeft.Y && p.Y <= BottomLeft.Y);
            }

            public bool PointOnRightLine(Point p)
            {
                return (p.X == TopRight.X && p.Y >= TopRight.Y && p.Y <= BottomRight.Y);
            }
        }

        private class Cart
        {
            public int CartId { get; set; }
            public Point Location { get; set; }
            public Direction Direction { get; set; }
            public int TurnCount { get; set; }

            public bool IsDead { get; set; }
        }

        public Day13()
        {
        }

        public int Part1()
        {
            var lines = InputUtils.GetDayInputLines(2018, 13);

            // go through each line, finding '/' character for top left.
            // create a track based on that location
            // if we find a cart, add it to carts
            List<Track> tracks = new List<Track>();
            List<Cart> carts = new List<Cart>();
            int cartId = 0;

            for(int line = 0; line < lines.Count; line++)
            {
                for(int i = 0; i < lines[line].Length; i++)
                {
                    // if it's a cart
                    if(lines[line][i] == '<' ||
                        lines[line][i] == '>' ||
                        lines[line][i] == '^' ||
                        lines[line][i] == 'v')
                    {
                        Cart cart = new Cart();
                        cart.Location = new Point(i, line);
                        switch(lines[line][i])
                        {
                            case '>':
                                cart.Direction = Direction.East;
                                break;
                            case '<':
                                cart.Direction = Direction.West;
                                break;
                            case '^':
                                cart.Direction = Direction.North;
                                break;
                            case 'v':
                                cart.Direction = Direction.South;
                                break;
                        }

                        cart.CartId = cartId++;
                        carts.Add(cart);
                    }
                    else if(lines[line][i] == '/' && i != lines[line].Length - 1 && (lines[line][i + 1] == '-' || lines[line][i + 1] == '+'))
                    {
                        // top left of a track
                        Track track = new Track();
                        track.TopLeft = new Point(i, line);

                        bool foundTrack = true;

                        // find the top right by travelling right
                        for(int j = i + 2; j < lines[line].Length; j++)
                        {
                            if(lines[line][j] == ' ' ||
                                lines[line][j] == '|')
                            {
                                // not a line
                                foundTrack = false;
                                break;
                            }

                            if(lines[line][j] == '\\')
                            {
                                track.TopRight = new Point(j, line);
                                break;
                            }
                        }

                        if(track.TopRight == null)
                        {
                            foundTrack = false;
                        }

                        // find the bottom left by travelling down
                        for(int j = line; j < lines.Count; j++)
                        {
                            if(lines[j][i] == '\\')
                            {
                                track.BottomLeft = new Point(i, j);
                                break;
                            }
                        }

                        if (track.BottomLeft == null)
                        {
                            foundTrack = false;
                        }

                        // find bottom right by travelling right from bottom left
                        for (int j = i; j < lines[line].Length; j++)
                        {
                            if (foundTrack)
                            {
                                if (lines[track.BottomLeft.Y][j] == '/')
                                {
                                    track.BottomRight = new Point(j, track.BottomLeft.Y);
                                    break;
                                }
                            }
                            else
                            {
                                foundTrack = false;
                            }
                        }

                        if (track.BottomRight == null)
                        {
                            foundTrack = false;
                        }

                        if (foundTrack)
                        {
                            tracks.Add(track);
                        }
                    }
                }

            }

            int tick = 0;
            bool firstCrash = true;
            while (true)
            {
                foreach (var cart in carts.OrderBy(p => p.Location.Y).ThenBy(p => p.Location.X))
                {
                    if(cart.IsDead)
                    {
                        continue;
                    }

                    // move cart!
                    // move the cart one space
                    if (cart.Direction == Direction.East)
                    {
                        cart.Location.X++;
                    }
                    else if (cart.Direction == Direction.West)
                    {
                        cart.Location.X--;
                    }
                    else if (cart.Direction == Direction.South)
                    {
                        cart.Location.Y++;
                    }
                    else if (cart.Direction == Direction.North)
                    {
                        cart.Location.Y--;
                    }

                    // if the cart is now on a corner, change its direction
                    // to go around the corner
                    var corner = tracks.FirstOrDefault(p => p.TopLeft.Equals(cart.Location) ||
                                        p.TopRight.Equals(cart.Location) ||
                                        p.BottomLeft.Equals(cart.Location) ||
                                        p.BottomRight.Equals(cart.Location));

                    if (corner != null)
                    {
                        if (corner.TopLeft.Equals(cart.Location))
                        {
                            // up -> right
                            // left -> down
                            if (cart.Direction == Direction.North) cart.Direction = Direction.East;
                            if (cart.Direction == Direction.West) cart.Direction = Direction.South;
                        }
                        if (corner.TopRight.Equals(cart.Location))
                        {
                            // up -> left
                            // right -> down
                            if (cart.Direction == Direction.North) cart.Direction = Direction.West;
                            if (cart.Direction == Direction.East) cart.Direction = Direction.South;
                        }
                        if (corner.BottomLeft.Equals(cart.Location))
                        {
                            // down -> right
                            // left -> up
                            if (cart.Direction == Direction.South) cart.Direction = Direction.East;
                            if (cart.Direction == Direction.West) cart.Direction = Direction.North;
                        }
                        if (corner.BottomRight.Equals(cart.Location))
                        {
                            // down -> left
                            // right -> up
                            if (cart.Direction == Direction.South) cart.Direction = Direction.West;
                            if (cart.Direction == Direction.East) cart.Direction = Direction.North;
                        }
                    }

                    // if the cart is at a crossing, turn it based on the tick
                    // crossing == the cart is on two tracks, one vertical and
                    // one horizontal.
                    // each track is defined by its horizontal lines (TopLeft -> TopRight)
                    // and (BottomLeft -> BottomRight) and its vertical lines
                    // (TopLeft --> BottomLeft) and (TopRight --> BottomRight)

                    // Each time a cart has the option to turn (by arriving at 
                    // any intersection), it turns left the first time, goes straight 
                    // the second time, turns right the third time, and then 
                    // repeats those directions starting again with left the 
                    // fourth time, straight the fifth time, and so on. 
                    // This process is independent of the particular intersection at 
                    // which the cart has arrived - that is, the cart has no per-intersection memory.
                    if (tracks.Count(p => p.PointOnTopLine(cart.Location)
                                        || p.PointOnBottomLine(cart.Location)
                                        || p.PointOnLeftLine(cart.Location)
                                        || p.PointOnRightLine(cart.Location)) == 2)
                    {
                        if (cart.TurnCount % 3 == 0)
                        {
                            // turn left
                            if (cart.Direction == Direction.West) cart.Direction = Direction.South;
                            else if (cart.Direction == Direction.East) cart.Direction = Direction.North;
                            else if (cart.Direction == Direction.North) cart.Direction = Direction.West;
                            else if (cart.Direction == Direction.South) cart.Direction = Direction.East;
                        }
                        if (cart.TurnCount % 3 == 2)
                        {
                            // turn right
                            if (cart.Direction == Direction.West) cart.Direction = Direction.North;
                            else if (cart.Direction == Direction.East) cart.Direction = Direction.South;
                            else if (cart.Direction == Direction.North) cart.Direction = Direction.East;
                            else if (cart.Direction == Direction.South) cart.Direction = Direction.West;
                        }
                        cart.TurnCount++;
                    }

                    // check for crashes
                    foreach (var c in carts.Where(p => !p.IsDead).Except(new[] { cart }))
                    {
                        if (cart.Location.Equals(c.Location))
                        {
                            //crash!

                            // part 1 - return first crash
                            if(firstCrash)
                            {
                                Console.WriteLine("First crash: {0}", cart.Location);
                                firstCrash = false;
                            }

                            // part 2 - find last cart standing
                            c.IsDead = true;
                            cart.IsDead = true;
                        }
                    }
                }

                if (carts.Count(p => !p.IsDead) == 1)
                {
                    Console.WriteLine("Final cart location: {0}", carts.First(p => !p.IsDead).Location);
                    return 0;
                }
                tick++;
            }
        }

        public int Part2()
        {
            return 0;
        }
    }
}
