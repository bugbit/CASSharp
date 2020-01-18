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
using System.Threading.Tasks;
using System.Windows.Forms;

using Core = CASSharp.Core;
using CAS = CASSharp.Core.CAS;
using Exprs = CASSharp.Core.Exprs;
using ST = CASSharp.Core.Syntax;
using System.Threading;

namespace CASSharp.WinForms.App
{
    public sealed class CASWinFormsApp : Core.App.CASApp
    {
        private UI.FrmMain mFrm;

        //protected override void PrintExpr(string argNameVarPrompt, Exprs.Expr e)=>mFrm.pri

        public override void PrintExprOut(string argNameVarPrompt, Exprs.Expr e) => mFrm.PrintExprOut(argNameVarPrompt, e);

        public Task EvalPrompt(string[] argText)
        {
            var pToken = mTokenCancel;

            if (pToken != null && !pToken.IsCancellationRequested)
                pToken.Cancel();

            mTokenCancel = pToken = new CancellationTokenSource();

            var pTask = Task.Run(() => mCAS.EvalPrompt(argText, true, pToken.Token)).ContinueWith
            (
                t =>
                {
                    if (t.Exception != null)
                    {
                        MessageBox.Show(mFrm, t.Exception.Message, mFrm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            );

            return pTask;
        }

        protected override CAS.ICASPost NewPos() => new CASWinFormsAppPost(this);

        protected override void BeforeRun()
        {
            var pNameVar = mCAS.Vars.NameVarPrompt;

            base.BeforeRun();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mFrm = new UI.FrmMain(this)
            {
                InstructionsNames = mCAS.InstructionsNames,
                FunctionsNames = mCAS.Suggestions
            };
            GetHeader(out string argText, out string argTitle);
            mFrm.PrintHeader(argText, argTitle);
            mFrm.Load += (s, e) => mFrm.PrintPrompt(NamePromptVar, false);
        }

        protected override void RunInternal()
        {
            base.RunInternal();
            Application.Run(mFrm);
        }
    }
}
