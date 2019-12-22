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
        sfloat:float    (Function SFloat)
        float: double (default) (Function Float)
        bfloat:BigDecimal (default) (Function BFloat)
        Function Integer
     */
    [DebuggerDisplay("TypeExpr : {TypeExpr} Precision : {Precision} Constant : {Constant}")]
    public class NumberExpr : CteExpr<BigDecimal>
    {
        public EPrecisionNumber Precision { get; }

        public NumberExpr(EPrecisionNumber argPrecision, BigDecimal argCte) : base(ETypeExpr.Number, argCte)
        {
            Precision = argPrecision;
        }
        public NumberExpr(BigDecimal argCte) : this(EPrecisionNumber.BFloat, argCte) { }
        public NumberExpr(NumberExpr e) : this(e.Precision, e.Constant) { }

        public override Expr Clone() => new NumberExpr(this);

        //public abstract BigNumberExpr BFloat();
        //public abstract NumberIntegerExpr Integer();

        public static NumberExpr Create(string argNumberStr)
        {
            var n = BigDecimal.Parse(argNumberStr, CultureInfo.InvariantCulture);

            return new NumberExpr(n);
        }
    }

    [DebuggerDisplay("TypeExpr : {TypeExpr} Precision : {Precision} Constant : {Constant} PrecisionValue : {PrecisionValue}")]
    abstract public class NumberPrecisionExpr<T> : NumberExpr
    {
        public T PrecisionValue { get; }

        public NumberPrecisionExpr(EPrecisionNumber argPrecision, BigDecimal n, T vp) : base(argPrecision, n)
        {
            PrecisionValue = vp;
        }
        public NumberPrecisionExpr(NumberPrecisionExpr<T> e) : this(e.Precision, e.Constant, e.PrecisionValue) { }

        public override string ToString() => PrecisionValue.ToString();
    }

    [DebuggerDisplay("TypeExpr : {TypeExpr} Precision : {Precision} Constant : {Constant}")]
    public class NumberIntegerExpr : NumberPrecisionExpr<BigInteger>
    {
        public NumberIntegerExpr(BigDecimal n, BigInteger np) : base(EPrecisionNumber.Integer, n, np) { }
        public NumberIntegerExpr(NumberIntegerExpr e) : this(e.Constant, e.PrecisionValue) { }

        public override Expr Clone() => new NumberIntegerExpr(this);
    }
}
