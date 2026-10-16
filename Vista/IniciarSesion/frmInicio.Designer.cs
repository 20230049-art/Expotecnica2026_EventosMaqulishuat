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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInicio));
            this.pnlVistaInicio = new System.Windows.Forms.Panel();
            this.rediseñoImagen1 = new Vista.Utilidades.RediseñoImagen();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlVistaInicio.SuspendLayout();
            this.rediseñoImagen1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlVistaInicio
            // 
            this.pnlVistaInicio.BackColor = System.Drawing.Color.Transparent;
            this.pnlVistaInicio.Controls.Add(this.rediseñoImagen1);
            this.pnlVistaInicio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlVistaInicio.Location = new System.Drawing.Point(0, 0);
            this.pnlVistaInicio.Name = "pnlVistaInicio";
            this.pnlVistaInicio.Size = new System.Drawing.Size(1924, 973);
            this.pnlVistaInicio.TabIndex = 0;
            // 
            // rediseñoImagen1
            // 
            this.rediseñoImagen1.ColumnCount = 3;
            this.rediseñoImagen1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.82952F));
            this.rediseñoImagen1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.14553F));
            this.rediseñoImagen1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.02495F));
            this.rediseñoImagen1.Controls.Add(this.pbLogo, 1, 2);
            this.rediseñoImagen1.Controls.Add(this.label1, 1, 1);
            this.rediseñoImagen1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rediseñoImagen1.Location = new System.Drawing.Point(0, 0);
            this.rediseñoImagen1.Name = "rediseñoImagen1";
            this.rediseñoImagen1.RowCount = 4;
            this.rediseñoImagen1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.03289F));
            this.rediseñoImagen1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.33299F));
            this.rediseñoImagen1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.49024F));
            this.rediseñoImagen1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.rediseñoImagen1.Size = new System.Drawing.Size(1924, 973);
            this.rediseñoImagen1.TabIndex = 7;
            this.toolTip1.SetToolTip(this.rediseñoImagen1, "Click en cualquier parte");
            this.rediseñoImagen1.Click += new System.EventHandler(this.rediseñoImagen1_Click);
            // 
            // pbLogo
            // 
            this.pbLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbLogo.Image = global::Vista.Properties.Resources.LogoInicio;
            this.pbLogo.Location = new System.Drawing.Point(422, 279);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(1055, 495);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLogo.TabIndex = 1;
            this.pbLogo.TabStop = false;
            this.toolTip1.SetToolTip(this.pbLogo, "Da click en cualquier lugar de la pantalla");
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Georgia", 40.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(83)))), ((int)(((byte)(45)))));
            this.label1.Location = new System.Drawing.Point(422, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1055, 120);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bienvenido";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label1, "Da click en cualquier lugar de la pantalla");
            // 
            // frmInicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Violet;
            this.BackgroundImage = global::Vista.Properties.Resources.Fondo2;
            this.ClientSize = new System.Drawing.Size(1924, 973);
            this.Controls.Add(this.pnlVistaInicio);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmInicio";
            this.Text = "Inicio";
            this.TransparencyKey = System.Drawing.Color.Violet;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmInicio_Load);
            this.pnlVistaInicio.ResumeLayout(false);
            this.rediseñoImagen1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlVistaInicio;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.ToolTip toolTip1;
        private Utilidades.RediseñoImagen rediseñoImagen1;
    }
}