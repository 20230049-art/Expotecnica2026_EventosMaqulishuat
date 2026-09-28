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
    public partial class frmConfiguracionInicial : Form
    {
        public frmConfiguracionInicial()
        {
            InitializeComponent();
        }

        private void lblDatosParaUsuario_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnFinalizarConfiguracion_Click(object sender, EventArgs e)
        {
            frmInicioCompleto formualrio = new frmInicioCompleto();
            formualrio.ShowDialog();
        }
    }
}
