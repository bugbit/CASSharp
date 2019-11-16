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
            this.laTex1 = new CASSharp.UI.LaTex();
            this.SuspendLayout();
            // 
            // laTex1
            // 
            this.laTex1.LaTexStr = null;
            this.laTex1.Location = new System.Drawing.Point(0, 0);
            this.laTex1.Name = "laTex1";
            this.laTex1.Size = new System.Drawing.Size(127, 97);
            this.laTex1.TabIndex = 0;
            this.laTex1.Text = "\\frac{1}{\\sqrt{x}}";
            // 
            // PromptControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.laTex1);
            this.Name = "PromptControl";
            this.ResumeLayout(false);

        }

        #endregion

        private LaTex laTex1;
    }
}
