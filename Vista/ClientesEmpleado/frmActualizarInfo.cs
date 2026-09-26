using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarInformacionCliente()
        {
            try
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
            catch (ArgumentException ex)
            {
                MessageBox.Show("Una columna esperada no existe en los datos del cliente: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la información del cliente: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
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
            catch (FormatException ex)
            {
                MessageBox.Show("El tipo de cliente seleccionado no tiene el formato correcto: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al actualizar el cliente en la base de datos: " + ex.Message,
                        "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el cliente: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarTipoCliente()
        {
            try
            {
                cmbTipoCliente.DataSource = TipoClientes.ObtenerTipoCliente();
            cmbTipoCliente.DisplayMember = "TipoCliente";
            cmbTipoCliente.ValueMember = "IdTipoCliente";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los tipos de cliente: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cerrar el formulario: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtDui_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
