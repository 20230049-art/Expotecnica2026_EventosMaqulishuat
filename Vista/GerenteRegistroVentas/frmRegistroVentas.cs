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
                panelContenedor.BackColor = Color.FromArgb(253, 241, 217);
                panelContenedor.Tag = fila["NombreEmpleado"];

                Panel pnlContenedorInfoo = new Panel();
                pnlContenedorInfoo.Width = pnlContenedorInfo.Width;
                pnlContenedorInfoo.Height = pnlContenedorInfo.Height;
                pnlContenedorInfoo.BackColor = pnlContenedorInfo.BackColor;
                pnlContenedorInfoo.BorderStyle = pnlContenedorInfo.BorderStyle;
                pnlContenedorInfoo.Margin = new Padding(10);
                pnlContenedorInfoo.Tag = fila["IdVenta"];

                Label lblVentaTitulo = new Label();
                lblVentaTitulo.Text = "N° Venta";
                lblVentaTitulo.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                lblVentaTitulo.Location = new Point(18, 19);
                lblVentaTitulo.ForeColor = Color.FromArgb(64, 6, 6);
                lblVentaTitulo.AutoSize = true;

                Panel pnlNOVenta = new Panel();
                pnlNOVenta.BackColor = Color.FromArgb(255, 255, 255);
                pnlNOVenta.Location = new Point(15, 15);
                pnlNOVenta.Size = new Size(195, 39);
                pnlNOVenta.BorderStyle = BorderStyle.FixedSingle;

                Label lblVenta = new Label();
                lblVenta.Text = fila["IdVenta"].ToString();
                lblVenta.Font = new Font("Bookman Old Style", 15, FontStyle.Regular);
                lblVenta.Location = new Point(120, 12);
                lblVenta.AutoSize = true;

                Label lblEmpleadoTitulo = new Label();
                lblEmpleadoTitulo.Text = "Empleado";
                lblEmpleadoTitulo.Font = new Font("Book Antiqua", 14, FontStyle.Bold);
                lblEmpleadoTitulo.Location = new Point(15, 58);
                lblEmpleadoTitulo.ForeColor = Color.FromArgb(64, 6, 6);
                lblEmpleadoTitulo.AutoSize = true;

                Label lblEmpleado = new Label();
                lblEmpleado.Text = fila["NombreEmpleado"].ToString();
                lblEmpleado.Font = new Font("Bookman Old Style", 14, FontStyle.Regular);
                lblEmpleado.Location = new Point(15, 82);
                lblEmpleado.MaximumSize = new Size(215, 0);
                lblEmpleado.AutoSize = true;

                Panel pnlDec1 = new Panel();
                pnlDec1.Size = new Size(1, 105);
                pnlDec1.Location = new Point(242, 12);
                pnlDec1.BackColor = Color.Black;

                Label lblClienteTitulo = new Label();
                lblClienteTitulo.Text = "Cliente";
                lblClienteTitulo.Font = new Font("Book Antiqua", 16, FontStyle.Bold);
                lblClienteTitulo.Location = new Point(262, 18);
                lblClienteTitulo.ForeColor = Color.FromArgb(64, 6, 6);
                lblClienteTitulo.AutoSize = true;

                Label lblCliente = new Label();
                lblCliente.Text = fila["NombreCliente"].ToString() + " " + fila["ApellidoCliente"].ToString();
                lblCliente.Font = new Font("Bookman Old Style", 16, FontStyle.Regular);
                lblCliente.Location = new Point(262, 42);
                lblCliente.MaximumSize = new Size(230, 0);
                lblCliente.AutoSize = true;

                Panel pnlDec2 = new Panel();
                pnlDec2.Size = new Size(1, 105);
                pnlDec2.Location = new Point(465, 12);
                pnlDec2.BackColor = Color.Black;

                Label lblFPedidoTitulo = new Label();
                lblFPedidoTitulo.Text = "Fecha Pedido";
                lblFPedidoTitulo.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                lblFPedidoTitulo.Location = new Point(470, 15);
                lblFPedidoTitulo.ForeColor = Color.FromArgb(64, 6, 6);
                lblFPedidoTitulo.AutoSize = true;

                DateTime fPedido = Convert.ToDateTime(fila["Fechaventa"]);
                Label lblFPedido = new Label();
                lblFPedido.Text = fPedido.ToString("dd/MM/yyyy");
                lblFPedido.Font = new Font("Bookman Old Style", 14, FontStyle.Regular);
                lblFPedido.Location = new Point(470, 39);
                lblFPedido.AutoSize = true;

                Label lblFEntregaTitulo = new Label();
                lblFEntregaTitulo.Text = "Fecha Entrega";
                lblFEntregaTitulo.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                lblFEntregaTitulo.Location = new Point(472, 68);
                lblFEntregaTitulo.ForeColor = Color.FromArgb(64, 6, 6);
                lblFEntregaTitulo.AutoSize = true;

                DateTime fUso = Convert.ToDateTime(fila["FechaUso"]);
                Label lblFEntrega = new Label();
                lblFEntrega.Text = fUso.ToString("dd/MM/yyyy");
                lblFEntrega.Font = new Font("Bookman Old Style", 14, FontStyle.Regular);
                lblFEntrega.Location = new Point(472, 92);
                lblFEntrega.AutoSize = true;

                Panel pnlDec3 = new Panel();
                pnlDec3.Size = new Size(1, 105);
                pnlDec3.Location = new Point(660, 12);
                pnlDec3.BackColor = Color.Black;

                Label lblEstadoTitulo = new Label();
                lblEstadoTitulo.Text = "Estado";
                lblEstadoTitulo.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                lblEstadoTitulo.Location = new Point(680, 17);
                lblEstadoTitulo.ForeColor = Color.FromArgb(64, 6, 6);
                lblEstadoTitulo.AutoSize = true;

                Label lblEstado = new Label();
                lblEstado.Text = fila["EstadoVenta"].ToString();
                lblEstado.Font = new Font("Bookman Old Style", 13, FontStyle.Regular);
                lblEstado.BackColor = Color.White;
                lblEstado.BorderStyle = BorderStyle.FixedSingle;
                lblEstado.Location = new Point(680, 42);
                lblEstado.Size = new Size(170, 24);
                lblEstado.TextAlign = ContentAlignment.MiddleLeft;
                lblEstado.Padding = new Padding(4, 0, 0, 0);

                Label lblItemsTitulo = new Label();
                lblItemsTitulo.Text = "Items Vendidos";
                lblItemsTitulo.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                lblItemsTitulo.Location = new Point(680, 70);
                lblItemsTitulo.ForeColor = Color.FromArgb(64, 6, 6);
                lblItemsTitulo.AutoSize = true;

                Label lblItems = new Label();
                lblItems.Text = fila["ItemsVendidos"].ToString();
                lblItems.Font = new Font("Bookman Old Style", 13, FontStyle.Bold);
                lblItems.Location = new Point(695, 83);
                lblItems.AutoSize = true;

                Panel pnlDec4 = new Panel();
                pnlDec4.Size = new Size(1, 105);
                pnlDec4.Location = new Point(820, 12);
                pnlDec4.BackColor = Color.Black;

                Panel pnlTotal = new Panel();
                pnlTotal.BackColor = Color.FromArgb(249, 190, 121);
                pnlTotal.Location = new Point(830, 68);
                pnlTotal.Size = new Size(170, 42);
                pnlTotal.BorderStyle = BorderStyle.FixedSingle;

                Label lblTotal = new Label();
                lblTotal.Text = "Total: " + Convert.ToDecimal(fila["TotalVenta"]).ToString("N2");
                lblTotal.Font = new Font("Bookman Old Style", 13, FontStyle.Bold);
                lblTotal.Location = new Point(8, 10);
                lblTotal.ForeColor = Color.FromArgb(64, 6, 6);
                lblTotal.AutoSize = true;

                //pnlTotal.Controls.Add(lblTotal);

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
    }
}
