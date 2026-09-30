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

namespace Vista.GerenteRegistroVentas
{
    public partial class frmRegistroVentas : Form
    {
        private ErrorProvider errorProvider;

        private const int REGISTROS_POR_PAGINA = 20;
        private int paginaActual = 1;
        private int totalPaginas = 1;
        private int totalRegistros = 0;
        private string busquedaActual = "";

        public frmRegistroVentas()
        {
            try
            {
                InitializeComponent();
                errorProvider = new ErrorProvider();
                errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
                errorProvider.ContainerControl = this;

                //txtBarraBuscar.TextChanged += txtBarraBuscar_TextChanged;
                txtBarraBuscar.TextChanged += (s, e) => errorProvider.SetError(txtBarraBuscar, "");

                //btnPaginaAnterior.Click += btnPaginaAnterior_Click;
                //btnPaginaSiguiente.Click += btnPaginaSiguiente_Click;

                CargarPagina(1);
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

                DataTable ventas;

                if (!string.IsNullOrWhiteSpace(busquedaActual))
                {
                    Venta v = new Venta();
                    ventas = v.BuscarVenta(busquedaActual);
                    totalRegistros = ventas.Rows.Count;
                    totalPaginas = 1;
                    paginaActual = 1;
                }
                else
                {
                    ventas = Venta.MostrarVentasPagina(pagina, REGISTROS_POR_PAGINA, out totalRegistros);

                    paginaActual = pagina;
                    totalPaginas = (int)Math.Ceiling((double)totalRegistros / REGISTROS_POR_PAGINA);
                    if (totalPaginas < 1) totalPaginas = 1;
                }

                CargarVentasPantalla(ventas);
                ActualizarControlesPaginacion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la página: " + ex.Message,
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
                System.Diagnostics.Debug.WriteLine("Error paginación: " + ex.Message);
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
                    btnPagina.Height = 30;
                    btnPagina.FlatStyle = FlatStyle.Flat;
                    btnPagina.FlatAppearance.BorderSize = 1;
                    btnPagina.Font = new Font("Book Antiqua", 10, FontStyle.Bold);
                    btnPagina.Margin = new Padding(2);

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
                System.Diagnostics.Debug.WriteLine("Error botones: " + ex.Message);
            }
        }

