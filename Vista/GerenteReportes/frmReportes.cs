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

namespace Vista.GerenteReportes
{
    public partial class frmReportes : Form
    {
        private ErrorProvider errorProvider;
        private bool modoEliminar = false;

        private const int REGISTROS_POR_PAGINA = 20;
        private int paginaActual = 1;
        private int totalPaginas = 1;
        private int totalRegistros = 0;

        private string busquedaActual = "";

        public frmReportes()
        {
            try
            {
                InitializeComponent();
                errorProvider = new ErrorProvider();
                errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
                errorProvider.ContainerControl = this;

                btnPaginaAnterior.Click += btnPaginaAnterior_Click;
                btnPaginaSiguiente.Click += btnPaginaSiguiente_Click;
                CargarPagina(1);

                txtBuscaInve.TextChanged += (s, e) => errorProvider.SetError(txtBuscaInve, "");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarReportes()
        {
            try
            {
                DataTable reportes = Reportes.MostrarReportes();
                CargarReportesPantalla(reportes);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los reportes: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarReportesPantalla(DataTable reportes)
        {
            try
            {
                flpRegistroPagos.Controls.Clear();

                if (reportes == null || reportes.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron reportes que mostrar.",
                            "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (DataRow fila in reportes.Rows)
                {
                    Panel panel = MostrarReportes(fila);

                    if (panel == null) continue;

                    DataRow filaLocal = fila;

                    panel.Click += (s, e) =>
                    {
                        try
                        {
                            if (modoEliminar)
                            {
                                ConfirmarEliminar(filaLocal);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al procesar el clic del reporte: " + ex.Message,
                                    "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };

                    flpRegistroPagos.Controls.Add(panel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar los reportes en pantalla: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel MostrarReportes(DataRow fila)
        {
            try
            {
                if (fila == null) return null;

                Panel panelFondo = new Panel();

                //panelFondo.Width = panelFondo.Width;
                //panelFondo.Height = pnlFondo.Height;
                //panelFondo.BorderStyle = pnlFondo.BorderStyle;
                panelFondo.Margin = new Padding(4);
                //panelFondo.Tag = fila["IdReporte"];
                panelFondo.BackColor = modoEliminar
                    ? Color.FromArgb(220, 110, 110)
                    : Color.FromArgb(253, 241, 217);

                Panel panelReportesInformacion = new Panel();

                //panelReportesInformacion.Margin = new Padding(10);
                panelReportesInformacion.Width = pnlReportesInfo.Width;
                panelReportesInformacion.Height = pnlReportesInfo.Height;
                panelReportesInformacion.BackColor = modoEliminar
                    ? Color.FromArgb(220, 110, 110)
                    : pnlReportesInfo.BackColor;
                panelReportesInformacion.BorderStyle = pnlReportesInfo.BorderStyle;
                panelReportesInformacion.Location = pnlReportesInfo.Location;

                Label lblTituloFecha = new Label();
                lblTituloFecha.Text = "Fecha";
                lblTituloFecha.Font = new Font("Book Antiqua", 25, FontStyle.Bold);
                lblTituloFecha.Location = new Point(37, 23);
                lblTituloFecha.ForeColor = Color.FromArgb(64, 6, 6);
                lblTituloFecha.AutoSize = true;

                Label lblFecha = new Label();
                DateTime fecha = Convert.ToDateTime(fila["FechaReporte"]);
                lblFecha.Text = fecha.ToString("dd/MM/yyyy");
                lblFecha.Font = new Font("Bookman Old Style", 20, FontStyle.Regular);
                lblFecha.Location = new Point(37, 69);
                lblFecha.AutoSize = true;

                Panel pnlDecoracion = new Panel();
                pnlDecoracion.Size = new Size(1, 98);
                pnlDecoracion.Location = new Point(270, 18);
                pnlDecoracion.BackColor = Color.Black;

                Label lblTituloNombre = new Label();
                lblTituloNombre.Text = "Nombre";
                lblTituloNombre.Font = new Font("Book Antiqua", 22, FontStyle.Bold);
                lblTituloNombre.Location = new Point(288, 20);
                lblTituloNombre.ForeColor = Color.FromArgb(64, 6, 6);
                lblTituloNombre.AutoSize = true;

                Label lblNombre = new Label();
                lblNombre.Text = fila["NombreReporte"].ToString();
                lblNombre.Font = new Font("Bookman Old Style", 17, FontStyle.Regular);
                lblNombre.Location = new Point(288, 56);
                lblNombre.MaximumSize = new Size(230, 0);
                lblNombre.AutoSize = true;

                Panel pnlDecoracion2 = new Panel();
                pnlDecoracion2.Size = new Size(1, 98);
                pnlDecoracion2.Location = new Point(544, 18);
                pnlDecoracion2.BackColor = Color.Black;

                Label lblTituloReportes = new Label();
                lblTituloReportes.Text = "Reporte";
                lblTituloReportes.Font = new Font("Book Antiqua", 23, FontStyle.Bold);
                lblTituloReportes.Location = new Point(555, 18);
                lblTituloReportes.ForeColor = Color.FromArgb(64, 6, 6);
                lblTituloReportes.AutoSize = true;

                Panel pnlScrollInfo = new Panel();
                pnlScrollInfo.Width = pnlScrollInformacion.Width;
                pnlScrollInfo.Height = pnlScrollInformacion.Height;
                pnlScrollInfo.Location = new Point(550, 51);
                pnlScrollInfo.AutoScroll = true;

                Label lblReportes = new Label();
                lblReportes.Text = fila["DescripcionReporte"].ToString();
                lblReportes.Font = new Font("Bookman Old Style", 18, FontStyle.Regular);
                lblReportes.Location = new Point(8, 1);
                lblReportes.MaximumSize = new Size(642, 0);
                lblReportes.AutoSize = true;

                panelFondo.Controls.Add(panelReportesInformacion);
                panelReportesInformacion.Controls.Add(lblTituloFecha);
                panelReportesInformacion.Controls.Add(lblFecha);
                panelReportesInformacion.Controls.Add(pnlDecoracion);
                panelReportesInformacion.Controls.Add(lblTituloNombre);
                panelReportesInformacion.Controls.Add(lblNombre);
                panelReportesInformacion.Controls.Add(pnlDecoracion2);
                panelReportesInformacion.Controls.Add(lblTituloReportes);
                panelReportesInformacion.Controls.Add(pnlScrollInfo);
                pnlScrollInfo.Controls.Add(lblReportes);

                return panelFondo;

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Una columna esperada no existe en los datos del reporte: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el panel del reporte: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void txtBuscaInve_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string busqueda = txtBuscaInve.Text.Trim();

                if (string.IsNullOrWhiteSpace(busqueda))
                {
                    errorProvider.SetError(txtBuscaInve, "");
                    CargarReportes();
                    return;
                }

                Reportes reportes = new Reportes();
                DataTable reportesFiltrados = reportes.BuscarReporte(busqueda);
                CargarReportesPantalla(reportesFiltrados);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(txtBuscaInve, "Error en la búsqueda.");
                MessageBox.Show("Error al buscar reportes: " + ex.Message,
                        "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                frmAgregarReportes abrir = new frmAgregarReportes();
                abrir.ShowDialog();

                CargarReportes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el formulario de agregar reporte: " + ex.Message,
                        "ERROR-CAMBIO-103", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                modoEliminar = !modoEliminar;

                CargarReportes();

                if (modoEliminar)
                {
                    MessageBox.Show(
                        "Selecciona el reporte que deseas eliminar.",
                        "INFO-ELIMINAR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al activar el modo eliminar: " + ex.Message,
                        "ERROR-CAMBIO-103", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfirmarEliminar(DataRow fila)
        {
            try
            {
                if (fila == null) return;

                int idReporte = Convert.ToInt32(fila["IdReporte"]);
                string nombreReporte = fila["NombreReporte"].ToString();

                DialogResult confirmacion = MessageBox.Show(
                    $"¿Estás seguro de eliminar el reporte?\n\n" +
                    $"Reporte: \"{nombreReporte}\"\n\n" +
                    "Esta acción NO se puede deshacer.",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (confirmacion != DialogResult.Yes)
                {
                    modoEliminar = false;
                    CargarReportes();
                    return;
                }

                EliminarReporteConfirmado(idReporte);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al confirmar la eliminación: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarReporteConfirmado(int idReporte)
        {
            try
            {
                modoEliminar = false;

                bool exito = Reportes.EliminarReporte(idReporte);

                if (!exito)
                {
                    MessageBox.Show("No se pudo eliminar el reporte de la base de datos.",
                            "ERROR-ELIMINAR-015", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CargarReportes();
                    return;
                }

                MessageBox.Show("Reporte eliminado correctamente.",
                        "PROCEDIMIENTO-EXITOSO", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarReportes();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error de base de datos al eliminar el reporte: " + ex.Message,
                        "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el reporte: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                modoEliminar = false;
            }
        }

        private void CargarPagina(int pagina)
        {
            try
            {
                if (pagina < 1) pagina = 1;
                if (pagina > totalPaginas && totalPaginas > 0) pagina = totalPaginas;

                DataTable reportes;

                if (string.IsNullOrWhiteSpace(busquedaActual))
                {
                    reportes = Reportes.MostrarReportesPagina(pagina, REGISTROS_POR_PAGINA, out totalRegistros);
                }
                else
                {
                    Reportes rep = new Reportes();
                    reportes = rep.BuscarReporte(busquedaActual);
                    totalRegistros = reportes?.Rows.Count ?? 0;
                }

                paginaActual = pagina;
                totalPaginas = (int)Math.Ceiling((double)totalRegistros / REGISTROS_POR_PAGINA);
                if (totalPaginas < 1) totalPaginas = 1;

                CargarReportesPantalla(reportes);
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
                CargarPagina(paginaActual - 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ir a la página anterior: " + ex.Message,
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
