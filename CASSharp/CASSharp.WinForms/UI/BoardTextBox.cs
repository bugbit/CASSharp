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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CASSharp.WinForms.UI
{
    public class BoardTextBox : FastColoredTextBox
    {
        TextStyle HyperlinkStyle = new TextStyle(Brushes.Blue, null, FontStyle.Underline);
        TextStyle GreenStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
        TextStyle MagentaStyle = new TextStyle(Brushes.Magenta, null, FontStyle.Regular);
        TextStyle BlueStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);

        private string[] mInstructionsNames;
        private string mInstructionsNamesRegEx;

        public string[] InstructionsNames
        {
            get => mInstructionsNames;
            set
            {
                mInstructionsNames = value;
                mInstructionsNamesRegEx = string.Join("|", mInstructionsNames);
            }
        }

        public BoardTextBox()
        {
            InitializeComponent();
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
            else if (FindVisualMarkerForPoint(e.Location) == null)
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
            ShowLineNumbers = true;
            ShowFoldingLines = true;
            LeftBracket = '(';
            RightBracket = ')';
            LeftBracket2 = '[';
            RightBracket2 = ']';
            Language = Language.Custom;
            AddStyle(HyperlinkStyle);
            AddStyle(GreenStyle);
            TextChanged += BoardTextBox_TextChanged;
        }

        bool CharIsHyperlink(Place place)
        {
            var mask = GetStyleIndexMask(new Style[] { HyperlinkStyle });
            if (place.iChar < GetLineLength(place.iLine))
                if ((this[place].style & mask) != 0)
                    return true;

            return false;
        }

        private void BoardTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.ChangedRange.ClearStyle(HyperlinkStyle, GreenStyle, MagentaStyle, BlueStyle);

            // Hyperlink
            e.ChangedRange.SetStyle(HyperlinkStyle, @"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?");
            //comment highlighting
            e.ChangedRange.SetStyle(GreenStyle, @"//.*$", RegexOptions.Multiline);
            e.ChangedRange.SetStyle(GreenStyle, @"(/\*.*?\*/)|(/\*.*)", RegexOptions.Singleline);
            e.ChangedRange.SetStyle(GreenStyle, @"(/\*.*?\*/)|(.*\*/)", RegexOptions.Singleline | RegexOptions.RightToLeft);
            //number highlighting
            e.ChangedRange.SetStyle(MagentaStyle, @"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\b0x[a-fA-F\d]+\b");

            // instruccions
            e.ChangedRange.SetStyle(BlueStyle, $@"\b({mInstructionsNamesRegEx})\b|#region\b|#endregion\b");

            //clear folding markers
            e.ChangedRange.ClearFoldingMarkers();

            e.ChangedRange.SetFoldingMarkers(@"/\*", @"\*/");//allow to collapse comment block
        }
    }
}
