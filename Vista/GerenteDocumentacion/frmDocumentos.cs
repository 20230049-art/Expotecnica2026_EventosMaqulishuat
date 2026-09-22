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

namespace Vista.GerenteDocumentacion
{
    public partial class frmDocumentos : Form
    {
        private bool modoEliminar = false;
        private bool modoRestaurar = false;
        private bool modoEliminarPermanentemente = false;
        private int idDocumentoSeleccionado = 0;
        private DataRow filaSeleccionada = null;
        private Form activeForm = null;

        private ErrorProvider errorProvider;
        public frmDocumentos()
        {
            try
            {
                InitializeComponent();

                errorProvider = new ErrorProvider();
                errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
                errorProvider.ContainerControl = this;

                DataTable documentos = Documentos.MostrarDocumentos();
                CargarDocumentosEnPantalla(documentos);

                EventosGloblales.DocumentosAgregados += RegarcarPanelDocumentos;
                EventosGloblales.DocumentosEliminados += RegarcarPanelDocumentos;

                try
                {
                    Redondeo.RedondearFig(pnlVistaMisDocumentos, 15);
                    Redondeo.RedondearFig(pnlContenedor, 15);
                    Redondeo.RedondearFig(panel14, 15);
                    Redondeo.RedondearFig(panel5, 15);
                    Redondeo.RedondearFig(panel13, 15);
                }
                catch (Exception exResize)
                {
                    System.Diagnostics.Debug.WriteLine("Error al redondear figuras: " + exResize.Message);
                }

                txtBarraBuscar.TextChanged += (s, e) => errorProvider.SetError(txtBarraBuscar, "");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RestaurarBotones()
        {
            try
            {
                btnDocumentosFavoritos.BackColor = Color.FromArgb(244, 220, 197);
                btnDocumentosPapelera.BackColor = Color.FromArgb(244, 220, 197);
                btnMisDocumentos.BackColor = Color.FromArgb(244, 220, 197);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al restaurar botones: " + ex.Message);
            }
        }
    }
}
