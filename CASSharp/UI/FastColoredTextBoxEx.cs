using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
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
    }
}
