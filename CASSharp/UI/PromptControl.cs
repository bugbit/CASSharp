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
        FastColoredTextBoxEx txtPrompt = new FastColoredTextBoxEx();

        public PromptControl()
        {
            this.Controls.Add(txtPrompt);
            InitializeComponent();
        }

        public void SetPrompt(string[] argLines)
        {
            txtPrompt.Clear();
            foreach (var l in argLines)
                txtPrompt.AppendText($"{l}\n");

            var pSize = txtPrompt.GetSizeOfAllLines();

            if (pSize.Width > txtPrompt.Size.Width || pSize.Height > txtPrompt.Size.Height)
                txtPrompt.Size = pSize;
        }
    }
}
