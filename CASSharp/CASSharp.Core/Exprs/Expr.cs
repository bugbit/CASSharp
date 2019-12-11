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

        public static TokensExpr Tokens(ST.STTokens argTokens) => new TokensExpr(argTokens);

        public static NumberExpr Number(BigDecimal n) => new NumberExpr(n);
    }
}
