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
        private readonly Dictionary<string, FunctionInfo> mFunctions;

        private CasVars mVars = new CasVars();
        private ICASPost mPost;

        public CasVars Vars => mVars;

        // var fpprec
        public int FPPrec { get; set; } = 16;
        // var primep_number_of_tests
        public int PrimePNumberOfTest { get; set; } = 25;

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
            mFunctions = BuildFuncs<FunctionAttribute, FunctionInfo>
            (
                (i, m, a) =>
                {
                    i.Method = (FunctionHandler)Delegate.CreateDelegate(typeof(FunctionHandler), this, m);
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
            var pReader = new ST.STTokensReader(argTokens, argCancelToken);
            var pIn = STToExprIn(pReader);
            var pContext = new EvalContext { CancelToken = argCancelToken };
            var pOut = EvalIn(pContext, pIn);
            var pIOE = mVars.AddInOut(pIn, pOut, out string pNameVarIn, out string pNameVarOut);

            if (argTokens.Terminate == ST.ESTTokenizerTerminate.ShowResult)
                mPost.PrintExprOutPost(GetPromptVar(pNameVarOut), pOut);

            return new EvalExprInResult { Terminate = argTokens.Terminate, InExpr = pIn, OutExpr = pOut, NameVarIn = pNameVarIn, NameVarOut = pNameVarOut };
        }

        public Exprs.Expr STToExprIn(ST.STTokensReader argReader) => STToExpr(argReader);

        public Exprs.Expr STToExpr(ST.STTokensReader argReader)
        {
            argReader.CancelToken.ThrowIfCancellationRequested();

            var pToken = argReader.Token;
            var pTypeToken = pToken?.Token;

            if (!pTypeToken.HasValue)
                return Exprs.Expr.Null;

            switch (pTypeToken.Value)
            {
                case ST.ESTToken.Number:
                    return Exprs.Expr.Number(argReader.TokenStr.Text);
                case ST.ESTToken.Word:
                    var pWord = argReader.TokenStr;

                    if (argReader.NextIfNodeNextValue(t => t.Token == ST.ESTToken.Parenthesis))
                        return STToFunctionExpr(pWord, argReader);

                    break;
            }

            throw new ST.STException(string.Format(Properties.Resources.NoExpectTokenException, pToken.ToString()), pToken.Line, pToken.Position);
        }

        public Exprs.FunctionExpr STToFunctionExpr(ST.STTokenStr argWord, ST.STTokensReader argReader)
        {
            argReader.CancelToken.ThrowIfCancellationRequested();

            var pArgs = STBlocktoExprs(argReader);

            return Exprs.Expr.Function(argWord.Text, pArgs.ToArray());
        }

        public Exprs.ExprCollection STBlocktoExprs(ST.STTokensReader argReader)
        {
            var pExprs = new Exprs.ExprCollection();

            if (argReader.Token is ST.STTokenBlock pBlock)
            {
                foreach (var pTokens in pBlock.Tokens)
                {
                    argReader.CancelToken.ThrowIfCancellationRequested();

                    var pReader = new ST.STTokensReader(pTokens, argReader.CancelToken);
                    var pExpr = STToExpr(pReader);

                    pExprs.Add(pExpr);
                }
            }

            return pExprs;
        }

        public Exprs.Expr EvalIn(EvalContext argContext, Exprs.Expr e)
        {
            var pExpr = (e.TypeExpr == Exprs.ETypeExpr.Function && e is Exprs.FunctionExpr pFn) ? Eval(argContext, pFn, false) : Eval(argContext, e);

            return pExpr;
        }

        public Exprs.Expr Eval(EvalContext argContext, Exprs.Expr e)
        {
            switch (e.TypeExpr)
            {
                case Exprs.ETypeExpr.Number:
                    if (e is Exprs.NumberExpr ne)
                        return ne.ConverTo(argContext.Precision, FPPrec);

                    break;
                case Exprs.ETypeExpr.Function:
                    if (e is Exprs.FunctionExpr pFn)
                        return Eval(argContext, pFn);

                    break;
            }

            return e;
        }

        public Exprs.Expr Eval(EvalContext argContext, Exprs.FunctionExpr e, bool argNoExecInstr = true)
        {
            var pName = e.FunctionName;

            if (mInstructions.TryGetValue(pName, out InstructionInfo pInstr))
            {
                if (argNoExecInstr)
                    throw new EvalException(string.Format(Properties.Resources.NoExecInsTrNoStartExprException, pName));

                try
                {
                    pInstr.Method.Invoke(argContext, e.Args);
                }
                catch (Exception ex)
                {
                    throw new EvalFunctionException(string.Format(Properties.Resources.EvalFunctionException, pInstr.Name, ex.Message), ex, pInstr);
                }

                return Exprs.Expr.Null;
            }
            if (mFunctions.TryGetValue(pName, out FunctionInfo pFN))
            {
                try
                {
                    return pFN.Method.Invoke(argContext, e.Args);
                }
                catch (Exception ex)
                {
                    throw new EvalFunctionException(string.Format(Properties.Resources.EvalFunctionException, pFN.Name, ex.Message), ex, pFN);
                }
            }

            return null;
        }

        public Exprs.Expr Approx(EvalContext argContext, Exprs.EPrecisionNumber argPrecision, Exprs.Expr e)
        {
            var c = new EvalContext(argContext, argPrecision);
            var e1 = Eval(c, e);

            // falta aplicar operaciones y funciones matemáticas

            var e2 = e1;

            return (e2.TypeExpr == Exprs.ETypeExpr.Number && e2 is Exprs.NumberExpr ne) ? ne.ConverTo(argPrecision, FPPrec) : e2;
        }

        public bool Integer(EvalContext argContext, Exprs.Expr e, out Exprs.Expr argRet, out BigInteger argInteger)
        {
            var pRet = Approx(argContext, Exprs.EPrecisionNumber.Integer, e);

            argRet = pRet;
            if (pRet.TypeExpr == Exprs.ETypeExpr.Number && pRet is Exprs.IntegerNumberExpr en)
            {
                argInteger = en.PrecisionValue;

                return true;
            }

            argInteger = null;

            return false;
        }

        public BigInteger Integer(EvalContext argContext, Exprs.Expr e)
        {
            if (!Integer(argContext, e, out Exprs.Expr argRet, out BigInteger argInteger))
                throw new EvalException(string.Format(Properties.Resources.NoExprIntegerException, e));

            return argInteger;
        }

        public Exprs.BooleanExpr PrimeP(EvalContext argContext, Exprs.Expr n)
        {
            var pInt = Integer(argContext, n);

            var pRet = BigInteger.IsProbablePrime(pInt, PrimePNumberOfTest, argContext.CancelToken);

            return Exprs.Expr.Boolean(pRet);
        }

        public Exprs.NumberExpr NextPrime(EvalContext argContext, Exprs.Expr n)
        {
            var pInt = Integer(argContext, n);

            var pRet = BigInteger.NextProbablePrime(pInt, argContext.CancelToken);

            return Exprs.Expr.Number(pRet);
        }

        public Exprs.ListExpr Primes(EvalContext argContext, Exprs.Expr start, Exprs.Expr end)
        {
            var pStartInt = Integer(argContext, start);
            var pEndInt = Integer(argContext, end);
            var pPrimes = from n in Math.MathEx.Primes(pStartInt, pEndInt, PrimePNumberOfTest, argContext.CancelToken) select Exprs.Expr.Number(n);

            return Exprs.Expr.List(pPrimes);
        }

        private void VerifNumArgs(int argNumArgs, Exprs.Expr[] argParams)
        {
            if (argNumArgs != argParams.Length)
                throw new EvalException(string.Format(Properties.Resources.NoEqualFnArgsException, argNumArgs));
        }

        private void VerifMinMaxArgs(int argMinArgs, int argMaxArgs, Exprs.Expr[] argParams)
        {
            var pNumArgs = argParams.Length;

            if (pNumArgs < argMinArgs)
                throw new EvalException(string.Format(Properties.Resources.NoMinFnArgsException, argMinArgs));

            if (pNumArgs > argMaxArgs)
                throw new EvalException(string.Format(Properties.Resources.NoMaxFnArgsException, argMaxArgs));
        }

        [Instruction]
        private void Quit(EvalContext argContext, Exprs.Expr[] argParams)
        {
            VerifNumArgs(0, argParams);

            mPost.QuitPost();
        }

        [Function]
        private Exprs.Expr PrimeP(EvalContext argContext, Exprs.Expr[] argParams)
        {
            VerifNumArgs(1, argParams);

            var n = argParams[0];
            var pRet = PrimeP(argContext, n);

            return pRet;
        }

        [Function(Name = "next_prime")]
        private Exprs.Expr NextPrime(EvalContext argContext, Exprs.Expr[] argParams)
        {
            VerifNumArgs(1, argParams);

            var n = argParams[0];
            var pRet = NextPrime(argContext, n);

            return pRet;
        }

        [Function(Name = "primes")]
        private Exprs.Expr Primes(EvalContext argContext, Exprs.Expr[] argParams)
        {
            VerifNumArgs(2, argParams);

            var start = argParams[0];
            var end = argParams[1];
            var pRet = Primes(argContext, start, end);

            return pRet;
        }

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

                pInfo.Name = pName;
                argInit.Invoke(pInfo, m, pAttr);
                pDicts[pName] = pInfo;
            }

            return pDicts;
        }
    }
}
