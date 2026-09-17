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

namespace Vista.EmpleadosVenta
{
    public partial class frmProductoDetalle : Form
    {
        public frmProductoDetalle()
        {
            InitializeComponent();
            MostrarCliente();
        }

        private void MostrarCliente()
        {
            flpVistaClientes.Controls.Clear();

            DataTable clientes = Clientes.MostrarClientes();

            foreach (DataRow fila in clientes.Rows)
            {

                Panel panelContenedor = new Panel();

                panelContenedor.Width = pnlContenedor.Width;
                panelContenedor.Height = pnlContenedor.Height;
                panelContenedor.BorderStyle = pnlContenedor.BorderStyle;
                panelContenedor.BackColor = Color.FromArgb(253, 241, 217);

                Panel panelCliente = new Panel();

                panelCliente.Width = pnlContenedorInfo.Width;
                panelCliente.Height = pnlContenedorInfo.Height;
                panelCliente.BackColor = pnlContenedorInfo.BackColor;
                panelCliente.BorderStyle = pnlContenedorInfo.BorderStyle;
                panelCliente.Location = pnlContenedorInfo.Location;
                panelCliente.Tag = fila["IdCliente"];

                Panel panelCompra = new Panel();

                panelCompra.Width = pnlAgregarCompra.Width;
                panelCompra.Height = pnlAgregarCompra.Height;
                panelCompra.BackColor = pnlAgregarCompra.BackColor;
                panelCompra.BorderStyle = pnlAgregarCompra.BorderStyle;
                panelCompra.Location = pnlAgregarCompra.Location;

                Label lblTituloNombre = new Label();

                lblTituloNombre.Text = "Nombre".ToString();
                lblTituloNombre.Font = new Font("Book Antiqua", 20, FontStyle.Bold);
                lblTituloNombre.ForeColor = Color.FromArgb(64, 6, 6);
                lblTituloNombre.Location = new Point(18, 13);
                lblTituloNombre.AutoSize = true;

                Label lblNombre = new Label();

                lblNombre.Text = fila["NombreCliente"].ToString();
                lblNombre.Font = new Font("Book Antiqua", 18, FontStyle.Regular);
                lblNombre.Location = new Point(18, 42);
                lblNombre.AutoSize = true;

                Label lblApellido = new Label();

                lblApellido.Text = fila["ApellidoCliente"].ToString();
                lblApellido.Font = new Font("Book Antiqua", 18, FontStyle.Regular);
                lblApellido.Location = new Point(18, 70);
                lblApellido.AutoSize = true;

                Label lblDocumento = new Label();
                lblDocumento.Text = "DUI: " + fila["DUICliente"].ToString();
                lblDocumento.Font = new Font("Book Antiqua", 18, FontStyle.Regular);
                lblDocumento.Location = new Point(256, 20);
                lblDocumento.AutoSize = true;

                Label lblTituloTipoCliente = new Label();
                lblTituloTipoCliente.Text = "Tipo de Cliente: ".ToString();
                lblTituloTipoCliente.Font = new Font("Book Antiqua", 18, FontStyle.Regular);
                lblTituloTipoCliente.Location = new Point(256, 48);
                lblTituloTipoCliente.AutoSize = true;

                Label lblTipoCliente = new Label();
                lblTipoCliente.Text = fila["TipoCliente"].ToString();
                lblTipoCliente.Font = new Font("Book Antiqua", 18, FontStyle.Regular);
                lblTipoCliente.Location = new Point(257, 73);
                lblTipoCliente.AutoSize = true;

                Label lblContacto = new Label();
                lblContacto.Text = "Contacto".ToString();
                lblContacto.Font = new Font("Book Antiqua", 19, FontStyle.Bold);
                lblContacto.ForeColor = Color.FromArgb(64, 6, 6);
                lblContacto.Location = new Point(479, 10);
                lblContacto.AutoSize = true;

                Label lblTelefono = new Label();
                lblTelefono.Text = fila["TelefonoCliente"].ToString();
                lblTelefono.Font = new Font("Book Antiqua", 16, FontStyle.Regular);
                lblTelefono.Location = new Point(482, 38);
                lblTelefono.AutoSize = true;

                Label lblCorreo = new Label();
                lblCorreo.Text = fila["CorreoCliente"].ToString();
                lblCorreo.Font = new Font("Book Antiqua", 16, FontStyle.Regular);
                lblCorreo.Location = new Point(482, 59);
                lblCorreo.MaximumSize = new Size(262, 0);
                lblCorreo.AutoSize = true;

                Panel pnlDecoracion = new Panel();
                pnlDecoracion.Size = new Size(1, 80);
                pnlDecoracion.Location = new Point(248, 18);
                pnlDecoracion.BackColor = Color.Black;

                Panel pnlDecoracion1 = new Panel();
                pnlDecoracion1.Size = new Size(1, 80);
                pnlDecoracion1.Location = new Point(468, 18);
                pnlDecoracion1.BackColor = Color.Black;

                panelContenedor.Controls.Add(panelCliente);
                panelContenedor.Controls.Add(panelCompra);
                panelCliente.Controls.Add(lblTituloNombre);
                panelCliente.Controls.Add(lblNombre);
                panelCliente.Controls.Add(lblApellido);
                panelCliente.Controls.Add(lblDocumento);
                panelCliente.Controls.Add(lblTituloTipoCliente);
                panelCliente.Controls.Add(lblTipoCliente);
                panelCliente.Controls.Add(lblContacto);
                panelCliente.Controls.Add(lblTelefono);
                panelCliente.Controls.Add(lblCorreo);
                panelCliente.Controls.Add(pnlDecoracion);
                panelCliente.Controls.Add(pnlDecoracion1);

                flpVistaClientes.Controls.Add(panelContenedor);
            }
        }
    }
}
