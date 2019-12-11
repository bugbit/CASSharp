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
using System.Reflection;
using System.Text;

namespace CASSharp.Core.CAS
{
    public class CASApp
    {
        private string[] mArgs;

        protected CAS mCAS = new CAS();

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

        protected virtual void PrintException(Exception ex) { }
        protected void ParseCommandLine(out bool argExit) { argExit = false; }

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
    }
}
