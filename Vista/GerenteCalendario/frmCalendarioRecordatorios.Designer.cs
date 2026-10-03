namespace Vista.GerenteCalendario
{
    partial class frmCalendarioRecordatorios
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
            this.pnlVistaCalendarioActualizar = new System.Windows.Forms.Panel();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlDatos = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.flpRecordatorios = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlContenedorRecordatorio = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.btnAnadirRecordatorio = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblDia = new System.Windows.Forms.Label();
            this.pnlVistaCalendarioActualizar.SuspendLayout();
            this.panel4.SuspendLayout();
            this.pnlDatos.SuspendLayout();
            this.panel3.SuspendLayout();
            this.flpRecordatorios.SuspendLayout();
            this.pnlContenedorRecordatorio.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlVistaCalendarioActualizar
            // 
            this.pnlVistaCalendarioActualizar.Controls.Add(this.btnCerrar);
            this.pnlVistaCalendarioActualizar.Controls.Add(this.panel4);
            this.pnlVistaCalendarioActualizar.Controls.Add(this.pnlDatos);
            this.pnlVistaCalendarioActualizar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlVistaCalendarioActualizar.Location = new System.Drawing.Point(0, 0);
            this.pnlVistaCalendarioActualizar.Name = "pnlVistaCalendarioActualizar";
            this.pnlVistaCalendarioActualizar.Size = new System.Drawing.Size(1086, 602);
            this.pnlVistaCalendarioActualizar.TabIndex = 1;
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(180)))), ((int)(((byte)(141)))));
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Book Antiqua", 16.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrar.Location = new System.Drawing.Point(438, 520);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(190, 46);
            this.btnCerrar.TabIndex = 19;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.LightCoral;
            this.panel4.Controls.Add(this.lblTitle);
            this.panel4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel4.Location = new System.Drawing.Point(337, 28);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(399, 74);
            this.panel4.TabIndex = 17;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 25.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblTitle.Location = new System.Drawing.Point(41, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(301, 57);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Recordatorios";
            // 
            // pnlDatos
            // 
            this.pnlDatos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(219)))), ((int)(((byte)(201)))));
            this.pnlDatos.Controls.Add(this.panel3);
            this.pnlDatos.Controls.Add(this.btnAnadirRecordatorio);
            this.pnlDatos.Controls.Add(this.panel2);
            this.pnlDatos.Location = new System.Drawing.Point(76, 60);
            this.pnlDatos.Name = "pnlDatos";
            this.pnlDatos.Size = new System.Drawing.Size(922, 483);
            this.pnlDatos.TabIndex = 18;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(250)))), ((int)(((byte)(232)))));
            this.panel3.Controls.Add(this.flpRecordatorios);
            this.panel3.Location = new System.Drawing.Point(25, 136);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(871, 329);
            this.panel3.TabIndex = 12;
            // 
            // flpRecordatorios
            // 
            this.flpRecordatorios.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpRecordatorios.AutoScroll = true;
            this.flpRecordatorios.Controls.Add(this.pnlContenedorRecordatorio);
            this.flpRecordatorios.Location = new System.Drawing.Point(8, 7);
            this.flpRecordatorios.Name = "flpRecordatorios";
            this.flpRecordatorios.Size = new System.Drawing.Size(852, 314);
            this.flpRecordatorios.TabIndex = 12;
            // 
            // pnlContenedorRecordatorio
            // 
            this.pnlContenedorRecordatorio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(213)))), ((int)(((byte)(191)))));
            this.pnlContenedorRecordatorio.Controls.Add(this.label8);
            this.pnlContenedorRecordatorio.Controls.Add(this.label7);
            this.pnlContenedorRecordatorio.Controls.Add(this.label6);
            this.pnlContenedorRecordatorio.Controls.Add(this.label5);
            this.pnlContenedorRecordatorio.Controls.Add(this.label3);
            this.pnlContenedorRecordatorio.Controls.Add(this.label18);
            this.pnlContenedorRecordatorio.Controls.Add(this.panel6);
            this.pnlContenedorRecordatorio.Controls.Add(this.label26);
            this.pnlContenedorRecordatorio.Controls.Add(this.label27);
            this.pnlContenedorRecordatorio.Cursor = System.Windows.Forms.Cursors.Default;
            this.pnlContenedorRecordatorio.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlContenedorRecordatorio.Location = new System.Drawing.Point(3, 3);
            this.pnlContenedorRecordatorio.Name = "pnlContenedorRecordatorio";
            this.pnlContenedorRecordatorio.Size = new System.Drawing.Size(822, 109);
            this.pnlContenedorRecordatorio.TabIndex = 11;
            this.pnlContenedorRecordatorio.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Bookman Old Style", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(7)))), ((int)(((byte)(4)))));
            this.label8.Location = new System.Drawing.Point(581, 55);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(227, 41);
            this.label8.TabIndex = 14;
            this.label8.Text = "Plaza Liberta";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Book Antiqua", 22.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(6)))), ((int)(((byte)(6)))));
            this.label7.Location = new System.Drawing.Point(422, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(206, 46);
            this.label7.TabIndex = 13;
            this.label7.Text = "Ubicación:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Bookman Old Style", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(7)))), ((int)(((byte)(4)))));
            this.label6.Location = new System.Drawing.Point(552, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(218, 41);
            this.label6.TabIndex = 12;
            this.label6.Text = "27/28/2026";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Book Antiqua", 22.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(6)))), ((int)(((byte)(6)))));
            this.label5.Location = new System.Drawing.Point(421, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(167, 46);
            this.label5.TabIndex = 11;
            this.label5.Text = "Cliente: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Book Antiqua", 22.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(6)))), ((int)(((byte)(6)))));
            this.label3.Location = new System.Drawing.Point(25, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 46);
            this.label3.TabIndex = 10;
            this.label3.Text = "Hora: ";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Bookman Old Style", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(7)))), ((int)(((byte)(4)))));
            this.label18.Location = new System.Drawing.Point(121, 55);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(321, 41);
            this.label18.TabIndex = 9;
            this.label18.Text = "8:20 AM - 9:30 AM";
            this.label18.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(34)))), ((int)(((byte)(23)))));
            this.panel6.Location = new System.Drawing.Point(406, 16);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1, 76);
            this.panel6.TabIndex = 6;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Bookman Old Style", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(7)))), ((int)(((byte)(4)))));
            this.label26.Location = new System.Drawing.Point(149, 18);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(198, 41);
            this.label26.TabIndex = 3;
            this.label26.Text = "Silvia Sofia";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Book Antiqua", 22.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(6)))), ((int)(((byte)(6)))));
            this.label27.Location = new System.Drawing.Point(23, 16);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(167, 46);
            this.label27.TabIndex = 2;
            this.label27.Text = "Cliente: ";
            // 
            // btnAnadirRecordatorio
            // 
            this.btnAnadirRecordatorio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(225)))), ((int)(((byte)(155)))));
            this.btnAnadirRecordatorio.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAnadirRecordatorio.FlatAppearance.BorderSize = 0;
            this.btnAnadirRecordatorio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnadirRecordatorio.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAnadirRecordatorio.Location = new System.Drawing.Point(694, 13);
            this.btnAnadirRecordatorio.Name = "btnAnadirRecordatorio";
            this.btnAnadirRecordatorio.Size = new System.Drawing.Size(202, 55);
            this.btnAnadirRecordatorio.TabIndex = 12;
            this.btnAnadirRecordatorio.Text = "Añadir nuevo recordatorio";
            this.btnAnadirRecordatorio.UseVisualStyleBackColor = false;
            this.btnAnadirRecordatorio.Click += new System.EventHandler(this.btnAnadirRecordatorio_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(250)))), ((int)(((byte)(232)))));
            this.panel2.Controls.Add(this.lblDia);
            this.panel2.Location = new System.Drawing.Point(54, 75);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(827, 53);
            this.panel2.TabIndex = 11;
            // 
            // lblDia
            // 
            this.lblDia.AutoSize = true;
            this.lblDia.Font = new System.Drawing.Font("Palatino Linotype", 21F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(44)))), ((int)(((byte)(35)))));
            this.lblDia.Location = new System.Drawing.Point(256, 4);
            this.lblDia.Name = "lblDia";
            this.lblDia.Size = new System.Drawing.Size(325, 47);
            this.lblDia.TabIndex = 2;
            this.lblDia.Text = "2 de Mayo del 2026";
            // 
            // frmCalendarioRecordatorios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(241)))), ((int)(((byte)(217)))));
            this.ClientSize = new System.Drawing.Size(1086, 602);
            this.Controls.Add(this.pnlVistaCalendarioActualizar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(1942, 1020);
            this.Name = "frmCalendarioRecordatorios";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmCalendarioRecordatorios";
            this.Load += new System.EventHandler(this.frmCalendarioRecordatorios_Load);
            this.pnlVistaCalendarioActualizar.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.pnlDatos.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.flpRecordatorios.ResumeLayout(false);
            this.pnlContenedorRecordatorio.ResumeLayout(false);
            this.pnlContenedorRecordatorio.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlVistaCalendarioActualizar;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlDatos;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel pnlContenedorRecordatorio;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Button btnAnadirRecordatorio;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblDia;
        private System.Windows.Forms.FlowLayoutPanel flpRecordatorios;
    }
}