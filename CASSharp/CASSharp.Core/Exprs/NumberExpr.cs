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
using System.Globalization;
using System.Linq;
using System.Text;

namespace CASSharp.Core.Exprs
{
    /*
        var fpprec : precisión (digitos)
        sfloat:float    (Function SFloat)
        float: double (default) (Function Float)
        bfloat:BigDecimal (default) (Function BFloat)
        Function Integer
     */
    [DebuggerDisplay("TypeExpr : {TypeExpr} Precision : {Precision} FPPrec : {FPPrec} Constant : {Constant}")]
    public class NumberExpr : CteExpr<BigDecimal>
    {
        public EPrecisionNumber Precision { get; }
        public int FPPrec { get; }

        public NumberExpr(EPrecisionNumber argPrecision, BigDecimal argCte, int argFPPrec = -1) : base(ETypeExpr.Number, argCte)
        {
            Precision = argPrecision;
            FPPrec = argFPPrec;
        }
        public NumberExpr(BigDecimal argCte) : this(EPrecisionNumber.None, argCte) { }
        public NumberExpr(NumberExpr e) : this(e.Precision, e.Constant) { }

        public override Expr Clone() => new NumberExpr(this);

        public BigNumberExpr BFloat(int argFPPrec = -1) => new BigNumberExpr(Constant, argFPPrec);
        public FloatNumberExpr Float() => new FloatNumberExpr(Constant, Constant.ToDouble());
        public SingleFloatNumberExpr SFloat() => new SingleFloatNumberExpr(Constant, Constant.ToSingle());
        public IntegerNumberExpr Integer() => IntegerNumberExpr.Create(Constant);

        public NumberExpr ConverTo(EPrecisionNumber argPrecision, int argFPPrec = -1)
        {
            if (Precision == argPrecision)
            {
                if (Precision != EPrecisionNumber.BFloat || FPPrec == argFPPrec)
                    return this;
            }

            switch (argPrecision)
            {
                case EPrecisionNumber.BFloat:
                    return BFloat(argFPPrec);
                case EPrecisionNumber.Float:
                    return Float();
                case EPrecisionNumber.SFloat:
                    return SFloat();
                case EPrecisionNumber.Integer:
                    return Integer();
            }

            return this;
        }

        public static NumberExpr Create(string argNumberStr)
        {
            var n = Math.MathEx.Parse(argNumberStr);

            return new NumberExpr(n);
        }
    }

    [DebuggerDisplay("TypeExpr : {TypeExpr} Precision : {Precision} FPPrec : {FPPrec} PrecisionValue : {PrecisionValue} Constant : {Constant}")]
    abstract public class NumberPrecisionExpr<T> : NumberExpr
    {
        public T PrecisionValue { get; }

        public NumberPrecisionExpr(EPrecisionNumber argPrecision, BigDecimal n, T vp, int argFPPrec = -1) : base(argPrecision, n, argFPPrec)
        {
            PrecisionValue = vp;
        }
        public NumberPrecisionExpr(NumberPrecisionExpr<T> e) : this(e.Precision, e.Constant, e.PrecisionValue, e.FPPrec) { }

        public override string ToString() => PrecisionValue.ToString();
    }

    [DebuggerDisplay("TypeExpr : {TypeExpr} Precision : {Precision} FPPrec : {FPPrec} PrecisionValue : {PrecisionValue} Constant : {Constant}")]
    public class BigNumberExpr : NumberPrecisionExpr<BigDecimal>
    {
        public BigNumberExpr(BigDecimal n, int argFPPrec) : base(EPrecisionNumber.BFloat, n, BigMath.Round(n, new MathContext(argFPPrec)), argFPPrec) { }
        public BigNumberExpr(BigNumberExpr e) : this(e.Constant, e.FPPrec) { }

        public override Expr Clone() => new BigNumberExpr(this);
    }

    [DebuggerDisplay("TypeExpr : {TypeExpr} Precision : {Precision} FPPrec : {FPPrec} PrecisionValue : {PrecisionValue} Constant : {Constant}")]
    public class FloatNumberExpr : NumberPrecisionExpr<double>
    {
        public FloatNumberExpr(BigDecimal n, double np) : base(EPrecisionNumber.Float, n, np) { }
        public FloatNumberExpr(FloatNumberExpr e) : this(e.Constant, e.PrecisionValue) { }

        public override Expr Clone() => new FloatNumberExpr(this);
    }

    [DebuggerDisplay("TypeExpr : {TypeExpr} Precision : {Precision} FPPrec : {FPPrec} PrecisionValue : {PrecisionValue} Constant : {Constant}")]
    public class SingleFloatNumberExpr : NumberPrecisionExpr<float>
    {
        public SingleFloatNumberExpr(BigDecimal n, float np) : base(EPrecisionNumber.SFloat, n, np) { }
        public SingleFloatNumberExpr(SingleFloatNumberExpr e) : this(e.Constant, e.PrecisionValue) { }

        public override Expr Clone() => new SingleFloatNumberExpr(this);
    }

    [DebuggerDisplay("TypeExpr : {TypeExpr} Precision : {Precision} FPPrec : {FPPrec} PrecisionValue : {PrecisionValue} Constant : {Constant}")]
    public class IntegerNumberExpr : NumberPrecisionExpr<BigInteger>
    {
        public IntegerNumberExpr(BigDecimal n, BigInteger np) : base(EPrecisionNumber.Integer, n, np) { }
        public IntegerNumberExpr(IntegerNumberExpr e) : this(e.Constant, e.PrecisionValue) { }

        public override Expr Clone() => new IntegerNumberExpr(this);

        public static IntegerNumberExpr Create(BigDecimal n)
        {
            var i = Math.MathEx.Integer(n);

            return new IntegerNumberExpr(n, i);
        }
    }
}
