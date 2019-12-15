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

using Core = CASSharp.Core;
using CAS = CASSharp.Core.CAS;

using static System.Console;
using CASSharp.Core.Exprs;

namespace CASSharp.Console.App
{
    public class CASConsoleApp : Core.App.CASApp
    {
        public CASConsoleApp() : base() { }

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

        protected override void PrintExpr(string argNameVarPrompt, Expr e) => WriteLine($"{argNameVarPrompt} {e}");

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
            PrintHeader();
            WriteLine();
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
        }

#if DEBUG

        #region Test

        protected override void PrintTest(string argText) => WriteLine(argText);

        #endregion
#endif
    }
}
