using System;
namespace AdventOfCode
{
    public static class MathUtils
    {
        // Use Euclid's algorithm to calculate the
        // greatest common divisor (GCD) of two numbers.
        public static long Gcd(long a, long b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            // Pull out remainders.
            while(true)
            {
                long remainder = a % b;
                if (remainder == 0)
                {
                    return b;
                }

                a = b;
                b = remainder;
            }
        }

        // Return the least common multiple
        // (LCM) of two numbers.
        public static long Lcm(long a, long b)
        {
            return a * b / Gcd(a, b);
        }
    }
}
