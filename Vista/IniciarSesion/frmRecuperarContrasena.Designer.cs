namespace Vista.IniciarSesion
{
    partial class frmRecuperarContrasena
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnValidarCodigo = new System.Windows.Forms.Button();
            this.btnEnviarCodigo = new System.Windows.Forms.Button();
            this.btnNuevaContraseña = new System.Windows.Forms.Button();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblCorreo = new System.Windows.Forms.Label();
            this.txtNuevaContrasena = new System.Windows.Forms.TextBox();
            this.txtConfirmarContrasena = new System.Windows.Forms.TextBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtCorreo = new System.Windows.Forms.TextBox();
            this.lblNombreUsuario = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.SeaShell;
            this.panel1.Controls.Add(this.btnCerrar);
            this.panel1.Controls.Add(this.btnValidarCodigo);
            this.panel1.Controls.Add(this.btnEnviarCodigo);
            this.panel1.Controls.Add(this.btnNuevaContraseña);
            this.panel1.Controls.Add(this.txtCodigo);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.lblCorreo);
            this.panel1.Controls.Add(this.txtNuevaContrasena);
            this.panel1.Controls.Add(this.txtConfirmarContrasena);
            this.panel1.Controls.Add(this.lblUsuario);
            this.panel1.Controls.Add(this.txtCorreo);
            this.panel1.Controls.Add(this.lblNombreUsuario);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(25, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(606, 584);
            this.panel1.TabIndex = 1;
            // 
            // btnValidarCodigo
            // 
            this.btnValidarCodigo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnValidarCodigo.FlatAppearance.BorderSize = 0;
            this.btnValidarCodigo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnValidarCodigo.Font = new System.Drawing.Font("Georgia", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidarCodigo.ForeColor = System.Drawing.Color.Black;
            this.btnValidarCodigo.Location = new System.Drawing.Point(26, 244);
            this.btnValidarCodigo.Name = "btnValidarCodigo";
            this.btnValidarCodigo.Size = new System.Drawing.Size(228, 44);
            this.btnValidarCodigo.TabIndex = 48;
            this.btnValidarCodigo.Text = "Validar Codigo";
            this.btnValidarCodigo.UseVisualStyleBackColor = false;
            this.btnValidarCodigo.Click += new System.EventHandler(this.btnValidarCodigo_Click);
            // 
            // btnEnviarCodigo
            // 
            this.btnEnviarCodigo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnEnviarCodigo.FlatAppearance.BorderSize = 0;
            this.btnEnviarCodigo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnviarCodigo.Font = new System.Drawing.Font("Georgia", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnviarCodigo.ForeColor = System.Drawing.Color.Black;
            this.btnEnviarCodigo.Location = new System.Drawing.Point(307, 244);
            this.btnEnviarCodigo.Name = "btnEnviarCodigo";
            this.btnEnviarCodigo.Size = new System.Drawing.Size(228, 44);
            this.btnEnviarCodigo.TabIndex = 47;
            this.btnEnviarCodigo.Text = "Enviar Codigo";
            this.btnEnviarCodigo.UseVisualStyleBackColor = false;
            this.btnEnviarCodigo.Click += new System.EventHandler(this.btnEnviarCodigo_Click);
            // 
            // btnNuevaContraseña
            // 
            this.btnNuevaContraseña.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(189)))), ((int)(((byte)(45)))));
            this.btnNuevaContraseña.FlatAppearance.BorderSize = 0;
            this.btnNuevaContraseña.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevaContraseña.Font = new System.Drawing.Font("Georgia", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNuevaContraseña.ForeColor = System.Drawing.Color.Black;
            this.btnNuevaContraseña.Location = new System.Drawing.Point(83, 500);
            this.btnNuevaContraseña.Name = "btnNuevaContraseña";
            this.btnNuevaContraseña.Size = new System.Drawing.Size(228, 44);
            this.btnNuevaContraseña.TabIndex = 46;
            this.btnNuevaContraseña.Text = "Cambiar contraseña";
            this.btnNuevaContraseña.UseVisualStyleBackColor = false;
            this.btnNuevaContraseña.Click += new System.EventHandler(this.btnNuevaContraseña_Click);
            // 
            // txtCodigo
            // 
            this.txtCodigo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtCodigo.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodigo.Location = new System.Drawing.Point(29, 182);
            this.txtCodigo.Margin = new System.Windows.Forms.Padding(4);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(527, 33);
            this.txtCodigo.TabIndex = 35;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Times New Roman", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label5.Location = new System.Drawing.Point(28, 154);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(243, 27);
            this.label5.TabIndex = 34;
            this.label5.Text = "Codigo enviado:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCorreo
            // 
            this.lblCorreo.Font = new System.Drawing.Font("Times New Roman", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCorreo.Location = new System.Drawing.Point(22, 323);
            this.lblCorreo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCorreo.Name = "lblCorreo";
            this.lblCorreo.Size = new System.Drawing.Size(203, 27);
            this.lblCorreo.TabIndex = 33;
            this.lblCorreo.Text = "Nueva Contraseña:";
            this.lblCorreo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtNuevaContrasena
            // 
            this.txtNuevaContrasena.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtNuevaContrasena.Font = new System.Drawing.Font("Microsoft Tai Le", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNuevaContrasena.Location = new System.Drawing.Point(29, 347);
            this.txtNuevaContrasena.Margin = new System.Windows.Forms.Padding(4);
            this.txtNuevaContrasena.Multiline = true;
            this.txtNuevaContrasena.Name = "txtNuevaContrasena";
            this.txtNuevaContrasena.Size = new System.Drawing.Size(527, 37);
            this.txtNuevaContrasena.TabIndex = 32;
            // 
            // txtConfirmarContrasena
            // 
            this.txtConfirmarContrasena.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtConfirmarContrasena.Font = new System.Drawing.Font("Microsoft Tai Le", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfirmarContrasena.Location = new System.Drawing.Point(26, 433);
            this.txtConfirmarContrasena.Margin = new System.Windows.Forms.Padding(4);
            this.txtConfirmarContrasena.Name = "txtConfirmarContrasena";
            this.txtConfirmarContrasena.Size = new System.Drawing.Size(527, 35);
            this.txtConfirmarContrasena.TabIndex = 29;
            // 
            // lblUsuario
            // 
            this.lblUsuario.Font = new System.Drawing.Font("Times New Roman", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuario.Location = new System.Drawing.Point(22, 405);
            this.lblUsuario.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(258, 27);
            this.lblUsuario.TabIndex = 27;
            this.lblUsuario.Text = "Confirmar Contraseña";
            this.lblUsuario.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCorreo
            // 
            this.txtCorreo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtCorreo.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCorreo.Location = new System.Drawing.Point(29, 108);
            this.txtCorreo.Margin = new System.Windows.Forms.Padding(4);
            this.txtCorreo.Name = "txtCorreo";
            this.txtCorreo.Size = new System.Drawing.Size(527, 33);
            this.txtCorreo.TabIndex = 25;
            // 
            // lblNombreUsuario
            // 
            this.lblNombreUsuario.Font = new System.Drawing.Font("Times New Roman", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombreUsuario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblNombreUsuario.Location = new System.Drawing.Point(28, 80);
            this.lblNombreUsuario.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNombreUsuario.Name = "lblNombreUsuario";
            this.lblNombreUsuario.Size = new System.Drawing.Size(322, 27);
            this.lblNombreUsuario.TabIndex = 24;
            this.lblNombreUsuario.Text = "Nombre Usuario/ Correo:";
            this.lblNombreUsuario.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(24, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(391, 40);
            this.label3.TabIndex = 10;
            this.label3.Text = "Recuperar contraseña";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Georgia", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrar.ForeColor = System.Drawing.Color.Black;
            this.btnCerrar.Location = new System.Drawing.Point(335, 500);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(144, 44);
            this.btnCerrar.TabIndex = 49;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // frmRecuperarContrasena
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(241)))), ((int)(((byte)(217)))));
            this.ClientSize = new System.Drawing.Size(662, 621);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmRecuperarContrasena";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmRecuperarContrasena";
            this.Load += new System.EventHandler(this.frmRecuperarContrasena_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnValidarCodigo;
        private System.Windows.Forms.Button btnEnviarCodigo;
        private System.Windows.Forms.Button btnNuevaContraseña;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblCorreo;
        private System.Windows.Forms.TextBox txtNuevaContrasena;
        private System.Windows.Forms.TextBox txtConfirmarContrasena;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.TextBox txtCorreo;
        private System.Windows.Forms.Label lblNombreUsuario;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCerrar;
    }
}