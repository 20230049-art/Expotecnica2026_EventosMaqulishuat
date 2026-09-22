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

namespace Vista.EmpleadosVenta
{
    public partial class frmProductoDetalles : Form
    {
        private int idProducto;
        private int idVentaActiva;
        private string nombreProducto;
        private int productoDisponible;
        private decimal precioUnitario;

        public frmProductoDetalles(int idProducto, int idVenta)
        {
            try
            {
                InitializeComponent();
            this.idProducto = idProducto;
            this.idVentaActiva = idVenta;
            MostrarInformacionProducto();

            ControlesBloqueo controlesBloqueo = new ControlesBloqueo();
            controlesBloqueo.BloquearControlesMTXT(mtbDescuent);
            controlesBloqueo.BloquearControlesMTXT(mtbSubTotal);
            controlesBloqueo.BloquearControlesMTXT(mtbTotal);
            controlesBloqueo.BloquearControlesNUD(nudCantidad);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarInformacionProducto()
        {
            try
            {
                DataTable dt = Productos.ObtenerProductoId(idProducto);

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontró el producto seleccionado.",
                            "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                nombreProducto = dt.Rows[0]["NombreProducto"].ToString();
                productoDisponible = Convert.ToInt32(dt.Rows[0]["CantidadProducto"]);
                precioUnitario = Convert.ToDecimal(dt.Rows[0]["PrecioAlquilerProducto"]);

                lblDBNombreProducto.Text = nombreProducto;
                lblDBCantidad.Text = productoDisponible.ToString();
                lblDBPrecio.Text = "$ " + precioUnitario.ToString("F2");

                if (productoDisponible <= 0)
                {
                    MessageBox.Show("El producto no está disponible en el inventario.", "ERROR-INVENTARIO-50",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    nudCantidad.Enabled = false;
                    nudCantidad.Minimum = 0;
                    nudCantidad.Maximum = 0;
                    nudCantidad.Value = 0;

                    mtbDescuent.Enabled = false;
                    btnConfirmar.Enabled = false;

                    mtbSubTotal.Text = "$0.00";
                    mtbDescuent.Text = "$";
                    mtbTotal.Text = "$0.00";
                    return;
                }

                nudCantidad.Minimum = 1;
                nudCantidad.Maximum = productoDisponible;
                nudCantidad.Value = 1;

                mtbDescuent.Text = "0.00";

                CalcularTotales();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Una columna esperada no existe en los datos del producto: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Un dato del producto no tiene el formato esperado: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la información del producto: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool calculando = false;
        private void CalcularTotales()
        {
            try
            {
                if (calculando) return;
                if (productoDisponible <= 0) return;

                calculando = true;

                int cantidad = (int)nudCantidad.Value;
                decimal subtotal = cantidad * precioUnitario;
                decimal descuento = 0;

                if (descuento > subtotal)
                {
                    MessageBox.Show($"El descuento (${descuento:F2}) no puede ser mayor al subtotal (${subtotal:F2}).",
                        "INFO-DESCUENTO-10", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    descuento = subtotal;
                    mtbDescuent.Text = descuento.ToString("F2");
                }

                if (!string.IsNullOrEmpty(mtbDescuent.Text))
                {
                    decimal.TryParse(mtbDescuent.Text, out descuento);
                    if (descuento > subtotal)
                    {
                        descuento = subtotal;
                    }
                }

                decimal total = subtotal - descuento;

                mtbSubTotal.Text = "$" + subtotal.ToString("F2");
                mtbTotal.Text = "$" + total.ToString("F2");

                calculando = false;

            }
            catch (Exception ex)
            {
                calculando = false;
                System.Diagnostics.Debug.WriteLine("Error en CalcularTotales: " + ex.Message);
            }
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (productoDisponible <= 0)
                {
                    MessageBox.Show("El producto no está disponible en el inventario.", "ERROR-INVENTARIO-50",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int cantidad = (int)nudCantidad.Value;
                decimal descuento = 0;

                if (!string.IsNullOrEmpty(mtbDescuent.Text))
                {
                    decimal.TryParse(mtbDescuent.Text, out descuento);
                }

                if (cantidad <= 0)
                {
                    MessageBox.Show("La cantidad debe ser mayor a 0.", "ERROR-CAMVACIO-001",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cantidad > productoDisponible)
                {
                    MessageBox.Show($"No hay suficiente en el inventario. Disponible: {productoDisponible}",
                        "ERROR-INVENTARIO-50", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal subtotal = cantidad * precioUnitario;
                if (descuento < 0)
                {
                    MessageBox.Show("El descuento no puede ser negativo.", "ERROR-DESCUENTO-05",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                if (idVentaActiva <= 0)
                {
                    MessageBox.Show("No hay una venta activa para agregar el producto.",
                            "INFO-NOVENTA-01", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Productos detalle = new Productos();
                detalle.AgregarProductoVenta(idVentaActiva, idProducto, cantidad, descuento);


                if (descuento > 0)
                {
                    MessageBox.Show($"Producto agregado con descuento de ${descuento:F2}",
                        "INFO-DESCUENTO-00", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                MessageBox.Show($"Producto agregado correctamente.\nCantidad: {cantidad}\nSubtotal: ${(cantidad * precioUnitario):F2}",
                    "INFO-VENTAPRODUC-05", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Uno de los valores numéricos no tiene el formato correcto: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error de base de datos al agregar el producto: " + ex.Message,
                        "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el producto: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void nudCantidad_ValueChanged_1(object sender, EventArgs e)
        {
            try
            {
                CalcularTotales();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al recalcular totales (nudCantidad): " + ex.Message);
            }
        }

        private void mtbDescuent_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                CalcularTotales();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al recalcular totales (TextChanged): " + ex.Message);
            }
        }

        private void mtbDescuent_KeyUp_1(object sender, KeyEventArgs e)
        {
            try
            {
                CalcularTotales();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al recalcular totales (KeyUp): " + ex.Message);
            }
        }
    }
}
