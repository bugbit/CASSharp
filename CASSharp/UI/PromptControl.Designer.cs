namespace CASSharp.UI
{
    partial class PromptControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PromptControl));
            this.txtPrompt = new FastColoredTextBoxNS.FastColoredTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrompt)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPrompt
            // 
            this.txtPrompt.AllowMacroRecording = false;
            this.txtPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrompt.AutoCompleteBrackets = true;
            this.txtPrompt.AutoCompleteBracketsList = new char[] {
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
            this.txtPrompt.AutoIndentCharsPatterns = "\r\n^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;]+);\r\n^\\s*(case|default)\\s*[^:]" +
    "*(?<range>:)\\s*(?<range>[^;]+);\r\n";
            this.txtPrompt.AutoScrollMinSize = new System.Drawing.Size(162, 14);
            this.txtPrompt.AutoSize = true;
            this.txtPrompt.BackBrush = null;
            this.txtPrompt.BracketsHighlightStrategy = FastColoredTextBoxNS.BracketsHighlightStrategy.Strategy2;
            this.txtPrompt.CharHeight = 14;
            this.txtPrompt.CharWidth = 8;
            this.txtPrompt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPrompt.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtPrompt.IsReplaceMode = false;
            this.txtPrompt.Language = FastColoredTextBoxNS.Language.CSharp;
            this.txtPrompt.LeftBracket = '(';
            this.txtPrompt.LeftBracket2 = '{';
            this.txtPrompt.Location = new System.Drawing.Point(0, 0);
            this.txtPrompt.Name = "txtPrompt";
            this.txtPrompt.Paddings = new System.Windows.Forms.Padding(0);
            this.txtPrompt.RightBracket = ')';
            this.txtPrompt.RightBracket2 = '}';
            this.txtPrompt.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtPrompt.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("txtPrompt.ServiceColors")));
            this.txtPrompt.ShowCaretWhenInactive = true;
            this.txtPrompt.ShowLineNumbers = false;
            this.txtPrompt.ShowScrollBars = false;
            this.txtPrompt.Size = new System.Drawing.Size(10, 10);
            this.txtPrompt.TabIndex = 0;
            this.txtPrompt.Text = "/* aadddsdsd \\n\\n */";
            this.txtPrompt.Zoom = 100;
            // 
            // PromptControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.txtPrompt);
            this.Name = "PromptControl";
            this.Size = new System.Drawing.Size(72, 72);
            ((System.ComponentModel.ISupportInitialize)(this.txtPrompt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FastColoredTextBoxNS.FastColoredTextBox txtPrompt;
    }
}
