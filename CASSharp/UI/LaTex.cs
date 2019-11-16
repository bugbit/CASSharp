using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSharpMath.SkiaSharp;
using SkiaSharp;

namespace CASSharp.UI
{
    // https://github.com/mattleibow/skiasharp-samples/blob/master/Skia.WindowsDesktop.Demo/SkiaView.cs
    class LaTex : Control
    {
        private MathPainter mPainter = new MathPainter(12);

        public LaTex()
        {
            DoubleBuffered = true;
        }

        public string LaTexStr
        {
            get => mPainter.LaTeX;
            set
            {
                if (value == null)
                    return;

                mPainter.LaTeX = value;

                var pM = mPainter.Measure;

                if (pM.HasValue)
                    Size = pM.Value.Size.ToSize();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var pW = Width;
            var pH = Height;

            using (var pBmp = new Bitmap(pW, pH, PixelFormat.Format32bppPArgb))
            {
                var pData = pBmp.LockBits(new Rectangle(0, 0, pW, pH), ImageLockMode.WriteOnly, pBmp.PixelFormat);

                using (var surface = SKSurface.Create(pW, pH, SKColorType.Argb4444, SKAlphaType.Premul, pData.Scan0, pW * 4))
                {
                    var pSKCanvas = surface.Canvas;

                    mPainter.Draw(pSKCanvas, alignment: CSharpMath.Rendering.TextAlignment.Left);
                }
                pBmp.UnlockBits(pData);

                e.Graphics.DrawImage(pBmp, new Rectangle(0, 0, Width, Height));
            }
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            Invalidate();
        }
    }
}
