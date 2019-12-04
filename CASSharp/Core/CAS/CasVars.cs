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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CASSharp.Core.CAS
{
    class CasVars : Vars
    {
        private List<InOutExpr> mInOutExprs = new List<InOutExpr>();

        override public IEnumerable<string> NameVars
        {
            get
            {
                var pNVars = new List<string>();
                var n = 1;

                foreach (var ine in mInOutExprs)
                {
                    if (ine.In != null)
                        pNVars.Add(InNVar(n));
                    if (ine.Out != null)
                        pNVars.Add(OutNVar(n));
                }

                pNVars.AddRange(mVars.Keys);

                return pNVars.ToArray();
            }
        }

        public string NameVarPromt
        {
            get
            {
                lock (mInOutExprs)
                {
                    return InNVar(mInOutExprs.Count + 1);
                }
            }
        }

        public static string InNVar(int n) => $"i{n}";
        public static string OutNVar(int n) => $"i{n}";
    }
}
