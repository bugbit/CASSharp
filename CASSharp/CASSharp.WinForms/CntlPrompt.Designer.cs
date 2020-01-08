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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbNameVar = new System.Windows.Forms.Label();
            this.txtExpr = new FastColoredTextBoxNS.FastColoredTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpr)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.lbNameVar, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtExpr, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(479, 33);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lbNameVar
            // 
            this.lbNameVar.AutoSize = true;
            this.lbNameVar.Location = new System.Drawing.Point(3, 0);
            this.lbNameVar.Name = "lbNameVar";
            this.lbNameVar.Size = new System.Drawing.Size(35, 13);
            this.lbNameVar.TabIndex = 0;
            this.lbNameVar.Text = "label1";
            // 
            // txtExpr
            // 
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
            this.txtExpr.AutoSize = true;
            this.txtExpr.BackBrush = null;
            this.txtExpr.BracketsHighlightStrategy = FastColoredTextBoxNS.BracketsHighlightStrategy.Strategy2;
            this.txtExpr.CharHeight = 14;
            this.txtExpr.CharWidth = 8;
            this.txtExpr.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtExpr.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtExpr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtExpr.Font = new System.Drawing.Font("Courier New", 9.75F);
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
            // 
            // CntlPrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CntlPrompt";
            this.Size = new System.Drawing.Size(479, 33);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpr)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbNameVar;
        private FastColoredTextBoxNS.FastColoredTextBox txtExpr;
    }
}
