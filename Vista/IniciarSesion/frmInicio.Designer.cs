namespace Vista.IniciarSesion
{
    partial class frmInicio
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
            this.pnlVistaInicio = new System.Windows.Forms.Panel();
            this.tlpFondo = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlVistaInicio.SuspendLayout();
            this.tlpFondo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlVistaInicio
            // 
            this.pnlVistaInicio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(32)))), ((int)(((byte)(16)))));
            this.pnlVistaInicio.Controls.Add(this.tlpFondo);
            this.pnlVistaInicio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlVistaInicio.Location = new System.Drawing.Point(0, 0);
            this.pnlVistaInicio.Name = "pnlVistaInicio";
            this.pnlVistaInicio.Size = new System.Drawing.Size(1924, 973);
            this.pnlVistaInicio.TabIndex = 0;
            // 
            // tlpFondo
            // 
            this.tlpFondo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpFondo.BackgroundImage = global::Vista.Properties.Resources.Fondo2;
            this.tlpFondo.ColumnCount = 3;
            this.tlpFondo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.40541F));
            this.tlpFondo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.89189F));
            this.tlpFondo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.7027F));
            this.tlpFondo.Controls.Add(this.label1, 1, 1);
            this.tlpFondo.Controls.Add(this.pictureBox1, 1, 2);
            this.tlpFondo.Location = new System.Drawing.Point(0, 0);
            this.tlpFondo.Name = "tlpFondo";
            this.tlpFondo.RowCount = 4;
            this.tlpFondo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.46352F));
            this.tlpFondo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.98561F));
            this.tlpFondo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.8294F));
            this.tlpFondo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.92703F));
            this.tlpFondo.Size = new System.Drawing.Size(1924, 973);
            this.tlpFondo.TabIndex = 0;
            this.tlpFondo.Click += new System.EventHandler(this.tlpFondo_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Georgia", 40.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(83)))), ((int)(((byte)(45)))));
            this.label1.Image = global::Vista.Properties.Resources.Fondo2;
            this.label1.Location = new System.Drawing.Point(588, 178);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(800, 77);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bienvenido";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::Vista.Properties.Resources.LogoInicio;
            this.pictureBox1.Location = new System.Drawing.Point(588, 307);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 400);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // frmInicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1924, 973);
            this.Controls.Add(this.pnlVistaInicio);
            this.Name = "frmInicio";
            this.Text = "frmInicio";
            this.pnlVistaInicio.ResumeLayout(false);
            this.tlpFondo.ResumeLayout(false);
            this.tlpFondo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlVistaInicio;
        private System.Windows.Forms.TableLayoutPanel tlpFondo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}