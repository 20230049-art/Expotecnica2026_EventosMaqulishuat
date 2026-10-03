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

namespace Vista.GerenteCalendario
{
    public partial class frmCalendarioAgregarCitas : Form
    {
        private DateTime fechaPrecargada;

        public frmCalendarioAgregarCitas()
        {
            InitializeComponent();
            
            Redondeo.RedondearFormulario(this, 15);
            Redondeo.RedondearFig(pnlNombreRecordatorio, 7);
            Redondeo.RedondearFig(button1, 4);
            Redondeo.RedondearFig(btnCerrar, 4);

            ControlesBloqueo.LimitarTextBox(txtAsunto, 250);
            ControlesBloqueo.LimitarTextBox(txtUbicacion, 200);
        }

        public frmCalendarioAgregarCitas(DateTime fecha) : this()
        {
            fechaPrecargada = fecha;
        }

        private void frmCalendarioAgregarCitas_Load(object sender, EventArgs e)
        {
            try
            {
                dtpDia.Value = fechaPrecargada;
                dtpHora.Format = DateTimePickerFormat.Time;
                dtpHora.ShowUpDown = true;

                dtpDia.MinDate = DateTime.Today;

                CargarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarClientes()
        {
            try
            {
                DataTable clientes = Clientes.MostrarClientes();
                cmbCliente.DataSource = clientes;
                cmbCliente.DisplayMember = "NombreCliente";
                cmbCliente.ValueMember = "IdCliente";
                cmbCliente.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error clientes: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtAsunto.Text))
                {
                    MessageBox.Show("El asunto es obligatorio.", "INFO",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtUbicacion.Text))
                {
                    MessageBox.Show("La ubicación es obligatoria.", "INFO",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (cmbCliente.SelectedValue == null)
                {
                    MessageBox.Show("Selecciona un cliente.", "INFO",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var fecha = new Fechas
                {
                    DiaFecha = dtpDia.Value.Date,
                    HoraFecha = dtpHora.Value.TimeOfDay,
                    AsusntoFecha = txtAsunto.Text.Trim(),
                    LugarFecha = txtUbicacion.Text.Trim(),
                    IdCliente = Convert.ToInt32(cmbCliente.SelectedValue),
                    IdUsuario = UsuarioActual.Datos.IdUsuario
                };

                if (Fechas.Agregar(fecha))
                {
                    MessageBox.Show("Recordatorio agregado correctamente.",
                            "PROCEDIMIENTO-EXITOSO", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