        private void btnPaginaAnterior_Click(object sender, EventArgs e)
        {
            try { CargarPagina(paginaActual - 1); }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ir a la página anterior: " + ex.Message,
                        "ERROR-PAGINACION-170", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPaginaSiguiente_Click(object sender, EventArgs e)
        {
            try { CargarPagina(paginaActual + 1); }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ir a la página siguiente: " + ex.Message,
                        "ERROR-PAGINACION-170", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarVentasPantalla(DataTable ventas)
        {
            try
            {
                flpRegistroVenta.Controls.Clear();

                if (ventas == null || ventas.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron ventas que mostrar.",
                            "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (DataRow fila in ventas.Rows)
                {
                    Panel panel = MostrarVenta(fila);
                    if (panel != null) flpRegistroVenta.Controls.Add(panel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar las ventas: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel MostrarVenta(DataRow fila)
        {
            try
            {
                if (fila == null) return null;

                Panel panelContenedor = new Panel();
                panelContenedor.Width = pnlContenedor.Width;
                panelContenedor.Height = pnlContenedor.Height;
                panelContenedor.BorderStyle = pnlContenedor.BorderStyle;
                panelContenedor.BackColor = pnlContenedor.BackColor;
                panelContenedor.Margin = new Padding(10);
                panelContenedor.Tag = fila["IdVenta"];

                // ============================================
                //   PANEL INTERNO (RECUADRO AMARILLO)
                //   → Ocupa TODO el espacio disponible
                // ============================================
                Panel pnlContenedorInfoo = new Panel();
                pnlContenedorInfoo.Width = pnlContenedorInfo.Width;
                pnlContenedorInfoo.Height = pnlContenedorInfo.Height;
                pnlContenedorInfoo.BackColor = pnlContenedorInfo.BackColor;
                pnlContenedorInfoo.BorderStyle = pnlContenedorInfo.BorderStyle;
                pnlContenedorInfoo.Location = pnlContenedorInfo.Location;
                pnlContenedorInfoo.Tag = fila["IdVenta"];

                // ✅ Ancho total del panel para distribución proporcional
                int anchoTotal = pnlContenedorInfoo.Width;
                int altoTotal = pnlContenedorInfoo.Height;

                // ✅ Posiciones X de cada columna (4 columnas al 25% cada una)
                int col1 = 20;                                     // N° Venta / Empleado
                int col2 = (int)(anchoTotal * 0.25);               // Cliente / Estado
                int col3 = (int)(anchoTotal * 0.50);               // Fechas
                int col4 = (int)(anchoTotal * 0.75);               // Items / Total

                // ✅ Anchos máximos de cada texto (para que no se solapen)
                int anchoCol1 = col2 - col1 - 20;
                int anchoCol2 = col3 - col2 - 20;
                int anchoCol3 = col4 - col3 - 20;
                int anchoCol4 = anchoTotal - col4 - 20;

                // ============================================
                //   COLUMNA 1: N° Venta / Empleado
                // ============================================
                Label lblVentaTitulo = new Label();
                lblVentaTitulo.Text = "N° Venta";
                lblVentaTitulo.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                lblVentaTitulo.Location = new Point(col1, 15);
                lblVentaTitulo.ForeColor = Color.FromArgb(64, 6, 6);
                lblVentaTitulo.AutoSize = true;

                // ✅ Panel blanco con el número de venta al lado del título
                Panel pnlNOVenta = new Panel();
                pnlNOVenta.BackColor = Color.White;
                pnlNOVenta.Location = new Point(col1 + 105, 12);
                pnlNOVenta.Size = new Size(80, 32);
                pnlNOVenta.BorderStyle = BorderStyle.FixedSingle;

                Label lblVenta = new Label();
                lblVenta.Text = fila["IdVenta"].ToString();
                lblVenta.Font = new Font("Bookman Old Style", 15, FontStyle.Bold);
                lblVenta.Location = new Point(20, 4);
                lblVenta.AutoSize = true;

                Label lblEmpleadoTitulo = new Label();
                lblEmpleadoTitulo.Text = "Empleado";
                lblEmpleadoTitulo.Font = new Font("Book Antiqua", 14, FontStyle.Bold);
                lblEmpleadoTitulo.Location = new Point(col1, 55);
                lblEmpleadoTitulo.ForeColor = Color.FromArgb(64, 6, 6);
                lblEmpleadoTitulo.AutoSize = true;

                Label lblEmpleado = new Label();
                lblEmpleado.Text = fila["NombreEmpleado"].ToString();
                lblEmpleado.Font = new Font("Bookman Old Style", 13, FontStyle.Regular);
                lblEmpleado.Location = new Point(col1, 80);
                lblEmpleado.MaximumSize = new Size(anchoCol1, 0);
                lblEmpleado.AutoSize = true;

                // ✅ Separador vertical
                Panel pnlDec1 = new Panel();
                pnlDec1.Size = new Size(1, altoTotal - 20);
                pnlDec1.Location = new Point(col2 - 10, 10);
                pnlDec1.BackColor = Color.Black;

                // ============================================
                //   COLUMNA 2: Cliente / Estado
                // ============================================
                Label lblClienteTitulo = new Label();
                lblClienteTitulo.Text = "Cliente";
                lblClienteTitulo.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                lblClienteTitulo.Location = new Point(col2, 15);
                lblClienteTitulo.ForeColor = Color.FromArgb(64, 6, 6);
                lblClienteTitulo.AutoSize = true;

                Label lblCliente = new Label();
                lblCliente.Text = fila["NombreCliente"].ToString() + " " + fila["ApellidoCliente"].ToString();
                lblCliente.Font = new Font("Bookman Old Style", 14, FontStyle.Regular);
                lblCliente.Location = new Point(col2, 40);
                lblCliente.MaximumSize = new Size(anchoCol2, 0);
                lblCliente.AutoSize = true;

                Label lblEstadoTitulo = new Label();
                lblEstadoTitulo.Text = "Estado";
                lblEstadoTitulo.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                lblEstadoTitulo.Location = new Point(col2, 68);
                lblEstadoTitulo.ForeColor = Color.FromArgb(64, 6, 6);
                lblEstadoTitulo.AutoSize = true;

                Label lblEstado = new Label();
                lblEstado.Text = fila["EstadoVenta"].ToString();
                lblEstado.Font = new Font("Bookman Old Style", 13, FontStyle.Regular);
                lblEstado.BackColor = Color.White;
                lblEstado.BorderStyle = BorderStyle.FixedSingle;
                lblEstado.Location = new Point(col2, 92);
                lblEstado.Size = new Size(anchoCol2 - 10, 24);
                lblEstado.TextAlign = ContentAlignment.MiddleLeft;
                lblEstado.Padding = new Padding(4, 0, 0, 0);

                // ✅ Separador vertical
                Panel pnlDec2 = new Panel();
                pnlDec2.Size = new Size(1, altoTotal - 20);
                pnlDec2.Location = new Point(col3 - 10, 10);
                pnlDec2.BackColor = Color.Black;

                // ============================================
                //   COLUMNA 3: Fecha Pedido / Fecha Entrega
                // ============================================
                Label lblFPedidoTitulo = new Label();
                lblFPedidoTitulo.Text = "Fecha Pedido";
                lblFPedidoTitulo.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                lblFPedidoTitulo.Location = new Point(col3, 15);
                lblFPedidoTitulo.ForeColor = Color.FromArgb(64, 6, 6);
                lblFPedidoTitulo.AutoSize = true;

                DateTime fPedido = Convert.ToDateTime(fila["Fechaventa"]);
                Label lblFPedido = new Label();
                lblFPedido.Text = fPedido.ToString("dd/MM/yyyy");
                lblFPedido.Font = new Font("Bookman Old Style", 14, FontStyle.Regular);
                lblFPedido.Location = new Point(col3, 40);
                lblFPedido.AutoSize = true;

                Label lblFEntregaTitulo = new Label();
                lblFEntregaTitulo.Text = "Fecha Entrega";
                lblFEntregaTitulo.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                lblFEntregaTitulo.Location = new Point(col3, 68);
                lblFEntregaTitulo.ForeColor = Color.FromArgb(64, 6, 6);
                lblFEntregaTitulo.AutoSize = true;

                DateTime fUso = Convert.ToDateTime(fila["FechaUso"]);
                Label lblFEntrega = new Label();
                lblFEntrega.Text = fUso.ToString("dd/MM/yyyy");
                lblFEntrega.Font = new Font("Bookman Old Style", 14, FontStyle.Regular);
                lblFEntrega.Location = new Point(col3, 92);
                lblFEntrega.AutoSize = true;

                // ✅ Separador vertical
                Panel pnlDec3 = new Panel();
                pnlDec3.Size = new Size(1, altoTotal - 20);
                pnlDec3.Location = new Point(col4 - 10, 10);
                pnlDec3.BackColor = Color.Black;

                // ============================================
                //   COLUMNA 4: Items Vendidos / Total
                // ============================================
                Label lblItemsTitulo = new Label();
                lblItemsTitulo.Text = "Items Vendidos";
                lblItemsTitulo.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                lblItemsTitulo.Location = new Point(col4, 15);
                lblItemsTitulo.ForeColor = Color.FromArgb(64, 6, 6);
                lblItemsTitulo.AutoSize = true;

                Label lblItems = new Label();
                lblItems.Text = fila["ItemsVendidos"].ToString();
                lblItems.Font = new Font("Bookman Old Style", 15, FontStyle.Bold);
                lblItems.Location = new Point(col4 + (anchoCol4 / 2) - 15, 42);
                lblItems.AutoSize = true;

                // ✅ Panel naranja del Total, ocupa todo el ancho de la columna
                Panel pnlTotal = new Panel();
                pnlTotal.BackColor = Color.FromArgb(249, 190, 121);
                pnlTotal.Location = new Point(col4, 70);
                pnlTotal.Size = new Size(anchoCol4, 45);
                pnlTotal.BorderStyle = BorderStyle.FixedSingle;

                Label lblTotal = new Label();
                lblTotal.Text = "Total: " + Convert.ToDecimal(fila["TotalVenta"]).ToString("N2");
                lblTotal.Font = new Font("Bookman Old Style", 14, FontStyle.Bold);
                lblTotal.Location = new Point(10, 12);
                lblTotal.ForeColor = Color.FromArgb(64, 6, 6);
                lblTotal.AutoSize = true;

                // ============================================
                //   ENSAMBLADO
                // ============================================
                panelContenedor.Controls.Add(pnlContenedorInfoo);

                pnlContenedorInfoo.Controls.Add(lblVentaTitulo);
                pnlContenedorInfoo.Controls.Add(pnlNOVenta);
                pnlNOVenta.Controls.Add(lblVenta);

                pnlContenedorInfoo.Controls.Add(lblEmpleadoTitulo);
                pnlContenedorInfoo.Controls.Add(lblEmpleado);

                pnlContenedorInfoo.Controls.Add(pnlDec1);

                pnlContenedorInfoo.Controls.Add(lblClienteTitulo);
                pnlContenedorInfoo.Controls.Add(lblCliente);
                pnlContenedorInfoo.Controls.Add(lblEstadoTitulo);
                pnlContenedorInfoo.Controls.Add(lblEstado);

                pnlContenedorInfoo.Controls.Add(pnlDec2);

                pnlContenedorInfoo.Controls.Add(lblFPedidoTitulo);
                pnlContenedorInfoo.Controls.Add(lblFPedido);
                pnlContenedorInfoo.Controls.Add(lblFEntregaTitulo);
                pnlContenedorInfoo.Controls.Add(lblFEntrega);

                pnlContenedorInfoo.Controls.Add(pnlDec3);

                pnlContenedorInfoo.Controls.Add(lblItemsTitulo);
                pnlContenedorInfoo.Controls.Add(lblItems);
                pnlContenedorInfoo.Controls.Add(pnlTotal);
                pnlTotal.Controls.Add(lblTotal);

                return panelContenedor;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Una columna esperada no existe en los datos de la venta: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el panel de la venta: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void txtBarraBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                busquedaActual = txtBarraBuscar.Text.Trim();
                CargarPagina(1);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(txtBarraBuscar, "Error en la búsqueda.");
                MessageBox.Show("Error al buscar ventas: " + ex.Message,
                        "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
