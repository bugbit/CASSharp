#region LICENSE
/*
    MIT License

    Copyright (c) 2018 Software free CAS Sharp

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/
#endregion

using Deveel.Math;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace CASSharp.Core.Math
{
    public static class MathEx
    {
        public static BigDecimal Parse(string argNumberStr) => BigDecimal.Parse(argNumberStr, CultureInfo.InvariantCulture);

        public static bool IntegerP(BigDecimal n) => n.Scale <= 0;

        public static BigInteger Integer(BigDecimal n)
        {
            if (!IntegerP(n))
                throw new OverflowException(string.Format(Properties.Resources.ConvertToIntegerException, n));

            return n.ToBigInteger();
        }

        public static IEnumerable<BigInteger> Primes(BigInteger start, BigInteger end, int certainty, CancellationToken argCancelToken)
        {
            var n = (BigInteger.IsProbablePrime(start, certainty, argCancelToken)) ? start : BigInteger.NextProbablePrime(start, argCancelToken);

            while (n <= end)
            {
                argCancelToken.ThrowIfCancellationRequested();
                yield return n;
                n = BigInteger.NextProbablePrime(n, argCancelToken);
            }
        }

        //public static bool Miller(int n, int iteration)
        //{
        //    if ((n < 2) || (n % 2 == 0)) return (n == 2);

        //    int s = n - 1;
        //    while (s % 2 == 0) s >>= 1;

        //    Random r = new Random();
        //    for (int i = 0; i < iteration; i++)
        //    {
        //        int a = r.Next(n - 1) + 1;
        //        int temp = s;
        //        long mod = 1;
        //        for (int j = 0; j < temp; ++j) mod = (mod * a) % n;
        //        while (temp != n - 1 && mod != 1 && mod != n - 1)
        //        {
        //            mod = (mod * mod) % n;
        //            temp *= 2;
        //        }

        //        if (mod != n - 1 && temp % 2 == 0) return false;
        //    }
        //    return true;
        //}

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
