using Deveel.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CASSharp.Core.Math
{
    public static class MathEx
    {
        public static bool Miller(int n, int iteration)
        {
            if ((n < 2) || (n % 2 == 0)) return (n == 2);

            int s = n - 1;
            while (s % 2 == 0) s >>= 1;

            Random r = new Random();
            for (int i = 0; i < iteration; i++)
            {
                int a = r.Next(n - 1) + 1;
                int temp = s;
                long mod = 1;
                for (int j = 0; j < temp; ++j) mod = (mod * a) % n;
                while (temp != n - 1 && mod != 1 && mod != n - 1)
                {
                    mod = (mod * mod) % n;
                    temp *= 2;
                }

                if (mod != n - 1 && temp % 2 == 0) return false;
            }
            return true;
        }

        //public static bool Miller(BigInteger n, int iteration)
        //{
        //    if (n == 2 || n == 3)
        //        return true;
        //    if (n < 2 || n % 2 == 0)
        //        return false;

        //    BigInteger d = n - 1;
        //    int s = 0;

        //    while (d % 2 == 0)
        //    {
        //        d /= 2;
        //        s += 1;
        //    }

        //    // There is no built-in method for generating random BigInteger values.
        //    // Instead, random BigIntegers are constructed from randomly generated
        //    // byte arrays of the same length as the source.
        //    RandomNumberGenerator rng = RandomNumberGenerator.Create();
        //    byte[] bytes = new byte[n.ToByteArray().LongLength];
        //    BigInteger a;

        //    for (int i = 0; i < iteration; i++)
        //    {
        //        do
        //        {
        //            // This may raise an exception in Mono 2.10.8 and earlier.
        //            // http://bugzilla.xamarin.com/show_bug.cgi?id=2761
        //            rng.GetBytes(bytes);
        //            a = new BigInteger(bytes);
        //        }
        //        while (a < 2 || a >= n - 2);

        //        BigInteger x = BigInteger.ModPow(a, d, n);
        //        if (x == 1 || x == n - 1)
        //            continue;

        //        for (int r = 1; r < s; r++)
        //        {
        //            x = BigInteger.ModPow(x, 2, n);
        //            if (x == 1)
        //                return false;
        //            if (x == n - 1)
        //                break;
        //        }

        //        if (x != n - 1)
        //            return false;
        //    }

        //    return true;
        //}
    }
}
