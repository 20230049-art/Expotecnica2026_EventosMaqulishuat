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
    public partial class frmInicio : Form
    {
        public frmInicio()
        {
            InitializeComponent();

        }

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

        private void tlpFondo_Click(object sender, EventArgs e)
        {
            abrirFormulario(new frmSeleccionarPefil());
        }

        private void frmInicio_Load(object sender, EventArgs e)
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
    }
}
