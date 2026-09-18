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

namespace Vista.EmpleadosVenta
{
    public partial class frmCompraFinal : Form
    {
        private int idVentaActiva;
        private string nombreCliente;
        private FacturaCompleta facturaActual;
        private bool ventaFinalizada = false;

        public frmCompraFinal(int idVenta, string cliente)
        {
            InitializeComponent();
        }
    }
}
