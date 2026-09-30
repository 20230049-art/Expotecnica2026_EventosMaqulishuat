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

namespace Vista.GerenteInventario
{
    public partial class frmInventario : Form
    {
        private ErrorProvider errorProvider;

        private const int REGISTROS_POR_PAGINA = 20;
        private int paginaActual = 1;
        private int totalPaginas = 1;
        private int totalRegistros = 0;

        private int filtroActual = 1;

        private string busquedaActual = "";

        public frmInventario()
        {
            try
            {
                InitializeComponent();

                errorProvider = new ErrorProvider();
                errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
                errorProvider.ContainerControl = this;

                btnPaginaAnterior.Click += btnPaginaAnterior_Click;
                btnPaginaSiguiente.Click += btnPaginaSiguiente_Click;

                btnClientesTotales.Click += btnClientesTotales_Click;
                btnClientesActivos.Click += btnClientesActivos_Click;
                btnProductosAgotados.Click += btnProductosAgotados_Click;

                Redondeo.RedondearFig(txtBarraBuscar, 10);
                Redondeo.RedondearFig(btnClientesTotales, 16);
                Redondeo.RedondearFig(btnClientesActivos, 16);
                Redondeo.RedondearFig(btnProductosAgotados, 16);
                Redondeo.RedondearFig(btnAgregar, 8);
                Redondeo.RedondearFig(btnEliminar, 8);

                ActivarBoton(btnClientesTotales); CargarPagina(1);

                txtBarraBuscar.TextChanged += (s, e) => errorProvider.SetError(txtBarraBuscar, "");

                EstilizarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarPagina(int pagina)
        {
            try
            {
                if (pagina < 1) pagina = 1;
                if (pagina > totalPaginas && totalPaginas > 0) pagina = totalPaginas;

                DataTable productos;

                if (string.IsNullOrWhiteSpace(busquedaActual))
                {
                    productos = Productos.MostrarProductosPagina(pagina, REGISTROS_POR_PAGINA, filtroActual, out totalRegistros);
                }
                else
                {
                    Productos prod = new Productos();
                    productos = prod.BuscarProductoEmpleado(busquedaActual);
                    totalRegistros = productos?.Rows.Count ?? 0;
                }

                paginaActual = pagina;
                totalPaginas = (int)Math.Ceiling((double)totalRegistros / REGISTROS_POR_PAGINA);
                if (totalPaginas < 1) totalPaginas = 1;

                AsignarProductosGrid(productos);
                ActualizarControlesPaginacion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la página: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AsignarProductosGrid(DataTable productos)
        {
            try
            {
                dgvProductos.DataSource = null;

                if (productos == null || productos.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron productos que mostrar.",
                            "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dgvProductos.DataSource = productos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al asignar los productos al grid: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarControlesPaginacion()
        {
            try
            {
                lblInfoPagina.Text = $"Página {paginaActual} de {totalPaginas}";
                lblTotalRegistros.Text = $"Total: {totalRegistros} registro(s)";

                btnPaginaAnterior.Enabled = paginaActual > 1;
                btnPaginaSiguiente.Enabled = paginaActual < totalPaginas;

                GenerarBotonesNumeros();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al actualizar paginación: " + ex.Message);
            }
        }

        private void GenerarBotonesNumeros()
        {
            try
            {
                flpNumerosPagina.Controls.Clear();

                int inicio = Math.Max(1, paginaActual - 3);
                int fin = Math.Min(totalPaginas, inicio + 6);
                if (fin - inicio < 6) inicio = Math.Max(1, fin - 6);

                for (int i = inicio; i <= fin; i++)
                {
                    Button btnPagina = new Button();
                    btnPagina.Text = i.ToString();
                    btnPagina.Width = 35;
                    btnPagina.Height = 32;
                    btnPagina.FlatStyle = FlatStyle.Flat;
                    btnPagina.FlatAppearance.BorderSize = 0;
                    btnPagina.Font = new Font("Book Antiqua", 12, FontStyle.Bold);
                    btnPagina.Margin = new Padding(1);

                    if (i == paginaActual)
                    {
                        btnPagina.BackColor = Color.FromArgb(208, 112, 3);
                        btnPagina.ForeColor = Color.White;
                    }
                    else
                    {
                        btnPagina.BackColor = Color.FromArgb(206, 183, 175);
                        btnPagina.ForeColor = Color.Black;
                    }

                    int paginaBoton = i;
                    btnPagina.Click += (s, e) => CargarPagina(paginaBoton);

                    flpNumerosPagina.Controls.Add(btnPagina);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al generar botones de página: " + ex.Message);
            }
        }

        private void btnPaginaAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                CargarPagina(paginaActual - 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ir a la página anterior: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPaginaSiguiente_Click(object sender, EventArgs e)
        {
            try
            {
                CargarPagina(paginaActual + 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ir a la página siguiente: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RestaurarBotones()
        {
            try
            {
                btnClientesTotales.BackColor = Color.FromArgb(244, 220, 197);
                btnClientesActivos.BackColor = Color.FromArgb(244, 220, 197);
                btnProductosAgotados.BackColor = Color.FromArgb(244, 220, 197);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al restaurar botones: " + ex.Message);
            }
        }

        private void ActivarBoton(Button botones)
        {
            try
            {
                if (botones == null) return;

                RestaurarBotones();
                botones.BackColor = Color.FromArgb(237, 180, 141);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al activar botón: " + ex.Message);
            }
        }

        private void btnClientesTotales_Click(object sender, EventArgs e)
        {
            try
            {
                ActivarBoton(btnClientesTotales);
                filtroActual = 1;
                paginaActual = 1;
                CargarPagina(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar todos los productos: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClientesActivos_Click(object sender, EventArgs e)
        {
            try
            {
                ActivarBoton(btnClientesActivos);
                filtroActual = 2;
                paginaActual = 1;
                CargarPagina(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los productos activos: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnProductosAgotados_Click(object sender, EventArgs e)
        {
            try
            {
                ActivarBoton(btnProductosAgotados);
                filtroActual = 3;
                paginaActual = 1;
                CargarPagina(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los productos agotados: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                frmFondoNegro fondo = new frmFondoNegro();
                fondo.StartPosition = FormStartPosition.CenterParent;
                fondo.WindowState = FormWindowState.Maximized;
                fondo.Show();

                frmInventarioAgregar abrir = new frmInventarioAgregar();
                abrir.ShowDialog();

                CargarPagina(paginaActual);

                fondo.Close();

                if (totalPaginas > 1 && string.IsNullOrWhiteSpace(busquedaActual))
                {
                    CargarPagina(totalPaginas);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el formulario de agregar producto: " + ex.Message,
                        "ERROR-CAMBIO-103", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductos.CurrentRow == null)
                {
                    MessageBox.Show("Debe seleccionar un producto del listado para eliminar.",
                            "ERROR-CAMVACIO-001", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dgvProductos.CurrentRow.Cells["IdProducto"].Value == null)
                {
                    MessageBox.Show("No se pudo obtener el ID del producto seleccionado.",
                            "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int idProducto = Convert.ToInt32(dgvProductos.CurrentRow.Cells["IdProducto"].Value);
                string nombreProducto = dgvProductos.CurrentRow.Cells["NombreProducto"].Value?.ToString() ?? "";

                DialogResult confirmacion = MessageBox.Show(
                    $"¿Está seguro que desea eliminar el siguiente producto?\n\n" +
                    $"Producto: \"{nombreProducto}\"\n\n" +
                    "El producto no se borrará permanentemente, solo se marcará como eliminado.",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (confirmacion != DialogResult.Yes) return;

                bool exito = Productos.EliminarProducto(idProducto);

                if (!exito)
                {
                    MessageBox.Show("No se pudo eliminar el producto de la base de datos.",
                            "ERROR-ELIMINAR-015", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Producto eliminado correctamente.",
                        "PROCEDIMIENTO-EXITOSO", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarPagina(paginaActual);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Error de base de datos al eliminar el producto: " + ex.Message,
                    "ERROR-SQL-100",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al eliminar el producto: " + ex.Message,
                    "ERROR-EXCEPCION-102",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void frmInventario_Load(object sender, EventArgs e)
        {
            try
            {
                CargarPagina(paginaActual);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtBarraBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                busquedaActual = txtBarraBuscar.Text.Trim();

                if (string.IsNullOrWhiteSpace(busquedaActual))
                {
                    errorProvider.SetError(txtBarraBuscar, "");
                    paginaActual = 1;
                    CargarPagina(1);
                    return;
                }

                Productos productos = new Productos();
                DataTable productosFiltrados = productos.BuscarProductoEmpleado(busquedaActual);

                totalRegistros = productosFiltrados?.Rows.Count ?? 0;
                totalPaginas = 1;
                paginaActual = 1;

                AsignarProductosGrid(productosFiltrados);
                ActualizarControlesPaginacion();
            }
            catch (Exception ex)
            {
                errorProvider.SetError(txtBarraBuscar, "Error en la búsqueda.");
                MessageBox.Show("Error al buscar productos: " + ex.Message,
                        "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dgvProductos_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;

                DataGridViewRow filaSeleccionada = dgvProductos.Rows[e.RowIndex];

                if (filaSeleccionada.Cells["IdProducto"].Value == null)
                    return;

                int idProducto = Convert.ToInt32(filaSeleccionada.Cells["IdProducto"].Value);

                frmFondoNegro fondo = new frmFondoNegro();
                fondo.StartPosition = FormStartPosition.CenterParent;
                fondo.WindowState = FormWindowState.Maximized;
                fondo.Show();

                frmActualizarProducto frmActualizar = new frmActualizarProducto(idProducto);
                frmActualizar.ShowDialog();

                CargarPagina(paginaActual);

                fondo.Close();
            }
            catch (FormatException ex)
            {
                MessageBox.Show("El identificador del producto no tiene el formato correcto: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el formulario de actualización: " + ex.Message,
                        "ERROR-CAMBIO-103", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EstilizarGrid()
        {
            try
            {
                dgvProductos.EnableHeadersVisualStyles = false;
                dgvProductos.BorderStyle = BorderStyle.FixedSingle;
                dgvProductos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvProductos.GridColor = Color.FromArgb(200, 180, 160);
                dgvProductos.BackgroundColor = Color.FromArgb(253, 241, 217);
                dgvProductos.RowHeadersVisible = false;
                dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvProductos.MultiSelect = false;
                dgvProductos.ReadOnly = true;
                dgvProductos.AllowUserToAddRows = false;
                dgvProductos.AllowUserToDeleteRows = false;
                dgvProductos.AllowUserToResizeRows = false;
                dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dgvProductos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(208, 112, 3);
                dgvProductos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvProductos.ColumnHeadersDefaultCellStyle.Font = new Font("Book Antiqua", 12, FontStyle.Bold);
                dgvProductos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvProductos.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 0, 0);
                dgvProductos.ColumnHeadersHeight = 38;
                dgvProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                dgvProductos.DefaultCellStyle.Font = new Font("Bookman Old Style", 11, FontStyle.Regular);
                dgvProductos.DefaultCellStyle.ForeColor = Color.FromArgb(64, 6, 6);
                dgvProductos.DefaultCellStyle.BackColor = Color.FromArgb(253, 241, 217);
                dgvProductos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(249, 190, 121);
                dgvProductos.DefaultCellStyle.SelectionForeColor = Color.FromArgb(64, 6, 6);
                dgvProductos.DefaultCellStyle.Padding = new Padding(6, 0, 0, 0);
                dgvProductos.RowTemplate.Height = 34;

                dgvProductos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 228, 200);
                dgvProductos.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(249, 190, 121);
                dgvProductos.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(64, 6, 6);

                dgvProductos.AdvancedCellBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al estilizar el grid: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
