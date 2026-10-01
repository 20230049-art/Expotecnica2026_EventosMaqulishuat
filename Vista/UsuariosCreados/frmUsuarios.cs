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
using Vista.Utilidades;

namespace Vista.UsuariosCreados
{
    public partial class frmUsuarios : Form
    {
        private ErrorProvider errorProvider;

        private const int REGISTROS_POR_PAGINA = 20;
        private int paginaActual = 1;
        private int totalPaginas = 1;
        private int totalRegistros = 0;

        private bool modoEliminar = false;

        public frmUsuarios()
        {
            try
            {
                InitializeComponent();
                errorProvider = new ErrorProvider();
                errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
                errorProvider.ContainerControl = this;

                Redondeo.RedondearFig(panel1,10);
                Redondeo.RedondearFig(pnlPedidaDeDatos, 10);
                Redondeo.RedondearFig(btnGuardar, 6);
                Redondeo.RedondearFig(btnEliminar, 6);
                Redondeo.RedondearFig(panel2, 10);
                Redondeo.RedondearFig(btnPaginaAnterior, 4);
                Redondeo.RedondearFig(btnPaginaSiguiente, 4);
                CargarRoles();

                CargarPagina(1);
                VisibilidadPorRol();
                ConfigurarGrid();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar: " + ex.Message);
            }
        }
        private void CargarRoles()
        {
            try
            {
                cmbRol.Items.Clear();
                cmbRol.Items.Add("Gerente");
                cmbRol.Items.Add("Empleado");
                cmbRol.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al cargar roles: " + ex.Message);
            }
        }

