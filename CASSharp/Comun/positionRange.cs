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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CASSharp.Comun
{
    internal struct PositionRange : IComparable<PositionRange>, IEquatable<PositionRange>, ICloneable
    {
        public int PosIni { get; set; }
        public int PosFin { get; set; }

        public PositionRange(int argIni, int argFin)
        {
            PosIni = argIni;
            PosFin = argFin;
        }

        public int CompareTo(PositionRange obj)
        {
            var pCmp = PosIni - obj.PosIni;

            if (pCmp == 0)
                pCmp = PosFin - obj.PosFin;

            return pCmp;
        }

        public bool Equals(PositionRange other) => PosIni == other.PosIni && PosFin == other.PosFin;

        public override bool Equals(object obj) => (obj is PositionRange pPosR) && Equals(pPosR);

        public override int GetHashCode() => PosIni.GetHashCode() ^ PosFin.GetHashCode();
        public override string ToString() => $"[{PosIni} - {PosFin}]";

        public PositionRange Clone() => new PositionRange(PosIni, PosFin);

        public bool Intersection(PositionRange argPosR, out PositionRange argPosRI)
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
                             50 B ------------------------ 300   B (50,300)  =>     (100,200)  (>,<)
             */

            var pOk = (argPosR.PosFin >= PosIni && argPosR.PosIni <= PosFin);

            argPosRI = (pOk) ? new PositionRange(Math.Max(PosIni, argPosR.PosIni), Math.Min(PosFin, argPosR.PosFin)) : default(PositionRange);

            return pOk;
        }

        object ICloneable.Clone() => Clone();
    }
}
