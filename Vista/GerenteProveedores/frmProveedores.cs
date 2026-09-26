using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista.GerenteProveedores
{
    public partial class frmProveedores : Form
    {
        private ErrorProvider errorProvider;

        private const int REGISTROS_POR_PAGINA = 20;
        private int paginaActual = 1;
        private int totalPaginas = 1;
        private int totalRegistros = 0;
        private string busquedaActual = "";

        private bool modoEliminar = false;
        private DataRow filaSeleccionada = null;
        private int idProveedorSeleccionado = 0;

        public frmProveedores()
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

                txtBarraBuscar.TextChanged += (s, e) => errorProvider.SetError(txtBarraBuscar, "");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Paginas
        private void CargarPagina(int pagina)
        {
            try
            {
                if (pagina < 1) pagina = 1;
                if (pagina > totalPaginas && totalPaginas > 0) pagina = totalPaginas;

                DataTable proveedores;

                if (string.IsNullOrWhiteSpace(busquedaActual))
                {
                    proveedores = Proveedores.MostrarProveedoresPaginado(pagina, REGISTROS_POR_PAGINA, out totalRegistros);
                }
                else
                {
                    Proveedores prov = new Proveedores();
                    proveedores = prov.BuscarProveedor(busquedaActual);
                    totalRegistros = proveedores.Rows.Count;
                }

                paginaActual = pagina;
                totalPaginas = (int)Math.Ceiling((double)totalRegistros / REGISTROS_POR_PAGINA);
                if (totalPaginas < 1) totalPaginas = 1;

                CargarProveedoresPantalla(proveedores);
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
                System.Diagnostics.Debug.WriteLine("Error al generar botones de página: " + ex.Message);
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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                frmFondoNegro fondo = new frmFondoNegro();
                fondo.StartPosition = FormStartPosition.CenterParent;
                fondo.WindowState = FormWindowState.Maximized;
                fondo.Show();

                frmAnadirProveedor abrir = new frmAnadirProveedor();
                    abrir.ShowDialog();

                CargarPagina(paginaActual);

                fondo.Close();

                if (totalPaginas > 1 && string.IsNullOrWhiteSpace(busquedaActual))
                {
                CargarPagina(totalPaginas);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el formulario de agregar proveedor: " + ex.Message,
                        "ERROR-PROVEEDOR-18", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarProveedoresPantalla(DataTable proveedores)
        {
            try
            {
                flpProvedores.Controls.Clear();

                if (proveedores == null || proveedores.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron proveedores que mostrar.",
                            "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (DataRow fila in proveedores.Rows)
                {
                    Panel panelProveedores = MostrarProvedores(fila);

                    if (panelProveedores == null) continue;

                    DataRow filaLocal = fila;

                    panelProveedores.Click += (s, e) =>
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
                            MessageBox.Show("Error al procesar el clic: " + ex.Message,
                                    "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };

                    flpProvedores.Controls.Add(panelProveedores);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar los proveedores en pantalla: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel MostrarProvedores(DataRow fila)
        {
            try
            {
                if (fila == null)
                {
                    return null;
                }

                Panel panelProveedoresFondo = new Panel();

                panelProveedoresFondo.Width = pnlContenedor.Width;
                panelProveedoresFondo.Height = pnlContenedor.Height;
                panelProveedoresFondo.BackColor = pnlContenedor.BackColor;
                panelProveedoresFondo.BorderStyle = pnlContenedor.BorderStyle;
                panelProveedoresFondo.Margin = new Padding(10);

                Panel panelProveedoresInformacion = new Panel();

                panelProveedoresInformacion.Width = pnlPlantilla.Width;
                panelProveedoresInformacion.Height = pnlPlantilla.Height;
                panelProveedoresInformacion.BackColor = pnlPlantilla.BackColor;
                panelProveedoresInformacion.BorderStyle = pnlPlantilla.BorderStyle;
                panelProveedoresInformacion.Location = pnlPlantilla.Location;
                panelProveedoresInformacion.Tag = fila["IdProveedor"];

                PictureBox pbLogo = new PictureBox();
                pbLogo.Width = 116;
                pbLogo.Height = 100;
                pbLogo.Location = new Point(50, 17);
                pbLogo.BorderStyle = BorderStyle.None;
                pbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
                pbLogo.BackColor = Color.White;

                try
                {
                    string rutaBD = fila["FotoProveedor"] != DBNull.Value ? fila["FotoProveedor"].ToString() : "";

                    if (!string.IsNullOrWhiteSpace(rutaBD))
                    {
                        string rutaCompleta = Path.IsPathRooted(rutaBD)? rutaBD : Path.Combine(Application.StartupPath, rutaBD);

                        if (File.Exists(rutaCompleta))
                        {
                            using (var stream = new FileStream(rutaCompleta, FileMode.Open, FileAccess.Read))
                            {
                                pbLogo.Image = Image.FromStream(stream);
                            }
                        }
                        else
                        {
                            pbLogo.Image = null;
                            pbLogo.BackColor = Color.LightGray;
                        }
                    }
                    else
                    {
                        pbLogo.Image = null;
                        pbLogo.BackColor = Color.LightGray;
                    }
                }
                catch (Exception exFoto)
                {
                    System.Diagnostics.Debug.WriteLine("Error al cargar foto: " + exFoto.Message);
                    pbLogo.Image = null;
                    pbLogo.BackColor = Color.LightGray;
                }

                Label lblTituloNombre = new Label();
                lblTituloNombre.Text = "Nombre".ToString();
                lblTituloNombre.Font = new Font("Book Antiqua", 24, FontStyle.Bold);
                lblTituloNombre.Location = new Point(216, 23);
                lblTituloNombre.AutoSize = true;
                lblTituloNombre.ForeColor = Color.FromArgb(64, 6, 6);

                Label lblNombre = new Label();
                lblNombre.Text = fila["NombreProveedor"].ToString();
                lblNombre.Font = new Font("Bookman Old Style", 18, FontStyle.Regular);
                lblNombre.Location = new Point(216, 63);
                lblNombre.AutoSize = true;

                // Panel Decoración 
                Panel pnlDecoracion = new Panel();
                pnlDecoracion.Size = new Size(1, 105);
                pnlDecoracion.Location = new Point(638, 14);
                pnlDecoracion.BackColor = Color.Black;

                Label lblTituloCorreo = new Label();
                lblTituloCorreo.Text = "Correo".ToString();
                lblTituloCorreo.Font = new Font("Book Antiqua", 17, FontStyle.Bold);
                lblTituloCorreo.Location = new Point(672, 9);
                lblTituloCorreo.ForeColor = Color.FromArgb(64, 6, 6);  
                lblTituloCorreo.AutoSize = true;

                Label lblCorreo = new Label();
                lblCorreo.Text = fila["CorreoProveedor"].ToString();
                lblCorreo.Font = new Font("Bookman Old Style", 16, FontStyle.Regular);
                lblCorreo.Location = new Point(672, 32);
                lblCorreo.AutoSize = true;

                Label lblTituloTelefono = new Label();
                lblTituloTelefono.Text = "Teléfono".ToString();
                lblTituloTelefono.Font = new Font("Book Antiqua", 17, FontStyle.Bold);
                lblTituloTelefono.Location = new Point(672, 68);
                lblTituloTelefono.ForeColor = Color.FromArgb(64, 6, 6); 
                lblTituloTelefono.AutoSize = true;

                Label lblTelefono = new Label();
                lblTelefono.Text = fila["TelefonoProveedor"].ToString();
                lblTelefono.Font = new Font("Bookman Old Style", 16, FontStyle.Regular);
                lblTelefono.Location = new Point(672, 95);
                lblTelefono.AutoSize = true;

                panelProveedoresFondo.Controls.Add(panelProveedoresInformacion);
                panelProveedoresInformacion.Controls.Add(pbLogo);
                panelProveedoresInformacion.Controls.Add(lblTituloNombre);
                panelProveedoresInformacion.Controls.Add(lblNombre);
                panelProveedoresInformacion.Controls.Add(pnlDecoracion);
                panelProveedoresInformacion.Controls.Add(lblTituloCorreo);
                panelProveedoresInformacion.Controls.Add(lblCorreo);
                panelProveedoresInformacion.Controls.Add(lblTituloTelefono);
                panelProveedoresInformacion.Controls.Add(lblTelefono);

                Color colorPanel = modoEliminar ? Color.FromArgb(220, 110, 110) : pnlContenedor.BackColor;

                panelProveedoresFondo.BackColor = colorPanel;
                panelProveedoresInformacion.BackColor = colorPanel;


                return panelProveedoresInformacion;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Una columna esperada no existe en los datos del proveedor: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el panel del proveedor: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void AbrirfrmActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                Panel panelProveedor = sender as Panel;

                if (panelProveedor == null || panelProveedor.Tag == null)
                {
                    MessageBox.Show("No se pudo identificar el proveedor seleccionado.",
                            "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int idProveedor = Convert.ToInt32(panelProveedor.Tag);

                frmEditarProveedores abrir = new frmEditarProveedores(idProveedor);
                abrir.ShowDialog();

                CargarPagina(paginaActual);
            }
            catch (FormatException ex)
            {
                MessageBox.Show("El identificador del proveedor no tiene el formato correcto: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el formulario de actualización: " + ex.Message,
                        "ERROR-CAMBIO-103", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    paginaActual = 1;
                    CargarPagina(1);
                    return;
                }

                Proveedores proveedor = new Proveedores();
                DataTable proveedorFiltrados = proveedor.BuscarProveedor(busquedaActual);

                totalRegistros = proveedorFiltrados?.Rows.Count ?? 0;
                totalPaginas = 1;
                paginaActual = 1;

                CargarProveedoresPantalla(proveedorFiltrados);
                ActualizarControlesPaginacion();
            }
            catch (Exception ex)
            {
                errorProvider.SetError(txtBarraBuscar, "Error en la búsqueda.");
                MessageBox.Show("Error al buscar proveedores: " + ex.Message,
                        "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                modoEliminar = !modoEliminar;

                CargarPagina(paginaActual);

                if (modoEliminar)
                {
                    MessageBox.Show("Selecciona el proveedor que deseas eliminar.\n\n" +
                                    "El proveedor se moverá a la papelera y podrá restaurarse.",
                        "INFO-ELIMINAR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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

                int idProveedor = Convert.ToInt32(fila["IdProveedor"]);
                string nombreCompleto = fila["NombreProveedor"].ToString();

                DialogResult confirmacion = MessageBox.Show(
                    $"¿Estás seguro de eliminar este proveedor?\n\n" +
                    $"Proveedoro: \"{nombreCompleto}\"\n\n" +
                    "El proveedor dejará de aparecer en la lista, pero sus datos se conservan.",
                    "INFO-ELIMINAR",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (confirmacion != DialogResult.Yes)
                {
                    modoEliminar = false;
                    CargarPagina(paginaActual);
                    return;
                }

                EliminarProoveedorConfirmar(idProveedor);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al confirmar la eliminación: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarProoveedorConfirmar(int idProveedor)
        {
            try
            {
                modoEliminar = false;

                Proveedores.EliminarProveedor(idProveedor);

                MessageBox.Show("Proveedor eliminado correctamente.",
                        "PROCEDIMIENTO-EXITOSO", MessageBoxButtons.OK, MessageBoxIcon.Information);

                EventosGloblales.EnProveedoresEliminados();

                CargarPagina(paginaActual);

                if (flpProvedores.Controls.Count == 0 && paginaActual > 1)
                {
                    CargarPagina(paginaActual - 1);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message,
                        "No se puede eliminar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el proveedor: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                modoEliminar = false;
            }
        }
    }
}
