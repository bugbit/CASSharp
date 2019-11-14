using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CASSharp.UI
{
    public partial class PromptControl : UserControl
    {
        private FastColoredTextBoxEx mTxtPrompt = new FastColoredTextBoxEx
        {
            Name = "txtPrompt",
            Language = FastColoredTextBoxNS.Language.CSharp,
            ShowLineNumbers = false
        };
        private LaTex mLaTexIn;
        private LaTex mLaTexOut;

        public PromptControl()
        {
            mTxtPrompt.TextChanged += (s, e) => CalcSize();

            this.Controls.Add(mTxtPrompt);
            InitializeComponent();
        }

        public void SetPrompt(string[] argLines)
        {
            mTxtPrompt.Clear();
            foreach (var l in argLines)
                mTxtPrompt.AppendText($"{l}\n");
        }

        private void CalcSize()
        {
            var pSize = mTxtPrompt.GetSizeOfAllLines();

            if (pSize.Width > mTxtPrompt.Size.Width || pSize.Height > mTxtPrompt.Size.Height)
                mTxtPrompt.Size = pSize;

            Size = pSize;
        }
    }
}
