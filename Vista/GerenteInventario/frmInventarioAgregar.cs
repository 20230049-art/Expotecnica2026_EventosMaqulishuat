using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vista.Utilidades;

namespace Vista.GerenteInventario
{
    public partial class frmInventarioAgregar : Form
    {
        private ErrorProvider errorProvider;

        private string ImagenProducto1 = " ";
        public frmInventarioAgregar()
        {
            try
            {
                InitializeComponent();

                Redondeo.RedondearFormulario(this, 14);
                Redondeo.RedondearFig(pnltituloNombre, 8);
                Redondeo.RedondearFig(btnAgregar, 6);
                Redondeo.RedondearFig(btnCerrar, 6);

                errorProvider = new ErrorProvider();
                errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
                errorProvider.ContainerControl = this;

                ControlesBloqueo.LimitarTextBox(txtNombreProducto, 180);
                ControlesBloqueo.LimitarTextBox(txtCantidad, 10);
                ControlesBloqueo.LimitarTextBox(txtCosto, 10);
                ControlesBloqueo.LimitarTextBox(txtPrecioAlquiler, 10);
                ControlesBloqueo.LimitarTextBox(txtPrecioPerdida, 10);

                ControlesBloqueo controlesBloqueo = new ControlesBloqueo();
                controlesBloqueo.BloquearControlesTXT(txtNombreProducto);
                controlesBloqueo.BloquearControlesTXT(txtCantidad);
                controlesBloqueo.BloquearControlesTXT(txtPrecioAlquiler);
                controlesBloqueo.BloquearControlesTXT(txtPrecioPerdida);
                controlesBloqueo.BloquearControlesTXT(txtCosto);

                txtNombreProducto.KeyPress += (s, e) => controlesBloqueo.ValidarSoloLetras(e);
                txtCantidad.KeyPress += (s, e) => controlesBloqueo.ValidarSoloNumeros(txtCantidad, e);
                txtPrecioAlquiler.KeyPress += (s, e) => controlesBloqueo.ValidarSoloNumeros(txtPrecioAlquiler, e);
                txtPrecioPerdida.KeyPress += (s, e) => controlesBloqueo.ValidarSoloNumeros(txtPrecioPerdida, e);
                txtCosto.KeyPress += (s, e) => controlesBloqueo.ValidarSoloNumeros(txtCosto, e);

                txtNombreProducto.TextChanged += (s, e) => errorProvider.SetError(txtNombreProducto, "");
                txtCantidad.TextChanged += (s, e) => errorProvider.SetError(txtCantidad, "");
                txtPrecioAlquiler.TextChanged += (s, e) => errorProvider.SetError(txtPrecioAlquiler, "");
                txtPrecioPerdida.TextChanged += (s, e) => errorProvider.SetError(txtPrecioPerdida, "");
                txtCosto.TextChanged += (s, e) => errorProvider.SetError(txtCosto, "");

                btnSubirFoto.Click += btnSubirFoto_Click;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmInventarioAgregar_Load(object sender, EventArgs e)
        {
            try
            {
                CargarServicios();
                CargarProveedor();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el formulario: " + ex.Message,
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

        private void btnSubirFoto_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog documento = new OpenFileDialog())
                {
                    documento.Title = "Seleccionar imagen del producto";
                    documento.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp";
                    documento.Multiselect = false;

                    if (documento.ShowDialog() != DialogResult.OK) return;

                    string archivoOrigen = documento.FileName;

                    FileInfo info = new FileInfo(archivoOrigen);
                    if (info.Length > 5 * 1024 * 1024)
                    {
                        MessageBox.Show("La imagen no debe superar los 5 MB.",
                                "ERROR-LONGITUD-003", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string carpetaFotos = Path.Combine(Application.StartupPath, "FotosProductos");
                    if (!Directory.Exists(carpetaFotos))
                    {
                        Directory.CreateDirectory(carpetaFotos);
                    }

                    string extension = Path.GetExtension(archivoOrigen);
                    string nombreUnico = $"producto_{DateTime.Now:yyyyMMddHHmmss}{extension}";
                    string rutaDestino = Path.Combine(carpetaFotos, nombreUnico);

                    File.Copy(archivoOrigen, rutaDestino, true);

                    ImagenProducto1 = Path.Combine("FotosProductos", nombreUnico);

                    using (var stream = new FileStream(rutaDestino, FileMode.Open, FileAccess.Read))
                    {
                        pbProducto.Image = Image.FromStream(stream);
                    }
                    pbProducto.SizeMode = PictureBoxSizeMode.StretchImage;

                    errorProvider.SetError(pbProducto, "");
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("No se pudo copiar la imagen: " + ex.Message,
                        "ERROR-ARCHIVO-160", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("No tiene permisos para guardar la imagen: " + ex.Message,
                        "ERROR-PERMISO-161", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la imagen: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                errorProvider.Clear();
                bool hayErrores = false;

                if (string.IsNullOrWhiteSpace(txtNombreProducto.Text))
                {
                    errorProvider.SetError(txtNombreProducto, "El nombre es obligatorio.");
                    hayErrores = true;
                }

                int cantidad = 0;
                if (string.IsNullOrWhiteSpace(txtCantidad.Text) ||
                    !int.TryParse(txtCantidad.Text.Trim(), out cantidad) || cantidad <= 0)
                {
                    errorProvider.SetError(txtCantidad, "Ingrese una cantidad válida (mayor a 0).");
                    hayErrores = true;
                }

                decimal precioCosto = 0;
                if (string.IsNullOrWhiteSpace(txtCosto.Text) ||
                    !decimal.TryParse(txtCosto.Text.Trim(), out precioCosto) || precioCosto <= 0)
                {
                    errorProvider.SetError(txtCosto, "Ingrese un precio de costo válido.");
                    hayErrores = true;
                }

                decimal precioAlquiler = 0;
                if (string.IsNullOrWhiteSpace(txtPrecioAlquiler.Text) ||
                    !decimal.TryParse(txtPrecioAlquiler.Text.Trim(), out precioAlquiler) || precioAlquiler <= 0)
                {
                    errorProvider.SetError(txtPrecioAlquiler, "Ingrese un precio de alquiler válido.");
                    hayErrores = true;
                }

                decimal precioPerdida = 0;
                if (string.IsNullOrWhiteSpace(txtPrecioPerdida.Text) ||
                    !decimal.TryParse(txtPrecioPerdida.Text.Trim(), out precioPerdida) || precioPerdida <= 0)
                {
                    errorProvider.SetError(txtPrecioPerdida, "Ingrese un precio de pérdida válido.");
                    hayErrores = true;
                }

                if (cbProveedor.SelectedValue == null)
                {
                    errorProvider.SetError(cbProveedor, "Debe seleccionar un proveedor.");
                    hayErrores = true;
                }

                if (cmbServicio.SelectedValue == null)
                {
                    errorProvider.SetError(cmbServicio, "Debe seleccionar un servicio.");
                    hayErrores = true;
                }

                //if (string.IsNullOrWhiteSpace(ImagenProducto1))
                //{
                //    errorProvider.SetError(pbProducto, "Debe subir una imagen del producto.");
                //    hayErrores = true;
                //}

                if (hayErrores)
                {
                    MessageBox.Show("Por favor corrija los campos marcados en rojo.",
                            "ERROR-CAMVACIO-001", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Productos nuevoProducto = new Productos
                {
                    NombreProducto = txtNombreProducto.Text.Trim(),
                    CantidadProducto = cantidad,
                    PrecioCostoProducto = precioCosto,
                    PrecioAlquilerProducto = precioAlquiler,
                    PrecioPerdidaProducto = precioPerdida,

                    IdProveedor = Convert.ToInt32(cbProveedor.SelectedValue),
                    IdServicio = Convert.ToInt32(cmbServicio.SelectedValue),
                    ImagenProducto = ImagenProducto1
                };

                Productos.AgregarProducto(nuevoProducto);

                MessageBox.Show("Producto agregado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Uno de los valores no tiene el formato correcto: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    errorProvider.SetError(txtNombreProducto, "Ya existe un producto con ese nombre.");
                    MessageBox.Show("Ya existe un producto con esos datos.",
                            "ERROR-DADUPLICADO-002", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Error de base de datos al agregar el producto: " + ex.Message,
                            "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el producto: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarServicios()
        {
            try
            {
                DataTable dt = Servicios.ObtenerServicios();

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron servicios disponibles.",
                            "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                cmbServicio.DataSource = dt;
                cmbServicio.DisplayMember = "NombreServicio";
                cmbServicio.ValueMember = "IdServicio";   
                cmbServicio.SelectedIndex = -1;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Una columna esperada no existe en los servicios: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los servicios: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarProveedor()
        {
            try
            {
                DataTable dt = Proveedores.ObtnerProveedor();

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron proveedores disponibles.",
                            "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                cbProveedor.DataSource = dt;
                cbProveedor.DisplayMember = "NombreProveedor";
                cbProveedor.ValueMember = "IdProveedor";
                cbProveedor.SelectedIndex = -1;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Una columna esperada no existe en los proveedores: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los proveedores: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
