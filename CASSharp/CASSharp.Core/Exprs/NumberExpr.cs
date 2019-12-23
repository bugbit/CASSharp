#region LICENSE
/*
    Algebra Software free CAS
    Copyright © 2018 Óscar Hernández Bañó
    This file is part of Algebra.
    Algebra is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.
    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
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

        //public abstract BigNumberExpr BFloat();
        //public abstract NumberIntegerExpr Integer();

        public static NumberExpr Create(string argNumberStr)
        {
            var n = BigDecimal.Parse(argNumberStr, CultureInfo.InvariantCulture);

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
            if (n.Scale > 0)
                throw new ExprException(string.Format(Properties.Resources.ConvertToIntegerException, n));

            return new IntegerNumberExpr(n, n.ToBigInteger());
        }
    }
}
