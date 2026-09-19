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

        private void MostrarInformacionProducto()
        {
            DataTable dt = Productos.ObtenerProductoId(idProducto);

            if (dt.Rows.Count > 0)
            {
                nombreProducto = dt.Rows[0]["NombreProducto"].ToString();
                productoDisponible = Convert.ToInt32(dt.Rows[0]["CantidadProducto"]);
                precioUnitario = Convert.ToDecimal(dt.Rows[0]["PrecioAlquilerProducto"]);

                lblDBNombreProducto.Text = nombreProducto;
                lblDBCantidad.Text = productoDisponible.ToString();
                lblDBPrecio.Text = "$ " + precioUnitario.ToString("F2");

                if (productoDisponible <= 0)
                {
                    MessageBox.Show("El producto no está disponible en el inventario.", "Producto no disponible",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    nudCantidad.Enabled = false;
                    nudCantidad.Minimum = 0;
                    nudCantidad.Maximum = 0;
                    nudCantidad.Value = 0;

                    mtbDescuent.Enabled = false;
                    btnConfirmar.Enabled = false;

                    mtbSubTotal.Text = "$0.00";
                    mtbDescuent.Text = "$0.00";
                    mtbTotal.Text = "$0.00";
                    return;
                }

                nudCantidad.Minimum = 1;
                nudCantidad.Maximum = productoDisponible;
                nudCantidad.Value = 1;

                mtbDescuent.Text = "0.00";

                CalcularTotales();
            }
            else
            {
                MessageBox.Show("No se encontró el producto.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void nudCantidad_ValueChanged(object sender, EventArgs e)
        {
            CalcularTotales();
        }

        private void mtbDescuent_KeyUp(object sender, KeyEventArgs e)
        {
            CalcularTotales();
        }

        private void CalcularTotales()
        {
            if (productoDisponible <= 0) return;

            int cantidad = (int)nudCantidad.Value;
            decimal subtotal = cantidad * precioUnitario;
            decimal descuento = 0;

            if (!string.IsNullOrEmpty(mtbDescuent.Text))
            {
                decimal.TryParse(mtbDescuent.Text, out descuento);
                if (descuento > subtotal)
                {
                    descuento = subtotal;
                    mtbDescuent.Text = descuento.ToString("F2");
                }
            }

            decimal total = subtotal - descuento;

            mtbSubTotal.Text = "$" + subtotal.ToString("F2");
            mtbDescuent.Text = descuento.ToString("F2");
            //mtbDescuento.Text = "$" + descuento.ToString("F2");
            mtbTotal.Text = "$" + total.ToString("F2");
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (productoDisponible <= 0)
                {
                    MessageBox.Show("El producto no está disponible en el inventario.", "Producto no disponible",
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
                    MessageBox.Show("La cantidad debe ser mayor a 0.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cantidad > productoDisponible)
                {
                    MessageBox.Show($"No hay suficiente en el inventario. Disponible: {productoDisponible}",
                        "Inventario insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Productos detalle = new Productos();
                detalle.AgregarProductoVenta(idVentaActiva, idProducto, cantidad);

                //Arreglar descuento por producto
                if (descuento > 0)
                {
                    MessageBox.Show($"Producto agregado con descuento de ${descuento:F2}",
                        "Descuento aplicado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                MessageBox.Show($"Producto agregado correctamente.\nCantidad: {cantidad}\nSubtotal: ${(cantidad * precioUnitario):F2}",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar producto: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
