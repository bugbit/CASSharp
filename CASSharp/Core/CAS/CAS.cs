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
using System.Threading;
using Deveel.Math;
using ST = CASSharp.Core.Syntax;

namespace CASSharp.Core.CAS
{
    class CAS
    {
        private CasVars mVars = new CasVars();
        private ST.STTokenizer mParser = new ST.STTokenizer();

        public CasVars Vars => mVars;

        public ST.STTokenizerResult Parse(string argText, CancellationToken argCancelToken) => mParser.Parse(argText, argCancelToken);

        public EvalStrResult Eval(string argText, CancellationToken argCancelToken)
        {
            var pResultP = Parse(argText, argCancelToken);
            var pResult = new EvalStrResult { Terminate = pResultP.Terminate };

            if (pResultP.Terminate == ST.ESTTokenizerTerminate.No)
                return pResult;

            var e = STToExprs(pResultP.Tokens, argCancelToken);
            var en = Eval(e, mVars, argCancelToken);

            pResult.Expr = en;
            pResult.PromptNoParse = pResultP.PromptNoParse;

            return pResult;
        }

        public Exprs.Expr STToExprs(ST.STTokens argTokens, CancellationToken argCancelToken)
        {
            return Exprs.Expr.Number(BigDecimal.Parse(argTokens.Tokens.OfType<ST.STTokenStr>().First().Text));
        }

        public Exprs.Expr Eval(Exprs.Expr e, IVars argVars, CancellationToken argCancelToken) => e;
    }
}
