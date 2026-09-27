using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista.GerenteAyuda
{
    public partial class frmAyuda : Form
    {
        private WebView2 visorPdf;

        public frmAyuda()
        {
            InitializeComponent();
        }

        private async void InicializarVisorPdf()
        {
            try
            {
                if (visorPdf != null) return; // ya está inicializado

                visorPdf = new WebView2
                {
                    Dock = DockStyle.Fill
                };

                pnlVisorPdf.Controls.Add(visorPdf);
                await visorPdf.EnsureCoreWebView2Async(null);

                // Opciones del visor
                visorPdf.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
                visorPdf.CoreWebView2.Settings.AreDevToolsEnabled = false;
                visorPdf.CoreWebView2.Settings.IsZoomControlEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo inicializar el visor de PDF: " + ex.Message,
                        "ERROR-VISOR-PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarPdfEnPanel(string rutaRelativa)
        {
            try
            {
                if (visorPdf == null)
                {
                    MessageBox.Show("El visor aún no está listo, intenta de nuevo.",
                            "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string rutaCompleta = Path.IsPathRooted(rutaRelativa)
                    ? rutaRelativa
                    : Path.Combine(Application.StartupPath, rutaRelativa);

                if (!File.Exists(rutaCompleta))
                {
                    MessageBox.Show("No se encontró el archivo PDF en:\n" + rutaCompleta,
                            "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // El truco: WebView2 renderiza PDFs nativamente usando el motor de Edge
                visorPdf.Source = new Uri(rutaCompleta);
                pnlVisorPdf.Visible = true;
                pnlVisorPdf.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el PDF: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
