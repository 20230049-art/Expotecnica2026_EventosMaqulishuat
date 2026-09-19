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

namespace Vista.ClientesEmpleado
{
    public partial class frmActualizarInfo : Form
    {
        private int idCliente;

        public frmActualizarInfo(int idCliente)
        {
            InitializeComponent();
            this.idCliente = idCliente;
            MostrarInformacionCliente();
            CargarTipoCliente();

            Redondeo.RedondearFormulario(this, 13);
            Redondeo.RedondearFig(pnltituloNombre, 9);
            Redondeo.RedondearFig(btnActualizar, 6);
            Redondeo.RedondearFig(btnSalir, 6);

            ControlesBloqueo controlesBloqueo = new ControlesBloqueo();
            controlesBloqueo.BloquearControlesTXT(txtNombre);
            controlesBloqueo.BloquearControlesTXT(txtApellido);
            controlesBloqueo.BloquearControlesTXT(txtNcr);
            controlesBloqueo.BloquearControlesTXT(txtDui);
            controlesBloqueo.BloquearControlesTXT(txtNit);
            controlesBloqueo.BloquearControlesMTXT(mtxbTelefono);
            controlesBloqueo.BloquearControlesTXT(txtCorreo);

            txtDui.KeyPress += (s, e) => controlesBloqueo.ValidarSoloNumeros((TextBox)s, e);
            txtNcr.KeyPress += (s, e) => controlesBloqueo.ValidarSoloNumeros((TextBox)s, e);
            txtNit.KeyPress += (s, e) => controlesBloqueo.ValidarSoloNumeros((TextBox)s, e);

            txtNombre.KeyPress += (s, e) => controlesBloqueo.ValidarSoloLetras(e);
            txtApellido.KeyPress += (s, e) => controlesBloqueo.ValidarSoloLetras(e);
        }

        private void MostrarInformacionCliente()
        {
            DataTable dt = Clientes.ObtenerClienteId(idCliente);

            if (dt.Rows.Count > 0)
            {
                txtNombre.Text = dt.Rows[0]["NombreCliente"].ToString();
                txtApellido.Text = dt.Rows[0]["ApellidoCliente"].ToString();
                txtNcr.Text = dt.Rows[0]["NCRCliente"].ToString();
                txtDui.Text = dt.Rows[0]["DUICliente"].ToString();
                txtNit.Text = dt.Rows[0]["NITCliente"].ToString();
                mtxbTelefono.Text = dt.Rows[0]["TelefonoCliente"].ToString();
                txtCorreo.Text = dt.Rows[0]["CorreoCliente"].ToString();
                cmbTipoCliente.SelectedValue = dt.Rows[0]["IdTipoCliente"];
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            Clientes cliente = new Clientes();

            cliente.IdCliente = idCliente;
            cliente.NombreCliente = txtNombre.Text;
            cliente.ApellidoCliente = txtApellido.Text;
            cliente.NCRCliente1 = txtNcr.Text;
            cliente.DUICliente1 = txtDui.Text;
            cliente.NITCliente1 = txtNit.Text;
            cliente.TelefonoCliente = mtxbTelefono.Text;
            cliente.CorreoCliente = txtCorreo.Text;
            cliente.IdTipoCliente = Convert.ToInt32(cmbTipoCliente.SelectedValue);

            Clientes.ActualizarCliente(cliente);

            MessageBox.Show("Cliente actualizado correctamente.", "Exito");

            EventosGloblales.EnClienteActualizado();

            this.Close();
        }

        private void CargarTipoCliente()
        {
            cmbTipoCliente.DataSource = TipoClientes.ObtenerTipoCliente();
            cmbTipoCliente.DisplayMember = "TipoCliente";
            cmbTipoCliente.ValueMember = "IdTipoCliente";
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
