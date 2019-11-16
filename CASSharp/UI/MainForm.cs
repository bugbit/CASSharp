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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CASSharp.UI
{
    public partial class MainForm : Form
    {
        private PromptControl mPrompt;

        public MainForm()
        {
            InitializeComponent();
            InitFrmName();
#if DEBUG
            InitMenuTest();
#endif
        }

        private void InitFrmName()
        {
            var pAssembly = Assembly.GetExecutingAssembly();
            var pName = pAssembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;
            var pVersion = pAssembly.GetName().Version.ToString();

            Text = $"{pName} {pVersion}";
        }

#if DEBUG
        private void InitMenuTest()
        {
            var pMenuTests = new System.Windows.Forms.ToolStripMenuItem
            {
                Name = "test1ToolStripMenuItem",
                Alignment = System.Windows.Forms.ToolStripItemAlignment.Right,
                Text = "&Test"
            };
            var pMenuTest1 = new System.Windows.Forms.ToolStripMenuItem
            {
                Name = "test1ToolStripMenuItem",
                Text = "Test1"
            };

            pMenuTest1.Click += new System.EventHandler(test1ToolStripMenuItem_Click);
            pMenuTests.DropDownItems.AddRange(new[] { pMenuTest1 });
            mnuMain.Items.Add(pMenuTests);
        }
#endif

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void test1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mPrompt = new PromptControl();

            mPrompt.SetPrompt
            (
                new[]
                {
                    "/*",
                    "comentario1",
                    "comentario2",
                    "comentario3",
                    "comentario4",
                    "comentario5",
                    "comentario6",
                    "comentario7",
                    "*/"
                }
            );
            mPrompt.SetLaTex(@"\frac{1}{\sqrt{x}}");
            boardControl1.Controls.Add(mPrompt);
        }
    }
}
