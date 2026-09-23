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

namespace Vista.GerenteEmpleados
{
    public partial class frmEmpleadosGerente : Form
    {
        private ErrorProvider errorProvider;

        private const int REGISTROS_POR_PAGINA = 20;
        private int paginaActual = 1;
        private int totalPaginas = 1;
        private int totalRegistros = 0;

        private string busquedaActual = "";

        public frmEmpleadosGerente()
        {
            try
            {
                InitializeComponent();

                errorProvider = new ErrorProvider();
                errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
                errorProvider.ContainerControl = this;

                CargarPagina(1);

                //DataTable Empleado = Empleados.MostrarEmpleado();
                //CargarEmpleadosEnPantalla(Empleado);

                EventosGloblales.EmpleadosAgregados += RegarcarPanelEmpleados;
                EventosGloblales.EmpleadosActualizar += RegarcarPanelEmpleados;
                EventosGloblales.EmpleadosEliminados += RegarcarPanelEmpleados;

                txtBarraBuscar.TextChanged += (s, e) => errorProvider.SetError(txtBarraBuscar, "");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarEmpleadosEnPantalla(DataTable empleados)
        {
            try
            {
                flpEmpleados.Controls.Clear();

                if (empleados == null || empleados.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron empleados que mostrar.",
                            "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (DataRow fila in empleados.Rows)
                {
                    Panel panelEmpleado = MostrarEmpleado(fila);

                    if (panelEmpleado == null)
                    {
                        continue;
                    }

                    panelEmpleado.Click += AbrirfrmActualizar_Click;

                    flpEmpleados.Controls.Add(panelEmpleado);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar los empleados en pantalla: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RegarcarPanelEmpleados(object sender, EventArgs e)
        {
            try
            {
                RecargarEmpleados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al recargar los empleados: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RecargarEmpleados()
        {
            try
            {
                DataTable empleados = Empleados.MostrarEmpleado();

                CargarEmpleadosEnPantalla(empleados);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al recargar la lista de empleados: " + ex.Message, "ERROR-CARGADATOS-008",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel MostrarEmpleado(DataRow fila)
        {
            try
            {
                if (fila == null)
                {
                    return null;
                }

                Panel panelContenedor = new Panel();

                panelContenedor.Width = pnlPlantilla.Width;
                panelContenedor.Height = pnlPlantilla.Height;
                panelContenedor.BackColor = Color.FromArgb(244, 220, 197);
                panelContenedor.BorderStyle = pnlPlantilla.BorderStyle;
                panelContenedor.Margin = new Padding(10);
                panelContenedor.Tag = fila["IdEmpleado"];

                PictureBox pbLogo = new PictureBox();
                pbLogo.Width = 110;
                pbLogo.Height = 98;
                pbLogo.Location = new Point(25, 14);
                pbLogo.BorderStyle = BorderStyle.None;
                pbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
                pbLogo.BackColor = Color.White;

                try
                {
                    string rutaBD = fila["FotoEmpleado"] != DBNull.Value ? fila["FotoEmpleado"].ToString()
                        : "";

                    if (!string.IsNullOrWhiteSpace(rutaBD))
                    {
                        string rutaCompleta = System.IO.Path.IsPathRooted(rutaBD) ? rutaBD : System.IO.Path.Combine(Application.StartupPath, rutaBD);

                        if (System.IO.File.Exists(rutaCompleta))
                        {
                            using (var stream = new System.IO.FileStream(rutaCompleta, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                            {
                                pbLogo.Image = Image.FromStream(stream);
                            }
                        }
                        else
                        {
                            pbLogo.Image = null;
                            pbLogo.BackColor = Color.White;
                        }
                    }
                    else
                    {
                        pbLogo.Image = null;
                        pbLogo.BackColor = Color.White;
                    }
                }
                catch (Exception exFoto)
                {
                    System.Diagnostics.Debug.WriteLine("Error al cargar foto: " + exFoto.Message);
                    pbLogo.Image = null;
                    pbLogo.BackColor = Color.White;
                }

                Label lblTituloNombre = new Label();
                lblTituloNombre.Text = "Nombre".ToString();
                lblTituloNombre.Font = new Font("Book Antiqua", 23, FontStyle.Bold);
                lblTituloNombre.Location = new Point(148, 17);
                lblTituloNombre.ForeColor = Color.FromArgb(64, 6, 6);
                lblTituloNombre.AutoSize = true;

                Label lblNombre = new Label();
                lblNombre.Text = fila["NombreEmpleado"].ToString();
                lblNombre.Font = new Font("Bookman Old Style", 20, FontStyle.Regular);
                lblNombre.Location = new Point(148, 48);
                lblNombre.AutoSize = true;

                Label lblApellido = new Label();
                lblApellido.Text = fila["ApellidoEmpleado"].ToString();
                lblApellido.Font = new Font("Bookman Old Style", 18, FontStyle.Regular);
                lblApellido.Location = new Point(148, 76);
                lblApellido.AutoSize = true;

                // Panel Decoración
                Panel pnlDecoracion = new Panel();
                pnlDecoracion.Size = new Size(1, 98);
                pnlDecoracion.Location = new Point(396, 18);
                pnlDecoracion.BackColor = Color.Black;

                Label lblTituloContacto = new Label();
                lblTituloContacto.Text = "Contacto".ToString();
                lblTituloContacto.Font = new Font("Book Antiqua", 20, FontStyle.Bold);
                lblTituloContacto.Location = new Point(420, 17);
                lblTituloContacto.ForeColor = Color.FromArgb(64, 6, 6);
                lblTituloContacto.AutoSize = true;

                Label lblNumero = new Label();
                lblNumero.Text = fila["TelefonoEmpleado"].ToString();
                lblNumero.Font = new Font("Bookman Old Style", 19, FontStyle.Regular);
                lblNumero.Location = new Point(420, 47);
                lblNumero.AutoSize = true;

                Label lblCorreo = new Label();
                lblCorreo.Text = fila["CorreoEmpleado"].ToString();
                lblCorreo.Font = new Font("Bookman Old Style", 19, FontStyle.Regular);
                lblCorreo.Location = new Point(420, 74);
                lblCorreo.AutoSize = true;

                // Panel Decoración
                Panel pnlDecoracion1 = new Panel();
                pnlDecoracion1.Size = new Size(1, 98);
                pnlDecoracion1.Location = new Point(812, 17);
                pnlDecoracion1.BackColor = Color.Black;

                Label lblTituloCargo = new Label();
                lblTituloCargo.Text = "Cargo".ToString();
                lblTituloCargo.Font = new Font("Book Antiqua", 20, FontStyle.Bold);
                lblTituloCargo.Location = new Point(832, 17);
                lblTituloCargo.ForeColor = Color.FromArgb(64, 6, 6);
                lblTituloCargo.AutoSize = true;

                Label lblCargo = new Label();
                lblCargo.Text = fila["CargoEmpleado"].ToString();
                lblCargo.Font = new Font("Bookman Old Style", 19, FontStyle.Regular);
                lblCargo.Location = new Point(832, 46);
                lblCargo.MaximumSize = new Size(199, 0);
                lblCargo.AutoSize = true;

                // Panel Decoración
                Panel pnlDecoracion2 = new Panel();
                pnlDecoracion2.Size = new Size(1, 98);
                pnlDecoracion2.Location = new Point(1064, 18);
                pnlDecoracion2.BackColor = Color.Black;

                Label lblTituloEstado = new Label();
                lblTituloEstado.Text = "Estado".ToString();
                lblTituloEstado.Font = new Font("Book Antiqua", 20, FontStyle.Bold);
                lblTituloEstado.Location = new Point(1099, 19);
                lblTituloEstado.ForeColor = Color.FromArgb(64, 6, 6);
                lblTituloEstado.AutoSize = true;

                ComboBox cmbEstado = new ComboBox();
                cmbEstado.Location = new Point(1074, 62);
                cmbEstado.Size = new Size(164, 49);
                cmbEstado.Font = new Font("Book Antiqua", 19, FontStyle.Regular);
                cmbEstado.DropDownStyle = ComboBoxStyle.DropDownList;

                cmbEstado.Tag = fila["IdEmpleado"];

                DataTable estados = EstadoEmpleados.MostrarEstadoEmpleado();

                if (estados != null && estados.Rows.Count > 0)
                {
                    cmbEstado.DataSource = estados;
                    cmbEstado.DisplayMember = "EstadoEmpleado";
                    cmbEstado.ValueMember = "EstadoEmpleado";
                }

                // ⚠️ PENDIENTE: La lógica de cambio de estado está comentada.
                // Descomentar cuando esté lista la conexión con la capa de datos.
                // cmbEstado.SelectionChangeCommitted += CambiarEstadoEmpleado;

                panelContenedor.Controls.Add(pbLogo);
                panelContenedor.Controls.Add(lblTituloNombre);
                panelContenedor.Controls.Add(lblNombre);
                panelContenedor.Controls.Add(lblApellido);
                panelContenedor.Controls.Add(pnlDecoracion);
                panelContenedor.Controls.Add(lblTituloContacto);
                panelContenedor.Controls.Add(lblNumero);
                panelContenedor.Controls.Add(lblCorreo);
                panelContenedor.Controls.Add(pnlDecoracion1);
                panelContenedor.Controls.Add(lblTituloCargo);
                panelContenedor.Controls.Add(lblCargo);
                panelContenedor.Controls.Add(pnlDecoracion2);
                panelContenedor.Controls.Add(lblTituloEstado);
                panelContenedor.Controls.Add(cmbEstado);

                return panelContenedor;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Una columna esperada no existe en los datos del empleado: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el panel del empleado: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void AbrirfrmActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                Panel panelEmpleado = sender as Panel;

                if (panelEmpleado == null || panelEmpleado.Tag == null)
                {
                    MessageBox.Show("No se pudo identificar el empleado seleccionado.",
                            "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int idEmpleado = Convert.ToInt32(panelEmpleado.Tag);

                frmFondoNegro fondo = new frmFondoNegro();
                fondo.StartPosition = FormStartPosition.CenterParent;
                fondo.WindowState = FormWindowState.Maximized;
                fondo.Show();

                frmActualizarEmpleado abrir = new frmActualizarEmpleado(idEmpleado);
                abrir.ShowDialog();

                fondo.Close();

                CargarPagina(paginaActual);

                if (totalPaginas > 1)
                {
                    CargarPagina(totalPaginas);
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("El identificador del empleado no tiene el formato correcto: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el formulario de actualización: " + ex.Message,
                        "ERROR-CAMBIO-103", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                frmFondoNegro fondo = new frmFondoNegro();
                fondo.StartPosition = FormStartPosition.CenterParent;
                fondo.WindowState = FormWindowState.Maximized;
                fondo.Show();

                frmAnadirEmpleado abrir = new frmAnadirEmpleado();
                abrir.ShowDialog();

                fondo.Close();

                CargarPagina(paginaActual);
                if (totalPaginas > 1)
                {
                    CargarPagina(totalPaginas);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el formulario de agregar empleado: " + ex.Message,
                        "ERROR-CAMBIO-103", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                Empleados empleado = new Empleados();
                DataTable empleadoFiltrados = empleado.BuscarEmpleado(busquedaActual);

                totalRegistros = empleadoFiltrados?.Rows.Count ?? 0;
                totalPaginas = 1;
                paginaActual = 1;

                CargarEmpleadosEnPantalla(empleadoFiltrados);
                ActualizarControlesPaginacion();
            }
            catch (Exception ex)
            {
                errorProvider.SetError(txtBarraBuscar, "Error en la búsqueda.");
                MessageBox.Show("Error al buscar empleados: " + ex.Message,
                        "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmEmpleadosGerente_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                EventosGloblales.EmpleadosAgregados -= RegarcarPanelEmpleados;
                EventosGloblales.EmpleadosActualizar -= RegarcarPanelEmpleados;
                EventosGloblales.EmpleadosEliminados -= RegarcarPanelEmpleados;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al desuscribir eventos: " + ex.Message);
            }
        }

        private void CargarPagina(int pagina)
        {
            try
            {
                if (pagina < 1) pagina = 1;
                if (pagina > totalPaginas && totalPaginas > 0) pagina = totalPaginas;

                DataTable empleados;

                if (string.IsNullOrWhiteSpace(busquedaActual))
                {
                    empleados = Empleados.MostrarEmpleadoPagina(pagina, REGISTROS_POR_PAGINA, out totalRegistros);
                }
                else
                {
                    Empleados emp = new Empleados();
                    empleados = emp.BuscarEmpleado(busquedaActual);
                    totalRegistros = empleados.Rows.Count;
                }

                paginaActual = pagina;
                totalPaginas = (int)Math.Ceiling((double)totalRegistros / REGISTROS_POR_PAGINA);
                if (totalPaginas < 1) totalPaginas = 1;

                CargarEmpleadosEnPantalla(empleados);
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
    }
}
