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
        public PromptControl()
        {
            InitializeComponent();
        }

        public void SetPrompt(string[] argLines)
        {
            txtPrompt.Clear();
            foreach (var l in argLines)
                txtPrompt.AppendText($"{l}\n");

            var pHeight = txtPrompt.GetAllLinesHeight();
        }
    }
}
