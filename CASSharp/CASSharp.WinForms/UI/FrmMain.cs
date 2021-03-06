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

using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSharpMath.SkiaSharp;
using System.Drawing.Imaging;
using SkiaSharp;

using CAS = CASSharp.Core.CAS;

using Exprs = CASSharp.Core.Exprs;

namespace CASSharp.WinForms.UI
{
    public partial class FrmMain : Form
    {
        private App.CASWinFormsApp mCASApp;

        public FrmMain(App.CASWinFormsApp argCASApp = null)
        {
            mCASApp = argCASApp;
            InitializeComponent();
            board.CASApp = mCASApp;
        }

        public string[] InstructionsNames { get => board.InstructionsNames; set => board.InstructionsNames = value; }

        public string[] FunctionsNames { get => board.FunctionsNames; set => board.FunctionsNames = value; }

        public void PrintHeader(string argText, string argTitle)
        {
            board.SetHeader(argText);
            Text = argTitle;
        }

        public void PrintPrompt(string argNameVarPrompt, bool newline) => board.PrintPrompt(argNameVarPrompt, newline);

        public void PrintExprOut(string argNameVarPrompt, Exprs.Expr e) => board.PrintExprOut(argNameVarPrompt, e);
    }
}
