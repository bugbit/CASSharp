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

using Exprs = CASSharp.Core.Exprs;

namespace CASSharp.WinForms.UI
{
    public class BoardTextBox : FastColoredTextBox
    {
        private Core.App.ProgressAppPost mPost;

        private TextStyle HyperlinkStyle = new TextStyle(Brushes.Blue, null, FontStyle.Underline);
        private TextStyle NameVarStyle = new TextStyle(Brushes.Gray, null, FontStyle.Regular);
        private LaTexStyle mLaTextStyle = new LaTexStyle();
        private TextStyle GreenStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
        private TextStyle MagentaStyle = new TextStyle(Brushes.Magenta, null, FontStyle.Regular);
        private TextStyle BlueStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);
        private TextStyle WordsStyle = new TextStyle(Brushes.Maroon, null, FontStyle.Bold);

        private string[] mInstructionsNames;
        private string mInstructionsNamesRegEx;
        private volatile bool mIsPromptMode;
        private volatile bool mIsUpdating;
        private Place mStartReadPlace;
        private AutocompleteMenu mPopupMenu;

        public App.CASWinFormsApp CASApp { get; set; }

        public string[] InstructionsNames
        {
            get => mInstructionsNames;
            set
            {
                mInstructionsNames = value;
                mInstructionsNamesRegEx = (mInstructionsNames != null) ? string.Join("|", mInstructionsNames) : string.Empty;
            }
        }

        public string[] FunctionsNames { get; set; }

        public string[] Prompt => new Range(this, mStartReadPlace, Range.End).Text.TrimEnd('\r').Split('\n');

        public Place InstruccionEnd => new Range(this, mStartReadPlace, Range.End).GetFragment(@"\w+").End;

        public BoardTextBox()
        {
            mPost = new Core.App.ProgressAppPost();
            InitializeComponent();
        }

        public void SetHeader(string argText)
        {
            Write($"/*\n{argText}\n\n*/\n\n");
        }

        public void PrintPrompt(string argNameVarPrompt, bool newline)
        {
            if (newline)
                Write(argNameVarPrompt, newline);
            else
                Write(argNameVarPrompt + ' ');
        }

        public void PrintExprOut(string argNameVarPrompt, Exprs.Expr e)
        {
            Write($"{argNameVarPrompt} {e}", true);
        }

        public override void OnTextChanging(ref string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            if (mIsPromptMode)
            {
                if (Selection.Start < mStartReadPlace || Selection.End < mStartReadPlace)
                    GoEnd();//move caret to entering position

                if (Selection.Start == mStartReadPlace || Selection.End == mStartReadPlace)
                    if (text == "\b") //backspace
                    {
                        text = ""; //cancel deleting of last char of readonly text
                        return;
                    }
            }

            base.OnTextChanged();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Enter)
            {
                var pPrompt = Prompt;

                if (pPrompt != null)
                {
                    var pLastLine = pPrompt.LastOrDefault();

                    if (pLastLine != null && pLastLine.LastOrDefault() == ';')
                    {
                        mIsPromptMode = false;

                        mPost.PostContinue(CASApp.EvalPrompt(pPrompt), t =>
                        {
                            if (t.IsCompleted || t.IsCanceled)
                                PrintPrompt($"\n{CASApp.NamePromptVar}", false);
                            mIsPromptMode = true;
                        });

                        e.Handled = true;
                    }
                }
            }
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
            AddStyle(NameVarStyle);
            AddStyle(mLaTextStyle);
            AddStyle(GreenStyle);
            AddStyle(BlueStyle);
            TextChanged += BoardTextBox_TextChanged;

            //create autocomplete popup menu
            mPopupMenu = new AutocompleteMenu(this);
            mPopupMenu.ForeColor = Color.White;
            mPopupMenu.BackColor = Color.Gray;
            mPopupMenu.SelectedColor = Color.Purple;
            mPopupMenu.SearchPattern = @"[\w\.]";
            mPopupMenu.AllowTabKey = true;
            mPopupMenu.AlwaysShowTooltip = true;
            //assign DynamicCollection as items source
            mPopupMenu.Items.SetAutocompleteItems(new AutocompleteItems(mPopupMenu, this));
        }

        private bool CharIsHyperlink(Place place)
        {
            var mask = GetStyleIndexMask(new Style[] { HyperlinkStyle });
            if (place.iChar < GetLineLength(place.iLine))
                if ((this[place].style & mask) != 0)
                    return true;

            return false;
        }

        private void BoardTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.ChangedRange.ClearStyle(HyperlinkStyle, NameVarStyle, GreenStyle, MagentaStyle, BlueStyle, WordsStyle);

            // Hyperlink
            e.ChangedRange.SetStyle(HyperlinkStyle, @"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?");
            // NameVar
            e.ChangedRange.SetStyle(NameVarStyle, @"\(%\w+\)?");
            // latex
            e.ChangedRange.SetStyle(mLaTextStyle, @"#begintex (\w+).*?#endtex", RegexOptions.IgnoreCase);
            //comment highlighting
            e.ChangedRange.SetStyle(GreenStyle, @"//.*$", RegexOptions.Multiline);
            e.ChangedRange.SetStyle(GreenStyle, @"(/\*.*?\*/)|(/\*.*)", RegexOptions.Singleline);
            e.ChangedRange.SetStyle(GreenStyle, @"(/\*.*?\*/)|(.*\*/)", RegexOptions.Singleline | RegexOptions.RightToLeft);
            //number highlighting
            e.ChangedRange.SetStyle(MagentaStyle, @"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\b0x[a-fA-F\d]+\b");
            // words
            e.ChangedRange.SetStyle(WordsStyle, @"\b\w+\b");

            // instruccions
            e.ChangedRange.SetStyle(BlueStyle, $@"\b({mInstructionsNamesRegEx})\b|#region\b|#endregion\b");

            //clear folding markers
            e.ChangedRange.ClearFoldingMarkers();

            e.ChangedRange.SetFoldingMarkers(@"/\*", @"\*/");//allow to collapse comment block
        }

        private void Write(string argText, bool argNewLine = false)
        {
            mIsPromptMode = false;
            mIsUpdating = true;
            try
            {
                GoEnd();
                AppendText(argText);
                if (argNewLine)
                    AppendText("\n");
                GoEnd();
                mStartReadPlace = Range.End;
            }
            finally
            {
                mIsUpdating = false;
                mIsPromptMode = true;
            }
        }
    }
}
