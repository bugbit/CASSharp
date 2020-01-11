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

using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSharpMath.SkiaSharp;
using System.Drawing.Imaging;
using SkiaSharp;

namespace CASSharp.WinForms
{
    public partial class FrmMain : Form
    {
        EllipseStyle ellipseStyle = new EllipseStyle();
        EllipseStyle2 ellipseStyle2;
        EllipseStyle3 ellipseStyle3;
        EllipseStyle3 ellipseStyle4;

        public FrmMain()
        {
            InitializeComponent();
            ellipseStyle2 = new EllipseStyle2(fastColoredTextBox1);
            ellipseStyle3 = new EllipseStyle3(fastColoredTextBox1) { Painter = new MathPainter(fastColoredTextBox1.CharHeight) { LaTeX = @"$${{1}\over{6}}$$" } };
            ellipseStyle4 = new EllipseStyle3(fastColoredTextBox1) { Painter = new MathPainter(fastColoredTextBox1.CharHeight) { LaTeX = @"$${{23\,x}\over{x+1}}$$" } };
            fastColoredTextBox1.Text = @"/*
Prueba
*/
(%i1) primep;
(% o1) :D


(% o2) :D


";

            var pLines = (int)Math.Ceiling((decimal)ellipseStyle3.Size.Height / fastColoredTextBox1.CharHeight) + 1;

            fastColoredTextBox1.Text += @":E" + new string('\n', pLines);

            var pStart = fastColoredTextBox1.Range.End;

            fastColoredTextBox1.Text += "xx\n";

            new Range(fastColoredTextBox1, pStart, fastColoredTextBox1.Range.End).SetStyle(ellipseStyle4);

            fastColoredTextBox1.Text += "\n\n\n";
        }
        public void SetHeader(string argText, string argTitle)
        {
            Text = argTitle;
            lbHeader.Text = argText;
        }
        public CntlPrompt AddPrompt(string argNameVar)
        {
            var pPrompt = new CntlPrompt() { NameVarPrompt = argNameVar, Width = Width };

            lyBoard.Controls.Add(pPrompt);

            return pPrompt;
        }

        private void fastColoredTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            string pattern = Regex.Replace(":D", @"[\^\$\[\]\(\)\.\\\*\+\|\?\{\}]", "\\$0");
            //clear old styles of chars
            //e.ChangedRange.ClearStyle(ellipseStyle);
            e.ChangedRange.ClearStyle(ellipseStyle2);
            //append style for word 'Babylon'
            //e.ChangedRange.SetStyle(ellipseStyle, @"\bBabylon\b", RegexOptions.IgnoreCase);
            e.ChangedRange.SetStyle(ellipseStyle2, @":D", RegexOptions.IgnoreCase);
            e.ChangedRange.SetStyle(ellipseStyle3, @":E", RegexOptions.IgnoreCase);
        }
    }

    class EllipseStyle : Style
    {
        public override void Draw(Graphics gr, Point position, Range range)
        {
            //get size of rectangle
            Size size = GetSizeOfRange(range);
            //create rectangle
            Rectangle rect = new Rectangle(position, size);
            //inflate it
            rect.Inflate(2, 2);
            //get rounded rectangle
            var path = GetRoundedRectangle(rect, 7);
            //draw rounded rectangle
            gr.DrawPath(Pens.Red, path);
        }
    }

    class EllipseStyle2 : TextStyle
    {
        public EllipseStyle2(FastColoredTextBox parent) : base(null, null, FontStyle.Regular)
        {
        }
        public override void Draw(Graphics gr, Point position, Range range)
        {
            var pPainter = new MathPainter(12) { LaTeX = @"\frac{1}{\sqrt{x}}" };
            var pSize = pPainter.Measure.Value.Size.ToSize();
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
            position.Offset(pSize.Width, 0);

            // mPainter.Draw(e.Surface.Canvas, alignment: CSharpMath.Rendering.TextAlignment.Left);
        }
    }

    class EllipseStyle3 : TextStyle
    {
        public MathPainter Painter { get; set; }
        public Size Size => (Painter == null) ? new Size() : Painter.Measure.Value.Size.ToSize();

        public EllipseStyle3(FastColoredTextBox parent) : base(null, null, FontStyle.Regular)
        {
        }

        public override void Draw(Graphics gr, Point position, Range range)
        {
            if (Painter == null)
                return;

            var pSize = Painter.Measure.Value.Size.ToSize();
            var bitmap = new Bitmap(pSize.Width, pSize.Height, PixelFormat.Format32bppPArgb);
            var info = new SKImageInfo(pSize.Width, pSize.Height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);
            var data = bitmap.LockBits(new Rectangle(0, 0, pSize.Width, pSize.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

            // create the surface
            using (var surface = SKSurface.Create(info, data.Scan0, data.Stride))
            {
                // start drawing
                //                OnPaintSurface(new SKPaintSurfaceEventArgs(surface, info));
                // mPainter.Draw(e.Surface.Canvas, alignment: CSharpMath.Rendering.TextAlignment.Left);
                Painter.Draw(surface.Canvas, alignment: CSharpMath.Rendering.TextAlignment.Left);

                surface.Canvas.Flush();
            }

            // write the bitmap to the graphics
            bitmap.UnlockBits(data);
            gr.DrawImage(bitmap, position);
            //position.Offset(pSize.Width, 0);

            // mPainter.Draw(e.Surface.Canvas, alignment: CSharpMath.Rendering.TextAlignment.Left);
        }
    }
}
