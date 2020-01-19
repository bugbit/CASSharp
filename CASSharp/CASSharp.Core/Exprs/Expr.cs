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
using System.Diagnostics;
using System.Linq;
using System.Text;
using ST = CASSharp.Core.Syntax;

namespace CASSharp.Core.Exprs
{
    [DebuggerDisplay("TypeExpr : {TypeExpr}")]
    abstract public class Expr : ICloneable
    {
        public ETypeExpr TypeExpr { get; }

        protected Expr(ETypeExpr argTypeExpr)
        {
            TypeExpr = argTypeExpr;
        }

        protected Expr(Expr e) : this(e.TypeExpr) { }

        virtual public Expr Clone() => throw new NotImplementedException();

        object ICloneable.Clone() => Clone();

        public static Expr Null => NullExpr.Value;

        public static BooleanExpr Boolean(bool argBool) => (argBool) ? BooleanExpr.True : BooleanExpr.False;
        public static StringExpr String(string s) => new StringExpr(s);

        public static NumberExpr Number(string n) => NumberExpr.Create(n);
        public static NumberExpr Number(BigDecimal n) => new NumberExpr(n);
        public static NumberExpr Number(BigInteger n) => new IntegerNumberExpr(n, n);
        public static ListExpr List(ExprCollection e) => new ListExpr(e);
        public static ListExpr List(IEnumerable<Expr> e) => List(new ExprCollection(e));
        public static FunctionExpr Function(string argName, Expr[] argArgs) => new FunctionExpr(argName, argArgs);
    }
}
