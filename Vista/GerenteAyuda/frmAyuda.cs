using Microsoft.Web.WebView2.WinForms;
using Modelos.Entidades;
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
using Vista.Utilidades;

namespace Vista.GerenteAyuda
{
    public partial class frmAyuda : Form
    {
        public frmAyuda()
        {
            InitializeComponent();

            Redondeo.RedondearFig(panel1, 8);
            Redondeo.RedondearFig(panel4, 8);
            Redondeo.RedondearFig(panel2, 6);
            Redondeo.RedondearFig(panel3, 6);
            Redondeo.RedondearFig(button5, 5);
            Redondeo.RedondearFig(button1, 5);
        }

        private void AbrirUrl(string url)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                {
                    MessageBox.Show("No hay un enlace configurado.",
                            "ERROR-CAMVACIO-001", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                MessageBox.Show("No se pudo abrir el navegador: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el enlace: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmAyuda_Load(object sender, EventArgs e)
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

        private void button5_Click_1(object sender, EventArgs e)
        {
            try
            {
                AbrirUrl("https://drive.google.com/drive/folders/1YkG44-1j2Mhz9Dr5XbnV_ubzIiF0E-tj?usp=drive_link");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el manual: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                AbrirUrl("https://drive.google.com/drive/folders/1YkG44-1j2Mhz9Dr5XbnV_ubzIiF0E-tj?usp=drive_link");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el manual: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                AbrirUrl("https://drive.google.com/drive/folders/1YkG44-1j2Mhz9Dr5XbnV_ubzIiF0E-tj?usp=drive_link");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el manual: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                AbrirUrl("https://drive.google.com/drive/folders/1YkG44-1j2Mhz9Dr5XbnV_ubzIiF0E-tj?usp=drive_link");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el manual: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                AbrirUrl("https://drive.google.com/drive/folders/1YkG44-1j2Mhz9Dr5XbnV_ubzIiF0E-tj?usp=drive_link");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el manual: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVerTuTorialRecuperar_Click(object sender, EventArgs e)
        {
            try
            {
                AbrirUrl("https://drive.google.com/drive/folders/1YkG44-1j2Mhz9Dr5XbnV_ubzIiF0E-tj?usp=drive_link");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el manual: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
