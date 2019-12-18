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
