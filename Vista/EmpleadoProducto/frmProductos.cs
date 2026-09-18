using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vista.ClientesEmpleado;
using Vista.EmpleadosVenta;

namespace Vista.EmpleadoProducto
{
    public partial class frmProductos : Form
    {
        private int idVentaActiva;
        private string nombreCliente;
        private Button btnCarrito;
        private Panel pnlContenedor;
        private FlowLayoutPanel flpProducto;

        public frmProductos(int idVenta, string nombreCliente)
        {
            InitializeComponent();
            this.idVentaActiva = idVenta;
            this.nombreCliente = nombreCliente;
            MostrarProductoEmpleado();
            BotonVenta();
        }

        public frmProductos()
        {
            InitializeComponent();
            this.idVentaActiva = 0;
            this.nombreCliente = "";
            MostrarProductoEmpleado();
        }

        private void BotonVenta()
        {
            if (idVentaActiva <= 0)
            {
                return;
            }

            btnCarrito = new Button();
            btnCarrito.Text = "Ver Compra";
            btnCarrito.Font = new Font("Book Antiqua", 14, FontStyle.Bold);
            btnCarrito.BackColor = Color.FromArgb(76, 175, 80);
            btnCarrito.ForeColor = Color.White;
            btnCarrito.FlatStyle = FlatStyle.Flat;
            btnCarrito.FlatAppearance.BorderSize = 0;
            btnCarrito.Size = new Size(160, 45);
            btnCarrito.Location = new Point(this.ClientSize.Width - 180, 15);
            btnCarrito.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCarrito.Click += BtnCarrito_Click;

            btnCarrito.MouseEnter += (s, e) => btnCarrito.BackColor = Color.FromArgb(56, 142, 60);
            btnCarrito.MouseLeave += (s, e) => btnCarrito.BackColor = Color.FromArgb(76, 175, 80);

            this.Controls.Add(btnCarrito);
            btnCarrito.BringToFront();
        }

