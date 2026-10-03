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
using Vista.ClientesEmpleado;
using Vista.EmpleadoProducto;
using Vista.EmpleadoServicios;
using Vista.GerenteCalendario;
using Vista.GerenteClientes;
using Vista.IniciarSesion;
using Vista.Utilidades;

namespace Vista.EmpleadoMenu
{
    public partial class frmEmpleadoMenu : Form
    {
        private Size tamañoOriginalFormulario;
        private float tamañoFuenteOriginal;

        public frmEmpleadoMenu()
        {
            InitializeComponent();
            this.Resize += frmEmpleadoMenu_Resize;

            Redondeo.RedondearFig(btnCerrarSesionGere, 6);
            Redondeo.RedondearFig(pnlTituloHora, 6);
            Redondeo.RedondearFig(pnlContenedorTitulo, 6);
        }

        private void frmEmpleadoMenu_Load(object sender, EventArgs e)
        {
            tamañoOriginalFormulario = this.ClientSize;

            tamañoFuenteOriginal = lblTitulo.Font.Size;

            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;

            lblTituloHorario.Dock = DockStyle.Fill;
            lblTituloHorario.TextAlign = ContentAlignment.MiddleCenter;

            AjustarTitulo();

            try
            {
                var img = DatosEmpresa.ObtenerLogo();
                if (img != null)
                {
                    pbLogo1.Image = img;
                    pbLogo1.SizeMode = PictureBoxSizeMode.StretchImage;

                    pbLogo.Image = img;
                    pbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al cargar logo: " + ex.Message);
            }

        }

        private void frmEmpleadoMenu_Resize(object sender, EventArgs e)
        {
            AjustarTitulo();
        }

        private void AjustarTitulo()
        {
            if (tamañoOriginalFormulario.Width <= 0 ||
                tamañoOriginalFormulario.Height <= 0)
                return;

            // Calculamos cuánto cambió el formulario horizontalmente
            float escalaX =(float)this.ClientSize.Width /
                tamañoOriginalFormulario.Width;

            // Calculamos cuánto cambió verticalmente
            float escalaY =(float)this.ClientSize.Height /
                tamañoOriginalFormulario.Height;

            // Utilizamos la escala menor para mantener la proporción
            float escala = Math.Min(escalaX, escalaY);

            // Calculamos el nuevo tamaño de la letra
            float nuevoTamaño =tamañoFuenteOriginal * escala;

            // Límites para evitar que la letra sea demasiado pequeña
            // o demasiado grande
            nuevoTamaño = Math.Max(14, nuevoTamaño);
            nuevoTamaño = Math.Min(40, nuevoTamaño);

            // Aplicamos el nuevo tamaño
            lblTitulo.Font = new Font(
                lblTitulo.Font.FontFamily,
                nuevoTamaño,
                lblTitulo.Font.Style
            );
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
                activeForm.Close();
                pnlContenedorVistas.Controls.Remove(activeForm);
                activeForm.Dispose();
            }

            activeForm = formularioAbrir;
            formularioAbrir.TopLevel = false;
            formularioAbrir.FormBorderStyle = FormBorderStyle.None;
            formularioAbrir.Dock = DockStyle.Fill;

            pnlContenedorVistas.Controls.Add(formularioAbrir);
            formularioAbrir.BringToFront();
            formularioAbrir.Show();
        }

        private void btnInicioGere_Click(object sender, EventArgs e)
        {
            abrirForm(new frmEmlpeadoInicio());
            ActivarBoton(btnInicioGere);

            //Responsive.AjustarFuente(this);
        }

        private void btnInventarioGere_Click(object sender, EventArgs e)
        {
            abrirForm(new frmProductos());
            ActivarBoton(btnInventarioGere);
        }

        private void btnServicios_Click(object sender, EventArgs e)
        {
            abrirForm(new frmServicios());
            ActivarBoton(btnServicios);
        }

        private void btnClienteEmpleado_Click(object sender, EventArgs e)
        {
            abrirForm(new frmClientes());
            ActivarBoton(btnClienteEmpleado);
        }

        private void btnCalendario_Click(object sender, EventArgs e)
        {
            abrirForm(new frmCalendario());
            ActivarBoton(btnCalendario);
        }

        private void RestaurarBotones()
        {
            btnInicioGere.BackColor = Color.FromArgb(241, 206, 184);
            btnInventarioGere.BackColor = Color.FromArgb(241, 206, 184);
            btnServicios.BackColor = Color.FromArgb(241, 206, 184);
            btnClienteEmpleado.BackColor = Color.FromArgb(241, 206, 184);
            btnCalendario.BackColor = Color.FromArgb(241, 206, 184);
        }

        private void ActivarBoton(Button botones)
        {
            RestaurarBotones();

            botones.BackColor = Color.FromArgb(253, 241, 217);
        }

        private void btnCerrarSesionGere_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Está seguro que desea cerrar sesión?",
                   "INFO-CERRAR-SESION", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (respuesta != DialogResult.Yes) return;

                this.Close();

                activeForm = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cerrar sesión: " + ex.Message,
                        "ERROR-CERRARSESION-014", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void BloquearNavegacion(bool bloquear)
        {
            try
            {
                btnInicioGere.Enabled = !bloquear;
                btnInventarioGere.Enabled = !bloquear;
                btnServicios.Enabled = !bloquear;
                btnClienteEmpleado.Enabled = !bloquear;
                btnCalendario.Enabled = !bloquear;
                btnCerrarSesionGere.Enabled = !bloquear;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al bloquear navegación: " + ex.Message);
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
