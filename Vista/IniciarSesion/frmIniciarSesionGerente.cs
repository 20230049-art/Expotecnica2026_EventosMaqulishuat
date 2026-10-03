using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vista.EmpleadoMenu;
using Vista.GerenteInicio;
using Vista.Utilidades;

namespace Vista.IniciarSesion
{
    public partial class frmIniciarSesionGerente : Form
    {
        private ErrorProvider errorProvider = new ErrorProvider();
        public frmIniciarSesionGerente()
        {
            InitializeComponent();

            Redondeo.RedondearFig(panel1, 10);
            Redondeo.RedondearFig(btnIngresarEmpleado, 6);
            Redondeo.RedondearFig(btnRegresar, 6);

            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;

            txtNombreUsuario.TextChanged += (s, e) => errorProvider.SetError(txtNombreUsuario, "");
            txtContrasena.TextChanged += (s, e) => errorProvider.SetError(txtContrasena, "");
        }

        //Cambio de formualrios
        private Form activeForm = null;
        private void abrirFormulario(Form formularioAbrir)
        {
            try
            {
                if (activeForm != null && activeForm.GetType() == formularioAbrir.GetType())
                {
                    return;
                }

                if (activeForm != null)
                {
                    activeForm.Close();
                    pnlVistaIniciarEmpleado.Controls.Remove(activeForm);
                    activeForm.Dispose();
                }

                activeForm = formularioAbrir;
                formularioAbrir.TopLevel = false;
                formularioAbrir.FormBorderStyle = FormBorderStyle.None;
                formularioAbrir.Dock = DockStyle.Fill;

                pnlVistaIniciarEmpleado.Controls.Add(formularioAbrir);
                formularioAbrir.BringToFront();
                formularioAbrir.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cambiar de formulario: " + ex.Message,
                        "ERROR-CAMBIO-103", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIngresarEmpleado_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            string nombreUsuario = txtNombreUsuario.Text.Trim();
            string contrasena = txtContrasena.Text;

            if (string.IsNullOrEmpty(nombreUsuario))
            {
                errorProvider.SetError(txtNombreUsuario, "Por favor ingrese el usuario.");
            }

            if (string.IsNullOrEmpty(contrasena))
            {
                errorProvider.SetError(txtContrasena, "Por favor ingrese la contraseña.");
            }

            if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(contrasena))
            {
                MessageBox.Show("Por favor ingrese usuario y contraseña.",
                    "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Usuario usuario = new Usuario();
            Usuario usuarioValidado = usuario.ValidarLoginGerente(nombreUsuario, contrasena);

            if (usuarioValidado != null && usuarioValidado.IdTipoUsuario == 1)
            {
                UsuarioActual.Datos = usuarioValidado;

                MessageBox.Show($"¡Bienvenido {usuarioValidado.NombreUsuario}!",
                    "Login exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                abrirFormulario(new frmMenuGerente());
            }
            else if (usuarioValidado != null && usuarioValidado.IdTipoUsuario != 1)
            {
                MessageBox.Show("El usuario no tiene permisos de gerente.",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                errorProvider.SetError(txtNombreUsuario, "Usuario o contraseña incorrectos.");
                errorProvider.SetError(txtContrasena, "Usuario o contraseña incorrectos.");

                MessageBox.Show("Usuario o contraseña incorrectos.",
                    "Error de autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            try
            {
                abrirFormulario(new frmSeleccionarPefil());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al regresar al formulario anterior: " + ex.Message,
                        "ERROR-NAVANTERIOR-104", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmIniciarSesionGerente_Load(object sender, EventArgs e)
        {
            try
            {
                var img = DatosEmpresa.ObtenerLogo();
                if (img != null)
                {
                    pbLogo.Image = img;
                    pbLogo.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al cargar logo: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            frmFondoNegro fondo = new frmFondoNegro();
            fondo.StartPosition = FormStartPosition.CenterParent;
            fondo.WindowState = FormWindowState.Maximized;
            fondo.Show();

            frmRecuperarContrasena formulario = new frmRecuperarContrasena();

            formulario.ShowDialog();

            fondo.Close();
        }
    }
}