        private void cmbRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                VisibilidadPorRol();
            }
            catch { }
        }

        private void VisibilidadPorRol()
        {
            try
            {

                string rol = cmbRol.SelectedItem?.ToString();
                bool esGerente = rol == "Gerente";

                txtNombre.Visible = esGerente;
                txtApellidos.Visible = esGerente;
                txtCorreo.Visible = esGerente;
                txtNombreUsuario.Visible = true;
                txtContrasena.Visible = true;

                errorProvider.Clear();

                if (!esGerente)
                {
                    txtNombre.Clear();
                    txtApellidos.Clear();
                    txtCorreo.Clear();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al aplicar visibilidad: " + ex.Message);
            }
        }
        private void CargarPagina(int pagina)
        {
            try
            {
                if (pagina < 1) pagina = 1;
                if (pagina > totalPaginas && totalPaginas > 0) pagina = totalPaginas;

                DataTable usuarios = Usuario.MostrarUsuariosPagina(
                    pagina, REGISTROS_POR_PAGINA, out totalRegistros);

                paginaActual = pagina;
                totalPaginas = (int)Math.Ceiling((double)totalRegistros / REGISTROS_POR_PAGINA);
                if (totalPaginas < 1) totalPaginas = 1;

                dgvUsuarios.DataSource = usuarios;

                if (dgvUsuarios.Columns.Contains("IdUsuario"))
                    dgvUsuarios.Columns["IdUsuario"].Visible = false;

                EstilizarColumnas();
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
                btnPaginaAnterior.Enabled = paginaActual > 1;
                btnPaginaSiguiente.Enabled = paginaActual < totalPaginas;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error paginación: " + ex.Message);
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

        private void btnPaginaAnterior_Click(object sender, EventArgs e)
        {
            try { CargarPagina(paginaActual - 1); }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ir a la página anterior: " + ex.Message,
                        "ERROR-PAGINACION-170", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                errorProvider.Clear();

                string rol = cmbRol.SelectedItem?.ToString();
                if (string.IsNullOrWhiteSpace(rol))
                {
                    errorProvider.SetError(cmbRol, "Selecciona un rol.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNombreUsuario.Text))
                {
                    errorProvider.SetError(txtNombreUsuario, "Campo obligatorio.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtContrasena.Text))
                {
                    errorProvider.SetError(txtContrasena, "Campo obligatorio.");
                    return;
                }

                if (txtContrasena.Text.Length < 6)
                {
                    errorProvider.SetError(txtContrasena, "Mínimo 6 caracteres.");
                    return;
                }

                string hash = BCrypt.Net.BCrypt.HashPassword(txtContrasena.Text);

                if (rol == "Gerente")
                {
                    if (string.IsNullOrWhiteSpace(txtNombre.Text))
                    {
                        errorProvider.SetError(txtNombre, "Campo obligatorio.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtApellidos.Text))
                    {
                        errorProvider.SetError(txtApellidos, "Campo obligatorio.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtCorreo.Text))
                    {
                        errorProvider.SetError(txtCorreo, "Campo obligatorio.");
                        return;
                    }

                    try
                    {
                        var correo = new System.Net.Mail.MailAddress(txtCorreo.Text.Trim());
                    }
                    catch (FormatException)
                    {
                        errorProvider.SetError(txtCorreo, "Correo inválido.");
                        return;
                    }

                    Usuario nuevo = new Usuario
                    {
                        NombreGerente = txtNombre.Text.Trim(),
                        ApellidoGerente = txtApellidos.Text.Trim(),
                        CorreoGerente = txtCorreo.Text.Trim(),
                        NombreUsuario = txtNombreUsuario.Text.Trim(),
                        ContrasenaUsuario = hash
                    };

                    if (nuevo.RegistrarGerente())
                    {
                        MessageBox.Show("Gerente creado correctamente.",
                                "PROCEDIMIENTO-EXITOSO", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LimpiarFormulario();
                        CargarPagina(paginaActual);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo crear el gerente. Verifica que el correo y el usuario no existan.",
                                "ERROR-REGISTRO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else 
                {
                    Usuario nuevo = new Usuario
                    {
                        NombreUsuario = txtNombreUsuario.Text.Trim(),
                        ContrasenaUsuario = hash
                    };

                    int id = nuevo.RegistrarEmpleado();

                    if (id > 0)
                    {
                        MessageBox.Show("Usuario de empleado creado correctamente.",
                                "PROCEDIMIENTO-EXITOSO", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LimpiarFormulario();
                        CargarPagina(paginaActual);
                    }
                    else
                    {
                        MessageBox.Show(
                            "No se pudo crear el usuario.\n\n" +
                            "Verifica que el correo ingresado pertenezca a un empleado registrado " +
                            "y que no tenga ya una cuenta.",
                            "ERROR-REGISTRO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error de base de datos: " + ex.Message,
                        "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el usuario: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            try
            {
                txtNombre.Clear();
                txtApellidos.Clear();
                txtCorreo.Clear();
                txtNombreUsuario.Clear();
                txtContrasena.Clear();
                errorProvider.Clear();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al limpiar: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsuarios.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Selecciona un usuario en la lista para eliminar.",
                            "INFO-ELIMINAR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataGridViewRow row = dgvUsuarios.SelectedRows[0];

                if (row.Cells["IdUsuario"].Value == null)
                {
                    MessageBox.Show("No se pudo identificar el usuario seleccionado.",
                            "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int idUsuario = Convert.ToInt32(row.Cells["IdUsuario"].Value);
                string rol = row.Cells["Rol"].Value?.ToString() ?? "";
                string usuario = row.Cells["NombreUsuario"].Value?.ToString() ?? "";

                DialogResult confirmacion = MessageBox.Show(
                    $"¿Estás seguro de eliminar este usuario?\n\n" +
                    $"Usuario: \"{usuario}\"\n" +
                    $"Rol: {rol}\n\n" +
                    "El usuario dejará de aparecer en la lista, pero sus datos se conservan.",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (confirmacion != DialogResult.Yes) return;

                bool exito = Usuario.EliminarUsuario(idUsuario);

                if (exito)
                {
                    MessageBox.Show("Usuario eliminado correctamente.",
                            "PROCEDIMIENTO-EXITOSO", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CargarPagina(paginaActual);

                    if (dgvUsuarios.Rows.Count == 0 && paginaActual > 1)
                    {
                        CargarPagina(paginaActual - 1);
                    }
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar el usuario.",
                            "ERROR-ELIMINAR-015", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error de base de datos al eliminar: " + ex.Message,
                        "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el usuario: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarGrid()
        {
            try
            {
                dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvUsuarios.MultiSelect = false;
                dgvUsuarios.ReadOnly = true;
                dgvUsuarios.AllowUserToAddRows = false;
                dgvUsuarios.AllowUserToDeleteRows = false;
                dgvUsuarios.AllowUserToResizeRows = false;
                dgvUsuarios.RowHeadersVisible = false;
                dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvUsuarios.BorderStyle = BorderStyle.FixedSingle;
                dgvUsuarios.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvUsuarios.GridColor = Color.FromArgb(200, 170, 140);

                dgvUsuarios.BackgroundColor = Color.FromArgb(255, 251, 234);
                dgvUsuarios.ForeColor = Color.FromArgb(64, 6, 6);
                dgvUsuarios.Font = new Font("Book Antiqua", 11, FontStyle.Regular);

                dgvUsuarios.EnableHeadersVisualStyles = false;
                dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(210, 170, 130); 
                dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(64, 6, 6);       
                dgvUsuarios.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(210, 170, 130);
                dgvUsuarios.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(64, 6, 6);
                dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Book Antiqua", 12, FontStyle.Bold);
                dgvUsuarios.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvUsuarios.ColumnHeadersDefaultCellStyle.Padding = new Padding(5, 0, 5, 0);
                dgvUsuarios.ColumnHeadersHeight = 34;
                dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                dgvUsuarios.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 251, 234);
                dgvUsuarios.RowsDefaultCellStyle.ForeColor = Color.FromArgb(64, 6, 6);
                dgvUsuarios.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(208, 112, 3);   
                dgvUsuarios.RowsDefaultCellStyle.SelectionForeColor = Color.White;
                dgvUsuarios.RowsDefaultCellStyle.Padding = new Padding(5, 0, 5, 0);

                dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 235, 205);
                dgvUsuarios.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(64, 6, 6);
                dgvUsuarios.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(208, 112, 3);
                dgvUsuarios.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;

                dgvUsuarios.RowTemplate.Height = 30;

                foreach (DataGridViewColumn col in dgvUsuarios.Columns)
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al configurar grid: " + ex.Message);
            }
        }

        private void EstilizarColumnas()
        {
            try
            {
                //// Ocultar IdUsuario
                //if (dgvUsuarios.Columns.Contains("IdUsuario"))
                //    dgvUsuarios.Columns["IdUsuario"].Visible = false;

                if (dgvUsuarios.Columns.Contains("Rol"))
                {
                    dgvUsuarios.Columns["Rol"].HeaderText = "Rol";
                    dgvUsuarios.Columns["Rol"].FillWeight = 70;
                    dgvUsuarios.Columns["Rol"].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
                }

                if (dgvUsuarios.Columns.Contains("NombreUsuario"))
                {
                    dgvUsuarios.Columns["NombreUsuario"].HeaderText = "Nombre Usuario";
                    dgvUsuarios.Columns["NombreUsuario"].FillWeight = 130;
                }

                if (dgvUsuarios.Columns.Contains("NombreCompleto"))
                {
                    dgvUsuarios.Columns["NombreCompleto"].HeaderText = "Nombre Completo";
                    dgvUsuarios.Columns["NombreCompleto"].FillWeight = 150;
                }

                if (dgvUsuarios.Columns.Contains("Correo"))
                {
                    dgvUsuarios.Columns["Correo"].HeaderText = "Correo";
                    dgvUsuarios.Columns["Correo"].FillWeight = 140;
                }

                foreach (DataGridViewColumn col in dgvUsuarios.Columns)
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al estilizar columnas: " + ex.Message);
            }
        }
    }
}
