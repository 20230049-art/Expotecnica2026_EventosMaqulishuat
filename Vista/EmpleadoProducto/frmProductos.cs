    using Modelos.Entidades;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
using System.IO;
using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Vista.ClientesEmpleado;
using Vista.EmpleadoMenu;
using Vista.EmpleadosVenta;
    using Vista.Utilidades;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Vista.EmpleadoProducto
{
    public partial class frmProductos : Form
    {
        private const int REGISTROS_POR_PAGINA = 20;
        private int paginaActual = 1;
        private int totalPaginas = 1;
        private int totalRegistros = 0;
        private string busquedaActual = "";
        private ErrorProvider errorProvider;

        private int idVentaActiva;
        private string nombreCliente;
        private System.Windows.Forms.Button btnCarrito;
        private Panel pnlContenedor;
        private FlowLayoutPanel flpProducto;
        private bool vieneDeClientes = false;
        private int filtroActual = 1;

        public frmProductos(int idVenta, string nombreCliente)
        {
            try
            {
                InitializeComponent();
                this.idVentaActiva = idVenta;
                this.nombreCliente = nombreCliente;
                this.vieneDeClientes = true;

                if (idVenta > 0)
                {
                    VentaActiva.Iniciar(idVenta, nombreCliente);
                }

                if (VentaActiva.HayVentaActiva)
                {
                    this.idVentaActiva = VentaActiva.IdVenta;
                    this.nombreCliente = VentaActiva.NombreCliente;
                }
                else
                {
                    this.idVentaActiva = 0;
                    this.nombreCliente = "";
                }

                errorProvider = new ErrorProvider();
                errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
                errorProvider.ContainerControl = this;

                CargarPagina(1);

                BotonVenta();

                ControlesBloqueo.LimitarTextBox(txtBuscar, 100);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public frmProductos()
        {
            try
            {
                InitializeComponent();
                this.vieneDeClientes = false;

                if (VentaActiva.HayVentaActiva)
                {
                    this.idVentaActiva = VentaActiva.IdVenta;
                    this.nombreCliente = VentaActiva.NombreCliente;
                }
                else
                {
                    this.idVentaActiva = 0;
                    this.nombreCliente = "";
                }

                errorProvider = new ErrorProvider();
                errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
                errorProvider.ContainerControl = this;

                CargarPagina(1);

                ControlesBloqueo.LimitarTextBox(txtBuscar, 100);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Form activeForm = null;

        private void abrirForm(Form formularioAbrir)
        {
            try
            {
                if (activeForm != null && activeForm.GetType() == formularioAbrir.GetType())
                {
                    return;
                }

                if (activeForm != null)
                {
                    activeForm.Hide();
                    pnlVistaProducto.Controls.Remove(activeForm);
                }

                activeForm = formularioAbrir;
                formularioAbrir.TopLevel = false;
                formularioAbrir.FormBorderStyle = FormBorderStyle.None;
                formularioAbrir.Dock = DockStyle.Fill;

                pnlVistaProducto.Controls.Add(formularioAbrir);
                formularioAbrir.BringToFront();
                formularioAbrir.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el formulario: " + ex.Message, "ERROR-CAMBIO-103",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BotonVenta()
        {
            try
            {

                if (idVentaActiva <= 0)
                {
                    return;
                }

                btnCarrito = new System.Windows.Forms.Button();
                btnCarrito.Text = "Ver Compra";
                btnCarrito.Font = new Font("Book Antiqua", 14, FontStyle.Bold);
                btnCarrito.BackColor = Color.FromArgb(76, 175, 80);
                btnCarrito.ForeColor = Color.White;
                btnCarrito.FlatStyle = FlatStyle.Flat;
                btnCarrito.FlatAppearance.BorderSize = 0;
                btnCarrito.Size = new Size(160, 45);
                btnCarrito.Location = new Point(this.ClientSize.Width - 180, 15);
                btnCarrito.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                btnCarrito.Click += BtnCarrito_Click;

                btnCarrito.MouseEnter += (s, e) => btnCarrito.BackColor = Color.FromArgb(56, 142, 60);
                btnCarrito.MouseLeave += (s, e) => btnCarrito.BackColor = Color.FromArgb(76, 175, 80);

                this.Controls.Add(btnCarrito);
                btnCarrito.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el botón de venta: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCarrito_Click(object sender, EventArgs e)
        {
            try
            {
                int idVentaUsar = VentaActiva.HayVentaActiva ? VentaActiva.IdVenta : idVentaActiva;

                if (idVentaActiva <= 0)
                {
                    MessageBox.Show("No hay una venta activa.", "VENTA-NOACTIVA-120", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                frmEmpleadoMenu menu = this.ParentForm as frmEmpleadoMenu;

                string nombreUsar = VentaActiva.HayVentaActiva ? VentaActiva.NombreCliente : nombreCliente;

               // menu?.BloquearNavegacion(false);

                if (btnCarrito != null)
                {
                    btnCarrito.Visible = false;
                }
                //if (activeForm != null)
                //{
                //    activeForm.Close();
                //    pnlVistaProducto.Controls.Remove(activeForm);
                //    activeForm.Dispose();
                //    activeForm = null;
                //}

                frmCompraFinal frmCompraFinal = new frmCompraFinal(idVentaActiva, nombreCliente);
                frmCompraFinal.StartPosition = FormStartPosition.CenterScreen;
                frmCompraFinal.WindowState = FormWindowState.Maximized;
                frmCompraFinal.ShowDialog();



                if (frmCompraFinal.DialogResult == DialogResult.Cancel)
                {
                    VentaActiva.Limpiar();
                    this.idVentaActiva = 0;
                    this.nombreCliente = "";

                    if (vieneDeClientes)
                    {
                        this.DialogResult = DialogResult.Cancel;
                        this.Close();
                    }
                    else
                    {
                        if (btnCarrito != null)
                        {
                            btnCarrito.Dispose();
                            btnCarrito = null;
                        }
                        MostrarProductoEmpleado();
                    }
                }
                else
                {
                    if (btnCarrito != null)
                    {
                        btnCarrito.Visible = true;
                    }

                    ActualizarBotonCarrito();
                    MostrarProductoEmpleado();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el carrito: " + ex.Message,
                "ERROR-CAMBIO-103", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarProductoEmpleado()
        {
            try
            {
                flpProductos.Controls.Clear();

                DataTable producto = Productos.MostrarProductosEmpleado();

                foreach (DataRow fila in producto.Rows)
                {
                    Panel panelProductos = PanelProducto(fila);
                    panelProductos.Anchor = AnchorStyles.Left | AnchorStyles.Right;

                    flpProductos.Controls.Add(panelProductos);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los productos: " + ex.Message,
                                "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCompra_Click(object sender, EventArgs e)
        {
            try
            {

                System.Windows.Forms.Button panelProducto = (System.Windows.Forms.Button)sender;
                int idProducto = Convert.ToInt32(panelProducto.Tag);

                if (idVentaActiva <= 0 && !VentaActiva.HayVentaActiva)
                {
                    DialogResult result = MessageBox.Show("Actualmente no cuenta con un cliente seleccionado. ¿Desea seleccionar un cliente para iniciar una nueva venta?",
                        "INFO-NOVENTA-01", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        frmFondoNegro fondoo = new frmFondoNegro();
                        fondoo.StartPosition = FormStartPosition.CenterParent;
                        fondoo.WindowState = FormWindowState.Maximized;
                        fondoo.Show();

                        frmBuscarCliente frmBuscar = new frmBuscarCliente();
                        frmBuscar.StartPosition = FormStartPosition.CenterParent;

                        DialogResult resultadoBuscar = frmBuscar.ShowDialog();
                        frmBuscar.BringToFront();

                        fondoo.Close();

                        if (resultadoBuscar == DialogResult.OK && VentaActiva.HayVentaActiva)
                        {
                            this.idVentaActiva = VentaActiva.IdVenta;
                            this.nombreCliente = VentaActiva.NombreCliente;

                            if (btnCarrito != null)
                            {
                                btnCarrito.Dispose();
                                btnCarrito = null;
                            }
                            BotonVenta();

                            frmFondoNegro fondo2 = new frmFondoNegro();
                            fondo2.StartPosition = FormStartPosition.CenterParent;
                            fondo2.WindowState = FormWindowState.Maximized;
                            fondo2.Show();

                            frmProductoDetalles abrir = new frmProductoDetalles(idProducto, idVentaActiva);
                            abrir.ShowDialog();

                            fondo2.Close();

                            ActualizarBotonCarrito();
                        }
                    }
                    return;
                }
                int idVentaUsar = VentaActiva.HayVentaActiva ? VentaActiva.IdVenta : idVentaActiva;

                frmFondoNegro fondo = new frmFondoNegro();
                fondo.StartPosition = FormStartPosition.CenterParent;
                fondo.WindowState = FormWindowState.Maximized;
                fondo.Show();

                frmProductoDetalles abrir2 = new frmProductoDetalles(idProducto, idVentaUsar);
                abrir2.ShowDialog();

                fondo.Close();

                ActualizarBotonCarrito();
            }
            catch (FormatException ex)
            {
                MessageBox.Show("El identificador del producto no tiene el formato correcto: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el detalle del producto: " + ex.Message,
                        "ERROR-CAMBIO-103", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarBotonCarrito()
        {
            try
            {
                if (btnCarrito == null) return;

                int idVentaConsultar = VentaActiva.HayVentaActiva ? VentaActiva.IdVenta : idVentaActiva;

                if (idVentaConsultar <= 0) return;

                DetalledeVenta detalleVenta = new DetalledeVenta();
                var detalle = detalleVenta.ObtenerDetalleVenta(idVentaConsultar);

                if (detalle == null || detalle.Count == 0)
                {
                    btnCarrito.Enabled = false;
                    btnCarrito.BackColor = Color.Gray;
                    btnCarrito.Text = "Sin productos";
                }
                else
                {
                    btnCarrito.Enabled = true;
                    btnCarrito.BackColor = Color.FromArgb(76, 175, 80);
                    btnCarrito.Text = "Ver Compra";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error ActualizarBotonCarrito: " + ex.Message);
            }
        }

        private Panel PanelProducto(DataRow fila)
        {
            try
            {

                Panel panelProductos = new Panel();

                panelProductos.Width = pnlPlantilla.Width;
                panelProductos.Height = pnlPlantilla.Height;
                panelProductos.BorderStyle = pnlPlantilla.BorderStyle;
                panelProductos.BackColor = Color.FromArgb(253, 241, 217);

                PictureBox pbProducto = new PictureBox();
                pbProducto.Width = 110;
                pbProducto.Height = 98;
                pbProducto.Location = new Point(38, 14);
                pbProducto.BorderStyle = BorderStyle.None;
                pbProducto.SizeMode = PictureBoxSizeMode.StretchImage;
                pbProducto.BackColor = Color.White;
                pbProducto.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;

                string rutaImagen = (fila.Table.Columns.Contains("ImagenProducto") && fila["ImagenProducto"] != DBNull.Value) ? fila["ImagenProducto"].ToString() : "";

                ImagenVista.AsignarImagen(pbProducto, rutaImagen);

                Label lblTituloNombre = new Label();
                lblTituloNombre.Text = "Nombre".ToString();
                lblTituloNombre.Font = new Font("Book Antiqua", 21, FontStyle.Bold);
                lblTituloNombre.Location = new Point(168, 22);
                lblTituloNombre.ForeColor = Color.FromArgb(64, 6, 6);
                lblTituloNombre.AutoSize = true;
                lblTituloNombre.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;

                Label lblNombre = new Label();
                lblNombre.Text = fila["NombreProducto"].ToString();
                lblNombre.Font = new Font("Bookman Old Style", 19, FontStyle.Regular);
                lblNombre.Location = new Point(168, 57);
                lblNombre.MaximumSize = new Size(400, 0);
                lblNombre.AutoSize = true;
                lblNombre.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;

                Panel pnlDecoracion = new Panel();
                pnlDecoracion.Size = new Size(1, 88);
                pnlDecoracion.Location = new Point(573, 20);
                pnlDecoracion.BackColor = Color.Black;

                Label lblTituloCantidad = new Label();
                lblTituloCantidad.Text = "Cantidad ".ToString();
                lblTituloCantidad.Font = new Font("Book Antiqua", 19, FontStyle.Bold);
                lblTituloCantidad.Location = new Point(595, 22);
                lblTituloCantidad.ForeColor = Color.FromArgb(64, 6, 6);
                lblTituloCantidad.AutoSize = true;

                Label lblcantidad = new Label();
                lblcantidad.Text = fila["CantidadProducto"].ToString();
                lblcantidad.Font = new Font("Bookman Old Style", 22, FontStyle.Regular);
                lblcantidad.Location = new Point(604, 50);
                lblcantidad.AutoSize = true;

                Panel pnlDecoracion2 = new Panel();
                pnlDecoracion2.Size = new Size(1, 88);
                pnlDecoracion2.Location = new Point(742, 20);
                pnlDecoracion2.BackColor = Color.Black;

                Label lblTituloPrecio = new Label();
                lblTituloPrecio.Text = "Precio ".ToString();
                lblTituloPrecio.Font = new Font("Book Antiqua", 19, FontStyle.Bold);
                lblTituloPrecio.Location = new Point(765, 22);
                lblTituloPrecio.ForeColor = Color.FromArgb(64, 6, 6);
                lblTituloPrecio.AutoSize = true;

                Label lblPrecio = new Label();
                decimal precio = Convert.ToDecimal(fila["PrecioAlquilerProducto"]);
                lblPrecio.Text = precio.ToString("F2");
                lblPrecio.Font = new Font("Bookman Old Style", 20, FontStyle.Regular);
                lblPrecio.Location = new Point(765, 57);
                lblPrecio.AutoSize = true;

                Panel pnlDecoracion3 = new Panel();
                pnlDecoracion3.Size = new Size(1, 88);
                pnlDecoracion3.Location = new Point(900, 20);
                pnlDecoracion3.BackColor = Color.Black;

                Panel pnlDecoracion4 = new Panel();
                pnlDecoracion4.Size = new Size(1, 88);
                pnlDecoracion4.Location = new Point(910, 167);
                pnlDecoracion4.BackColor = Color.Black;

                System.Windows.Forms.Button btnCompra = new System.Windows.Forms.Button();
                btnCompra.Text = "Agregar a la compra ".ToString();
                btnCompra.Font = new Font("Bookman Old Style", 17, FontStyle.Regular);
                btnCompra.Location = new Point(935, 48);
                btnCompra.BackColor = Color.FromArgb(255, 170, 96);
                btnCompra.AutoSize = true;
                btnCompra.Tag = fila["IdProducto"];
                btnCompra.TabIndex = 3;
                btnCompra.Click += btnCompra_Click;

                Panel pnlDecoracion1 = new Panel();
                pnlDecoracion1.Size = new Size(1620, 1);
                pnlDecoracion1.Location = new Point(18, 150);
                pnlDecoracion1.BackColor = Color.Black;

                panelProductos.Controls.Add(pbProducto);
                panelProductos.Controls.Add(lblTituloNombre);
                panelProductos.Controls.Add(lblNombre);
                panelProductos.Controls.Add(lblTituloCantidad);
                panelProductos.Controls.Add(lblcantidad);
                panelProductos.Controls.Add(pnlDecoracion);
                panelProductos.Controls.Add(pnlDecoracion2);
                panelProductos.Controls.Add(lblTituloPrecio);
                panelProductos.Controls.Add(lblPrecio);
                panelProductos.Controls.Add(pnlDecoracion4);
                panelProductos.Controls.Add(pnlDecoracion3);
                panelProductos.Controls.Add(btnCompra);
                panelProductos.Controls.Add(pnlDecoracion1);

                return panelProductos;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Una columna esperada no existe en los datos del producto: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Un dato del producto no tiene el formato esperado: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el panel del producto: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

        }

        private void txtBuscar_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                string busqueda = txtBuscar.Text.Trim();
                Productos productos = new Productos();
                DataTable productosFiltrados = productos.BuscarProductoEmpleado(busqueda);
                MostrarProductosEmpleado(productosFiltrados);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al realizar la búsqueda: " + ex.Message,
                        "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarProductosEmpleado(DataTable productosFiltrados)
        {
            try
            {
                flpProductos.Controls.Clear();

                if (productosFiltrados == null || productosFiltrados.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron productos que coincidan con la búsqueda.", "ERROR-NODATO-007",
                        MessageBoxButtons.OK, MessageBoxIcon.Information
                    );

                    return;
                }

                foreach (DataRow fila in productosFiltrados.Rows)
                {
                    Panel panelProducto = PanelProducto(fila);

                    flpProductos.Controls.Add(panelProducto);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar los productos filtrados: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCompra_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmProductos_Load(object sender, EventArgs e)
        {
            try
            {
                var img = DatosEmpresa.ObtenerLogo();
                if (img != null)
                {
                    pbLogo.Image = img;
                    pbLogo.SizeMode = PictureBoxSizeMode.StretchImage;

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al cargar logo: " + ex.Message);
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

                MostrarProductosEmpleado(productos);
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
                    System.Windows.Forms.Button btnPagina = new System.Windows.Forms.Button();
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
            try { CargarPagina(paginaActual - 1); }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ir a la página anterior: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPaginaSiguiente_Click(object sender, EventArgs e)
        {
            try { CargarPagina(paginaActual + 1); }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ir a la página siguiente: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
