﻿#region LICENSE
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
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CASSharp.Core.Exprs
{
    [DebuggerDisplay("TypeExpr : {TypeExpr} FunctionName : {FunctionName}")]
    public class FunctionExpr : Expr
    {
        public string FunctionName { get; }
        public Expr[] Args { get; set; }

        public FunctionExpr(string argFunctionName, Expr[] argArgs) : base(ETypeExpr.Function)
        {
            FunctionName = argFunctionName;
            Args = argArgs;
        }

        public FunctionExpr(FunctionExpr e) : this(e.FunctionName, e.Args) { }

        public override Expr Clone() => new FunctionExpr(this);

        public override string ToString()
        {
            var pAgrsStr = string.Join(",", Args.Select(e => e.ToString()));
            var pStr = $"{FunctionName}({pAgrsStr})";

            return pStr;
        }
    }
}
