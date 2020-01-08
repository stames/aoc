using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;
using System.Numerics;

namespace aoc2019
{
    public class Day22
    {
        List<int> deck = new List<int>();

        int deckSize = 10007;
        long bigDeckSize = 119_315_717_514_047;
        long bigRepeat = 101_741_582_076_661;

        public Day22()
        {
        }

        public int Part1()
        {
            for (int i = 0; i < deckSize; i++)
            {
                deck.Add(i);
            }

            var lines = InputUtils.GetDayInputLines(22);

            foreach (string line in lines)
            {
                if (line.StartsWith("deal into new", StringComparison.Ordinal))
                {
                    DealIntoNewStack();
                }
                else if (line.StartsWith("deal with increment", StringComparison.Ordinal))
                {
                    int increment = int.Parse(line.Trim().Split(' ')[3]);
                    DealWithIncrement(increment);
                }
                else
                {
                    // cut
                    int cutValue = int.Parse(line.Trim().Split(' ')[1]);
                    Cut(cutValue);
                }
            }

            for (int i = 0; i < deckSize; i++)
            {
                if (deck[i] == 2019)
                {
                    return i;
                }
            }

            return 0;
        }

        public long Part2()
        {
            string[] lines = File.ReadAllLines("/Users/jjacoby/testing/advent2019/day22.txt");

            var ops = lines.Select(p => Shuffler.Parse(p, bigDeckSize));
            var agg = ops.Aggregate((a, b) => a.Merge(b));

            var pow = agg.Power(bigRepeat);
            var inv = pow.Invert();
            var app = inv.Apply(2020);

            return app;
        }

        public void Go(string[] args)
        {

          

            //bool factoryState = true;
            //// check if it goes back to factory state
            //for (int i = 0; i < deckSize; i++)
            //{
            //    if (deck[i] != i)
            //    {
            //        factoryState = false;
            //        break;
            //    }
            //}

            //if(factoryState)
            //{
            //    Console.WriteLine(iteration);
            //}


            // repeats every 5003rd iteration

        }

        void DealIntoNewStack()
        {
            deck.Reverse();
        }

        void Cut(int value)
        {
            if (value > 0)
            {
                List<int> top = deck.Take(value).ToList();
                List<int> bottom = deck.Skip(value).ToList();

                deck = new List<int>();
                deck.AddRange(bottom);
                deck.AddRange(top);
            }
            else
            {
                List<int> bottom = deck.Skip(deckSize - Math.Abs(value)).ToList();
                List<int> top = deck.Take(deckSize - Math.Abs(value)).ToList();

                deck = new List<int>();
                deck.AddRange(bottom);
                deck.AddRange(top);
            }
        }

        void DealWithIncrement(int increment)
        {
            int[] newDeck = new int[deckSize];

            newDeck[0] = deck[0];
            for (int i = 1; i < deckSize; i++)
            {
                int newIndex = (i * increment) % deckSize;
                newDeck[newIndex] = deck[i];
            }

            deck = new List<int>(newDeck);
        }
    }

    public class Shuffler
    {
        public long Value { get; set; }
        public long Factor { get; set; }
        public long Offset { get; set; }

        public Shuffler(long val, long fac, long offset)
        {
            Value = val;
            Factor = fac;
            Offset = offset;
        }

        public static Shuffler Reverse(long n)
        {
            return new Shuffler(n, -1, -1);
        }

        public static Shuffler Increment(int incrementValue, long n)
        {
            return new Shuffler(n, incrementValue, 0);
        }

        public static Shuffler Cut(int cutValue, long n)
        {
            return new Shuffler(n, 1, -1 * cutValue);
        }

        public Shuffler Power(long exponent)
        {
            Shuffler result = new Shuffler(Value, 1, 0);
            Shuffler doub = this;
            for (int i = 0; i < 64; i++)
            {
                if (0 != (exponent & (1L << i)))
                {
                    result = result.Merge(doub);
                }
                doub = doub.Merge(doub);
            }
            return result;
        }

        public Shuffler Merge(Shuffler other)
        {
            if (Value != other.Value)
            {
                throw new InvalidOperationException();
            }

            BigInteger factor = Factor * (BigInteger)other.Factor % Value;
            BigInteger offset = (Offset * (BigInteger)other.Factor % Value) + other.Offset;

            return new Shuffler(Value, (long)factor, (long)offset);
        }

        public long Multiply(long a, long b, long n)
        {
            BigInteger val = a * (BigInteger)b % n;

            return (long)val;
        }

        public long Apply(long card)
        {
            return Positive(Multiply(card, Factor, Value) + Offset, Value);
        }

        private static long Positive(long x, long n)
        {
            return ((x % n) + n) % n;
        }

        public Shuffler Invert()
        {
            long[] gcds = Gcd(Factor, Value);
            long inverseFactor = gcds[1];

            long inverseOffset = Multiply(Offset, inverseFactor, Value);
            return new Shuffler(Value, inverseFactor, Value - inverseOffset);
        }

        private static long[] Gcd(long p, long q)
        {
            if (q == 0)
            {
                return new long[] { p, 1, 0 };
            }

            long[] vals = Gcd(q, p % q);
            long d = vals[0];
            long a = vals[2];
            long b = vals[1] - (p / q) * vals[2];
            return new long[] { d, a, b };
        }

        public static Shuffler Parse(String line, long n)
        {
            String[] parts = line.Split(' ');
            if (parts[1].Equals("into"))
            {
                return Reverse(n);
            }
            else if (parts[1].Equals("with"))
            {
                return Increment(int.Parse(parts[3]), n);
            }
            else if (parts[0].Equals("cut"))
            {
                return Cut(int.Parse(parts[1]), n);
            }

            throw new InvalidOperationException(line);
        }
    }
}
