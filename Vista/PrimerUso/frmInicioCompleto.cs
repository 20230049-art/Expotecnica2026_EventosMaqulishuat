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

namespace Vista.PrimerUso
{
    public partial class frmInicioCompleto : Form
    {
        public frmInicioCompleto()
        {
            InitializeComponent();
        }

        private void frmInicioCompleto_Load(object sender, EventArgs e)
        {
            try
            {
                // Mostrar datos del gerente
                lblUsuario.Text = "Usuario: " + DatosInicioCompleto.UsuarioGerente;
                lblContrasena.Text = "Contraseña: " + DatosInicioCompleto.ContrasenaGerente;

                // Mostrar logo
                var img = DatosEmpresa.ObtenerLogo();
                if (img != null)
                {
                    pbLogo.Image = img;
                    pbLogo.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al cargar datos: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
