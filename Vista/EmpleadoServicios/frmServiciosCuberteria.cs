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

namespace Vista.EmpleadoServicios
{
    public partial class frmServiciosCuberteria : Form
    {
        public frmServiciosCuberteria()
        {
            try
            {
                InitializeComponent();
                MostrarServicio();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Cambio de formularios
        private Form activeForm = null;
        private void abrirForm(Form formularioAbrir)
        {
            try
            {
                if (activeForm != null && activeForm.GetType() == formularioAbrir.GetType())
            {
                return;
            }

            if (activeForm != null)
            {
                activeForm.Close();
                pnlVistaCuberteria.Controls.Remove(activeForm);
                activeForm.Dispose();
            }

            activeForm = formularioAbrir;
            formularioAbrir.TopLevel = false;
            formularioAbrir.FormBorderStyle = FormBorderStyle.None;
            formularioAbrir.Dock = DockStyle.Fill;

            pnlVistaCuberteria.Controls.Add(formularioAbrir);
            formularioAbrir.BringToFront();
            formularioAbrir.Show();
             }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el formulario: " + ex.Message, "ERROR-CAMBIO-103",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
}

        private void pbRegresar_Click(object sender, EventArgs e)
        {
            try
            {
                abrirForm(new frmServicios());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al regresar al formulario anterior: " + ex.Message, "ERROR-NAVANTERIOR-104",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarServicio()
        {
            try
            {
                DataTable productos = Productos.VistaEmpleadoServicios(3);

                if (productos == null || productos.Rows.Count == 0)
                {
                    flpServicio.Controls.Clear();
                    MessageBox.Show("No hay productos disponibles para este servicio.", "INFO-SERVICIOS-06",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                MostrarProductos(productos);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los productos del servicio: " + ex.Message,
                                "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarProductos(DataTable productos)
        {
            try { 
            flpServicio.Controls.Clear();

            foreach (DataRow fila in productos.Rows)
            {
                Panel panelPrincipal = new Panel();

                panelPrincipal.Width = pnlPlantilla.Width;
                panelPrincipal.Height = pnlPlantilla.Height;
                panelPrincipal.BackColor = Color.FromArgb(253, 241, 217);
                panelPrincipal.BorderStyle = pnlPlantilla.BorderStyle;
                panelPrincipal.Margin = new Padding(10);

                PictureBox pbProducto = new PictureBox();
                pbProducto.Width = 110;
                pbProducto.Height = 98;
                pbProducto.Location = new Point(38, 14);
                pbProducto.BorderStyle = BorderStyle.None;
                pbProducto.SizeMode = PictureBoxSizeMode.StretchImage;
                pbProducto.BackColor = Color.White;

                Label lblTituloNombre = new Label();
                lblTituloNombre.Text = "Nombre".ToString();
                lblTituloNombre.Font = new Font("Book Antiqua", 19, FontStyle.Bold);
                lblTituloNombre.Location = new Point(168, 22);
                lblTituloNombre.ForeColor = Color.FromArgb(64, 6, 6);
                lblTituloNombre.AutoSize = true;

                Label lblNombre = new Label();
                lblNombre.Text = fila["NombreProducto"].ToString();
                lblNombre.Font = new Font("Bookman Old Style", 18, FontStyle.Regular);
                lblNombre.Location = new Point(168, 55);
                lblNombre.MaximumSize = new Size(400, 0);
                lblNombre.AutoSize = true;

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
                btnCompra.Text = "Agregar".ToString();
                btnCompra.Font = new Font("Bookman Old Style", 18, FontStyle.Regular);
                btnCompra.Location = new Point(923, 22);
                btnCompra.BackColor = Color.FromArgb(255, 170, 96);
                btnCompra.Size = new Size(170, 68);
                btnCompra.MaximumSize = new Size(250, 68);
                btnCompra.AutoSize = true;

                panelPrincipal.Controls.Add(pbProducto);
                panelPrincipal.Controls.Add(lblTituloNombre);
                panelPrincipal.Controls.Add(lblNombre);
                panelPrincipal.Controls.Add(lblTituloCantidad);
                panelPrincipal.Controls.Add(lblcantidad);
                panelPrincipal.Controls.Add(pnlDecoracion);
                panelPrincipal.Controls.Add(pnlDecoracion2);
                panelPrincipal.Controls.Add(lblTituloPrecio);
                panelPrincipal.Controls.Add(lblPrecio);
                panelPrincipal.Controls.Add(pnlDecoracion4);
                panelPrincipal.Controls.Add(pnlDecoracion3);
                panelPrincipal.Controls.Add(btnCompra);

                flpServicio.Controls.Add(panelPrincipal);
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar los productos: " + ex.Message, "ERROR-CARGADATOS-008",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string busqueda = txtBuscar.Text.Trim();

                if (string.IsNullOrWhiteSpace(busqueda))
                {
                    MostrarServicio();
                    return;
                }

                DataTable productos = Productos.BuscarProductosPorServicio(3, busqueda);

                if (productos == null || productos.Rows.Count == 0)
                {
                    MostrarProductos(productos);

                    MessageBox.Show("No se encontraron productos que coincidan con la búsqueda.", "ERROR-NODATO-007",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                MostrarProductos(productos);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al intentar buscar los productos: " + ex.Message,
                                "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);

                MostrarProductos(new DataTable());
            }
        }
    }
}
