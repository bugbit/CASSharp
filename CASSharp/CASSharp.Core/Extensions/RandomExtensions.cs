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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CASSharp.Core.Extensions
{
    public static class RandomExtensions
    {
        public static ulong Next(this Random r, ulong min, ulong max)
        {
            var hight = r.Next((int)(min >> 32), (int)(max >> 32));
            var minLow = System.Math.Min((int)min, (int)max);
            var maxLow = System.Math.Max((int)min, (int)max);
            var low = (uint)r.Next(minLow, maxLow);
            ulong result = (ulong)hight;
            result <<= 32;
            result |= (ulong)low;
            return result;
        }

        public static long Next(this Random r, long max) => (long)r.Next(0, (ulong)System.Math.Abs(max));

        public static long Next(this Random r, long min, long max) => min + (long)r.Next(0, (ulong)System.Math.Abs(max - min));
    }
}
