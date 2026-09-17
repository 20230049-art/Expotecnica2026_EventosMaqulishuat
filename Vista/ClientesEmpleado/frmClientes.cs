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
using Vista.EmpleadoProducto;
using Vista.Utilidades;

namespace Vista.ClientesEmpleado
{
    public partial class frmClientes : Form
    {
        public frmClientes()
        {
            InitializeComponent();
            DataTable clientes = Clientes.MostrarClientes();
            CargarClientesEnPantalla(clientes);

            Redondeo.RedondearFig(btnAgregar, 6);

            EventosGloblales.ClienteAgregado += RegarcarPanelCliente;
            EventosGloblales.ClienteActualizado += RegarcarPanelCliente;
        }

        private void RegarcarPanelCliente(object sender, EventArgs e)
        {
            DataTable clientes = Clientes.MostrarClientes();
            CargarClientesEnPantalla(clientes);
        }

        //Muestra los datos de la DB atraves del panel creado

        private void CargarClientesEnPantalla(DataTable clientes)
        {
            flpClientes.Controls.Clear();

            foreach (DataRow fila in clientes.Rows)
            {
                Panel panelCliente = MostrarCliente(fila);
                flpClientes.Controls.Add(panelCliente);
                panelCliente.Click += AbrirFormularioActualizar_Click;
            }
        }

        private Panel MostrarCliente(DataRow fila)
        {

            Panel panelContenedor = new Panel();

            panelContenedor.Width = pnlContenedor.Width;
            panelContenedor.Height = pnlContenedor.Height;
            panelContenedor.BorderStyle = pnlContenedor.BorderStyle;
            panelContenedor.BackColor = Color.FromArgb(253, 241, 217);
            panelContenedor.Tag = fila["IdCliente"];

            Panel panelCliente = new Panel();

            panelCliente.Width = pnlContenedorInfo.Width;
            panelCliente.Height = pnlContenedorInfo.Height;
            panelCliente.BackColor = pnlContenedorInfo.BackColor;
            panelCliente.BorderStyle = pnlContenedorInfo.BorderStyle;
            panelCliente.Location = pnlContenedorInfo.Location;
            Redondeo.RedondearFig(panelCliente, 11);
            panelCliente.Tag = fila["IdCliente"];
            

            Panel panelCompra = new Panel();

            panelCompra.Width = pnlAgregarCompra.Width;
            panelCompra.Height = pnlAgregarCompra.Height;
            panelCompra.BackColor = pnlAgregarCompra.BackColor;
            panelCompra.BorderStyle = pnlAgregarCompra.BorderStyle;
            panelCompra.Location = pnlAgregarCompra.Location;
            Redondeo.RedondearFig(panelCompra, 9);

            Label lblTituloNombre = new Label();

            lblTituloNombre.Text = "Nombre".ToString();
            lblTituloNombre.Font = new Font("Book Antiqua", 22, FontStyle.Bold);
            lblTituloNombre.ForeColor = Color.FromArgb(64, 6, 6);
            lblTituloNombre.Location = new Point(28, 21);
            lblTituloNombre.AutoSize = true;

            Label lblNombre = new Label();

            lblNombre.Text = fila["NombreCliente"].ToString();
            lblNombre.Font = new Font("Book Antiqua", 20, FontStyle.Regular);
            lblNombre.Location = new Point(28, 52);
            lblNombre.AutoSize = true;

            Label lblApellido = new Label();

            lblApellido.Text = fila["ApellidoCliente"].ToString();
            lblApellido.Font = new Font("Book Antiqua", 20, FontStyle.Regular);
            lblApellido.Location = new Point(28, 84);
            lblApellido.AutoSize = true;

            Label lblDocumento = new Label();
            lblDocumento.Text = "DUI: " + fila["DUICliente"].ToString();
            lblDocumento.Font = new Font("Book Antiqua", 21, FontStyle.Regular);
            lblDocumento.Location = new Point(324, 30);
            lblDocumento.AutoSize = true;

            Label lblTipoCliente = new Label();
            lblTipoCliente.Text = "Tipo de Cliente: " + fila["TipoCliente"].ToString();
            lblTipoCliente.Font = new Font("Book Antiqua", 21, FontStyle.Regular);
            lblTipoCliente.Location = new Point(324, 72);
            lblTipoCliente.AutoSize = true;

            Label lblContacto = new Label();
            lblContacto.Text = "Contacto".ToString();
            lblContacto.Font = new Font("Book Antiqua", 22, FontStyle.Bold);
            lblContacto.ForeColor = Color.FromArgb(64, 6, 6);
            lblContacto.Location = new Point(674, 21);
            lblContacto.AutoSize = true;

            Label lblTelefono = new Label();
            lblTelefono.Text = fila["TelefonoCliente"].ToString();
            lblTelefono.Font = new Font("Book Antiqua", 19, FontStyle.Regular);
            lblTelefono.Location = new Point(674, 52);
            lblTelefono.AutoSize = true;

            Label lblCorreo = new Label();
            lblCorreo.Text = fila["CorreoCliente"].ToString();
            lblCorreo.Font = new Font("Book Antiqua", 19, FontStyle.Regular);
            lblCorreo.Location = new Point(674, 83);
            lblCorreo.MaximumSize = new Size(400, 0);
            lblCorreo.AutoSize = true;

            Panel pnlDecoracion = new Panel();
            pnlDecoracion.Size = new Size(1, 105);
            pnlDecoracion.Location = new Point(310, 18);
            pnlDecoracion.BackColor = Color.Black;

            Panel pnlDecoracion1 = new Panel();
            pnlDecoracion1.Size = new Size(1, 105);
            pnlDecoracion1.Location = new Point(654, 18);
            pnlDecoracion1.BackColor = Color.Black;

            Button btnCompra = new Button();
            btnCompra.BackColor = Color.SandyBrown;
            btnCompra.Size = new Size(124, 55);
            btnCompra.MaximumSize = new Size(124, 65);
            btnCompra.Text = "AGREGAR COMPRA ".ToString();
            btnCompra.Font = new Font("Book Antiqua", 11, FontStyle.Bold);
            btnCompra.Location = new Point(10, 76);
            btnCompra.FlatStyle = FlatStyle.Flat;
            btnCompra.FlatAppearance.BorderSize = 0;
            btnCompra.AutoSize = true;
            btnCompra.Tag = fila["IdCliente"];
            btnCompra.Click += BtnCompra_Click;

            PictureBox pbLogo = new PictureBox();
            pbLogo.Location = new Point(38, 12);
            pbLogo.Size = pbImg.Size;
            pbLogo.Image = pbImg.Image;

            panelContenedor.Controls.Add(panelCliente);
            panelContenedor.Controls.Add(panelCompra);
            panelCliente.Controls.Add(lblTituloNombre);
            panelCliente.Controls.Add(lblNombre);
            panelCliente.Controls.Add(lblApellido);
            panelCliente.Controls.Add(lblDocumento);
            panelCliente.Controls.Add(lblTipoCliente);
            panelCliente.Controls.Add(lblContacto);
            panelCliente.Controls.Add(lblTelefono);
            panelCliente.Controls.Add(lblCorreo);
            panelCliente.Controls.Add(pnlDecoracion);
            panelCliente.Controls.Add(pnlDecoracion1);

            panelCompra.Controls.Add(btnCompra);
            panelCompra.Controls.Add(pbLogo);

            flpClientes.Controls.Add(panelContenedor);

            return panelContenedor;
        }

