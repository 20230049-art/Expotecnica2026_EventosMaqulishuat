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

namespace Vista.GerenteEmpleados
{
    public partial class frmAnadirEmpleado : Form
    {
        private ErrorProvider errorProvider;
        private string rutaFotoRelativa = "";

        public frmAnadirEmpleado()
        {
            try
            {
                InitializeComponent();

                errorProvider = new ErrorProvider();
                errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
                errorProvider.ContainerControl = this;

                ControlesBloqueo controlesBloqueo = new ControlesBloqueo();

                controlesBloqueo.BloquearControlesTXT(txtNombre);
                controlesBloqueo.BloquearControlesTXT(txtApellido);
                controlesBloqueo.BloquearControlesTXT(txtDui);
                controlesBloqueo.BloquearControlesTXT(txtTelefono);
                controlesBloqueo.BloquearControlesTXT(txtCorreo);
                controlesBloqueo.BloquearControlesTXT(txtCargo);
                controlesBloqueo.BloquearControlesTXT(txtCuentaBancaria);

                txtNombre.KeyPress += (s, e) => controlesBloqueo.ValidarSoloLetras(e);
                txtApellido.KeyPress += (s, e) => controlesBloqueo.ValidarSoloLetras(e);
                txtCargo.KeyPress += (s, e) => controlesBloqueo.ValidarSoloLetras(e);
                txtTelefono.KeyPress += (s, e) => controlesBloqueo.ValidarSoloNumeros(txtTelefono, e);
                txtDui.KeyPress += (s, e) => controlesBloqueo.ValidarSoloNumeros(txtDui, e);

                txtNombre.TextChanged += (s, e) => errorProvider.SetError(txtNombre, "");
                txtApellido.TextChanged += (s, e) => errorProvider.SetError(txtApellido, "");
                txtDui.TextChanged += (s, e) => errorProvider.SetError(txtDui, "");
                txtTelefono.TextChanged += (s, e) => errorProvider.SetError(txtTelefono, "");
                txtCorreo.TextChanged += (s, e) => errorProvider.SetError(txtCorreo, "");
                txtCargo.TextChanged += (s, e) => errorProvider.SetError(txtCargo, "");
                txtCuentaBancaria.TextChanged += (s, e) => errorProvider.SetError(txtCuentaBancaria, "");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                if (string.IsNullOrWhiteSpace(txtApellido.Text))
                {
                    errorProvider.SetError(txtApellido, "El apellido es obligatorio.");
                    hayErrores = true;
                }

                if (string.IsNullOrWhiteSpace(txtCargo.Text))
                {
                    errorProvider.SetError(txtCargo, "El cargo es obligatorio.");
                    hayErrores = true;
                }

                string dui = txtDui.Text.Trim();
                if (string.IsNullOrWhiteSpace(dui))
                {
                    errorProvider.SetError(txtDui, "El DUI es obligatorio.");
                    hayErrores = true;
                }
                else
                {
                    int soloDigitos = dui.Count(char.IsDigit);

                    if (soloDigitos != 9)
                    {
                        errorProvider.SetError(txtDui, "El DUI debe tener 9 dígitos.");
                        hayErrores = true;
                    }
                }

                string telefono = txtTelefono.Text.Trim();
                if (string.IsNullOrWhiteSpace(telefono))
                {
                    errorProvider.SetError(txtTelefono, "El teléfono es obligatorio.");
                    hayErrores = true;
                }
                else if (telefono.Length < 8)
                {
                    errorProvider.SetError(txtTelefono, "El teléfono debe tener al menos 8 dígitos.");
                    hayErrores = true;
                }

                if (!string.IsNullOrWhiteSpace(txtCorreo.Text))
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

                string cuenta = txtCuentaBancaria.Text.Trim();
                if (!string.IsNullOrWhiteSpace(cuenta) && cuenta.Length < 10)
                {
                    errorProvider.SetError(txtCuentaBancaria, "La cuenta bancaria debe tener al menos 10 dígitos.");
                    hayErrores = true;
                }

                if (hayErrores)
                {
                    MessageBox.Show("Por favor corrija los campos marcados en rojo.",
                            "ERROR-CAMVACIO-001", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(rutaFotoRelativa))
                {
                    errorProvider.SetError(pbFoto, "Debe subir una foto del empleado.");
                    hayErrores = true;
                }

                if (hayErrores)
                {
                    MessageBox.Show("Por favor corrija los campos marcados en rojo.",
                            "ERROR-CAMVACIO-001", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Empleados empleado = new Empleados();

                empleado.NombreEmpleado = txtNombre.Text.Trim();
                empleado.ApellidoEmpleado = txtApellido.Text.Trim();
                empleado.TelefonoEmpleado = telefono;
                empleado.CargoEmpleado = txtCargo.Text.Trim();
                empleado.DUIEmpleado = dui;
                empleado.CorreoEmpleado = txtCorreo.Text.Trim();
                empleado.CuentaBancariaEmpleado = cuenta;

                empleado.FotoEmpleado = rutaFotoRelativa;

                Empleados.AgregarEmpleado(empleado);

                MessageBox.Show("Empleado agregado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                EventosGloblales.EnEmpleadosAgregados();

                // Limpiar controles
                txtNombre.Clear();
                txtApellido.Clear();
                txtTelefono.Clear();
                txtCorreo.Clear();
                txtCargo.Clear();
                txtCuentaBancaria.Clear();
                txtDui.Clear();
                pbFoto.Image = null;
                rutaFotoRelativa = "";
                errorProvider.Clear();
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Uno de los valores no tiene el formato correcto: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                // ✅ Si SQL detecta duplicado (DUI, correo), el número de error es 2627 o 2601
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    errorProvider.SetError(txtDui, "Ya existe un empleado con ese DUI o correo.");
                    MessageBox.Show("Ya existe un empleado con esos datos (DUI o correo duplicado).",
                            "ERROR-DADUPLICADO-002", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Error de base de datos al agregar el empleado: " + ex.Message,
                            "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el empleado: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSubirFoto_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Title = "Seleccionar foto del empleado";
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

                    string carpetaFotos = Path.Combine(Application.StartupPath, "FotosEmpleados");
                    if (!Directory.Exists(carpetaFotos))
                    {
                        Directory.CreateDirectory(carpetaFotos);
                    }

                    string extension = Path.GetExtension(archivoOrigen);
                    string nombreUnico = $"empleado_{DateTime.Now:yyyyMMddHHmmss}{extension}";
                    string rutaDestino = Path.Combine(carpetaFotos, nombreUnico);

                    File.Copy(archivoOrigen, rutaDestino, true);

                    rutaFotoRelativa = Path.Combine("FotosEmpleados", nombreUnico);

                    using (var stream = new FileStream(rutaDestino, FileMode.Open, FileAccess.Read))
                    {
                        pbFoto.Image = Image.FromStream(stream);
                    }
                    pbFoto.SizeMode = PictureBoxSizeMode.StretchImage;

                    errorProvider.SetError(pbFoto, "");
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
    }   }
}
