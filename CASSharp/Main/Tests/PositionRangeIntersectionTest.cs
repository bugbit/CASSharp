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

#if DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CASSharp.Comun;

using static System.Console;

//[assembly: CASSharp.Main.Tests.Test(typeof(CASSharp.Main.Tests.PositionRangeIntersectionTest))]

namespace CASSharp.Main.Tests
{
    class PositionRangeIntersectionTest : ITest
    {
        /*
            * 
                   0 -------------- 100 --------------- 2000

                                     A ------------- 200    A (100,200)
                       B ------- 90           B (50,80)  =>     No
                                     A ------------- 200    A (100,200)
                                                           250 B ------- 300           B (250,300)  =>     No


                                    A ------------- 200    A (100,200)
                       B ------------------ 150           B (50,150)  =>     (100,150)

                                    A -------------- 200   A (100,200)
                                        110 B ------ 170   B (110,170) =>    (110,170)

                                    A -------------- 200        A (100,200)
                                        110 B ----------- 300   B (110,300) =>     (110,200)
                                    A -------------- 200        A (100,200)
                            50 B ------------------------ 300   B (50,300)  =>     (100,200)  (<,>)
            */

        public PositionRange[,] mBloques = new[,]
        {
               {  new PositionRange(100,200),new PositionRange(50,80),default(PositionRange) },
               {  new PositionRange(100,200),new PositionRange(250,300),default(PositionRange) },
                {  new PositionRange(100,200),new PositionRange(50,150),new PositionRange(100,150) },
            {  new PositionRange(100,200),new PositionRange(110,170),new PositionRange(110,170) },
            {  new PositionRange(100,200),new PositionRange(110,300),new PositionRange(110,200) },
            {  new PositionRange(100,200),new PositionRange(50,300),new PositionRange(100,200) }
        };

        public void Run()
        {
            for (var i = 0; i < mBloques.GetLength(0); i++)
            {
                var a = mBloques[i, 0];
                var b = mBloques[i, 1];
                var ii = mBloques[i, 2];
                PositionRange r;
                var pIsInter = a.Intersection(b, out r);
                var pOk = r.Equals(ii);

                WriteLine($"a {a} b {b} i {ii} IsInter {pIsInter} r {r} Ok {pOk}");
            }
        }
    }
}

#endif