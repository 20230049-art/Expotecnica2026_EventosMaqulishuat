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
    public partial class frmCalendarioActualizarEliminar : Form
    {
        private int _idFecha;

        public frmCalendarioActualizarEliminar()
        {
            InitializeComponent();

            Redondeo.RedondearFormulario(this, 14);
            Redondeo.RedondearFig(btnElimiar, 4);
            Redondeo.RedondearFig(btnActualizar, 4);
            Redondeo.RedondearFig(btnCerrar, 4);
            Redondeo.RedondearFig(panel4, 6);

            ControlesBloqueo.LimitarTextBox(txtAsunto, 250);
            ControlesBloqueo.LimitarTextBox(txtUbicacion, 250);
        }

        public frmCalendarioActualizarEliminar(int idFecha) : this()
        {
            _idFecha = idFecha;

            Redondeo.RedondearFormulario(this, 14);
            Redondeo.RedondearFig(btnElimiar, 4);
            Redondeo.RedondearFig(btnActualizar, 4);
            Redondeo.RedondearFig(btnCerrar, 4);
            Redondeo.RedondearFig(panel4, 6);

            ControlesBloqueo.LimitarTextBox(txtAsunto, 250);
            ControlesBloqueo.LimitarTextBox(txtUbicacion, 250);
        }

        private void frmCalendarioActualizarEliminar_Load(object sender, EventArgs e)
        {
            try
            {
                dtpHora.Format = DateTimePickerFormat.Time;
                dtpHora.ShowUpDown = true;
                dtpDia.MinDate = DateTime.Today;

                CargarClientes();
                CargarFecha();
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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error clientes: " + ex.Message);
            }
        }

        private void CargarFecha()
        {
            try
            {
                DataTable dt = Fechas.ObtenerPorId(_idFecha);
                if (dt.Rows.Count == 0) return;

                DataRow fila = dt.Rows[0];
                dtpDia.Value = Convert.ToDateTime(fila["DiaFecha"]);
                dtpHora.Value = DateTime.Today.Add((TimeSpan)fila["HoraFecha"]);
                txtAsunto.Text = fila["AsuntoFecha"].ToString();
                txtUbicacion.Text = fila["LugarFecha"].ToString();
                cmbCliente.SelectedValue = Convert.ToInt32(fila["IdCliente"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la fecha: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtAsunto.Text) ||
                    string.IsNullOrWhiteSpace(txtUbicacion.Text) ||
                    cmbCliente.SelectedValue == null)
                {
                    MessageBox.Show("Completa todos los campos.", "INFO",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var fecha = new Fechas
                {
                    IdFecha = _idFecha,
                    DiaFecha = dtpDia.Value.Date,
                    HoraFecha = dtpHora.Value.TimeOfDay,
                    AsusntoFecha = txtAsunto.Text.Trim(),
                    LugarFecha = txtUbicacion.Text.Trim(),
                    IdCliente = Convert.ToInt32(cmbCliente.SelectedValue)
                };

                if (Fechas.Actualizar(fecha))
                {
                    MessageBox.Show("Recordatorio actualizado.",
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

        private void btnElimiar_Click(object sender, EventArgs e)
        {
            try
            {
                var conf = MessageBox.Show(
                    "¿Eliminar este recordatorio?\n\nEsta acción no se puede deshacer.",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (conf != DialogResult.Yes)
                    return;

                bool eliminado = Fechas.Eliminar(_idFecha);

                if (eliminado)
                {
                    MessageBox.Show(
                        "Recordatorio eliminado.",
                        "PROCEDIMIENTO-EXITOSO",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

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
            this.Close();
        }

        private void pnlVistaCalendarioActualiza_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
