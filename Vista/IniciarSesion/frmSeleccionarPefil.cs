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

            Redondeo.RedondearFig(panel1, 12);
            Redondeo.RedondearFig(panel3, 7);
            Redondeo.RedondearFig(panel4, 5);
            Redondeo.RedondearFig(panel6, 5);
            Redondeo.RedondearFig(btnSeleccionarGerente, 6);
            Redondeo.RedondearFig(btnSeleccionarEmpleado, 6);

            Responsive.Apply(this);
        }

        private void frmSeleccionarPefil_Load(object sender, EventArgs e)
        {
            try
            {
                var img = DatosEmpresa.ObtenerLogo();
                if (img != null)
                {
                    pbLogo.Image = img;
                    pbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al cargar logo: " + ex.Message);
            }

            tamañoOriginalFormulario = this.ClientSize;

            tamañoFuenteOriginal = lblTituloGerente.Font.Size;

            lblTituloGerente.Dock = DockStyle.Fill;
            lblTituloGerente.TextAlign = ContentAlignment.MiddleCenter;
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

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
