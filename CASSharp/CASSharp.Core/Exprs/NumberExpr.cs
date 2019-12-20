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
        sfloat:float
        float: double (default)
        bfloat:BigDecimal (default)
     */
    [DebuggerDisplay("TypeExpr : {TypeExpr} Precision : {Precision} Constant : {Constant}")]
    abstract public class NumberBaseExpr<T> : CteExpr<T>
    {
        public EPrecisionNumber Precision { get; }

        public NumberBaseExpr(ETypeExpr argTypeExpr, EPrecisionNumber argPrecision, T argCte) : base(argTypeExpr, argCte)
        {
            Precision = argPrecision;
        }
        public NumberBaseExpr(NumberBaseExpr<T> e) : this(e.TypeExpr, e.Precision, e.Constant)
        {
        }

        public abstract BigNumberExpr BFloat();
        public abstract NumberIntegerExpr Integer();
    }

    [DebuggerDisplay("TypeExpr : {TypeExpr} Precision : {Precision} Constant : {Constant}")]
    public class NumberExpr : NumberBaseExpr<string>
    {
        public NumberExpr(string n) : base(ETypeExpr.Number, EPrecisionNumber.None, n) { }
        public NumberExpr(NumberExpr e) : base(e) { }

        public override BigNumberExpr BFloat() => new BigNumberExpr(this);
        public override NumberIntegerExpr Integer() => new NumberIntegerExpr(this);
    }

    [DebuggerDisplay("TypeExpr : {TypeExpr} Precision : {Precision} Constant : {Constant}")]
    public class NumberIntegerExpr : NumberBaseExpr<BigInteger>
    {
        public NumberIntegerExpr(BigInteger n) : base(ETypeExpr.Number, EPrecisionNumber.Integer, n) { }
        public NumberIntegerExpr(NumberExpr n) : this(BigInteger.Parse(n.Constant, 10)) { }
        public NumberIntegerExpr(BigNumberExpr n) : this(n) { }
        public NumberIntegerExpr(NumberIntegerExpr e) : this(mn) { }

        public override Expr Clone() => new NumberIntegerExpr(this);

        public override BigNumberExpr BFloat() => new BigNumberExpr(this);
        public override NumberIntegerExpr Integer() => new NumberIntegerExpr(this);
    }

    [DebuggerDisplay("TypeExpr : {TypeExpr} Precision : {Precision} Constant : {Constant}")]
    public class BigNumberExpr : NumberBaseExpr<BigDecimal>
    {
        public BigNumberExpr(NumberExpr n) : this(BigDecimal.Parse(n.Constant, CultureInfo.InvariantCulture)) { }
        public BigNumberExpr(NumberIntegerExpr n) : this(n.Constant) { }
        public BigNumberExpr(BigDecimal n) : base(ETypeExpr.Number, EPrecisionNumber.BFloat, n) { }
        public BigNumberExpr(BigNumberExpr e) : base(e) { }

        public override Expr Clone() => new BigNumberExpr(this);

        public override BigNumberExpr BFloat() => new BigNumberExpr(this);
        public override NumberIntegerExpr Integer() => new NumberIntegerExpr(this);
    }
}
