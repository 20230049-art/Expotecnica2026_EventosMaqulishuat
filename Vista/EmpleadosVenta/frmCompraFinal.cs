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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Vista.EmpleadosVenta
{
    public partial class frmCompraFinal : Form
    {
        private int idVentaActiva;
        private string nombreCliente;
        private FacturaCompleta facturaActual;
        private bool ventaFinalizada = false;

        public frmCompraFinal(int idVenta, string cliente)
        {
            try
            {
                InitializeComponent();

                this.idVentaActiva = idVenta;
                this.nombreCliente = cliente;

                if (idVentaActiva <= 0)
                {
                    MessageBox.Show("No hay una venta activa para mostrar.",
                            "INFO-NOVENTA-01", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DetalledeVenta detalleVenta = new DetalledeVenta();
                var detalle = detalleVenta.ObtenerDetalleVenta(idVentaActiva);

                if (detalle == null || detalle.Count == 0)
                {
                    MessageBox.Show("La venta no cuenta con ningún producto. Agregue productos antes de ver el carrito.",
                            "VENTA-VACIA-110", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                ArreglosControles();
                CargarDatosVenta();

                Redondeo.RedondearFig(btnCancelarVenta, 7);
                Redondeo.RedondearFig(btnConfirmarCompra, 7);
                Redondeo.RedondearFig(pnlInfo, 4);
                Redondeo.RedondearFig(pnlContenedorInfo, 6);
                Redondeo.RedondearFig(btnFacturaGenerar, 6);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ArreglosControles()
        {
            try
            {
                DataTable dt = TipoPagoVentas.MostrarTipoPago();

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron tipos de pago disponibles.",
                            "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                cmbTipoPago.DataSource = dt;
                cmbTipoPago.DisplayMember = "TipoPago";
                cmbTipoPago.ValueMember = "IdTipoPago";

                if (cmbTipoPago.Items.Count > 0)
                {
                    cmbTipoPago.SelectedIndex = 0;
                }

                dtpFechaUso.Value = DateTime.Now.AddDays(1);
                dtpFechaUso.MinDate = DateTime.Now.Date;

                btnConfirmarCompra.Enabled = true;
                btnFacturaGenerar.Enabled = false;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Una columna esperada no existe en los tipos de pago: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al configurar los controles: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDatosVenta()
        {
            try
            {
                Venta venta = new Venta();

                DataTable dtVenta = Venta.ObtenerVentaPorId(idVentaActiva);
                if (dtVenta == null || dtVenta.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontró la venta seleccionada.",
                            "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataRow fila = dtVenta.Rows[0];

                lblClienteNombreCompleto.Text = fila["NombreCliente"].ToString() + " " + fila["ApellidoCliente"].ToString();
                lblNombre.Text = fila["NombreCliente"].ToString();
                lblApellido.Text = fila["ApellidoCliente"].ToString();
                lblDui.Text = fila["DUICliente"] != DBNull.Value ? fila["DUICliente"].ToString() : "No aplica";
                lblNit.Text = fila["NITCliente"] != DBNull.Value ? fila["NITCliente"].ToString() : "No aplica";
                lblTipoCliente.Text = fila["TipoCliente"].ToString();
                lblTelefono.Text = fila["TelefonoCliente"].ToString();
                lblCorreo.Text = fila["CorreoCliente"].ToString();

                if (fila["FechaUso"] != DBNull.Value)
                {
                    DateTime fechaUso = Convert.ToDateTime(fila["FechaUso"]);
                    dtpFechaUso.Value = fechaUso;
                }

                DetalledeVenta detalleVenta = new DetalledeVenta();
                List<DetalledeVenta> productoDetalle = detalleVenta.ObtenerDetalleVenta(idVentaActiva);
                dgvDetallesVenta.DataSource = productoDetalle;

                var totales = venta.ObtenerTotalesVenta(idVentaActiva);

                lblSubtotal.Text = "$" + totales.Subtotal.ToString("F2");
                lblIva.Text = "$" + totales.Iva.ToString("F2");
                lblDescuento.Text = "$" + totales.Descuento.ToString("F2");
                lblTotal.Text = "$" + totales.Total.ToString("F2");

                FacturaCompleta factura = new FacturaCompleta();
                var facturaInformacion = factura.MostrarFacturaCompleta(idVentaActiva);

                if (facturaInformacion != null && facturaInformacion.IdFactura > 0)
                {
                    ventaFinalizada = true;
                    facturaActual = facturaInformacion;
                    MostrarFacturaGenerada(facturaInformacion);
                }
                else
                {
                    MostrarDatosPorDefecto(totales);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Una columna esperada no existe en los datos de la venta: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Un dato de la venta no tiene el formato esperado: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error de base de datos al cargar la venta: " + ex.Message,
                        "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos de la venta: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarDatosPorDefecto((decimal Subtotal, decimal Iva, decimal Descuento, decimal Total, string TipoPago, string EstadoVenta) totales)
        {
            try
            {
                lblNumeroFactura.Text = "Pendiente";
                lblTipoDocumento.Text = "Pendiente";
                lblFechaVenta.Text = "Pendiente";
                lblHora.Text = "Pendiente";
                lblSello.Text = "Pendiente";

                lblDireccion.Text = "C. Berlin No.247, San Salvador";
                lblFACCorreo.Text = "example@empresa.com";
                lblNombreEmpleado.Text = "Vendedor";

                lblSubtotal.Text = "$" + totales.Subtotal.ToString("F2");
                lblIva.Text = "$" + totales.Iva.ToString("F2");
                lblDescuento.Text = "$" + totales.Descuento.ToString("F2");
                lblTotal.Text = "$" + totales.Total.ToString("F2");

                dtpFechaUso.Enabled = true;
                cmbTipoPago.Enabled = true;
                btnConfirmarCompra.Enabled = true;
                btnFacturaGenerar.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar los datos por defecto: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarFacturaGenerada(FacturaCompleta factura)
        {
            try
            {
                if (factura == null)
                {
                    MessageBox.Show("No hay información de factura para mostrar.",
                            "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                lblNumeroFactura.Text = factura.NumeroFactura;
                lblTipoDocumento.Text = factura.TipoDocumento;
                lblFechaVenta.Text = factura.FechaFactura.ToString("dd/MM/yyyy");
                lblHora.Text = factura.HoraFactura?.ToString(@"hh\:mm") ?? "";
                lblSello.Text = factura.SelloRecepcion;

                lblDireccion.Text = factura.Sucursal ?? "C. Berlin No.247, San Salvador";
                lblFACCorreo.Text = factura.CorreoCliente ?? "example@empresa.com";
                lblNombreEmpleado.Text = factura.NombreEmpleado ?? "Vendedor";

                lblSubtotal.Text = "$" + factura.SubtotalVenta.ToString("F2");
                lblIva.Text = "$" + factura.IVA.ToString("F2");
                lblDescuento.Text = "$" + factura.DescuentoVenta.ToString("F2");
                lblTotal.Text = "$" + factura.TotalVenta.ToString("F2");

                dtpFechaUso.Enabled = false;
                cmbTipoPago.Enabled = false;

                btnConfirmarCompra.BackColor = Color.Gray;
                btnConfirmarCompra.Enabled = false;
                btnConfirmarCompra.Text = " Venta Finalizada";

                btnFacturaGenerar.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar la factura generada: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConfirmarCompra_Click(object sender, EventArgs e)
        {
            try
            {
                if (ventaFinalizada)
                {
                    MessageBox.Show("La venta ya ha sido finalizada.",
                            "VENTA-FINALIZADA-200", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (idVentaActiva <= 0)
                {
                    MessageBox.Show("No hay una venta activa para finalizar.",
                            "INFO-NOVENTA-01", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DetalledeVenta detalleVenta = new DetalledeVenta();
                var detalle = detalleVenta.ObtenerDetalleVenta(idVentaActiva);

                if (detalle == null || detalle.Count == 0)
                {
                    MessageBox.Show("No hay productos en la venta.","ERROR-CAMVACIO-001",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbTipoPago.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un tipo de pago.", "ERROR-CAMVACIO-001", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DateTime fechaUso = dtpFechaUso.Value;
                int idTipoPago = Convert.ToInt32(cmbTipoPago.SelectedValue);

                if (fechaUso < DateTime.Now.Date)
                {
                    MessageBox.Show("La fecha de uso no puede ser anterior a hoy.", "ERROR-VALIDAR-109", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult respuesta = MessageBox.Show(
                    $"¿Finalizar esta venta?\n\n" +
                    $"Cliente: {nombreCliente}\n" +
                    $"Total: {lblTotal.Text}\n" +
                    $"Fecha de Uso: {fechaUso:dd/MM/yyyy}\n" +
                    $"Tipo de Pago: {cmbTipoPago.Text}",
                    "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (respuesta != DialogResult.Yes) return;

                Venta venta = new Venta();
                venta.SubirFechaUsoVenta(idVentaActiva, fechaUso);
                venta.SubirTipoPagoVenta(idVentaActiva, idTipoPago);

                FacturaCompleta factura = new FacturaCompleta();
                facturaActual = factura.FinalizarVenta(idVentaActiva);

                if (facturaActual == null || facturaActual.IdFactura <= 0)
                {
                    MessageBox.Show("No se pudo generar la factura. Verifique el estado de la venta.",
                            "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ventaFinalizada = true;

                MostrarFacturaGenerada(facturaActual);

                MessageBox.Show($"Venta finalizada correctamente.\nFactura: {facturaActual.NumeroFactura}",
                        "VENTA-FINALIZADA-200", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult pdfResultado = MessageBox.Show("¿Desea generar el PDF de la factura?",
                        "GENERARPDF-000", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (pdfResultado == DialogResult.Yes)
                {
                    btnFacturaGenerar_Click(this, EventArgs.Empty);
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("El tipo de pago seleccionado no tiene el formato correcto: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error de base de datos al finalizar la venta: " + ex.Message,
                        "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al finalizar la venta: {ex.Message}",
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFacturaGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                if (facturaActual == null || facturaActual.IdFactura <= 0)
                {
                    MessageBox.Show("Primero debe finalizar la venta para generar la factura.",
                            "VENTA-NOFINALIZADA-100", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string rutaPDF = DocumentoPDF.GenerarFacturaPDF(facturaActual);

                if (string.IsNullOrWhiteSpace(rutaPDF))
                {
                    MessageBox.Show("No se pudo generar el archivo PDF de la factura.",
                            "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                System.Diagnostics.Process.Start(rutaPDF);

                MessageBox.Show($"Factura generada correctamente.\nUbicación: {rutaPDF}", "PDF-GENERADO-001",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show("No se encontró el archivo PDF generado: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                MessageBox.Show("No se pudo abrir el PDF con el visor predeterminado: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar PDF: {ex.Message}",
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pbRegresar_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al regresar: " + ex.Message,
                    "ERROR-CAMBIO-103", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelarVenta_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Está seguro de cancelar esta compra?\n\n" + "La venta será ELIMINADA y los productos volverán al inventario.",
                "VENTA-CANCELAR-105", MessageBoxButtons.YesNo,MessageBoxIcon.Warning);

                if (respuesta == DialogResult.No) return;
                {
                    Venta venta = new Venta();
                    venta.CancelarVenta(idVentaActiva);

                    MessageBox.Show("Compra cancelada. La venta fue eliminada y los productos regresaron al inventario.",
                    "VENTA-CANCELAR-105", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cancelar la compra: {ex.Message}",
                "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
