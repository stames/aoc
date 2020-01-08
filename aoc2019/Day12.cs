using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;

namespace aoc2019
{
    class Moon
    {
        public Moon(int x, int y, int z)
        {
            PositionX = x;
            PositionY = y;
            PositionZ = z;
        }

        public int PositionX;
        public int PositionY;
        public int PositionZ;

        public int VelocityX;
        public int VelocityY;
        public int VelocityZ;
    }

    public class Day12
    {
        Moon io;
        Moon europa;
        Moon ganymede;
        Moon callisto;

        public Day12()
        {
            io = new Moon(5, -1, 5);
            europa = new Moon(0, -14, 2);
            ganymede = new Moon(16, 4, 0);
            callisto = new Moon(18, 1, 16);
        }

        public int Part1()
        {
            int time = 1;

            while (time <= 1000)
            {
                // apply gravity
                ApplyGravity(io, europa);
                ApplyGravity(io, ganymede);
                ApplyGravity(io, callisto);
                ApplyGravity(europa, ganymede);
                ApplyGravity(europa, callisto);
                ApplyGravity(ganymede, callisto);

                ApplyVelocity(io);
                ApplyVelocity(europa);
                ApplyVelocity(ganymede);
                ApplyVelocity(callisto);

                time++;
            }

            int totalEnergy = 0;
            totalEnergy += GetEnergy(io);
            totalEnergy += GetEnergy(ganymede);
            totalEnergy += GetEnergy(europa);
            totalEnergy += GetEnergy(callisto);

            return totalEnergy;
        }

        public long Part2()
        {
            io = new Moon(5, -1, 5);
            europa = new Moon(0, -14, 2);
            ganymede = new Moon(16, 4, 0);
            callisto = new Moon(18, 1, 16);

            Moon ioInitial = new Moon(5, -1, 5);
            Moon europaInitial = new Moon(0, -14, 2);
            Moon ganymedeInitial = new Moon(16, 4, 0);
            Moon callistoInitial = new Moon(18, 1, 16);

            int time = 1;
            List<long> xCycles = new List<long>();
            List<long> yCycles = new List<long>();
            List<long> zCycles = new List<long>();

            while (!xCycles.Any() || !yCycles.Any() || !zCycles.Any())
            {
                // apply gravity
                ApplyGravity(io, europa);
                ApplyGravity(io, ganymede);
                ApplyGravity(io, callisto);
                ApplyGravity(europa, ganymede);
                ApplyGravity(europa, callisto);
                ApplyGravity(ganymede, callisto);

                ApplyVelocity(io);
                ApplyVelocity(europa);
                ApplyVelocity(ganymede);
                ApplyVelocity(callisto);

                time++;

                if (ioInitial.PositionX == io.PositionX &&
                    europaInitial.PositionX == europa.PositionX &&
                    ganymedeInitial.PositionX == ganymede.PositionX &&
                    callistoInitial.PositionX == callisto.PositionX &&
                    ioInitial.VelocityX == io.VelocityX &&
                    europaInitial.VelocityX == europa.VelocityX &&
                    ganymedeInitial.VelocityX == ganymede.VelocityX &&
                    callistoInitial.VelocityX == callisto.VelocityX)
                {
                    Console.WriteLine("X cycle at {0}", time);
                    xCycles.Add(time - 1);
                }

                if (ioInitial.PositionY == io.PositionY &&
                    europaInitial.PositionY == europa.PositionY &&
                    ganymedeInitial.PositionY == ganymede.PositionY &&
                    callistoInitial.PositionY == callisto.PositionY &&
                    ioInitial.VelocityY == io.VelocityY &&
                    europaInitial.VelocityY == europa.VelocityY &&
                    ganymedeInitial.VelocityY == ganymede.VelocityY &&
                    callistoInitial.VelocityY == callisto.VelocityY)
                {
                    Console.WriteLine("Y cycle at {0}", time);
                    yCycles.Add(time - 1);
                }

                if (ioInitial.PositionZ == io.PositionZ &&
                    europaInitial.PositionZ == europa.PositionZ &&
                    ganymedeInitial.PositionZ == ganymede.PositionZ &&
                    callistoInitial.PositionZ == callisto.PositionZ &&
                    ioInitial.VelocityZ == io.VelocityZ &&
                    europaInitial.VelocityZ == europa.VelocityZ &&
                    ganymedeInitial.VelocityZ == ganymede.VelocityZ &&
                    callistoInitial.VelocityZ == callisto.VelocityZ)
                {
                    Console.WriteLine("Z cycle at {0}", time);
                    zCycles.Add(time - 1);
                }
            }

            return MathUtils.Lcm(MathUtils.Lcm(xCycles.Min(), yCycles.Min()), zCycles.Min());
        }

        void PrintState(Moon moon)
        {
            Console.WriteLine("pos=<x= {0}, y= {1}, z= {2}>, vel=<x= {3}, y= {4}, z= {5}>",
                moon.PositionX, moon.PositionY, moon.PositionZ,
                moon.VelocityX, moon.VelocityY, moon.VelocityZ);
        }

        int GetEnergy(Moon moon)
        {
            int pot = Math.Abs(moon.PositionX) + Math.Abs(moon.PositionY) + Math.Abs(moon.PositionZ);
            int kin = Math.Abs(moon.VelocityX) + Math.Abs(moon.VelocityY) + Math.Abs(moon.VelocityZ);

            return pot * kin;
        }

        void ApplyVelocity(Moon moon)
        {
            moon.PositionX += moon.VelocityX;
            moon.PositionY += moon.VelocityY;
            moon.PositionZ += moon.VelocityZ;
        }

        void ApplyGravity(Moon moon1, Moon moon2)
        {
            if (moon1.PositionX < moon2.PositionX)
            {
                moon1.VelocityX++;
                moon2.VelocityX--;
            }
            else if (moon1.PositionX > moon2.PositionX)
            {
                moon1.VelocityX--;
                moon2.VelocityX++;
            }

            if (moon1.PositionY < moon2.PositionY)
            {
                moon1.VelocityY++;
                moon2.VelocityY--;
            }
            else if (moon1.PositionY > moon2.PositionY)
            {
                moon1.VelocityY--;
                moon2.VelocityY++;
            }

            if (moon1.PositionZ < moon2.PositionZ)
            {
                moon1.VelocityZ++;
                moon2.VelocityZ--;
            }
            else if (moon1.PositionZ > moon2.PositionZ)
            {
                moon1.VelocityZ--;
                moon2.VelocityZ++;
            }
        }
    }
}
