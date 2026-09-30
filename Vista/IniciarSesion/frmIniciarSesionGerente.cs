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

namespace Vista.IniciarSesion
{
    public partial class frmIniciarSesionGerente : Form
    {
        public frmIniciarSesionGerente()
        {
            InitializeComponent();
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
            string nombreUsuario = txtNombreUsuario.Text.Trim();
            string contrasena = txtContrasena.Text;

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
    }
}
