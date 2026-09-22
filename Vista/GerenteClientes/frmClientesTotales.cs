using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vista.ClientesEmpleado;
using Vista.Utilidades;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Forms.Button;
using ToolTip = System.Windows.Forms.ToolTip;

namespace Vista.GerenteClientes
{
    public partial class frmClientesTotales : Form
    {
        public frmClientesTotales()
        {
            try
            {
                InitializeComponent();

                Redondeo.RedondearFig(txtBarraBuscar, 6);
                Redondeo.RedondearFig(btnClientesTotales, 7);
                Redondeo.RedondearFig(btnClientesNoActivos, 7);
                Redondeo.RedondearFig(btnClientesActivos, 7);
                Redondeo.RedondearFig(btnEliminar, 1);
                Redondeo.RedondearFig(btnAgregar, 1);

                DataTable clientes = Clientes.MostrarClientes();
                CargarClientesEnPantalla(clientes);

                EventosGloblales.ClienteAgregado += RegarcarPanelCliente;
                EventosGloblales.ClienteActualizado += RegarcarPanelCliente;
                EventosGloblales.ClienteEliminado += RegarcarPanelCliente;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                frmClientesAgregar abrir = new frmClientesAgregar();
                abrir.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el formulario de agregar cliente: " + ex.Message,
                        "ERROR-CAMBIO-103", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RestaurarBotones()
        {
            try
            {
                btnClientesActivos.BackColor = Color.FromArgb(244, 220, 197);
                btnClientesTotales.BackColor = Color.FromArgb(244, 220, 197);
                btnClientesNoActivos.BackColor = Color.FromArgb(244, 220, 197);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al restaurar botones: " + ex.Message);
            }
        }

        private void ActivarBoton(Button botones)
        {
            try
            {
                if (botones == null) return;

                RestaurarBotones();
                botones.BackColor = Color.FromArgb(237, 180, 141);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al activar botón: " + ex.Message);
            }
        }

        private void ColorPanelCliente(Panel panelCliente, bool estadoCliente)
        {
            try
            {
                if (panelCliente == null) return;

                if (estadoCliente)
                {
                    panelCliente.BackColor = pnlContenedorInfo.BackColor;
                }
                else
                {
                    panelCliente.BackColor = Color.FromArgb(185, 171, 160);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al colorear panel: " + ex.Message);
            }
        }

        private void RegarcarPanelCliente(object sender, EventArgs e)
        {
            try
            {
                RecargarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al recargar los clientes: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RecargarClientes()
        {
            try
            {
                DataTable clientes = null;

                if (btnClientesTotales.BackColor == Color.FromArgb(237, 180, 141))
                {
                    clientes = Clientes.MostrarClientes();
                }
                else if (btnClientesActivos.BackColor == Color.FromArgb(237, 180, 141))
                {
                    clientes = Clientes.MostrarClientesActivos();
                }
                else if (btnClientesNoActivos.BackColor == Color.FromArgb(237, 180, 141))
                {
                    clientes = Clientes.MostrarClienteInactivos();
                }
                else
                {
                    clientes = Clientes.MostrarClientes();
                }

                CargarClientesEnPantalla(clientes);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al recargar la lista de clientes: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClientesTotales_Click(object sender, EventArgs e)
        {
            try
            {
                ActivarBoton(btnClientesTotales);
                DataTable clientes = Clientes.MostrarClientes();
                CargarClientesEnPantalla(clientes);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar todos los clientes: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClientesActivos_Click(object sender, EventArgs e)
        {
            try
            {
                ActivarBoton(btnClientesActivos);
                DataTable clientes = Clientes.MostrarClientesActivos();
                CargarClientesEnPantalla(clientes);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los clientes activos: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClientesNoActivos_Click(object sender, EventArgs e)
        {
            try
            {
                ActivarBoton(btnClientesNoActivos);
                DataTable clientes = Clientes.MostrarClienteInactivos();
                CargarClientesEnPantalla(clientes);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los clientes inactivos: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarClientesEnPantalla(DataTable clientes)
        {
            try
            {
                flpClientes.Controls.Clear();

                if (clientes == null || clientes.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron clientes que coincidan con la búsqueda.",
                            "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (DataRow fila in clientes.Rows)
                {
                    Panel panelCliente = MostrarClientes(fila);

                    if (panelCliente == null)
                    {
                        continue;
                    }

                    panelCliente.Click += AbrirfrmActualizar_Click;

                    flpClientes.Controls.Add(panelCliente);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar los clientes en pantalla: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel MostrarClientes(DataRow fila)
        {
            try
            {
                if (fila == null)
                {
                    return null;
                }

                Panel panelCliente = new Panel();

                panelCliente.Width = pnlContenedorInfo.Width;
                panelCliente.Height = pnlContenedorInfo.Height;
                panelCliente.BorderStyle = pnlContenedorInfo.BorderStyle;
                panelCliente.Tag = fila["IdCliente"];
                Redondeo.RedondearFig(panelCliente, 10);

                bool estadoCliente = Convert.ToBoolean(fila["EstadoCliente"]);
                ColorPanelCliente(panelCliente, estadoCliente);

                Label lblNombre = new Label();

                lblNombre.Text = "Nombre: " + fila["NombreCliente"].ToString();
                lblNombre.Font = new Font("Book Antiqua", 21, FontStyle.Regular);
                lblNombre.Location = new Point(17, 28);
                lblNombre.AutoSize = true;

                Label lblApellido = new Label();

                lblApellido.Text = "Apellido: " + fila["ApellidoCliente"].ToString();
                lblApellido.Font = new Font("Book Antiqua", 21, FontStyle.Regular);
                lblApellido.Location = new Point(17, 69);
                lblApellido.AutoSize = true;

                Label lblDocumento = new Label();
                lblDocumento.Text = "DUI: " + fila["DUICliente"].ToString();
                lblDocumento.Font = new Font("Book Antiqua", 21, FontStyle.Regular);
                lblDocumento.Location = new Point(429, 28);
                lblDocumento.AutoSize = true;

                Label lblTipoCliente = new Label();
                lblTipoCliente.Text = "Tipo de Cliente: " + fila["TipoCliente"].ToString();
                lblTipoCliente.Font = new Font("Book Antiqua", 21, FontStyle.Regular);
                lblTipoCliente.Location = new Point(429, 69);
                lblTipoCliente.AutoSize = true;

                Label lblTelefono = new Label();

                lblTelefono.Text = "Teléfono: " + fila["TelefonoCliente"].ToString();
                lblTelefono.Font = new Font("Book Antiqua", 21, FontStyle.Regular);
                lblTelefono.Location = new Point(781, 69);
                lblTelefono.AutoSize = true;

                Label lblCorreo = new Label();
                lblCorreo.Text = "Correo: " + fila["CorreoCliente"].ToString();
                lblCorreo.Font = new Font("Book Antiqua", 21, FontStyle.Regular);
                lblCorreo.Location = new Point(781, 28);
                lblCorreo.AutoSize = true;

                Panel pnlDecoracion = new Panel();
                pnlDecoracion.Size = new Size(1, 98);
                pnlDecoracion.Location = new Point(416, 17);
                pnlDecoracion.BackColor = Color.Black;

                Panel pnlDecoracion1 = new Panel();
                pnlDecoracion1.Size = new Size(1, 98);
                pnlDecoracion1.Location = new Point(759, 17);
                pnlDecoracion1.BackColor = Color.Black;

                panelCliente.Controls.Add(lblNombre);
                panelCliente.Controls.Add(lblApellido);
                panelCliente.Controls.Add(lblDocumento);
                panelCliente.Controls.Add(lblTipoCliente);
                panelCliente.Controls.Add(lblTelefono);
                panelCliente.Controls.Add(lblCorreo);
                panelCliente.Controls.Add(pnlDecoracion);
                panelCliente.Controls.Add(pnlDecoracion1);

                return panelCliente;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Una columna esperada no existe en los datos del cliente: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el panel del cliente: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void AbrirfrmActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                Panel panelCliente = sender as Panel;

                if (panelCliente == null || panelCliente.Tag == null)
                {
                    MessageBox.Show("No se pudo identificar el cliente seleccionado.",
                            "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int idCliente = Convert.ToInt32(panelCliente.Tag);

                frmActualizarInfo abrir = new frmActualizarInfo(idCliente);
                abrir.ShowDialog();
            }
            catch (FormatException ex)
            {
                MessageBox.Show("El identificador del cliente no tiene el formato correcto: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el formulario de actualización: " + ex.Message,
                        "ERROR-CAMBIO-103", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmClientesTotales_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                EventosGloblales.ClienteAgregado -= RegarcarPanelCliente;
                EventosGloblales.ClienteActualizado -= RegarcarPanelCliente;
                EventosGloblales.ClienteEliminado -= RegarcarPanelCliente;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al desuscribir eventos: " + ex.Message);
            }
        }

        private void txtBarraBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string busqueda = txtBarraBuscar.Text.Trim();

                if (string.IsNullOrWhiteSpace(busqueda))
                {
                    RecargarClientes();
                    return;
                }

                Clientes clientes = new Clientes();
                DataTable clientesFiltrados = clientes.BuscarCliente(busqueda);
                CargarClientesEnPantalla(clientesFiltrados);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al realizar la búsqueda de clientes: " + ex.Message,
                        "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
