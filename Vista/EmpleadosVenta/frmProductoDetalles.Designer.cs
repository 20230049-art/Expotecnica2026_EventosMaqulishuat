namespace Vista.EmpleadosVenta
{
    partial class frmProductoDetalles
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
            this.pnlVistaDetalleProducto = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.mtbDescuent = new System.Windows.Forms.MaskedTextBox();
            this.nudCantidad = new System.Windows.Forms.NumericUpDown();
            this.mtbTotal = new System.Windows.Forms.MaskedTextBox();
            this.mtbSubTotal = new System.Windows.Forms.MaskedTextBox();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.btnConfirmar = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlProducto = new System.Windows.Forms.Panel();
            this.lblDBPrecio = new System.Windows.Forms.Label();
            this.lblDBCantidad = new System.Windows.Forms.Label();
            this.lblDBNombreProducto = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label23 = new System.Windows.Forms.Label();
            this.pbProducto = new System.Windows.Forms.PictureBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pnlVistaDetalleProducto.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidad)).BeginInit();
            this.pnlProducto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbProducto)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlVistaDetalleProducto
            // 
            this.pnlVistaDetalleProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(241)))), ((int)(((byte)(217)))));
            this.pnlVistaDetalleProducto.Controls.Add(this.tableLayoutPanel1);
            this.pnlVistaDetalleProducto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlVistaDetalleProducto.Location = new System.Drawing.Point(0, 0);
            this.pnlVistaDetalleProducto.Name = "pnlVistaDetalleProducto";
            this.pnlVistaDetalleProducto.Size = new System.Drawing.Size(1137, 522);
            this.pnlVistaDetalleProducto.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.319261F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 94.635F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.957784F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.pnlProducto, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.173077F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.15789F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.84211F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1137, 522);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.mtbDescuent);
            this.panel2.Controls.Add(this.nudCantidad);
            this.panel2.Controls.Add(this.mtbTotal);
            this.panel2.Controls.Add(this.mtbSubTotal);
            this.panel2.Controls.Add(this.btnCerrar);
            this.panel2.Controls.Add(this.btnConfirmar);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(18, 207);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1070, 312);
            this.panel2.TabIndex = 2;
            // 
            // mtbDescuent
            // 
            this.mtbDescuent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(215)))), ((int)(((byte)(207)))));
            this.mtbDescuent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mtbDescuent.Font = new System.Drawing.Font("Lucida Bright", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mtbDescuent.Location = new System.Drawing.Point(67, 198);
            this.mtbDescuent.Name = "mtbDescuent";
            this.mtbDescuent.Size = new System.Drawing.Size(158, 30);
            this.mtbDescuent.TabIndex = 2;
            this.mtbDescuent.TextChanged += new System.EventHandler(this.mtbDescuent_TextChanged_1);
            this.mtbDescuent.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mtbDescuent_KeyUp_1);
            // 
            // nudCantidad
            // 
            this.nudCantidad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(215)))), ((int)(((byte)(207)))));
            this.nudCantidad.Font = new System.Drawing.Font("Lucida Bright", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudCantidad.Location = new System.Drawing.Point(67, 96);
            this.nudCantidad.Name = "nudCantidad";
            this.nudCantidad.Size = new System.Drawing.Size(158, 33);
            this.nudCantidad.TabIndex = 1;
            this.toolTip1.SetToolTip(this.nudCantidad, "Cantidad del producto para la venta");
            this.nudCantidad.ValueChanged += new System.EventHandler(this.nudCantidad_ValueChanged_1);
            // 
            // mtbTotal
            // 
            this.mtbTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(215)))), ((int)(((byte)(207)))));
            this.mtbTotal.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mtbTotal.Font = new System.Drawing.Font("Lucida Bright", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mtbTotal.Location = new System.Drawing.Point(289, 195);
            this.mtbTotal.Margin = new System.Windows.Forms.Padding(0);
            this.mtbTotal.Name = "mtbTotal";
            this.mtbTotal.ReadOnly = true;
            this.mtbTotal.Size = new System.Drawing.Size(158, 30);
            this.mtbTotal.TabIndex = 0;
            this.toolTip1.SetToolTip(this.mtbTotal, "Total de la venta del producto");
            this.mtbTotal.ValidatingType = typeof(int);
            // 
            // mtbSubTotal
            // 
            this.mtbSubTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(215)))), ((int)(((byte)(207)))));
            this.mtbSubTotal.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mtbSubTotal.Font = new System.Drawing.Font("Lucida Bright", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mtbSubTotal.Location = new System.Drawing.Point(289, 97);
            this.mtbSubTotal.Margin = new System.Windows.Forms.Padding(0);
            this.mtbSubTotal.Name = "mtbSubTotal";
            this.mtbSubTotal.ReadOnly = true;
            this.mtbSubTotal.Size = new System.Drawing.Size(158, 30);
            this.mtbSubTotal.TabIndex = 0;
            this.toolTip1.SetToolTip(this.mtbSubTotal, "Subtotal de la venta del producto");
            this.mtbSubTotal.ValidatingType = typeof(int);
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackColor = System.Drawing.Color.Silver;
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Bookman Old Style", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(10)))), ((int)(((byte)(4)))));
            this.btnCerrar.Location = new System.Drawing.Point(543, 156);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(432, 51);
            this.btnCerrar.TabIndex = 4;
            this.btnCerrar.Text = "Cerrar";
            this.toolTip1.SetToolTip(this.btnCerrar, "Cancelar Producto");
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // btnConfirmar
            // 
            this.btnConfirmar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(201)))), ((int)(((byte)(109)))));
            this.btnConfirmar.FlatAppearance.BorderSize = 0;
            this.btnConfirmar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmar.Font = new System.Drawing.Font("Bookman Old Style", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirmar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(10)))), ((int)(((byte)(4)))));
            this.btnConfirmar.Location = new System.Drawing.Point(543, 85);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(432, 51);
            this.btnConfirmar.TabIndex = 3;
            this.btnConfirmar.Text = "Confirmar";
            this.toolTip1.SetToolTip(this.btnConfirmar, "Confirmar producto");
            this.btnConfirmar.UseVisualStyleBackColor = false;
            this.btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Book Antiqua", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(53)))), ((int)(((byte)(39)))));
            this.label3.Location = new System.Drawing.Point(297, 148);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.label3.Size = new System.Drawing.Size(95, 47);
            this.label3.TabIndex = 32;
            this.label3.Text = "Total:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Book Antiqua", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(53)))), ((int)(((byte)(39)))));
            this.label4.Location = new System.Drawing.Point(298, 50);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.label4.Size = new System.Drawing.Size(149, 47);
            this.label4.TabIndex = 31;
            this.label4.Text = "SubTotal:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Book Antiqua", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(53)))), ((int)(((byte)(39)))));
            this.label2.Location = new System.Drawing.Point(78, 148);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.label2.Size = new System.Drawing.Size(167, 47);
            this.label2.TabIndex = 30;
            this.label2.Text = "Descuento:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Book Antiqua", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(53)))), ((int)(((byte)(39)))));
            this.label1.Location = new System.Drawing.Point(75, 50);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.label1.Size = new System.Drawing.Size(149, 47);
            this.label1.TabIndex = 29;
            this.label1.Text = "Cantidad:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlProducto
            // 
            this.pnlProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(241)))), ((int)(((byte)(217)))));
            this.pnlProducto.Controls.Add(this.pbProducto);
            this.pnlProducto.Controls.Add(this.panel1);
            this.pnlProducto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProducto.Location = new System.Drawing.Point(18, 45);
            this.pnlProducto.Name = "pnlProducto";
            this.pnlProducto.Size = new System.Drawing.Size(1070, 156);
            this.pnlProducto.TabIndex = 3;
            // 
            // lblDBPrecio
            // 
            this.lblDBPrecio.AutoSize = true;
            this.lblDBPrecio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(213)))), ((int)(((byte)(191)))));
            this.lblDBPrecio.Font = new System.Drawing.Font("Bookman Old Style", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDBPrecio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(23)))), ((int)(((byte)(19)))));
            this.lblDBPrecio.Location = new System.Drawing.Point(771, 59);
            this.lblDBPrecio.Name = "lblDBPrecio";
            this.lblDBPrecio.Size = new System.Drawing.Size(74, 37);
            this.lblDBPrecio.TabIndex = 19;
            this.lblDBPrecio.Text = "150";
            // 
            // lblDBCantidad
            // 
            this.lblDBCantidad.AutoSize = true;
            this.lblDBCantidad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(213)))), ((int)(((byte)(191)))));
            this.lblDBCantidad.Font = new System.Drawing.Font("Bookman Old Style", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDBCantidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(23)))), ((int)(((byte)(19)))));
            this.lblDBCantidad.Location = new System.Drawing.Point(525, 59);
            this.lblDBCantidad.Name = "lblDBCantidad";
            this.lblDBCantidad.Size = new System.Drawing.Size(74, 37);
            this.lblDBCantidad.TabIndex = 18;
            this.lblDBCantidad.Text = "150";
            // 
            // lblDBNombreProducto
            // 
            this.lblDBNombreProducto.AutoSize = true;
            this.lblDBNombreProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(213)))), ((int)(((byte)(191)))));
            this.lblDBNombreProducto.Font = new System.Drawing.Font("Bookman Old Style", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDBNombreProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(23)))), ((int)(((byte)(19)))));
            this.lblDBNombreProducto.Location = new System.Drawing.Point(146, 52);
            this.lblDBNombreProducto.Name = "lblDBNombreProducto";
            this.lblDBNombreProducto.Size = new System.Drawing.Size(237, 37);
            this.lblDBNombreProducto.TabIndex = 17;
            this.lblDBNombreProducto.Text = "Armando Jose ";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(34)))), ((int)(((byte)(18)))));
            this.panel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(56)))), ((int)(((byte)(33)))));
            this.panel3.Location = new System.Drawing.Point(441, 11);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1, 100);
            this.panel3.TabIndex = 12;
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Book Antiqua", 19.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(53)))), ((int)(((byte)(39)))));
            this.label23.Location = new System.Drawing.Point(146, 16);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.label23.Size = new System.Drawing.Size(146, 51);
            this.label23.TabIndex = 7;
            this.label23.Text = "Nombre:";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbProducto
            // 
            this.pbProducto.Image = global::Vista.Properties.Resources.Persona;
            this.pbProducto.Location = new System.Drawing.Point(47, 26);
            this.pbProducto.Name = "pbProducto";
            this.pbProducto.Size = new System.Drawing.Size(121, 108);
            this.pbProducto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbProducto.TabIndex = 1;
            this.pbProducto.TabStop = false;
            // 
            // label22
            // 
            this.label22.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Book Antiqua", 19.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(53)))), ((int)(((byte)(39)))));
            this.label22.Location = new System.Drawing.Point(480, 8);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.label22.Size = new System.Drawing.Size(162, 51);
            this.label22.TabIndex = 8;
            this.label22.Text = "Cantidad:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Book Antiqua", 19.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(53)))), ((int)(((byte)(39)))));
            this.label17.Location = new System.Drawing.Point(758, 16);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.label17.Size = new System.Drawing.Size(108, 51);
            this.label17.TabIndex = 14;
            this.label17.Text = "Precio";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(214)))), ((int)(((byte)(191)))));
            this.panel1.Controls.Add(this.lblDBPrecio);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.lblDBNombreProducto);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.label23);
            this.panel1.Controls.Add(this.lblDBCantidad);
            this.panel1.Controls.Add(this.label22);
            this.panel1.Location = new System.Drawing.Point(36, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(992, 125);
            this.panel1.TabIndex = 20;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(34)))), ((int)(((byte)(18)))));
            this.panel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(56)))), ((int)(((byte)(33)))));
            this.panel4.Location = new System.Drawing.Point(709, 11);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1, 100);
            this.panel4.TabIndex = 13;
            // 
            // frmProductoDetalles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1137, 522);
            this.Controls.Add(this.pnlVistaDetalleProducto);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(1178, 522);
            this.MinimumSize = new System.Drawing.Size(1137, 522);
            this.Name = "frmProductoDetalles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmProductoDetalles";
            this.Load += new System.EventHandler(this.frmProductoDetalles_Load);
            this.pnlVistaDetalleProducto.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidad)).EndInit();
            this.pnlProducto.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbProducto)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlVistaDetalleProducto;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown nudCantidad;
        private System.Windows.Forms.MaskedTextBox mtbTotal;
        private System.Windows.Forms.MaskedTextBox mtbSubTotal;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Button btnConfirmar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlProducto;
        private System.Windows.Forms.Label lblDBPrecio;
        private System.Windows.Forms.Label lblDBCantidad;
        private System.Windows.Forms.Label lblDBNombreProducto;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.PictureBox pbProducto;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.MaskedTextBox mtbDescuent;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
    }
}