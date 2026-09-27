namespace Vista.GerenteAyuda
{
    partial class frmAyuda
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlVsitaAyuda = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlVsitaAyuda
            // 
            this.pnlVsitaAyuda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlVsitaAyuda.Location = new System.Drawing.Point(0, 0);
            this.pnlVsitaAyuda.Name = "pnlVsitaAyuda";
            this.pnlVsitaAyuda.Size = new System.Drawing.Size(1924, 973);
            this.pnlVsitaAyuda.TabIndex = 0;
            // 
            // frmAyuda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(241)))), ((int)(((byte)(217)))));
            this.ClientSize = new System.Drawing.Size(1924, 973);
            this.Controls.Add(this.pnlVsitaAyuda);
            this.Name = "frmAyuda";
            this.Text = "frmAyuda";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlVsitaAyuda;
    }
}