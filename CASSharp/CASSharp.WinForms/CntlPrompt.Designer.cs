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

namespace CASSharp.WinForms
{
    partial class CntlPrompt
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CntlPrompt));
            this.tbMain = new System.Windows.Forms.TableLayoutPanel();
            this.lbNameVarPrompt = new System.Windows.Forms.Label();
            this.txtExpr = new FastColoredTextBoxNS.FastColoredTextBox();
            this.tbMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpr)).BeginInit();
            this.SuspendLayout();
            // 
            // tbMain
            // 
            this.tbMain.ColumnCount = 2;
            this.tbMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tbMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbMain.Controls.Add(this.lbNameVarPrompt, 0, 0);
            this.tbMain.Controls.Add(this.txtExpr, 1, 0);
            this.tbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMain.Location = new System.Drawing.Point(0, 0);
            this.tbMain.Name = "tbMain";
            this.tbMain.RowCount = 1;
            this.tbMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbMain.Size = new System.Drawing.Size(479, 33);
            this.tbMain.TabIndex = 0;
            // 
            // lbNameVarPrompt
            // 
            this.lbNameVarPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbNameVarPrompt.Location = new System.Drawing.Point(3, 0);
            this.lbNameVarPrompt.Name = "lbNameVarPrompt";
            this.lbNameVarPrompt.Size = new System.Drawing.Size(74, 13);
            this.lbNameVarPrompt.TabIndex = 0;
            this.lbNameVarPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtExpr
            // 
            this.txtExpr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExpr.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.txtExpr.AutoIndentCharsPatterns = "\r\n^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\r\n^\\s*(case|default)\\s*[^:" +
    "]*(?<range>:)\\s*(?<range>[^;]+);\r\n";
            this.txtExpr.AutoScrollMinSize = new System.Drawing.Size(2, 14);
            this.txtExpr.BackBrush = null;
            this.txtExpr.BracketsHighlightStrategy = FastColoredTextBoxNS.BracketsHighlightStrategy.Strategy2;
            this.txtExpr.CharHeight = 14;
            this.txtExpr.CharWidth = 8;
            this.txtExpr.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtExpr.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtExpr.IsReplaceMode = false;
            this.txtExpr.Language = FastColoredTextBoxNS.Language.CSharp;
            this.txtExpr.LeftBracket = '(';
            this.txtExpr.LeftBracket2 = '{';
            this.txtExpr.Location = new System.Drawing.Point(83, 3);
            this.txtExpr.Name = "txtExpr";
            this.txtExpr.Paddings = new System.Windows.Forms.Padding(0);
            this.txtExpr.RightBracket = ')';
            this.txtExpr.RightBracket2 = '}';
            this.txtExpr.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtExpr.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("txtExpr.ServiceColors")));
            this.txtExpr.ShowLineNumbers = false;
            this.txtExpr.Size = new System.Drawing.Size(393, 27);
            this.txtExpr.TabIndex = 1;
            this.txtExpr.Zoom = 100;
            this.txtExpr.LineInserted += new System.EventHandler<FastColoredTextBoxNS.LineInsertedEventArgs>(this.txtExpr_LineInserted);
            this.txtExpr.LineRemoved += new System.EventHandler<FastColoredTextBoxNS.LineRemovedEventArgs>(this.txtExpr_LineRemoved);
            // 
            // CntlPrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbMain);
            this.Name = "CntlPrompt";
            this.Size = new System.Drawing.Size(479, 33);
            this.tbMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtExpr)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tbMain;
        private System.Windows.Forms.Label lbNameVarPrompt;
        private FastColoredTextBoxNS.FastColoredTextBox txtExpr;
    }
}
