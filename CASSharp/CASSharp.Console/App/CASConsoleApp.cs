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

using Core = CASSharp.Core;
using CAS = CASSharp.Core.CAS;
using Exprs = CASSharp.Core.Exprs;
using ST = CASSharp.Core.Syntax;

using static System.Console;

namespace CASSharp.Console.App
{
    public class CASConsoleApp : Core.App.CASApp, IAutoCompleteHandler
    {
        private List<string> mPrompt = new List<string>();
        private CancellationTokenSource mTokenCancel = null;

        public char[] Separators { get; set; } = new char[] { ' ', ',', ';', '(', '[' };

        public CASConsoleApp() : base() { }

        public string[] GetSuggestions(string argTxt, int argIdx)
        {
            var i0 = argTxt.IndexOfAny(Separators);
            var i = argTxt.IndexOfAny(Separators, argIdx);
            var pText = (i == -1) ? argTxt.Substring(argIdx) : argTxt.Substring(argIdx, i - argIdx);
            var pSuggestions = (i0 < 0 || argIdx < i0) ? mCAS.InstrSuggestions : mCAS.Suggestions;
            var i2 = Array.BinarySearch(pSuggestions, pText);
            var i3 = (i2 < 0) ? -i2 - 1 : i2;
            var pRet = new List<string>();

            foreach (var s in pSuggestions.Skip(i3))
            {
                if (!s.StartsWith(pText))
                    break;
                pRet.Add(s);
            }

            return pRet.ToArray();
        }

        public override void PrintPrompt(string argNameVarPrompt, bool newline)
        {
            if (newline)
                WriteLine(argNameVarPrompt);
            else
            {
                Write(argNameVarPrompt);
                Write(' ');
            }
        }

        public override void PrintPrompt(string argNameVarPrompt, string argExpr)
        {
            PrintPrompt(argNameVarPrompt, false);
            WriteLine(argExpr);
        }

        protected override CAS.ICASPost NewPos() => new Core.App.CASAppPost<CASConsoleApp>(this);

        protected override void PrintExpr(string argNameVarPrompt, Exprs.Expr e) => WriteLine($"{argNameVarPrompt} {e}");

        protected override void PrintError(string argError)
        {
            var pForeColor = ForegroundColor;

            try
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine(argError);
            }
            finally
            {
                ForegroundColor = pForeColor;
            }
        }

        protected override void PrintException(Exception ex) => PrintError($"Error: {ex.Message}");

        protected override void BeforeRun()
        {
            base.BeforeRun();
            System.ReadLine.AutoCompletionHandler = this;
            System.ReadLine.HistoryEnabled = true;
            CancelKeyPress += Console_CancelKeyPress;
            PrintHeader();
            WriteLine();
        }

        private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            mTokenCancel?.Cancel();
            e.Cancel = true;
        }

        protected override void RunInternal()
        {
            base.RunInternal();
            for (; ; )
                Prompt();
        }

        private void PrintHeader()
        {
            GetHeader(out string pText, out string pTitle);
            Title = pTitle;
            WriteLine(pText);
        }

        private void Prompt()
        {
            var pNameVar = mCAS.Vars.NameVarPrompt;
            var pText = System.ReadLine.Read($"{mCAS.GetPromptVar(pNameVar)} ");

            //System.ReadLine.AddHistory(pText);
            mPrompt.Add(pText);
            mTokenCancel = new CancellationTokenSource();
            try
            {
                var pRet = mCAS.EvalPrompt(mPrompt.ToArray(), false, mTokenCancel.Token);

                if (pRet == null)
                    return;

                mPrompt = new List<string>(pRet.LinesNoParse);
            }
            catch (ST.STException ex)
            {
                PrintError(ex.Message);
                PrintError(mPrompt[ex.Line]);
                PrintError($"{new string(' ', ex.Position)}^");

                var pTexts2 = mPrompt.Skip(ex.Line + 1);

                mPrompt = new List<string>(pTexts2);
            }
            catch (Exception ex)
            {
                PrintException(ex);
                mPrompt.Clear();
            }
            finally
            {
                mTokenCancel?.Dispose();
                mTokenCancel = null;
            }
        }

#if DEBUG

        #region Test

        protected override void PrintTest(string argText) => WriteLine(argText);

        #endregion
#endif
    }
}
