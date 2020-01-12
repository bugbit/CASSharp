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

using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CASSharp.WinForms.UI
{
    public class BoardTextBox : FastColoredTextBox
    {
        TextStyle BlueStyle = new TextStyle(Brushes.Blue, null, FontStyle.Underline);

        public BoardTextBox()
        {
            InitializeComponent();
            Language = Language.Custom;
            AddStyle(BlueStyle);
            TextChanged += BoardTextBox_TextChanged;
        }

        public void SetHeader(string argText)
        {
            Text += $"/*\n{argText}\n\n*/\n\n";
        }

        public void PrintPrompt(string argNameVarPrompt, bool newline)
        {
            if (newline)
                Text += argNameVarPrompt + '\n';
            else
            {
                Text += argNameVarPrompt + ' ';
            }
        }

        public override void OnTextChanged()
        {
            base.OnTextChanged();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            var p = PointToPlace(e.Location);
            if (CharIsHyperlink(p))
                Cursor = Cursors.Hand;
            else
                Cursor = Cursors.IBeam;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            var p = PointToPlace(e.Location);
            if (CharIsHyperlink(p))
            {
                var url = GetRange(p, p).GetFragment(@"[\S]").Text;

                Process.Start(url);
            }
        }

        private void InitializeComponent()
        {
            ShowLineNumbers = false;
            Language = Language.CSharp;
        }

        bool CharIsHyperlink(Place place)
        {
            var mask = GetStyleIndexMask(new Style[] { BlueStyle });
            if (place.iChar < GetLineLength(place.iLine))
                if ((this[place].style & mask) != 0)
                    return true;

            return false;
        }

        private void BoardTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.ChangedRange.ClearStyle(BlueStyle);
            e.ChangedRange.SetStyle(BlueStyle, @"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?");
        }
    }
}
