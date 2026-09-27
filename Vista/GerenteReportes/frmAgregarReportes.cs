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

namespace Vista.GerenteReportes
{
    public partial class frmAgregarReportes : Form
    {
        private ErrorProvider errorProvider;

        public frmAgregarReportes()
        {
            try
            {
                InitializeComponent();
                errorProvider = new ErrorProvider();
                errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
                errorProvider.ContainerControl = this;

                ControlesBloqueo controlesBloqueo = new ControlesBloqueo();

                controlesBloqueo.BloquearControlesTXT(txtNombre);
                controlesBloqueo.BloquearControlesTXT(txtDescripcion);
                controlesBloqueo.BloquearControlesMTXT(mtxtFecha);

                txtNombre.KeyPress += (s, e) => controlesBloqueo.ValidarSoloLetras(e);

                txtNombre.TextChanged += (s, e) => errorProvider.SetError(txtNombre, "");
                txtDescripcion.TextChanged += (s, e) => errorProvider.SetError(txtDescripcion, "");
                mtxtFecha.TextChanged += (s, e) => errorProvider.SetError(mtxtFecha, "");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnCerrar_Click(object sender, EventArgs e)
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
                errorProvider.Clear();
                bool hayErrores = false;

                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    errorProvider.SetError(txtNombre, "El nombre del reporte es obligatorio.");
                    hayErrores = true;
                }

                if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                {
                    errorProvider.SetError(txtDescripcion, "La descripción del reporte es obligatoria.");
                    hayErrores = true;
                }

                DateTime fechaReporte = DateTime.MinValue;
                if (string.IsNullOrWhiteSpace(mtxtFecha.Text) || !DateTime.TryParse(mtxtFecha.Text, out fechaReporte))
                {
                    errorProvider.SetError(mtxtFecha, "Debe ingresar una fecha válida (dd/MM/yyyy).");
                    hayErrores = true;
                }

                if (hayErrores)
                {
                    MessageBox.Show("Por favor corrija los campos marcados en rojo.",
                            "ERROR-CAMVACIO-001", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult respuesta = MessageBox.Show(
                    $"¿Está seguro que desea agregar el siguiente reporte?\n\n" +
                    $"Nombre: {txtNombre.Text.Trim()}\n" +
                    $"Fecha: {fechaReporte:dd/MM/yyyy}",
                    "Confirmar Registro",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (respuesta != DialogResult.Yes) return;

                Reportes reporte = new Reportes();

                reporte.NombreReporte1 = txtNombre.Text.Trim();
                reporte.DescripciónReporte1 = txtDescripcion.Text.Trim();
                reporte.FechaReporte1 = fechaReporte;

                Reportes.IngresarReporte(reporte);

                MessageBox.Show("Reporte agregado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                EventosGloblales.EnReportesAgregados();

                txtNombre.Clear();
                txtDescripcion.Clear();
                mtxtFecha.Clear();
                errorProvider.Clear();

                this.Close();
            }
            catch (FormatException ex)
            {
                MessageBox.Show("El formato de la fecha no es correcto: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    errorProvider.SetError(txtNombre, "Ya existe un reporte con ese nombre.");
                    MessageBox.Show("Ya existe un reporte con esos datos.",
                            "ERROR-DADUPLICADO-002", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Error de base de datos al agregar el reporte: " + ex.Message,
                            "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el reporte: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
