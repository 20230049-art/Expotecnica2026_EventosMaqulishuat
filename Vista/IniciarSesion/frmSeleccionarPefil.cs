using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vista.Utilidades;

namespace Vista.IniciarSesion
{
    public partial class frmSeleccionarPefil : Form
    {
        private Size tamañoOriginalFormulario;
        private float tamañoFuenteOriginal;

        public frmSeleccionarPefil()
        {
            InitializeComponent();
            this.Resize += frmEmpleadoTitulo_Resize;
        }

        private void frmSeleccionarPefil_Load(object sender, EventArgs e)
        {
            tamañoOriginalFormulario = this.ClientSize;

            tamañoFuenteOriginal = lblTituloGerente.Font.Size;

            lblTituloGerente.Dock = DockStyle.Fill;
            lblTituloGerente.TextAlign = ContentAlignment.MiddleCenter;

            //lblTituloHorario.Dock = DockStyle.Fill;
            //lblTituloHorario.TextAlign = ContentAlignment.MiddleCenter;

            AjustarTitulo();
        }

        private void frmEmpleadoTitulo_Resize(object sender, EventArgs e)
        {
            AjustarTitulo();
        }

        private void AjustarTitulo()
        {
            if (tamañoOriginalFormulario.Width <= 0 ||
                tamañoOriginalFormulario.Height <= 0)
                return;

            // Calculamos cuánto cambió el formulario horizontalmente
            float escalaX = (float)this.ClientSize.Width /
                tamañoOriginalFormulario.Width;

            // Calculamos cuánto cambió verticalmente
            float escalaY = (float)this.ClientSize.Height /
                tamañoOriginalFormulario.Height;

            // Utilizamos la escala menor para mantener la proporción
            float escala = Math.Min(escalaX, escalaY);

            // Calculamos el nuevo tamaño de la letra
            float nuevoTamaño = tamañoFuenteOriginal * escala;

            // Límites para evitar que la letra sea demasiado pequeña
            // o demasiado grande
            nuevoTamaño = Math.Max(14, nuevoTamaño);
            nuevoTamaño = Math.Min(40, nuevoTamaño);

            // Aplicamos el nuevo tamaño
            lblTituloGerente.Font = new Font(
                lblTituloGerente.Font.FontFamily,
                nuevoTamaño,
                lblTituloGerente.Font.Style
            );
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
                pnlVistaInicio.Controls.Remove(activeForm);
                activeForm.Dispose();
            }

            activeForm = formularioAbrir;
            formularioAbrir.TopLevel = false;
            formularioAbrir.FormBorderStyle = FormBorderStyle.None;
            formularioAbrir.Dock = DockStyle.Fill;

            pnlVistaInicio.Controls.Add(formularioAbrir);
            formularioAbrir.BringToFront();
            formularioAbrir.Show();
        }

        private void btnSeleccionarGerente_Click_1(object sender, EventArgs e)
        {
            abrirFormulario(new frmIniciarSesionGerente());
        }

        private void btnSeleccionarEmpleado_Click_1(object sender, EventArgs e)
        {
            abrirFormulario(new frmfrmIniciarSesionEmpleado());
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
