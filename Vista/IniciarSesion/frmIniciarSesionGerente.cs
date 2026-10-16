using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
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
            Redondeo.RedondearFig(btnIngresarEmpleado, 2);
            Redondeo.RedondearFig(btnRegresar, 2);

            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;

            txtNombreUsuario.TextChanged += (s, e) => errorProvider.SetError(txtNombreUsuario, "");
            txtContrasena.TextChanged += (s, e) => errorProvider.SetError(txtContrasena, "");
        }

        //Cmbio de formularios 
        private Form activeForm = null;
        private void abrirFormulario(Form formularioAbrir)
        {
            if (activeForm != null && activeForm.GetType() == formularioAbrir.GetType())
            {
                return;
            }

            if (activeForm != null)
            {
                activeForm.Close();
                pnlVistaIniciarEmpleado1.Controls.Remove(activeForm);
                activeForm.Dispose();
            }

            activeForm = formularioAbrir;
            formularioAbrir.TopLevel = false;
            formularioAbrir.FormBorderStyle = FormBorderStyle.None;
            formularioAbrir.Dock = DockStyle.Fill;

            pnlVistaIniciarEmpleado1.Controls.Add(formularioAbrir);
            formularioAbrir.BringToFront();
            formularioAbrir.Show();
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

        private void label2_Click_1(object sender, EventArgs e)
        {
            frmFondoNegro fondo = new frmFondoNegro();
            fondo.StartPosition = FormStartPosition.CenterParent;
            fondo.WindowState = FormWindowState.Maximized;
            fondo.Show();

            frmRecuperarContrasena formulario = new frmRecuperarContrasena();

            formulario.ShowDialog();

            fondo.Close();
        }

       
        private void btnRegresar_Click(object sender, EventArgs e)
        {
            try
            {

                using (var frm = new frmSeleccionarPefil())
                {
                    frm.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al regresar al formulario anterior: " + ex.Message,
                        "ERROR-NAVANTERIOR-104", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIngresarEmpleado_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            string nombreUsuario = txtNombreUsuario.Text.Trim();
            string contrasena = txtContrasena.Text;

            bool hayError = false;

            if (string.IsNullOrEmpty(nombreUsuario))
            {
                errorProvider.SetError(txtNombreUsuario, "Por favor ingrese el usuario.");
                hayError = true;
            }

            if (string.IsNullOrEmpty(contrasena))
            {
                errorProvider.SetError(txtContrasena, "Por favor ingrese la contraseña.");
                hayError = true;
            }

            if (hayError)
            {
                MessageBox.Show("Por favor ingrese usuario y contraseña.",
                    "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Usuario usuarioValidado;
            try
            {
                usuarioValidado = new Usuario().ValidarLoginGerente(nombreUsuario, contrasena);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error de conexión con la base de datos: " + ex.Message,
                    "ERROR-SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado al iniciar sesión: " + ex.Message,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (usuarioValidado == null)
            {
                errorProvider.SetError(txtNombreUsuario, "Usuario o contraseña incorrectos.");
                errorProvider.SetError(txtContrasena, "Usuario o contraseña incorrectos.");

                MessageBox.Show("Usuario o contraseña incorrectos.",
                    "Error de autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (usuarioValidado.IdTipoUsuario != 1)
            {
                MessageBox.Show("El usuario no tiene permisos de gerente.",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UsuarioActual.Datos = usuarioValidado;

            MessageBox.Show($"¡Bienvenido {usuarioValidado.NombreUsuario}!",
                "Login exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            abrirFormulario(new frmMenuGerente());
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

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtContrasena_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
