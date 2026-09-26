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
        private bool modoEliminar = false;

        private const int REGISTROS_POR_PAGINA = 20;
        private int paginaActual = 1;
        private int totalPaginas = 1;
        private int totalRegistros = 0;

        private int filtroActual = 1;
        private string busquedaActual = "";

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

                ActivarBoton(btnClientesTotales);
                CargarPagina(1);

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
                frmFondoNegro fondo = new frmFondoNegro();
                fondo.StartPosition = FormStartPosition.CenterParent;
                fondo.WindowState = FormWindowState.Maximized;
                fondo.Show();

                frmClientesAgregar abrir = new frmClientesAgregar();
                abrir.ShowDialog();

                fondo.Close();

                CargarPagina(paginaActual);

                if (totalPaginas > 1 && string.IsNullOrWhiteSpace(busquedaActual))
                {
                    CargarPagina(totalPaginas);
                }
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

                if (modoEliminar)
                {
                    panelCliente.BackColor = Color.FromArgb(220, 110, 110);   
                }
                else if (estadoCliente)
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
                CargarPagina(paginaActual);
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
                filtroActual = 1;
                paginaActual = 1;
                CargarPagina(1);
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
                filtroActual = 2;
                paginaActual = 1;
                CargarPagina(1);
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
                filtroActual = 3;
                paginaActual = 1;
                CargarPagina(1);
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

                    DataRow filaLocal = fila;

                    panelCliente.Click += (s, e) =>
                    {
                        try
                        {
                            if (modoEliminar)
                            {
                                ConfirmarEliminar(filaLocal);
                            }
                            else
                            {
                                AbrirfrmActualizar_Click(s, e);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al procesar el clic del cliente: " + ex.Message,
                                    "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };

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

                frmFondoNegro fondo = new frmFondoNegro();
                fondo.StartPosition = FormStartPosition.CenterParent;
                fondo.WindowState = FormWindowState.Maximized;
                fondo.Show();

                frmActualizarInfo abrir = new frmActualizarInfo(idCliente);
                abrir.ShowDialog();

                fondo.Close();

                CargarPagina(paginaActual);
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
                    paginaActual = 1;
                    CargarPagina(1);
                    return;
                }

                Clientes clientes = new Clientes();
                DataTable clientesFiltrados = clientes.BuscarCliente(busqueda);

                totalRegistros = clientesFiltrados?.Rows.Count ?? 0;
                totalPaginas = 1;
                paginaActual = 1;

                CargarClientesEnPantalla(clientesFiltrados);
                ActualizarControlesPaginacion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al realizar la búsqueda de clientes: " + ex.Message,
                        "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                modoEliminar = true;
                CargarPagina(paginaActual);

                RecargarClientes();

                MessageBox.Show("Selecciona el cliente que deseas eliminar permanentemente.\n\n" + " Esta acción NO se puede deshacer.",
                    "INFO-ELIMINAR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cambiar el modo eliminar: " + ex.Message,
                        "ERROR-CAMBIO-103", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfirmarEliminar(DataRow fila)
        {
            try
            {
                if (fila == null) return;

                int idCliente = Convert.ToInt32(fila["IdCliente"]);
                string nombreCliente = fila["NombreCliente"].ToString() + " " + fila["ApellidoCliente"].ToString();

                DialogResult confirmacion = MessageBox.Show(
                    $"¿Estás seguro de eliminar PERMANENTEMENTE el cliente?\n\n" +
                    $"Cliente: \"{nombreCliente}\"\n\n" +
                    "Esta acción NO se puede deshacer.",
                    "Eliminación Permanente",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (confirmacion == DialogResult.Yes)
                {
                    EliminarPermanentemente(idCliente);
                }
                else
                {
                    modoEliminar = false;
                    CargarPagina(paginaActual);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al confirmar la eliminación permanente: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarPermanentemente(int idCliente)
        {
            try
            {
                bool exito = Clientes.EliminarCliente(idCliente);

                if (exito)
                {
                    MessageBox.Show("Cliente eliminado permanentemente.", "PROCEDIMIENTO-EXITOSO",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                    EventosGloblales.EnClienteEliminado();
                    modoEliminar = false;
                    CargarPagina(paginaActual);
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar el cliente de la base de datos.",
                            "ERROR-ELIMINAR-015", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show("Error de base de datos al eliminar el cliente: " + ex.Message,
                        "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el cliente: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                modoEliminar = false;
                QuitarSombreadoTodos();
            }
        }

        private void QuitarSombreadoTodos()
        {
            try
            {
                modoEliminar = false;
                CargarPagina(paginaActual);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al quitar sombreado: " + ex.Message);
            }
        }

        private void CargarPagina(int pagina)
        {
            try
            {
                if (pagina < 1) pagina = 1;
                if (pagina > totalPaginas && totalPaginas > 0) pagina = totalPaginas;

                DataTable clientes;

                if (!string.IsNullOrWhiteSpace(busquedaActual))
                {
                    Clientes cli = new Clientes();
                    clientes = cli.BuscarCliente(busquedaActual);
                    totalRegistros = clientes?.Rows.Count ?? 0;
                    totalPaginas = 1;
                }
                else
                {
                    clientes = Clientes.MostrarClientesPagina(pagina, REGISTROS_POR_PAGINA, filtroActual, out totalRegistros);
                }

                paginaActual = pagina;
                totalPaginas = (int)Math.Ceiling((double)totalRegistros / REGISTROS_POR_PAGINA);
                if (totalPaginas < 1) totalPaginas = 1;

                CargarClientesEnPantalla(clientes);
                ActualizarControlesPaginacion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la página: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarControlesPaginacion()
        {
            try
            {
                lblInfoPagina.Text = $"Página {paginaActual} de {totalPaginas}";
                lblTotalRegistros.Text = $"Total: {totalRegistros} registro(s)";

                btnPaginaAnterior.Enabled = paginaActual > 1;
                btnPaginaSiguiente.Enabled = paginaActual < totalPaginas;

                GenerarBotonesNumeros();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al actualizar paginación: " + ex.Message);
            }
        }

        private void GenerarBotonesNumeros()
        {
            try
            {
                flpNumerosPagina.Controls.Clear();

                int inicio = Math.Max(1, paginaActual - 3);
                int fin = Math.Min(totalPaginas, inicio + 6);
                if (fin - inicio < 6) inicio = Math.Max(1, fin - 6);

                for (int i = inicio; i <= fin; i++)
                {
                    Button btnPagina = new Button();
                    btnPagina.Text = i.ToString();
                    btnPagina.Width = 35;
                    btnPagina.Height = 32;
                    btnPagina.FlatStyle = FlatStyle.Flat;
                    btnPagina.FlatAppearance.BorderSize = 0;
                    btnPagina.Font = new Font("Book Antiqua", 12, FontStyle.Bold);
                    btnPagina.Margin = new Padding(1);

                    if (i == paginaActual)
                    {
                        btnPagina.BackColor = Color.FromArgb(208, 112, 3);
                        btnPagina.ForeColor = Color.White;
                    }
                    else
                    {
                        btnPagina.BackColor = Color.FromArgb(206, 183, 175);
                        btnPagina.ForeColor = Color.Black;
                    }

                    int paginaBoton = i;
                    btnPagina.Click += (s, e) => CargarPagina(paginaBoton);

                    flpNumerosPagina.Controls.Add(btnPagina);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al generar botones de página: " + ex.Message);
            }
        }

        private void btnPaginaAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                CargarPagina(paginaActual + 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ir a la página siguiente: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPaginaSiguiente_Click(object sender, EventArgs e)
        {
            try
            {
                CargarPagina(paginaActual + 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ir a la página siguiente: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
