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

#if DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CAS = CASSharp.Core.CAS;
using ST = CASSharp.Core.Syntax;

[assembly: CASSharp.Main.Tests.Test(typeof(CASSharp.Main.Tests.EvalTest))]

namespace CASSharp.Main.Tests
{
    class EvalTest : ITest
    {
        private string[] mTexts =
        {
            "10 20+ 30",
            "10",
            ";",
            "20;",
            "50$100;"
        };

        public void Run()
        {
            var pCAS = new CAS.CAS();
            var pCanceltoken = new CancellationTokenSource();

            try
            {
                foreach (var t in mTexts)
                {
                    var pText = t;

                    Console.WriteLine($"{{{pCAS.Vars.NameVarPromt}}} {t}");
                    try
                    {
                        do
                        {
                            var pResult = pCAS.Eval(pText, pCanceltoken.Token);

                            Console.WriteLine(pResult.Expr);

                            if (pResult.Terminate == ST.ESTTokenizerTerminate.No)
                                break;
                            pText = pResult.PromptNoParse;
                        } while (pText != null);
                    }
                    catch (AggregateException ex)
                    {
                        ex.Handle(e => PrintError(pText, e));
                    }
                    catch (Exception ex)
                    {
                        PrintError(pText, ex);
                    }
                }
            }
            finally
            {
                pCanceltoken.Cancel();
            }
        }

        private bool PrintError(string t, Exception ex)
        {
            var pColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine(ex.Message);

            Console.ForegroundColor = pColor;

            if (ex is ST.STException pSTEx)
            {
                Console.WriteLine(t);
                Console.Write(new string(' ', pSTEx.Position));
                Console.WriteLine('^');
            }

            return true;
        }
    }
}

#endif
