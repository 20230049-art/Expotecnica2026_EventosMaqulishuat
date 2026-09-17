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

namespace Vista.EmpleadoServicios
{
    public partial class frmServicios : Form
    {
        public frmServicios()
        {
            InitializeComponent();

            Redondeo.RedondearFig(panel2, 9);
            Redondeo.RedondearFig(panel10, 9);
            Redondeo.RedondearFig(panel11, 9);
            Redondeo.RedondearFig(panel12, 9);
            Redondeo.RedondearFig(panel13, 9);
            Redondeo.RedondearFig(panel14,9);
            Redondeo.RedondearFig(panel16, 9);
            Redondeo.RedondearFig(panel3, 9); 
            Redondeo.RedondearFig(panel4, 9);
            Redondeo.RedondearFig(panel5, 9);
            Redondeo.RedondearFig(panel6, 9);
            Redondeo.RedondearFig(panel7, 9);
            Redondeo.RedondearFig(panel8, 9);
            Redondeo.RedondearFig(panel9, 9);
            Redondeo.RedondearFig(panel1, 9);
        }

        //Cambio de formularios
        private Form activeForm = null;

        private void abrirForm(Form formularioAbrir)
        {
            if (activeForm != null && activeForm.GetType() == formularioAbrir.GetType())
            {
                return;
            }

            if (activeForm != null)
            {
                activeForm.Hide();
                pnlVistaServicios.Controls.Remove(activeForm);
                //activeForm.Dispose();
            }

            activeForm = formularioAbrir;
            formularioAbrir.TopLevel = false;
            formularioAbrir.FormBorderStyle = FormBorderStyle.None;
            formularioAbrir.Dock = DockStyle.Fill;

            pnlVistaServicios.Controls.Add(formularioAbrir);
            formularioAbrir.BringToFront();
            formularioAbrir.Show();
        }

        private void panel5_Click_1(object sender, EventArgs e)
        {
            abrirForm(new frmServiciosInmoviliarios());
        }

        private void lblTituloMobiliario_Click(object sender, EventArgs e)
        {
            abrirForm(new frmServiciosInmoviliarios());
        }

        private void lblTextoMobiliario_Click(object sender, EventArgs e)
        {
            abrirForm(new frmServiciosInmoviliarios());
        }

        private void panel6_Click_1(object sender, EventArgs e)
        {
            abrirForm(new frmServiciosCristaleria());
        }

        private void lblTituloCristaleria_Click(object sender, EventArgs e)
        {
            abrirForm(new frmServiciosCristaleria());
        }

        private void lbltTextoCristaleria_Click(object sender, EventArgs e)
        {
            abrirForm(new frmServiciosCristaleria());
        }

        private void pnlImagenCuberteria_Click_1(object sender, EventArgs e)
        {
            abrirForm(new frmServiciosCuberteria());
        }

        private void lblTituloCuberteria_Click(object sender, EventArgs e)
        {
            abrirForm(new frmServiciosCuberteria());
        }

        private void lblTextoCuberteria_Click(object sender, EventArgs e)
        {
            abrirForm(new frmServiciosCuberteria());
        }

        private void panel4_Click_1(object sender, EventArgs e)
        {
            abrirForm(new frmServiciosDiscomovileIluminación());
        }

        private void label1_Click(object sender, EventArgs e)
        {
            abrirForm(new frmServiciosDiscomovileIluminación());
        }

        private void lblTextoIluminacion_Click(object sender, EventArgs e)
        {
            abrirForm(new frmServiciosDiscomovileIluminación());
        }

        private void pnlImagenAnimacion_Click_1(object sender, EventArgs e)
        {
            abrirForm(new frmServiciosAnimacion());
        }

        private void lblTituloBaile_Click(object sender, EventArgs e)
        {
            abrirForm(new frmServiciosAnimacion());
        }

        private void lblTextoBaile_Click(object sender, EventArgs e)
        {
            abrirForm(new frmServiciosAnimacion());
        }

        private void pnlImagenDecoracion_Click_1(object sender, EventArgs e)
        {
            abrirForm(new frmServiciosDecoracion());
        }

        private void lblTituloDecoracion_Click(object sender, EventArgs e)
        {
            abrirForm(new frmServiciosDecoracion());
        }

        private void lblTextoDecoracion_Click(object sender, EventArgs e)
        {
            abrirForm(new frmServiciosDecoracion());
        }
    }
}
