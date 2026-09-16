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

            Redondeo.RedondearFig(btnCerrarSesionGere, 4);
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
            //abrirForm(new frmCalendarioInicio());
            ActivarBoton(btnCalendario);
        }

        private void btnCerrarSesionGere_Click(object sender, EventArgs e)
        {
            frmSeleccionarPefil frmSeleccionar = new frmSeleccionarPefil();

            this.pnlContenedorVistas.Visible = false;
            frmSeleccionar.TopLevel = false;
            frmSeleccionar.FormBorderStyle = FormBorderStyle.None;
            frmSeleccionar.Dock = DockStyle.Fill;

            pnlContenedorVistas.Controls.Clear();
            pnlContenedorVistas.Controls.Add(frmSeleccionar);
            frmSeleccionar.Show();
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

        private void lblTituloHorario_Click(object sender, EventArgs e)
        {

        }
    }
}
