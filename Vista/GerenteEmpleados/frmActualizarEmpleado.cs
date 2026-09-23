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
    public partial class frmActualizarEmpleado : Form
    {
        private int idEmpleado;
        private ErrorProvider errorProvider;
        private string rutaFotoRelativa = "";

        public frmActualizarEmpleado(int idEmpleado)
        {
            try
            {
                InitializeComponent();

                errorProvider = new ErrorProvider();
                errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
                errorProvider.ContainerControl = this;

                this.idEmpleado = idEmpleado;
                if (idEmpleado <= 0)
                {
                    MessageBox.Show("No se especificó un empleado válido para actualizar.",
                            "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MostrarInformacionEmpleado();

                ControlesBloqueo controlesBloqueo = new ControlesBloqueo();
                controlesBloqueo.BloquearControlesTXT(txtNombre);
                controlesBloqueo.BloquearControlesTXT(txtApellido);
                controlesBloqueo.BloquearControlesTXT(txtDui);
                controlesBloqueo.BloquearControlesMTXT(mtxbTelefono);
                controlesBloqueo.BloquearControlesTXT(txtCorreo);
                controlesBloqueo.BloquearControlesTXT(txtCargo);
                controlesBloqueo.BloquearControlesTXT(txtCuentaBancaria);

                txtNombre.KeyPress += (s, e) => controlesBloqueo.ValidarSoloLetras(e);
                txtApellido.KeyPress += (s, e) => controlesBloqueo.ValidarSoloLetras(e);
                txtCargo.KeyPress += (s, e) => controlesBloqueo.ValidarSoloLetras(e);
                mtxbTelefono.KeyPress += (s, e) => controlesBloqueo.ValidarSoloNumeros(mtxbTelefono, e);
                txtDui.KeyPress += (s, e) => controlesBloqueo.ValidarSoloNumeros(txtDui, e);

                txtNombre.TextChanged += (s, e) => errorProvider.SetError(txtNombre, "");
                txtApellido.TextChanged += (s, e) => errorProvider.SetError(txtApellido, "");
                txtDui.TextChanged += (s, e) => errorProvider.SetError(txtDui, "");
                mtxbTelefono.TextChanged += (s, e) => errorProvider.SetError(mtxbTelefono, "");
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

        private void MostrarInformacionEmpleado()
        {
            try
            {
                DataTable dt = Empleados.ObtenerEmpleadoId(idEmpleado);

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontró la información del empleado seleccionado.",
                            "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                txtNombre.Text = dt.Rows[0]["NombreEmpleado"].ToString();
                txtApellido.Text = dt.Rows[0]["ApellidoEmpleado"].ToString();
                txtDui.Text = dt.Rows[0]["DUIEmpleado"].ToString();
                txtCargo.Text = dt.Rows[0]["CargoEmpleado"].ToString();
                mtxbTelefono.Text = dt.Rows[0]["TelefonoEmpleado"].ToString();
                txtCorreo.Text = dt.Rows[0]["CorreoEmpleado"].ToString();
                txtCuentaBancaria.Text = dt.Rows[0]["CuentaBancariaEmpleado"].ToString();
                string rutaBD = dt.Rows[0]["FotoEmpleado"] != DBNull.Value? dt.Rows[0]["FotoEmpleado"].ToString() : "";

                if (!string.IsNullOrWhiteSpace(rutaBD))
                {
                    string rutaCompleta = Path.IsPathRooted(rutaBD)
                        ? rutaBD
                        : Path.Combine(Application.StartupPath, rutaBD);

                    if (File.Exists(rutaCompleta))
                    {
                        using (var stream = new FileStream(rutaCompleta, FileMode.Open, FileAccess.Read))
                        {
                            pctEmpleado.Image = Image.FromStream(stream);
                        }
                        pctEmpleado.SizeMode = PictureBoxSizeMode.StretchImage;
                        rutaFotoRelativa = rutaBD;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("No se encontró la foto: " + rutaCompleta);
                    }
                }


                // ⚠️ OJO: pctEmpleado es un PictureBox, no un TextBox.
                // .Text no carga la imagen. Deberías usar una URL o convertir los bytes a Image.
                // pctEmpleado.Image = ... (pendiente si tienes la lógica de conversión)
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Una columna esperada no existe en los datos del empleado: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error de base de datos al cargar el empleado: " + ex.Message,
                        "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la información del empleado: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
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

                string telefono =   mtxbTelefono.Text.Trim();
                if (!string.IsNullOrWhiteSpace(telefono) && telefono.Length < 8)
                {
                    errorProvider.SetError( mtxbTelefono, "El teléfono debe tener al menos 8 dígitos.");
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

                    if (hayErrores)
                    {
                        MessageBox.Show("Por favor corrija los campos marcados en rojo.",
                                "ERROR-CAMVACIO-001", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }


                //DialogResult respuesta = MessageBox.Show(
                //    $"¿Está seguro que desea actualizar los datos del empleado?\n\n" +
                //    $"Nombre: {txtNombre.Text.Trim()} {txtApellido.Text.Trim()}\n" +
                //    $"Cargo: {txtCargo.Text.Trim()}",
                //    "Confirmar Actualización",
                //    MessageBoxButtons.YesNo,
                //    MessageBoxIcon.Question);

                //if (respuesta != DialogResult.Yes) return;

                }
                Empleados empleado = new Empleados();

                empleado.IdEmpleado = idEmpleado;
                empleado.NombreEmpleado = txtNombre.Text.Trim();
                empleado.ApellidoEmpleado = txtApellido.Text.Trim();
                empleado.DUIEmpleado = txtDui.Text.Trim();
                empleado.TelefonoEmpleado =     mtxbTelefono.Text.Trim();
                empleado.CorreoEmpleado = txtCorreo.Text.Trim();
                empleado.CargoEmpleado = txtCargo.Text.Trim();
                empleado.CuentaBancariaEmpleado = txtCuentaBancaria.Text.Trim();

                empleado.FotoEmpleado = rutaFotoRelativa;

                Empleados.ActualizarEmpleado(empleado);

                MessageBox.Show("Empleado actualizado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                EventosGloblales.EnEmpleadosAgregados();

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
                    errorProvider.SetError(txtDui, "Ya existe un empleado con ese DUI.");
                    MessageBox.Show("Ya existe un empleado con esos datos (DUI o correo duplicado).",
                            "ERROR-DADUPLICADO-002", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Error de base de datos al actualizar el empleado: " + ex.Message,
                            "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el empleado: " + ex.Message,
                        "ERROR-ACTUALIZAR-009", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    pctEmpleado.Image = Image.FromFile(rutaDestino);
                    pctEmpleado.SizeMode = PictureBoxSizeMode.StretchImage;
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
    }
}
    
    

