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

namespace Vista.IniciarSesion
{
    public partial class frmRecuperarContrasena : Form
    {
        private Usuario usuario = new Usuario();
        private string correoActual = "";
        private ErrorProvider errorProvider;
        private bool codigoVerificado = false;

        public frmRecuperarContrasena()
        {
            InitializeComponent();

            errorProvider = new ErrorProvider();
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;

            txtCorreo.TextChanged += (s, e) => errorProvider.SetError(txtCorreo, "");
            txtCodigo.TextChanged += (s, e) => errorProvider.SetError(txtCodigo, "");
            txtNuevaContrasena.TextChanged += (s, e) => errorProvider.SetError(txtNuevaContrasena, "");
            txtConfirmarContrasena.TextChanged += (s, e) => errorProvider.SetError(txtConfirmarContrasena, "");

            Redondeo.RedondearFig(btnValidarCodigo, 6);
            Redondeo.RedondearFig(btnEnviarCodigo, 6);
            Redondeo.RedondearFig(btnNuevaContraseña, 6);

            txtCodigo.Visible = false;
            label5.Visible = false;
            btnValidarCodigo.Visible = false;
            lblCorreo.Visible = false;
            txtNuevaContrasena.Visible = false;
            lblUsuario.Visible = false;
            txtConfirmarContrasena.Visible = false;
            btnNuevaContraseña.Visible = false;
        }

        private void btnEnviarCodigo_Click(object sender, EventArgs e)
        {
            try
            {
                errorProvider.Clear();

                string correo = txtCorreo.Text.Trim();

                if (string.IsNullOrWhiteSpace(correo))
                {
                    errorProvider.SetError(txtCorreo, "Ingrese su correo electrónico.");
                    MessageBox.Show("Ingrese su correo electrónico.",
                            "ERROR-CAMVACIO-001", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    var mail = new System.Net.Mail.MailAddress(correo);
                }
                catch (FormatException)
                {
                    errorProvider.SetError(txtCorreo, "El correo no tiene un formato válido.");
                    MessageBox.Show("El correo no tiene un formato válido.",
                            "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult respuesta = MessageBox.Show(
                    $"¿Enviar código de recuperación a:\n\n{correo}?",
                    "Confirmar envío",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (respuesta != DialogResult.Yes) return;

                string codigo = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();

                if (!usuario.GuardarCodigoRecuperacion(correo, codigo))
                {
                    errorProvider.SetError(txtCorreo, "El correo no está registrado.");
                    MessageBox.Show("El correo no está registrado en el sistema.",
                            "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (Correo.EnviarCodigoRecuperacion(correo, codigo))
                {
                    correoActual = correo;

                    MessageBox.Show("Se envió un código a tu correo. Revisa tu bandeja.",
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtCodigo.Visible = true;
                    btnValidarCodigo.Visible = true;
                    btnEnviarCodigo.Visible = false;
                    txtCorreo.Visible = false;

                    txtCodigo.Focus();
                }
                else
                {
                    MessageBox.Show("No se pudo enviar el correo. Verifique su conexión a internet.",
                            "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error de base de datos al enviar el código: " + ex.Message,
                        "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el código de recuperación: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnValidarCodigo_Click(object sender, EventArgs e)
        {
            try
            {
                errorProvider.Clear();

            string codigo = txtCodigo.Text.Trim();

            if (string.IsNullOrWhiteSpace(codigo))
            {
                errorProvider.SetError(txtCodigo, "Ingrese el código que recibió.");
                MessageBox.Show("Ingrese el código que recibió.",
                        "ERROR-CAMVACIO-001", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (codigo.Length != 6)
            {
                errorProvider.SetError(txtCodigo, "El código debe tener 6 caracteres.");
                MessageBox.Show("El código debe tener 6 caracteres.",
                        "ERROR-LONGITUD-003", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
    
            if (!usuario.ValidarCodigoRecuperacion(correoActual, codigo))
            {
                errorProvider.SetError(txtCodigo, "Código inválido o expirado.");
                MessageBox.Show("Código inválido o expirado. Verifique el código recibido.",
                        "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

                codigoVerificado = true;
                MessageBox.Show("Código válido. Ahora ingresa tu nueva contraseña.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtNuevaContrasena.Visible = true;
            txtConfirmarContrasena.Visible = true;
            btnNuevaContraseña.Visible = true;
            txtCodigo.Visible = true;
            btnValidarCodigo.Visible = true;
            lblCorreo.Visible = true;
                lblUsuario.Visible = true;

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error de base de datos al validar el código: " + ex.Message,
                        "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al validar el código: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevaContraseña_Click(object sender, EventArgs e)
        {
            try
            {
                errorProvider.Clear();
                bool hayErrores = false;

                if (!codigoVerificado)
                {
                    MessageBox.Show(
                        "Primero debe verificar el código de recuperación.",
                        "Código no verificado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                string nueva = txtNuevaContrasena.Text;
                string confirmar = txtConfirmarContrasena.Text;

                if (string.IsNullOrWhiteSpace(nueva))
                {
                    errorProvider.SetError(txtNuevaContrasena, "Ingrese la nueva contraseña.");
                    hayErrores = true;
                }

                if (string.IsNullOrWhiteSpace(confirmar))
                {
                    errorProvider.SetError(txtConfirmarContrasena, "Confirme la nueva contraseña.");
                    hayErrores = true;
                }

                if (!string.IsNullOrWhiteSpace(nueva) && nueva.Length < 6)
                {
                    errorProvider.SetError(txtNuevaContrasena, "Debe tener al menos 6 caracteres.");
                    hayErrores = true;
                }

                if (!string.IsNullOrWhiteSpace(nueva) && !string.IsNullOrWhiteSpace(confirmar)
                    && nueva != confirmar)
                {
                    errorProvider.SetError(txtConfirmarContrasena, "Las contraseñas no coinciden.");
                    hayErrores = true;
                }

                if (hayErrores)
                {
                    MessageBox.Show("Por favor corrija los campos marcados en rojo.",
                            "ERROR-CAMVACIO-001", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult respuesta = MessageBox.Show(
                    "¿Está seguro que desea cambiar la contraseña?\n\n" +
                    "Después de esto, deberá iniciar sesión con la nueva contraseña.",
                    "Confirmar cambio",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (respuesta != DialogResult.Yes) return;

                if (usuario.ActualizarContrasenaRecuperacion(correoActual, nueva))

                {
                    MessageBox.Show("Contraseña actualizada correctamente.",
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar la contraseña. Verifique el código.",
                            "ERROR-ACTUALIZAR-009", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error de base de datos al cambiar la contraseña: " + ex.Message,
                        "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cambiar la contraseña: " + ex.Message,
                        "ERROR-ACTUALIZAR-009", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmRecuperarContrasena_Load(object sender, EventArgs e)
        {

        }
    }
}

