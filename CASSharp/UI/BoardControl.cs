using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastColoredTextBoxNS;

namespace CASSharp.UI
{
    public partial class BoardControl : UserControl
    {
        public BoardControl()
        {
            InitializeComponent();
        }

        public void AddLegend(string argText)
        {
            this.layMain.SuspendLayout();
            this.SuspendLayout();

            try
            {
                var pEdit = new FastColoredTextBox
                {
                    //Name = "txtPrompt",
                    Language = FastColoredTextBoxNS.Language.CSharp,
                    ShowLineNumbers = false,
                    Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
                    ReadOnly = true,
                    TabStop = false,
                    BorderStyle = BorderStyle.None,
                    Text = argText,
                };
                var pSize = pEdit.GetSizeOfAllLines();

                pEdit.Size = new Size(Width, pSize.Height);

                layMain.Controls.Add(pEdit);
            }
            finally
            {
                this.layMain.ResumeLayout(true);
                this.ResumeLayout(true);
            }
        }

        public void AddPrompt()
        {
            this.layMain.SuspendLayout();
            this.SuspendLayout();

            try
            {
                var pPrompt = new PromptControl
                {
                    Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
                    Width = Width
                };

                layMain.Controls.Add(pPrompt);
            }
            finally
            {
                this.layMain.ResumeLayout(true);
                this.ResumeLayout(true);
            }
        }
    }
}
