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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using CSharpMath.SkiaSharp;

using FastColoredTextBoxNS;
using SkiaSharp;

namespace CASSharp.WinForms.UI
{
    class LaTexStyle : TextStyle
    {
        private int mLastId = 0;
        private Dictionary<int, MathPainter> mPainters = new Dictionary<int, MathPainter>();

        public LaTexStyle(Brush foreBrush = null, Brush backgroundBrush = null, FontStyle fontStyle = FontStyle.Regular) : base(foreBrush, backgroundBrush, fontStyle)
        {
        }

        public Size CalcSize(MathPainter p) => p.Measure.Value.Size.ToSize() + new Size(4, 4);

        public int Add(MathPainter argPainter)
        {
            var pId = Interlocked.Increment(ref mLastId);

            mPainters[pId] = argPainter;

            return pId;
        }

        public override void Draw(Graphics gr, Point position, Range range)
        {
            if (int.TryParse(range.Text, out int pId) && mPainters.TryGetValue(pId, out MathPainter pPainter))
            {
                var pSize = CalcSize(pPainter);
                var bitmap = new Bitmap(pSize.Width, pSize.Height, PixelFormat.Format32bppPArgb);
                var info = new SKImageInfo(pSize.Width, pSize.Height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);
                var data = bitmap.LockBits(new Rectangle(0, 0, pSize.Width, pSize.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

                // create the surface
                using (var surface = SKSurface.Create(info, data.Scan0, data.Stride))
                {
                    // start drawing
                    //                OnPaintSurface(new SKPaintSurfaceEventArgs(surface, info));
                    // mPainter.Draw(e.Surface.Canvas, alignment: CSharpMath.Rendering.TextAlignment.Left);
                    pPainter.Draw(surface.Canvas, alignment: CSharpMath.Rendering.TextAlignment.Left);

                    surface.Canvas.Flush();
                }

                // write the bitmap to the graphics
                bitmap.UnlockBits(data);
                gr.DrawImage(bitmap, position);
            }
        }
    }
}
