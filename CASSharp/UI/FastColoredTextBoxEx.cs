using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CASSharp.UI
{
    class FastColoredTextBoxEx : FastColoredTextBox
    {
        public int GetAllLinesHeight()
        {
            var lines = this.Lines;
            var count = lines.Count;
            var TextHeight = 0;

            for (int i = 0; i < count; i++)
            {
                var lineInfo = LineInfos[i];

                if (lineInfo.VisibleState != VisibleState.Visible)
                    continue;
                TextHeight += lineInfo.WordWrapStringsCount * this.CharHeight + lineInfo.bottomPadding;
            }

            return TextHeight;
        }

        public Size GetSizeOfAllLines()
        {
            var lines = this.Lines;
            var count = lines.Count;
            var maxWidthLine = 0;
            var TextHeight = 0;

            for (int i = 0; i < count; i++)
            {
                var line = Lines[i];
                var lineInfo = LineInfos[i];

                if (lineInfo.VisibleState != VisibleState.Visible)
                    continue;

                var x = LeftIndent + Paddings.Left;
                for (int iWordWrapLine = 0; iWordWrapLine < lineInfo.WordWrapStringsCount; iWordWrapLine++)
                {
                    var indent = iWordWrapLine == 0 ? 0 : lineInfo.wordWrapIndent * CharWidth;

                    x += indent;
                }
                x += line.Length * CharWidth;
                if (x > maxWidthLine)
                    maxWidthLine = x;

                TextHeight += lineInfo.WordWrapStringsCount * this.CharHeight + lineInfo.bottomPadding;
            }

            return new Size(maxWidthLine, TextHeight);
        }
    }
}
