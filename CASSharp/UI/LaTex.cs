using System;
using System.Collections.Generic;
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
        public string EnunciadoStr => string.Empty;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //new SkiaSharp.SKCanvas()
            //v.
        }
    }
}
