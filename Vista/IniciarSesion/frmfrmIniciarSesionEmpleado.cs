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
using Vista.Utilidades;

namespace Vista.IniciarSesion
{
    public partial class frmfrmIniciarSesionEmpleado : Form
    {

        public frmfrmIniciarSesionEmpleado()
        {
            InitializeComponent();
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Resize += (s, e) => AjustarTitulo();
            AjustarTitulo();

            Redondeo.RedondearFig(panel1, 8);
            Redondeo.RedondearFig(panlContraseña, 4);
            Redondeo.RedondearFig(pnlUsuario, 4);
            Redondeo.RedondearFig(btnIngresarEmpleado, 4);
        }

        //Cambio de formualrios
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

        private void AjustarTitulo()
        {
            if (lblTitulo == null || lblTitulo.IsDisposed) return;

            // El label está dentro de un panel, usamos el ancho del panel padre
            int anchoDisponible = lblTitulo.Parent?.ClientSize.Width ?? this.ClientSize.Width;

            // Calcular tamaño de fuente proporcional al ancho
            // Ajusta los valores según tu diseño
            float tamanoFuente = anchoDisponible / 30f; // Entre más grande el divisor, más pequeña la fuente

            // Aplicar límites
            tamanoFuente = Math.Max(16f, Math.Min(100f, tamanoFuente));

            // Solo cambiar si es necesario
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
        //    try
        //    {
        //        string nombreUsuario = txtNombreUsuario.Text;
        //        string contrasena = txtContrasena.Text;

        //        if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(contrasena))
        //        {
        //            MessageBox.Show("Por favor ingrese usuario y contraseña.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        Usuario EmpleadoUsuario = new Usuario();
        //        Usuario EmpleadoValido = usuario.ValidarLoginEmpleado(nombreUsuario, contrasena);

        //        if (EmpleadoValido != null)
        //        {
        //            if (EmpleadoValido.IdTipoUsuario == 2)
        //            {
        //                MessageBox.Show($"¡Bienvenido {EmpleadoValido.NombreUsuario}!", "Login exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //                int idEmpleado = EmpleadoValido.IdEmpleado.Value;

        //                CuentaAbierta.IdEmpleado = idEmpleado;

                        abrirFormulario(new frmEmpleadoMenu());
        //            }
        //            else
        //            {
        //                MessageBox.Show("Tipo de usuario no reconocido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Usuario o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error al iniciar sesión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        }
    }
}
