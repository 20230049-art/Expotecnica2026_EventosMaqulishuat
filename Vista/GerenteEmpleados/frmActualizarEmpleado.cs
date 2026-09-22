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

namespace Vista.GerenteEmpleados
{
    public partial class frmActualizarEmpleado : Form
    {
        private int idEmpleado;
        private ErrorProvider errorProvider;
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
                txtTelefono.Text = dt.Rows[0]["TelefonoEmpleado"].ToString();
                txtCorreo.Text = dt.Rows[0]["CorreoEmpleado"].ToString();
                txtCuentaBancaria.Text = dt.Rows[0]["CuentaBancariaEmpleado"].ToString();

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
                else if (dui.Length != 9)
                {
                    errorProvider.SetError(txtDui, "El DUI debe tener 9 dígitos.");
                    hayErrores = true;
                }

                string telefono = txtTelefono.Text.Trim();
                if (!string.IsNullOrWhiteSpace(telefono) && telefono.Length < 8)
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

                    Empleados empleado = new Empleados();

                    empleado.IdEmpleado = idEmpleado;
                    empleado.NombreEmpleado = txtNombre.Text.Trim();
                    empleado.ApellidoEmpleado = txtApellido.Text.Trim();
                    empleado.DUIEmpleado = txtDui.Text.Trim();
                    empleado.TelefonoEmpleado = txtTelefono.Text.Trim();
                    empleado.CorreoEmpleado = txtCorreo.Text.Trim();
                    empleado.CargoEmpleado = txtCargo.Text.Trim();
                    empleado.CuentaBancariaEmpleado = txtCuentaBancaria.Text.Trim();

                    empleado.FotoEmpleado = pctEmpleado.Text;

                    Empleados.ActualizarEmpleado(empleado);

                    MessageBox.Show("Empleado actualizado correctamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Close();
                }
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
    }
}
    
    

