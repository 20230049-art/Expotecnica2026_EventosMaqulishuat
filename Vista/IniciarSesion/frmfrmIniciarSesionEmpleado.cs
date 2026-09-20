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
using Vista.EmpleadoServicios;
using Vista.Utilidades;

namespace Vista.IniciarSesion
{
    public partial class frmfrmIniciarSesionEmpleado : Form
    {
        private Usuario usuario = new Usuario();
        private ErrorProvider errorProvider = new ErrorProvider();
        public frmfrmIniciarSesionEmpleado()
        {
            try
            {
                InitializeComponent();
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Resize += (s, e) => AjustarTitulo();
            AjustarTitulo();

            Redondeo.RedondearFig(panel1, 10);
            Redondeo.RedondearFig(panlContraseña, 4);
            Redondeo.RedondearFig(pnlUsuario, 4);
            Redondeo.RedondearFig(btnIngresarEmpleado, 5);
            Redondeo.RedondearFig(btnRegresar, 5);

            ControlesBloqueo.LimitarTextBox(txtContrasena, 55);
            ControlesBloqueo.LimitarTextBox(txtNombreUsuario, 180);

            ControlesBloqueo controlesBloqueo = new ControlesBloqueo();
            controlesBloqueo.BloquearControlesTXT(txtNombreUsuario);
            controlesBloqueo.BloquearControlesTXT(txtNombreUsuario);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void AjustarTitulo()
        {
            if (lblTitulo == null || lblTitulo.IsDisposed) return;

            int anchoDisponible = lblTitulo.Parent?.ClientSize.Width ?? this.ClientSize.Width;

            float tamanoFuente = anchoDisponible / 28f; 

            tamanoFuente = Math.Max(16f, Math.Min(100f, tamanoFuente));

            if (Math.Abs(lblTitulo.Font.Size - tamanoFuente) > 0.1f)
            {
                lblTitulo.Font = new Font(
                    "Book Antiqua",
                    tamanoFuente,
                    FontStyle.Bold
                );
            }
        }

        private void btnIngresarEmpleado_Click(object sender, EventArgs e)
        {
            try
            {
                errorProvider.Clear();

                string nombreUsuario = txtNombreUsuario.Text;
                string contrasena = txtContrasena.Text;

                bool valido = true;

                if (string.IsNullOrEmpty(nombreUsuario))
                {
                    errorProvider.SetError(txtNombreUsuario,"Ingrese su usuario.");
          
                    valido = false;
                }

                if (string.IsNullOrEmpty(contrasena))
                {
                    errorProvider.SetError(txtContrasena, "Ingresa tu contraseña.");

                    valido = false;
                }

                if (!valido)
                {
                    MessageBox.Show( "Por favor ingrese usuario y contraseña.", "ERROR-CAMVACIO-001",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Usuario EmpleadoUsuario = new Usuario();
                Usuario EmpleadoValido = usuario.ValidarLoginEmpleado(nombreUsuario, contrasena);


                if (EmpleadoValido != null)
                {
                    if (EmpleadoValido.IdTipoUsuario == 2)
                    {
                        MessageBox.Show($"¡Bienvenido {EmpleadoValido.NombreUsuario}!", "USUARIO-ACTIVO-1", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        int idEmpleado = EmpleadoValido.IdEmpleado.Value;

                        CuentaAbierta.IdEmpleado = idEmpleado;

                        abrirFormulario(new frmEmpleadoMenu());
                    }
                    else
                    {
                        MessageBox.Show("Tipo de usuario no reconocido.", "ERROR-USUARIO-013", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "ERROR-CREDENCIALES-150", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar sesión: {ex.Message}", "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
