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
using Vista.Utilidades;

namespace Vista.GerenteProveedores
{
    public partial class frmAnadirProveedor : Form
    {
        private ErrorProvider errorProvider;

        private string rutaFotoRelativa = "";

        public frmAnadirProveedor()
        {
            try
            {
                InitializeComponent();

                errorProvider = new ErrorProvider();
                errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
                errorProvider.ContainerControl = this;

                ControlesBloqueo controlesBloqueo = new ControlesBloqueo();
                controlesBloqueo.BloquearControlesTXT(txtNombre);
                controlesBloqueo.BloquearControlesTXT(txtCorreo);
                controlesBloqueo.BloquearControlesMTXT(mtbTelefono);

                txtNombre.KeyPress += (s, e) => controlesBloqueo.ValidarSoloLetras(e);
                mtbTelefono.KeyPress += (s, e) => controlesBloqueo.ValidarSoloNumeros(mtbTelefono, e);

                txtNombre.TextChanged += (s, e) => errorProvider.SetError(txtNombre, "");
                txtCorreo.TextChanged += (s, e) => errorProvider.SetError(txtCorreo, "");
                mtbTelefono.TextChanged += (s, e) => errorProvider.SetError(mtbTelefono, "");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSubir_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Title = "Seleccionar foto del proveedor";
                    ofd.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp";
                    ofd.Multiselect = false;

                    if (ofd.ShowDialog() != DialogResult.OK) return;

                    string archivoOrigen = ofd.FileName;

                    FileInfo info = new FileInfo(archivoOrigen);
                    if (info.Length > 5 * 1024 * 1024)
                    {
                        MessageBox.Show("La imagen no debe superar los 5 MB.",
                                "ERROR-LONGITUD-003", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string carpetaFotos = Path.Combine(Application.StartupPath, "FotosProveedores");
                    if (!Directory.Exists(carpetaFotos))
                    {
                        Directory.CreateDirectory(carpetaFotos);
                    }

                    string extension = Path.GetExtension(archivoOrigen);
                    string nombreUnico = $"proveedor_{DateTime.Now:yyyyMMddHHmmss}{extension}";
                    string rutaDestino = Path.Combine(carpetaFotos, nombreUnico);

                    File.Copy(archivoOrigen, rutaDestino, true);

                    rutaFotoRelativa = Path.Combine("FotosProveedores", nombreUnico);

                    using (var stream = new FileStream(rutaDestino, FileMode.Open, FileAccess.Read))
                    {
                        pbLogo.Image = Image.FromStream(stream);
                    }
                    pbLogo.SizeMode = PictureBoxSizeMode.StretchImage;

                    errorProvider.SetError(pbLogo, "");
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("No se pudo copiar la foto: " + ex.Message,
                        "ERROR-ARCHIVO-160", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("No tiene permisos para guardar la foto: " + ex.Message,
                        "ERROR-PERMISO-161", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la foto: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                errorProvider.Clear();
                bool hayErrores = false;

                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    errorProvider.SetError(txtNombre, "El nombre es obligatorio.");
                    hayErrores = true;
                }

                if (string.IsNullOrWhiteSpace(txtCorreo.Text))
                {
                    errorProvider.SetError(txtCorreo, "El correo es obligatorio.");
                    hayErrores = true;
                }
                else
                {
                    try
                    {
                        var correo = new System.Net.Mail.MailAddress(txtCorreo.Text.Trim());
                    }
                    catch (FormatException)
                    {
                        errorProvider.SetError(txtCorreo, "El correo no tiene un formato válido.");
                        hayErrores = true;
                    }
                }

                string telefono = mtbTelefono.Text.Trim();
                if (string.IsNullOrWhiteSpace(telefono))
                {
                    errorProvider.SetError(mtbTelefono, "El teléfono es obligatorio.");
                    hayErrores = true;
                }
                else if (telefono.Count(char.IsDigit) != 8)
                {
                    errorProvider.SetError(mtbTelefono, "El teléfono debe tener 8 dígitos.");
                    hayErrores = true;
                }

                if (hayErrores)
                {
                    MessageBox.Show("Por favor corrija los campos marcados en rojo.",
                            "ERROR-CAMVACIO-001", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Proveedores nuevoProveedor = new Proveedores();

                nuevoProveedor.NombreProveedor = txtNombre.Text.Trim();
                nuevoProveedor.CorreoProveedor = txtCorreo.Text.Trim();
                nuevoProveedor.TelefonoProveedor = telefono;
                nuevoProveedor.FotoProveedor = rutaFotoRelativa;

                int resultado = Proveedores.IngresarProveedor(nuevoProveedor);

                switch (resultado)
                {
                    case 0:
                        MessageBox.Show("Proveedor agregado correctamente.", "Éxito",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case 1:
                        MessageBox.Show(
                            "Se restauró un proveedor que estaba eliminado con estos mismos datos.",
                            "PROCEDIMIENTO-EXITOSO",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case 2:
                        errorProvider.SetError(txtCorreo, "Ya existe un proveedor con ese correo o teléfono.");
                        MessageBox.Show(
                            "Ya existe un proveedor ACTIVO con esos datos (correo o teléfono duplicado).",
                            "ERROR-DADUPLICADO-002",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; 

                    default:
                        MessageBox.Show("Resultado inesperado al agregar el proveedor.",
                                "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }

                EventosGloblales.EnProveedoresAgregados();

                // Limpiar controles
                txtNombre.Clear();
                mtbTelefono.Clear();
                txtCorreo.Clear();
                pbLogo.Image = null;
                rutaFotoRelativa = "";
                errorProvider.Clear();

                this.Close();

            }
            catch (FormatException ex)
            {
                MessageBox.Show("Uno de los valores no tiene el formato correcto: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {

                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    errorProvider.SetError(txtCorreo, "Ya existe un proveedor con ese correo o teléfono.");
                    MessageBox.Show("Ya existe un proveedor con esos datos (correo o teléfono duplicado).",
                            "ERROR-DADUPLICADO-002", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Error de base de datos al agregar el proveedor: " + ex.Message,
                            "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el proveedor: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cerrar el formulario: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
