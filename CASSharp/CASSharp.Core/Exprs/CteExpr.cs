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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CASSharp.Core.Exprs
{
    [DebuggerDisplay("TypeExpr : {TypeExpr} Constant : {Constant}")]
    abstract public class CteExpr<T> : Expr
    {
        public T Constant { get; }

        public CteExpr(ETypeExpr argTypeExpr, T argCte) : base(ETypeExpr.Constant | (argTypeExpr & ETypeExpr.Flags))
        {
            Constant = argCte;
        }

        public CteExpr(CteExpr<T> e) : this(e.TypeExpr, e.Constant)
        {
        }

        public override string ToString() => Constant?.ToString();

    }
}
