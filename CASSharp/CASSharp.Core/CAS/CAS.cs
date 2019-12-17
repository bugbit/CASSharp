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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using ST = CASSharp.Core.Syntax;

namespace CASSharp.Core.CAS
{
    public sealed class CAS
    {
        private readonly Dictionary<string, InstructionInfo> mInstructions;

        private CasVars mVars = new CasVars();
        private ICASPost mPost;

        public CasVars Vars => mVars;

        public CAS(ICASPost argPost)
        {
            mPost = argPost;
            mInstructions = BuildFuncs<InstructionAttribute, InstructionInfo>
            (
                (i, m, a) =>
                {
                    i.Method = (InstructionHandler)Delegate.CreateDelegate(typeof(InstructionHandler), this, m);
                }
            );
        }

        public string GetPromptVar(string argNameVar) => $"({argNameVar})";
        public string GetPromptInVarAct() => GetPromptVar(mVars.NameVarPrompt);

        public EvalPromptResult EvalPrompt(string[] argLines, bool argEvalIfExprsInCompleted, CancellationToken argCancelToken)
        {
            var pRetP = ST.STTokenizer.Parse(argLines, argCancelToken);

            if (argEvalIfExprsInCompleted && ((pRetP.LinesNoParse != null && pRetP.LinesNoParse.Length > 0) || (pRetP.TokensOut.Any(t => t.Terminate == ST.ESTTokenizerTerminate.No))))
                return null;

            var pResults = new List<EvalExprInResult>();
            var pTokens = from t in pRetP.TokensOut where t.Terminate != ST.ESTTokenizerTerminate.No select t;

            foreach (var t in pTokens)
            {
                argCancelToken.ThrowIfCancellationRequested();

                var pRetE = EvalPrompt(t, argCancelToken);

                pResults.Add(pRetE);
            }

            return new EvalPromptResult { EvalResults = pResults.ToArray(), LinesNoParse = pRetP.LinesNoParse };
        }

        public EvalExprInResult EvalPrompt(ST.STTokensTerminate argTokens, CancellationToken argCancelToken)
        {
            var pIn = STToExprsIn(argTokens, argCancelToken);
            var pOut = Eval(pIn, mVars, argCancelToken);
            var pIOE = mVars.AddInOut(pIn, pOut, out string pNameVarIn, out string pNameVarOut);

            if (argTokens.Terminate == ST.ESTTokenizerTerminate.ShowResult)
                mPost.PrintExprOutPost(GetPromptVar(pNameVarOut), pOut);

            return new EvalExprInResult { Terminate = argTokens.Terminate, InExpr = pIn, OutExpr = pOut, NameVarIn = pNameVarIn, NameVarOut = pNameVarOut };
        }

        public Exprs.Expr STToExprsIn(ST.STTokens argTokens, CancellationToken argCancelToken) => STToExprs(argTokens, argCancelToken);

        public Exprs.Expr STToExprs(ST.STTokens argTokens, CancellationToken argCancelToken)
        {
            var pTokens = argTokens.Tokens.First;

            switch (pTokens.Value.Token)
            {
                case ST.ESTToken.Number:
                    return Exprs.Expr.Number(BigDecimal.Parse(((ST.STTokenStr)pTokens.Value).Text));
                case ST.ESTToken.Word:
                    if (pTokens.Next != null && pTokens.Next.Value.Token == ST.ESTToken.Parenthesis)
                    {

                    }

                    return Exprs.Expr.Null;
            }

            return Exprs.Expr.Number(BigDecimal.Parse(argTokens.Tokens.OfType<ST.STTokenStr>().First().Text));
        }

        public Exprs.Expr Eval(Exprs.Expr e, IVars argVars, CancellationToken argCancelToken) => e;

        [Instruction]
        private void Quit(CancellationToken argCancelToken, Exprs.Expr[] argParams) { }

        private Dictionary<string, T> BuildFuncs<A, T>(Action<T, MethodInfo, A> argInit) where A : FunctionBaseAttribute where T : FunctionBaseInfo, new()
        {
            var pDicts = new Dictionary<string, T>(StringComparer.InvariantCultureIgnoreCase);
            var pMethod = GetType().FindMembers
           (
               MemberTypes.Method, BindingFlags.Instance | BindingFlags.NonPublic,
               (m, c) => ((MethodInfo)m).GetCustomAttributes((Type)c, true).Length > 0,
               typeof(A)
           ).OfType<MethodInfo>();

            foreach (var m in pMethod)
            {
                var pAttr = (A)m.GetCustomAttributes(typeof(A), true)[0];
                var pName = pAttr.Name ?? m.Name.ToLowerInvariant();
                var pInfo = new T();

                argInit.Invoke(pInfo, m, pAttr);
                pDicts[pName] = pInfo;
            }

            return pDicts;
        }
    }
}
