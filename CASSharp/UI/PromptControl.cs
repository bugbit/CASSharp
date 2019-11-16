using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkiaSharp.Views.Desktop;
using CSharpMath.SkiaSharp;

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
        //private LaTex mLaTex = new LaTex();
        private SKControl mLaTex = new SKControl();
        private MathPainter mPainter = new MathPainter(12);

        public PromptControl()
        {
            mTxtPrompt.TextChanged += (s, e) => CalcSize();
            mLaTex.PaintSurface += LaTex_PaintSurface;

            this.Controls.Add(mTxtPrompt);
            this.Controls.Add(mLaTex);
            InitializeComponent();
        }

        public void SetPrompt(string[] argLines)
        {
            mTxtPrompt.Clear();
            foreach (var l in argLines)
                mTxtPrompt.AppendText($"{l}\n");
        }

        public void SetLaTex(string argLaTex)
        {
            mPainter.LaTeX = argLaTex;

            var pM = mPainter.Measure;

            if (pM.HasValue)
                mLaTex.Size = pM.Value.Size.ToSize();

            CalcSize();
        }

        private void CalcSize()
        {
            var pSize = mTxtPrompt.GetSizeOfAllLines();

            if (pSize.Width > mTxtPrompt.Size.Width || pSize.Height > mTxtPrompt.Size.Height)
                mTxtPrompt.Size = pSize;
            else
                pSize = mTxtPrompt.Size;

            mLaTex.Location = new Point(mTxtPrompt.Location.X, mTxtPrompt.Location.Y + mTxtPrompt.Height);

            pSize = new Size(Math.Max(pSize.Width, mLaTex.Width), mTxtPrompt.Height + mLaTex.Height);

            Size = pSize;
        }

        private void LaTex_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            mPainter.Draw(e.Surface.Canvas, alignment: CSharpMath.Rendering.TextAlignment.Left);
        }
    }
}
