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
            abrirFormulario(new frmMenuGerente());
        }
    }
}
