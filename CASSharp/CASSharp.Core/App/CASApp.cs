﻿#region LICENSE
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
using System.Reflection;
using System.Text;
using System.Threading;
using CAS = CASSharp.Core.CAS;
using ST = CASSharp.Core.Syntax;

namespace CASSharp.Core.App
{
    public class CASApp
    {
        private string[] mArgs;

        protected CAS.CAS mCAS = new CAS.CAS();

        public int Run(string[] args)
        {
            mArgs = args;
            try
            {
                ParseCommandLine(out bool pExit);
                if (pExit)
                    return 0;

                BeforeRun();
            }
            catch (Exception ex)
            {
                PrintException(ex);

                return -1;
            }

            RunInternal();

            try
            {
                AfterRun();
            }
            catch (Exception ex)
            {
                PrintException(ex);

                return -1;
            }

            return 0;
        }

        protected virtual void PrintError(string argError) { }
        protected virtual void PrintException(Exception ex) => PrintError(ex.Message);
        protected void ParseCommandLine(out bool argExit)
        {
            argExit = false;
#if DEBUG

            if (mArgs != null && mArgs.Length > 0 && mArgs[0].Equals("--t", StringComparison.InvariantCultureIgnoreCase))
            {
                argExit = true;

                Test(ref argExit);
            }
#endif
        }

        protected virtual void BeforeRun() { }
        protected virtual void RunInternal() { }
        protected virtual void AfterRun() { }

        protected void GetHeader(out string argText, out string argTitle)
        {
            var pAssembly = Assembly.GetEntryAssembly();
            var pAttrs = pAssembly.GetCustomAttributes(false);
            var pName = pAttrs.OfType<AssemblyTitleAttribute>().First().Title;
            var pVersion = pAssembly.GetName().Version.ToString();
            var pDescription = pAttrs.OfType<AssemblyDescriptionAttribute>().First().Description;
            var pLicense = pAttrs.OfType<AssemblyCopyrightAttribute>().First().Copyright;

            argTitle = $"{pName} {pVersion}";
            argText =
$@"
{pName} Version {pVersion}
{pDescription}
https://github.com/bugbit/cassharp

{pLicense}
MIT LICENSE"
;
        }

#if DEBUG

        #region Test

        private void Test(ref bool argExit)
        {
            var pType = this.GetType();
            var pMethods = new List<MethodInfo>();

            do
            {
                var pRet = pType.FindMembers(MemberTypes.Method, BindingFlags.Instance | BindingFlags.NonPublic, (m, c) => ((MethodInfo)m).GetCustomAttributes((Type)c, true).Length != 0, typeof(TestAttribute)).OfType<MethodInfo>();

                pMethods.AddRange(pRet);
                pType = pType.BaseType;
            } while (pType != null);

            foreach (var pMethod in pMethods)
            {
                var pArgs = pMethod.GetParameters();

                switch (pArgs.Length)
                {
                    case 0:
                        pMethod.Invoke(this, null);
                        break;
                    case 1:
                        var pParamType = pArgs[0].ParameterType;

                        if (!pParamType.IsByRef)
                            return;

                        var pParams = new object[] { argExit };

                        pMethod.Invoke(this, pParams);
                        argExit = (bool)pParams[0];

                        break;
                }
            }
        }

        protected virtual void PrintTest(string argText) { }

        [Test]
        private void TokernizerTest()
        {
            var pTexts = new[]
            {
                "10 20+ 30",
                "10",
                ";",
                "20;",
                "50$100;"
            };

            while (pTexts != null && pTexts.Length > 0)
            {
                try
                {
                    var pRet = ST.STTokenizer.Parse(pTexts, CancellationToken.None);
                    var pTokensOut = pRet.TokensOut;

                    if (pTokensOut != null)
                    {
                        foreach (var pTokens in pTokensOut)
                        {
                            PrintTest($"{pTokens.Terminate} {pTokens.Tokens}");
                        }
                    }

                    pTexts = pRet.LinesNoParse;
                }
                catch (ST.STException ex)
                {
                    PrintError(ex.Message);
                    PrintError(pTexts[ex.Linea]);
                    PrintError($"{new string(' ', ex.Position)}^");

                    var pTexts2 = pTexts.Skip(ex.Linea + 1);

                    pTexts = pTexts2.ToArray();
                }
                catch (Exception ex)
                {
                    PrintException(ex);
                }
            }
        }

        //[Test]
        //private void Test2(ref bool argExit)
        //{
        //    argExit = false;
        //}

        #endregion
#endif
    }
}