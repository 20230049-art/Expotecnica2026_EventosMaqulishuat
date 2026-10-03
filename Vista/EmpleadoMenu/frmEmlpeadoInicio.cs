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

namespace Vista.EmpleadoMenu
{
    public partial class frmEmlpeadoInicio : Form
    {
        public frmEmlpeadoInicio()
        {
            InitializeComponent();
        }

        private void frmEmlpeadoInicio_Load(object sender, EventArgs e)
        {
            try
            {
                var img = DatosEmpresa.ObtenerLogo();
                if (img != null)
                {
                    pbLogo.Image = img;
                    pbLogo.SizeMode = PictureBoxSizeMode.StretchImage;

                    pbLogo1.Image = img;
                    pbLogo1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al cargar logo: " + ex.Message);
            }
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


        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                AbrirUrl("https://drive.google.com/drive/u/2/folders/1_Mno_JF-QkwBpZ_EfwF8ZmGM-gqGk7wU");
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
                AbrirUrl("https://drive.google.com/drive/u/2/folders/1_Mno_JF-QkwBpZ_EfwF8ZmGM-gqGk7wU");
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
                AbrirUrl("https://drive.google.com/drive/u/2/folders/1_Mno_JF-QkwBpZ_EfwF8ZmGM-gqGk7wU");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el manual: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