        private void abrirfrmFondoNegro_Click(object sender, EventArgs e)
        {
            frmFondoNegro abrir = new frmFondoNegro();
            abrir.ShowDialog();
        }
        private void AbrirFormularioActualizar_Click(object sender, EventArgs e)
        {
            Panel panelCliente = (Panel)sender;
            int idCliente = Convert.ToInt32(panelCliente.Tag);

            frmFondoNegro fondo = new frmFondoNegro();
            fondo.StartPosition = FormStartPosition.CenterParent;
            fondo.WindowState = FormWindowState.Maximized; 
            fondo.Show();

            frmActualizarInfo formularioActualizar = new frmActualizarInfo(idCliente);
            formularioActualizar.StartPosition = FormStartPosition.CenterParent;

            formularioActualizar.ShowDialog();
            formularioActualizar.BringToFront();

            fondo.Close();
        }

        private void BtnCompra_Click(object sender, EventArgs e)
        {
            Button btnCompra = (Button)sender;
            int idCliente = Convert.ToInt32(btnCompra.Tag);

            DataTable dtCliente = Clientes.ObtenerClienteId(idCliente);
            string nombreCliente = "";

            if (dtCliente.Rows.Count > 0)
            {
                nombreCliente = dtCliente.Rows[0]["NombreCliente"].ToString()
                + ""
                + dtCliente.Rows[0]["ApellidoCliente"].ToString();
            }
            try
            {
                DateTime fechaUso = DateTime.Now.AddDays(1);
                int idTipoPago = 1;

                Venta nuevaVenta = new Venta();

                int idVenta = nuevaVenta.IngresarVenta(CuentaAbierta.IdEmpleado,
                    idCliente, fechaUso, idTipoPago);

                if (idVenta > 0)
                {
                    MessageBox.Show($"Venta iniciada para: {nombreCliente}", "Venta Creada",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                    frmProductos abrir = new frmProductos(idVenta, nombreCliente);
                    abrir.ShowDialog();
                    abrir.ShowIcon = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar venta: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtBuscar_TextChanged_1(object sender, EventArgs e)
        {
            string busqueda = txtBuscar.Text.Trim();
            Clientes clientes = new Clientes();
            DataTable clientesFiltrados = clientes.BuscarCliente(busqueda);
            CargarClientesEnPantalla(clientesFiltrados);
        }

        private void frmClientes_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            EventosGloblales.ClienteAgregado -= RegarcarPanelCliente;
            EventosGloblales.ClienteActualizado -= RegarcarPanelCliente;
        }

        private void pnlContenedorInfo_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