        private void BtnCarrito_Click(object sender, EventArgs e)
        {
            if (idVentaActiva <= 0)
            {
                MessageBox.Show("No hay una venta activa.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            frmCompraFinal frmCompraFinal = new frmCompraFinal(idVentaActiva, nombreCliente);
            frmCompraFinal.ShowDialog();
        }



        private void MostrarProductoEmpleado()
        {
            flpProductos.Controls.Clear();

            DataTable producto = Productos.MostrarProductosEmpleado();

            foreach (DataRow fila in producto.Rows)
            {
                Panel panelProductos = PanelProducto(fila);
                panelProductos.Anchor = AnchorStyles.Left | AnchorStyles.Right;

                flpProductos.Controls.Add(panelProductos);
            }
        }

        private void btnCompra_Click(object sender, EventArgs e)
        {
            Button panelProducto = (Button)sender;
            int idProducto = Convert.ToInt32(panelProducto.Tag);

            if (idVentaActiva <= 0)
            {
                DialogResult result = MessageBox.Show("Actualmente no cuenta con un cliente seleccionado. ¿Desea seleccionar un cliente para iniciar una nueva venta?",
                    "Venta no inicia", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    frmFondoNegro fondo = new frmFondoNegro();
                    fondo.StartPosition = FormStartPosition.CenterParent;
                    fondo.WindowState = FormWindowState.Maximized;
                    fondo.Show();

                    frmBuscarCliente frmBuscar = new frmBuscarCliente();
                    frmBuscar.StartPosition = FormStartPosition.CenterParent;
           
                    frmBuscar.ShowDialog();
                    frmBuscar.BringToFront();

                    fondo.Close();
                }
                return;
            }

            frmProductoDetalle abrir = new frmProductoDetalle(idProducto, idVentaActiva);
            abrir.ShowDialog();
        }

        private Panel PanelProducto(DataRow fila)
        {
            Panel panelProductos = new Panel();

            panelProductos.Width = pnlPlantilla.Width;
            panelProductos.Height = pnlPlantilla.Height;
            panelProductos.BorderStyle = pnlPlantilla.BorderStyle;
            panelProductos.BackColor = Color.FromArgb(253, 241, 217);

            PictureBox pbProducto = new PictureBox();
            pbProducto.Width = 110;
            pbProducto.Height = 98;
            pbProducto.Location = new Point(38, 14);
            pbProducto.BorderStyle = BorderStyle.None;
            pbProducto.SizeMode = PictureBoxSizeMode.StretchImage;
            pbProducto.BackColor = Color.White;
            pbProducto.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;

            flpProductos.Controls.Add(panelProductos);

            Label lblTituloNombre = new Label();
            lblTituloNombre.Text = "Nombre".ToString();
            lblTituloNombre.Font = new Font("Book Antiqua", 21, FontStyle.Bold);
            lblTituloNombre.Location = new Point(168, 22);
            lblTituloNombre.ForeColor = Color.FromArgb(64, 6, 6);
            lblTituloNombre.AutoSize = true;
            lblTituloNombre.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;

            Label lblNombre = new Label();
            lblNombre.Text = fila["NombreProducto"].ToString();
            lblNombre.Font = new Font("Bookman Old Style", 19, FontStyle.Regular);
            lblNombre.Location = new Point(168, 57);
            lblNombre.MaximumSize = new Size(400, 0);
            lblNombre.AutoSize = true;
            lblNombre.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;

            Panel pnlDecoracion = new Panel();
            pnlDecoracion.Size = new Size(1, 88);
            pnlDecoracion.Location = new Point(573, 20);
            pnlDecoracion.BackColor = Color.Black;

            Label lblTituloCantidad = new Label();
            lblTituloCantidad.Text = "Cantidad ".ToString();
            lblTituloCantidad.Font = new Font("Book Antiqua", 19, FontStyle.Bold);
            lblTituloCantidad.Location = new Point(595, 22);
            lblTituloCantidad.ForeColor = Color.FromArgb(64, 6, 6);
            lblTituloCantidad.AutoSize = true;

            Label lblcantidad = new Label();
            lblcantidad.Text = fila["CantidadProducto"].ToString();
            lblcantidad.Font = new Font("Bookman Old Style", 22, FontStyle.Regular);
            lblcantidad.Location = new Point(604, 50);
            lblcantidad.AutoSize = true;

            Panel pnlDecoracion2 = new Panel();
            pnlDecoracion2.Size = new Size(1, 88);
            pnlDecoracion2.Location = new Point(742, 20);
            pnlDecoracion2.BackColor = Color.Black;

            Label lblTituloPrecio = new Label();
            lblTituloPrecio.Text = "Precio ".ToString();
            lblTituloPrecio.Font = new Font("Book Antiqua", 19, FontStyle.Bold);
            lblTituloPrecio.Location = new Point(765, 22);
            lblTituloPrecio.ForeColor = Color.FromArgb(64, 6, 6);
            lblTituloPrecio.AutoSize = true;

            Label lblPrecio = new Label();
            decimal precio = Convert.ToDecimal(fila["PrecioAlquilerProducto"]);
            lblPrecio.Text = precio.ToString("F2");
            lblPrecio.Font = new Font("Bookman Old Style", 20, FontStyle.Regular);
            lblPrecio.Location = new Point(765, 57);
            lblPrecio.AutoSize = true;

            Panel pnlDecoracion3 = new Panel();
            pnlDecoracion3.Size = new Size(1, 88);
            pnlDecoracion3.Location = new Point(900, 20);
            pnlDecoracion3.BackColor = Color.Black;

            Panel pnlDecoracion4 = new Panel();
            pnlDecoracion4.Size = new Size(1, 88);
            pnlDecoracion4.Location = new Point(910, 167);
            pnlDecoracion4.BackColor = Color.Black;

            Button btnCompra = new Button();
            btnCompra.Text = "Agregar a la compra ".ToString();
            btnCompra.Font = new Font("Bookman Old Style", 17, FontStyle.Regular);
            btnCompra.Location = new Point(935, 48);
            btnCompra.BackColor = Color.FromArgb(255, 170, 96);
            btnCompra.AutoSize = true;
            btnCompra.Tag = fila["IdProducto"];
            btnCompra.TabIndex = 3;
            btnCompra.Click += btnCompra_Click;

            Panel pnlDecoracion1 = new Panel();
            pnlDecoracion1.Size = new Size(1620, 1);
            pnlDecoracion1.Location = new Point(18, 150);
            pnlDecoracion1.BackColor = Color.Black;

            panelProductos.Controls.Add(pbProducto);
            panelProductos.Controls.Add(lblTituloNombre);
            panelProductos.Controls.Add(lblNombre);
            panelProductos.Controls.Add(lblTituloCantidad);
            panelProductos.Controls.Add(lblcantidad);
            panelProductos.Controls.Add(pnlDecoracion);
            panelProductos.Controls.Add(pnlDecoracion2);
            panelProductos.Controls.Add(lblTituloPrecio);
            panelProductos.Controls.Add(lblPrecio);
            panelProductos.Controls.Add(pnlDecoracion4);
            panelProductos.Controls.Add(pnlDecoracion3);
            panelProductos.Controls.Add(btnCompra);
            panelProductos.Controls.Add(pnlDecoracion1);

            return panelProductos;
        }

        private void txtBuscar_TextChanged_1(object sender, EventArgs e)
        {
            string busqueda = txtBuscar.Text.Trim();
            Productos productos = new Productos();
            DataTable productosFiltrados = productos.BuscarProductoEmpleado(busqueda);
            MostrarProductosEmpleado(productosFiltrados);
        }

        private void MostrarProductosEmpleado(DataTable productosFiltrados)
        {
            flpProductos.Controls.Clear();

            if (productosFiltrados == null || productosFiltrados.Rows.Count == 0)
            {
                MessageBox.Show("No se encontraron productos que coincidan con la búsqueda.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information
                );

                return;
            }

            foreach (DataRow fila in productosFiltrados.Rows)
            {
                Panel panelProducto = PanelProducto(fila);

                flpProductos.Controls.Add(panelProducto);
            }
        }
    }
}
