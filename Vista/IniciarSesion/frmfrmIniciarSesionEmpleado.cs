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
    public partial class frmfrmIniciarSesionEmpleado : Form
    {

        public frmfrmIniciarSesionEmpleado()
        {
            InitializeComponent();
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Resize += (s, e) => AjustarTitulo();
            AjustarTitulo();
        }

        private void AjustarTitulo()
        {
            if (lblTitulo == null || lblTitulo.IsDisposed) return;

            // El label está dentro de un panel, usamos el ancho del panel padre
            int anchoDisponible = lblTitulo.Parent?.ClientSize.Width ?? this.ClientSize.Width;

            // Calcular tamaño de fuente proporcional al ancho
            // Ajusta los valores según tu diseño
            float tamanoFuente = anchoDisponible / 30f; // Entre más grande el divisor, más pequeña la fuente

            // Aplicar límites
            tamanoFuente = Math.Max(16f, Math.Min(100f, tamanoFuente));

            // Solo cambiar si es necesario
            if (Math.Abs(lblTitulo.Font.Size - tamanoFuente) > 0.1f)
            {
                lblTitulo.Font = new Font(
                    "Book Antiqua",
                    tamanoFuente,
                    FontStyle.Bold
                );
            }
        }

        private void lblTitulo_Click(object sender, EventArgs e)
        {
            
        }
    }
}
