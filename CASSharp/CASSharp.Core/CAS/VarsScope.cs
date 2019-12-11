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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CASSharp.Core.Exprs;

namespace CASSharp.Core.CAS
{
    abstract public class VarsScope : IVars
    {
        public IVars Vars { get; private set; }
        public IVars Parent { get; private set; }
        public abstract IEnumerable<string> NameVars { get; }

        public abstract void Clear();
        public abstract bool ExistVar(string nvar);
        public abstract Expr Get(string nvar);
        public abstract void Set(string nvar, Expr e);

        public IVars Create(IVars argParent, IVars argVars)
        {
            if (argParent == null && argVars != null)
                return argVars;

            if (argVars == null && argParent != null)
                return argParent;

            var pVars = NewVars();

            pVars.Parent = argParent;
            pVars.Vars = argVars;

            return pVars;
        }

        abstract protected VarsScope NewVars();
    }
}
