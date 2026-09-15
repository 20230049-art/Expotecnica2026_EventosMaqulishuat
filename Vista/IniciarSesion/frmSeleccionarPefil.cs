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
        public frmSeleccionarPefil()
        {
            InitializeComponent();
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
    }
}
