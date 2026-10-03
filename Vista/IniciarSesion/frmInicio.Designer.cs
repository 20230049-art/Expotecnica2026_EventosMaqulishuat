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
            this.tlpFondo = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.pnlVistaInicio.SuspendLayout();
            this.tlpFondo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
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
            this.tlpFondo.Controls.Add(this.pbLogo, 1, 2);
            this.tlpFondo.Controls.Add(this.tableLayoutPanel2, 2, 3);
            this.tlpFondo.Location = new System.Drawing.Point(0, 0);
            this.tlpFondo.Name = "tlpFondo";
            this.tlpFondo.RowCount = 4;
            this.tlpFondo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.46352F));
            this.tlpFondo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.98561F));
            this.tlpFondo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.8294F));
            this.tlpFondo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.92703F));
            this.tlpFondo.Size = new System.Drawing.Size(1924, 973);
            this.tlpFondo.TabIndex = 0;
            this.toolTip1.SetToolTip(this.tlpFondo, "Da click en cualquier lugar de la pantalla");
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
            this.toolTip1.SetToolTip(this.label1, "Da click en cualquier lugar de la pantalla");
            // 
            // pbLogo
            // 
            this.pbLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbLogo.Image = global::Vista.Properties.Resources.LogoInicio;
            this.pbLogo.Location = new System.Drawing.Point(588, 307);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(800, 400);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLogo.TabIndex = 1;
            this.pbLogo.TabStop = false;
            this.toolTip1.SetToolTip(this.pbLogo, "Da click en cualquier lugar de la pantalla");
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(39)))), ((int)(((byte)(3)))));
            this.tableLayoutPanel2.BackgroundImage = global::Vista.Properties.Resources.Fondo2;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.75F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23F));
            this.tableLayoutPanel2.Controls.Add(this.button1, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel2.ForeColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1394, 713);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 38.13953F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.02325F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 28.37209F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(527, 257);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Book Antiqua", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(143, 101);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(258, 79);
            this.button1.TabIndex = 1;
            this.button1.Text = "Cerrar";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmInicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1924, 973);
            this.Controls.Add(this.pnlVistaInicio);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmInicio";
            this.Text = "frmInicio";
            this.Load += new System.EventHandler(this.frmInicio_Load);
            this.pnlVistaInicio.ResumeLayout(false);
            this.tlpFondo.ResumeLayout(false);
            this.tlpFondo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlVistaInicio;
        private System.Windows.Forms.TableLayoutPanel tlpFondo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button button1;
    }
}