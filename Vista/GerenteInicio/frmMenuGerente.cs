using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vista.GerenteClientes;
using Vista.GerenteDocumentacion;
using Vista.GerenteEmpleados;
using Vista.IniciarSesion;
using Vista.Utilidades;

namespace Vista.GerenteInicio
{
    public partial class frmMenuGerente : Form
    {
        public frmMenuGerente()
        {
            try
            {
                InitializeComponent();

                Redondeo.RedondearFig(btnCerrarSesionGere, 16);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cambio de formularios
        private Form activeForm = null;

        private void abrirForm(Form formularioAbrir)
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
                    pnlContenedorVistas.Controls.Remove(activeForm);
                    activeForm.Dispose();
                }

                activeForm = formularioAbrir;
                formularioAbrir.TopLevel = false;
                formularioAbrir.FormBorderStyle = FormBorderStyle.None;
                formularioAbrir.Dock = DockStyle.Fill;

                pnlContenedorVistas.Controls.Add(formularioAbrir);
                formularioAbrir.BringToFront();
                formularioAbrir.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cambiar de formulario: " + ex.Message,
                        "ERROR-CAMBIO-103", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInicioGere_Click(object sender, EventArgs e)
        {
            abrirForm(new frmInicioGerente());
            ActivarBoton(btnInicioGere);
        }

        private void btnInventarioGere_Click(object sender, EventArgs e)
        {

        }

        private void btnClienteGere_Click(object sender, EventArgs e)
        {
            abrirForm(new frmClientesTotales());
            ActivarBoton(btnClienteGere);
        }

        private void btnCalendarioGere_Click(object sender, EventArgs e)
        {

        }

        private void btnRegisPagoGere_Click(object sender, EventArgs e)
        {

        }

        private void btnRegistroVentas_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            abrirForm(new frmDocumentos());
            ActivarBoton(button3);
        }

        private void btnEmpleado_Click(object sender, EventArgs e)
        {
            abrirForm(new frmEmpleadosGerente());
            ActivarBoton(btnEmpleado);
        }

        private void btnProveedores_Click(object sender, EventArgs e)
        {

        }

        private void btnReportes_Click(object sender, EventArgs e)
        {

        }

        private void btnCerrarSesionGere_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Está seguro que desea cerrar sesión?",
                   "INFO-CERRAR-SESION", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (respuesta != DialogResult.Yes) return;

                frmSeleccionarPefil frmSeleccionar = new frmSeleccionarPefil();

                this.pnlContenedorVistas.Visible = false;
                frmSeleccionar.TopLevel = false;
                frmSeleccionar.FormBorderStyle = FormBorderStyle.None;
                frmSeleccionar.Dock = DockStyle.Fill;

                pnlContenedorVistas.Controls.Clear();
                pnlContenedorVistas.Controls.Add(frmSeleccionar);
                frmSeleccionar.Show();

                activeForm = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cerrar sesión: " + ex.Message,
                        "ERROR-CERRARSESION-014", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RestaurarBotones()
        {
            try
            {
                btnInicioGere.BackColor = Color.FromArgb(241, 206, 184);
                btnInventarioGere.BackColor = Color.FromArgb(241, 206, 184);
                btnClienteGere.BackColor = Color.FromArgb(241, 206, 184);
                btnCalendarioGere.BackColor = Color.FromArgb(241, 206, 184);
                btnRegisPagoGere.BackColor = Color.FromArgb(241, 206, 184);
                btnRegistroVentas.BackColor = Color.FromArgb(241, 206, 184);
                btnDocumentosGere.BackColor = Color.FromArgb(241, 206, 184);
                btnEmpleado.BackColor = Color.FromArgb(241, 206, 184);
                btnProveedores.BackColor = Color.FromArgb(241, 206, 184);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al restaurar botones: " + ex.Message);
            }
        }

        private void ActivarBoton(Button botones)
        {
            RestaurarBotones();

            botones.BackColor = Color.FromArgb(253, 241, 217);
        }
    }
}
