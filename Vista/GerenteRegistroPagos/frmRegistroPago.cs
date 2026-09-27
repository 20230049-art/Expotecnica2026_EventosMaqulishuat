using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista.GerenteRegistroPagos
{
    public partial class frmRegistroPago : Form
    {
        private ErrorProvider errorProvider;

        private const int REGISTROS_POR_PAGINA = 20;
        private int paginaActual = 1;
        private int totalPaginas = 1;
        private int totalRegistros = 0;
        private string busquedaActual = "";

        private string filtroEstado = null;

        private bool cargandoCombos = false;

        public frmRegistroPago()
        {
            try
            {
                InitializeComponent();

                errorProvider = new ErrorProvider();
                errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
                errorProvider.ContainerControl = this;

                // Por si no están conectados en el Designer
                //btnTodoslosPagos.Click += btnTodoslosPagos_Click;
                //btnNoPagado.Click += btnProximos_Click;
                //btnPendientes.Click += btnPendientes_Click;
                //btnRealizados.Click += btnRealizados_Click;

                //btnPaginaAnterior.Click += btnPaginaAnterior_Click;
                //btnPaginaSiguiente.Click += btnPaginaSiguiente_Click;

                txtBarraBuscar.TextChanged += txtBarraBuscar_TextChanged;
                txtBarraBuscar.TextChanged += (s, e) => errorProvider.SetError(txtBarraBuscar, "");

                CargarPagina(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarPagina(int pagina)
        {
            try
            {
                if (pagina < 1) pagina = 1;
                if (pagina > totalPaginas && totalPaginas > 0) pagina = totalPaginas;

                DataTable registros;

                if (!string.IsNullOrWhiteSpace(busquedaActual))
                {
                    RegistrodePagos rep = new RegistrodePagos();
                    registros = rep.BuscarRegistroPago(busquedaActual);
                    totalRegistros = registros.Rows.Count;
                    totalPaginas = 1;
                    paginaActual = 1;
                }
                else
                {
                    registros = RegistrodePagos.MostrarRegistroPagosPagina(
                        pagina, REGISTROS_POR_PAGINA, filtroEstado, out totalRegistros);

                    paginaActual = pagina;
                    totalPaginas = (int)Math.Ceiling((double)totalRegistros / REGISTROS_POR_PAGINA);
                    if (totalPaginas < 1) totalPaginas = 1;
                }

                CargarRegistroPagoPantalla(registros);
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
                System.Diagnostics.Debug.WriteLine("Error paginación: " + ex.Message);
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
                    btnPagina.Height = 30;
                    btnPagina.FlatStyle = FlatStyle.Flat;
                    btnPagina.FlatAppearance.BorderSize = 1;
                    btnPagina.Font = new Font("Book Antiqua", 10, FontStyle.Bold);
                    btnPagina.Margin = new Padding(2);

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
                System.Diagnostics.Debug.WriteLine("Error botones: " + ex.Message);
            }
        }

        private void btnPaginaAnterior_Click(object sender, EventArgs e)
        {
            try { CargarPagina(paginaActual - 1); }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ir a la página anterior: " + ex.Message,
                        "ERROR-PAGINACION-170", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPaginaSiguiente_Click(object sender, EventArgs e)
        {
            try { CargarPagina(paginaActual + 1); }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ir a la página siguiente: " + ex.Message,
                        "ERROR-PAGINACION-170", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RestaurarBotones()
        {
            try
            {
                btnTodoslosPagos.BackColor = Color.FromArgb(244, 220, 197);
                btnNoPagado.BackColor = Color.FromArgb(244, 220, 197);
                btnPendientes.BackColor = Color.FromArgb(244, 220, 197);
                btnRealizados.BackColor = Color.FromArgb(244, 220, 197);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al restaurar botones: " + ex.Message);
            }
        }

        private void ActivarBoton(Button boton)
        {
            try
            {
                if (boton == null) return;
                RestaurarBotones();
                boton.BackColor = Color.FromArgb(237, 180, 141);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al activar botón: " + ex.Message);
            }
        }

        private void btnTodoslosPagos_Click(object sender, EventArgs e)
        {
            try
            {
                ActivarBoton(btnTodoslosPagos);
                filtroEstado = null;
                busquedaActual = "";
                txtBarraBuscar.Text = "";
                CargarPagina(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRealizados_Click(object sender, EventArgs e)
        {
            try
            {
                ActivarBoton(btnRealizados);
                filtroEstado = "Pagado";
                busquedaActual = "";
                txtBarraBuscar.Text = "";
                CargarPagina(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPendientes_Click(object sender, EventArgs e)
        {
            try
            {
                ActivarBoton(btnPendientes);
                filtroEstado = "Pendiente";
                busquedaActual = "";
                txtBarraBuscar.Text = "";
                CargarPagina(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNoPagado_Click(object sender, EventArgs e)
        {
            try
            {
                ActivarBoton(btnNoPagado);
                filtroEstado = "No pagado";
                busquedaActual = "";
                txtBarraBuscar.Text = "";
                CargarPagina(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarRegistroPagoPantalla(DataTable registroPagos)
        {
            try
            {
                flpRegistroPago.Controls.Clear();

                if (registroPagos == null || registroPagos.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron registros de pago.",
                            "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (DataRow fila in registroPagos.Rows)
                {
                    Panel panel = MostrarRegistroPagos(fila);
                    if (panel != null) flpRegistroPago.Controls.Add(panel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar los pagos: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel MostrarRegistroPagos(DataRow fila)
        {
            try
            {
                if (fila == null) return null;

                Panel panelContenedor = new Panel();
                panelContenedor.Width = pnlPlantilla.Width;
                panelContenedor.Height = pnlPlantilla.Height;
                panelContenedor.BackColor = Color.FromArgb(239, 199, 169);
                panelContenedor.BorderStyle = pnlPlantilla.BorderStyle;
                panelContenedor.Margin = new Padding(10);
                panelContenedor.Tag = fila["IdRegistroPago"];

                Label lblTituloNombre = new Label();
                lblTituloNombre.Text = "Nombre";
                lblTituloNombre.Font = new Font("Book Antiqua", 20, FontStyle.Bold);
                lblTituloNombre.Location = new Point(16, 18);
                lblTituloNombre.ForeColor = Color.FromArgb(64, 6, 6);
                lblTituloNombre.AutoSize = true;

                Label lblNombre = new Label();
                lblNombre.Text = fila["NombreCliente"].ToString();
                lblNombre.Font = new Font("Bookman Old Style", 18, FontStyle.Regular);
                lblNombre.Location = new Point(16, 50);
                lblNombre.AutoSize = true;

                Label lblApellido = new Label();
                lblApellido.Text = fila["ApellidoCliente"].ToString();
                lblApellido.Font = new Font("Bookman Old Style", 18, FontStyle.Regular);
                lblApellido.Location = new Point(16, 80);
                lblApellido.AutoSize = true;

                Panel pnlDecoracion = new Panel();
                pnlDecoracion.Size = new Size(1, 100);
                pnlDecoracion.Location = new Point(245, 18);
                pnlDecoracion.BackColor = Color.Black;

                Label lblTituloServicio = new Label();
                lblTituloServicio.Text = "Servicio";
                lblTituloServicio.Font = new Font("Book Antiqua", 20, FontStyle.Bold);
                lblTituloServicio.Location = new Point(278, 18);
                lblTituloServicio.ForeColor = Color.FromArgb(64, 6, 6);
                lblTituloServicio.AutoSize = true;

                Label lblServicio = new Label();
                lblServicio.Text = fila["NombreServicio"].ToString();
                lblServicio.Font = new Font("Bookman Old Style", 16, FontStyle.Regular);
                lblServicio.Location = new Point(258, 48);
                lblServicio.MaximumSize = new Size(180, 0);
                lblServicio.AutoSize = true;

                Panel pnlDecoracion1 = new Panel();
                pnlDecoracion1.Size = new Size(1, 100);
                pnlDecoracion1.Location = new Point(440, 18);
                pnlDecoracion1.BackColor = Color.Black;

                Label lblTituloCorreo = new Label();
                lblTituloCorreo.Text = "Correo";
                lblTituloCorreo.Font = new Font("Book Antiqua", 17, FontStyle.Bold);
                lblTituloCorreo.Location = new Point(462, 16);
                lblTituloCorreo.ForeColor = Color.FromArgb(64, 6, 6);
                lblTituloCorreo.AutoSize = true;

                Label lblCorreo = new Label();
                lblCorreo.Text = fila["CorreoCliente"].ToString();
                lblCorreo.Font = new Font("Bookman Old Style", 17, FontStyle.Regular);
                lblCorreo.Location = new Point(462, 39);
                lblCorreo.AutoSize = true;

                Label lblTituloTelefono = new Label();
                lblTituloTelefono.Text = "Teléfono";
                lblTituloTelefono.Font = new Font("Book Antiqua", 18, FontStyle.Bold);
                lblTituloTelefono.Location = new Point(462, 66);
                lblTituloTelefono.ForeColor = Color.FromArgb(64, 6, 6);
                lblTituloTelefono.AutoSize = true;

                Label lblTelefono = new Label();
                lblTelefono.Text = fila["TelefonoCliente"].ToString();
                lblTelefono.Font = new Font("Bookman Old Style", 17, FontStyle.Regular);
                lblTelefono.Location = new Point(462, 91);
                lblTelefono.AutoSize = true;

                Panel pnlDecoracion2 = new Panel();
                pnlDecoracion2.Size = new Size(1, 100);
                pnlDecoracion2.Location = new Point(802, 18);
                pnlDecoracion2.BackColor = Color.Black;

                Label lblTituloPago = new Label();
                lblTituloPago.Text = "Monto";
                lblTituloPago.Font = new Font("Book Antiqua", 18, FontStyle.Bold);
                lblTituloPago.Location = new Point(817, 17);
                lblTituloPago.ForeColor = Color.FromArgb(64, 6, 6);
                lblTituloPago.AutoSize = true;

                Label lblPago = new Label();
                lblPago.Text = fila["TotalVenta"].ToString();
                lblPago.Font = new Font("Bookman Old Style", 16, FontStyle.Regular);
                lblPago.Location = new Point(817, 42);
                lblPago.AutoSize = true;

                Label lblTituloFecha = new Label();
                lblTituloFecha.Text = "Fecha";
                lblTituloFecha.Font = new Font("Book Antiqua", 18, FontStyle.Bold);
                lblTituloFecha.Location = new Point(817, 68);
                lblTituloFecha.ForeColor = Color.FromArgb(64, 6, 6);
                lblTituloFecha.AutoSize = true;

                Label lblFecha = new Label();
                DateTime fecha = Convert.ToDateTime(fila["FechaPago"]);
                lblFecha.Text = fecha.ToString("dd/MM/yyyy");
                lblFecha.Font = new Font("Bookman Old Style", 16, FontStyle.Regular);
                lblFecha.Location = new Point(817, 93);
                lblFecha.AutoSize = true;

                Panel pnlDecoracion3 = new Panel();
                pnlDecoracion3.Size = new Size(1, 100);
                pnlDecoracion3.Location = new Point(1000, 18);
                pnlDecoracion3.BackColor = Color.Black;

                Label lblTituloEstado = new Label();
                lblTituloEstado.Text = "Estado";
                lblTituloEstado.Font = new Font("Book Antiqua", 18, FontStyle.Bold);
                lblTituloEstado.Location = new Point(1030, 18);
                lblTituloEstado.ForeColor = Color.FromArgb(64, 6, 6);
                lblTituloEstado.AutoSize = true;

                ComboBox cmbEstado = new ComboBox();
                cmbEstado.Location = new Point(1030, 62);
                cmbEstado.Size = new Size(155, 49);
                cmbEstado.Font = new Font("Book Antiqua", 14, FontStyle.Regular);
                cmbEstado.DropDownStyle = ComboBoxStyle.DropDownList;

                cmbEstado.Items.Add("Pagado");
                cmbEstado.Items.Add("No pagado");
                cmbEstado.Items.Add("Pendiente");

                cmbEstado.Tag = fila["IdRegistroPago"];

                cargandoCombos = true;

                string estadoActual = fila["EstadoPago"].ToString();

                if (cmbEstado.Items.Contains(estadoActual))
                {
                    cmbEstado.SelectedItem = estadoActual;
                }

                cargandoCombos = false;

                cmbEstado.SelectionChangeCommitted += CambiarEstadoPago;

                //cmbEstado.Items.Add("Pagado");
                //cmbEstado.Items.Add("No pagado");
                //cmbEstado.Items.Add("Pendiente");

                //cmbEstado.Tag = fila["IdRegistroPago"];

                //cargandoCombos = true;
                //cmbEstado.SelectedItem = fila["EstadoPago"].ToString();
                //cargandoCombos = false;

                //cmbEstado.SelectionChangeCommitted += CambiarEstadoPago;

                panelContenedor.Controls.Add(lblTituloNombre);
                panelContenedor.Controls.Add(lblNombre);
                panelContenedor.Controls.Add(lblApellido);
                panelContenedor.Controls.Add(pnlDecoracion);
                panelContenedor.Controls.Add(lblTituloServicio);
                panelContenedor.Controls.Add(lblServicio);
                panelContenedor.Controls.Add(pnlDecoracion1);
                panelContenedor.Controls.Add(lblTituloCorreo);
                panelContenedor.Controls.Add(lblCorreo);
                panelContenedor.Controls.Add(lblTituloTelefono);
                panelContenedor.Controls.Add(lblTelefono);
                panelContenedor.Controls.Add(pnlDecoracion2);
                panelContenedor.Controls.Add(lblTituloPago);
                panelContenedor.Controls.Add(lblPago);
                panelContenedor.Controls.Add(lblTituloFecha);
                panelContenedor.Controls.Add(lblFecha);
                panelContenedor.Controls.Add(pnlDecoracion3);
                panelContenedor.Controls.Add(lblTituloEstado);
                panelContenedor.Controls.Add(cmbEstado);

                return panelContenedor;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Una columna esperada no existe en los datos: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el panel: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void CambiarEstadoPago(object sender, EventArgs e)
        {
            try
            {
                if (cargandoCombos) return;

                ComboBox cmb = sender as ComboBox;
                if (cmb == null || cmb.Tag == null || cmb.SelectedItem == null) return;

                int idPago = Convert.ToInt32(cmb.Tag);
                //string estado = cmb.Text;

                string estado = cmb.SelectedItem.ToString();

                RegistrodePagos.ActualizaEstado(idPago, estado);

                MessageBox.Show("Estado del pago actualizado correctamente.",
                        "PROCEDIMIENTO-EXITOSO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error de base de datos al actualizar el estado: " + ex.Message,
                        "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CargarPagina(paginaActual);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el estado: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CargarPagina(paginaActual);
            }
        }

        private void txtBarraBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                busquedaActual = txtBarraBuscar.Text.Trim();

                if (string.IsNullOrWhiteSpace(busquedaActual))
                {
                    errorProvider.SetError(txtBarraBuscar, "");
                    CargarPagina(1);
                    return;
                }

                CargarPagina(1);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(txtBarraBuscar, "Error en la búsqueda.");
                MessageBox.Show("Error al buscar pagos: " + ex.Message,
                        "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
