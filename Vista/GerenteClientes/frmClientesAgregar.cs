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

namespace Vista.GerenteClientes
{
    public partial class frmClientesAgregar : Form
    {
        public frmClientesAgregar()
        {
            try
            {
                InitializeComponent();
                ControlesBloqueo controlesBloqueo = new ControlesBloqueo();

                controlesBloqueo.BloquearControlesTXT(txtNombre);
                controlesBloqueo.BloquearControlesTXT(txtApellido);
                controlesBloqueo.BloquearControlesTXT(txtNcr);
                //controlesBloqueo.BloquearControlesTXT(txtDui);
                controlesBloqueo.BloquearControlesTXT(txtNit);
                controlesBloqueo.BloquearControlesMTXT(mtxbTelefono);
                controlesBloqueo.BloquearControlesTXT(txtCorreo);

                txtNombre.KeyPress += (s, e) => controlesBloqueo.ValidarSoloLetras(e);
                txtApellido.KeyPress += (s, e) => controlesBloqueo.ValidarSoloLetras(e);

                txtDui.KeyPress += (s, e) => controlesBloqueo.ValidarSoloNumeros((TextBox)s, e);
                txtNcr.KeyPress += (s, e) => controlesBloqueo.ValidarSoloNumeros((TextBox)s, e);
                txtNit.KeyPress += (s, e) => controlesBloqueo.ValidarSoloNumeros((TextBox)s, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmClientesAgregar_Load(object sender, EventArgs e)
        {
            try
            {
                CargarTipoCliente();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtApellido.Text))
                {
                    MessageBox.Show("Debe completar el nombre y el apellido del cliente.",
                            "ERROR-CAMVACIO-001", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbTipoCliente.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un tipo de cliente.",
                            "ERROR-CAMVACIO-001", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string dui = txtDui.Text.Trim();
                if (!string.IsNullOrEmpty(dui) && dui.Length != 9)
                {
                    MessageBox.Show("El DUI debe tener 9 dígitos.",
                            "ERROR-LONGITUD-003", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string nit = txtNit.Text.Trim();
                if (!string.IsNullOrEmpty(nit) && nit.Length != 14)
                {
                    MessageBox.Show("El NIT debe tener 14 dígitos.",
                            "ERROR-LONGITUD-003", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string telefono = mtxbTelefono.Text.Trim().Replace("-", "").Replace(" ", "");
                if (!string.IsNullOrEmpty(telefono) && telefono.Length < 8)
                {
                    MessageBox.Show("El teléfono debe tener al menos 8 dígitos.",
                            "ERROR-LONGITUD-003", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Clientes cliente = new Clientes();

                cliente.NombreCliente = txtNombre.Text.Trim();
                cliente.ApellidoCliente = txtApellido.Text.Trim();
                cliente.NCRCliente1 = txtNcr.Text.Trim();
                cliente.DUICliente1 = dui;
                cliente.NITCliente1 = nit;
                cliente.TelefonoCliente = mtxbTelefono.Text.Trim();
                cliente.CorreoCliente = txtCorreo.Text.Trim();
                cliente.IdTipoCliente = Convert.ToInt32(cmbTipoCliente.SelectedValue);

                Clientes.AgregarCliente(cliente);

                MessageBox.Show("Cliente agregado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                EventosGloblales.EnClienteAgregado();

                // Limpiar controles
                txtNombre.Clear();
                txtApellido.Clear();
                mtxbTelefono.Clear();
                txtCorreo.Clear();
                cmbTipoCliente.SelectedIndex = -1;
                txtNcr.Clear();
                txtDui.Clear();
                txtNit.Clear();

            }
            catch (FormatException ex)
            {
                MessageBox.Show("El tipo de cliente seleccionado no tiene el formato correcto: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show("Ya existe un cliente con esos datos (DUI, NIT o NCR duplicado).",
                            "ERROR-DADUPLICADO-002", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Error de base de datos al agregar el cliente: " + ex.Message,
                            "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el cliente: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarTipoCliente()
        {
            try
            {
                DataTable dtTipos = TipoClientes.ObtenerTipoCliente();

                if (dtTipos == null || dtTipos.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron tipos de cliente disponibles.",
                            "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                cmbTipoCliente.DataSource = dtTipos;
                cmbTipoCliente.DisplayMember = "TipoCliente";
                cmbTipoCliente.ValueMember = "IdTipoCliente";
                cmbTipoCliente.SelectedIndex = -1;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Una columna esperada no existe en los tipos de cliente: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los tipos de cliente: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
