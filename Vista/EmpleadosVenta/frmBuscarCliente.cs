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
using Vista.Utilidades;

namespace Vista.EmpleadosVenta
{
    public partial class frmBuscarCliente : Form
    {
        public frmBuscarCliente()
        {
            InitializeComponent();
            DataTable clientes = Clientes.MostrarClientes();
            CargarClientesEnPantalla(clientes);

            ControlesBloqueo.LimitarTextBox(txtBuscar, 100);
        }

        //Muestra los datos de la DB atraves del panel creado

        private void CargarClientesEnPantalla(DataTable clientes)
        {
            flpVistaClientes.Controls.Clear();

            if (clientes == null || clientes.Rows.Count == 0)
            {
                MessageBox.Show("No se encontraron clientes que coincidan con la búsqueda.", "ERROR-NODATO-007",
                    MessageBoxButtons.OK, MessageBoxIcon.Information
                );

                return;
            }

            foreach (DataRow fila in clientes.Rows)
            {
                Panel panelCliente = MostrarCliente(fila);
                flpVistaClientes.Controls.Add(panelCliente);
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

            flpVistaClientes.Controls.Add(panelContenedor);

            return panelContenedor;
        }

        private void BtnCompra_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnCompra = (Button)sender;

                if (btnCompra == null || btnCompra.Tag == null)
                {
                    MessageBox.Show("No se pudo identificar el cliente seleccionado.",
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int idCliente = Convert.ToInt32(btnCompra.Tag);

                if (CuentaAbierta.IdEmpleado <= 0)
                {
                    MessageBox.Show("No hay un empleado con sesión activa.",
                        "INFO-VENTAEMPLE-02", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataTable dtCliente = Clientes.ObtenerClienteId(idCliente);
                string nombreCliente = "";

                if (dtCliente == null || dtCliente.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontró la información del cliente seleccionado.",
                        "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                nombreCliente = dtCliente.Rows[0]["NombreCliente"].ToString()
                                + " " + dtCliente.Rows[0]["ApellidoCliente"].ToString();

                DateTime fechaUso = DateTime.Now.AddDays(1);
                int idTipoPago = 1;

                Venta nuevaVenta = new Venta();

                int idVenta = nuevaVenta.IngresarVenta(
                    CuentaAbierta.IdEmpleado,
                    idCliente,
                    fechaUso,
                    idTipoPago);

                if (idVenta > 0)
                {
                    VentaActiva.Iniciar(idVenta, nombreCliente);

                    MessageBox.Show($"Venta iniciada para: {nombreCliente}","INFO-VENTA-00", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();

                    //Esta malo
                }
                else
                {
                    MessageBox.Show("No se pudo iniciar la venta. Intente nuevamente.",
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar la venta: " + ex.Message,
                    "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string busqueda = txtBuscar.Text.Trim();
            Clientes clientes = new Clientes();
            DataTable clientesFiltrados = clientes.BuscarCliente(busqueda);
            CargarClientesEnPantalla(clientesFiltrados);
        }
    }
}
