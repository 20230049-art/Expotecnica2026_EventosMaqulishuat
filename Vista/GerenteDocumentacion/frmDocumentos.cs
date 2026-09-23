using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vista.Utilidades;

namespace Vista.GerenteDocumentacion
{
    public partial class frmDocumentos : Form
    {
        private bool modoEliminar = false;
        private bool modoRestaurar = false;
        private bool modoEliminarPermanentemente = false;
        private int idDocumentoSeleccionado = 0;
        private DataRow filaSeleccionada = null;
        private Form activeForm = null;

        private ErrorProvider errorProvider;
        public frmDocumentos()
        {
            try
            {
                InitializeComponent();

                errorProvider = new ErrorProvider();
                errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
                errorProvider.ContainerControl = this;

                DataTable documentos = Documentos.MostrarDocumentos();
                CargarDocumentosEnPantalla(documentos);

                btnEliminarSiempre.Hide();
                btnRestaurarDocumento.Hide();

                EventosGloblales.DocumentosAgregados += RegarcarPanelDocumentos;
                EventosGloblales.DocumentosPapelera += RegarcarPanelDocumentos;
                EventosGloblales.DocumentosEliminados += RegarcarPanelDocumentos;

                try
                {
                    Redondeo.RedondearFig(pnlVistaMisDocumentos, 15);
                    Redondeo.RedondearFig(pnlContenedor, 15);
                    Redondeo.RedondearFig(panel14, 15);
                    Redondeo.RedondearFig(panel5, 15);
                    Redondeo.RedondearFig(panel13, 15);
                }
                catch (Exception exResize)
                {
                    System.Diagnostics.Debug.WriteLine("Error al redondear figuras: " + exResize.Message);
                }

                txtBarraBuscar.TextChanged += (s, e) => errorProvider.SetError(txtBarraBuscar, "");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RestaurarBotones()
        {
            try
            {
                btnDocumentosFavoritos.BackColor = Color.FromArgb(244, 220, 197);
                btnDocumentosPapelera.BackColor = Color.FromArgb(244, 220, 197);
                btnMisDocumentos.BackColor = Color.FromArgb(244, 220, 197);
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

        private void RegarcarPanelDocumentos(object sender, EventArgs e)
        {
            try
            {
                RecargarDocumentos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al recargar los documentos: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RecargarDocumentos()
        {
            try
            {
                DataTable documentos = null;

                if (btnMisDocumentos.BackColor == Color.FromArgb(237, 180, 141))
                {
                    documentos = Documentos.MostrarDocumentos();
                }
                else if (btnDocumentosFavoritos.BackColor == Color.FromArgb(237, 180, 141))
                {
                    documentos = Documentos.MostrarDocumentosFavoritos();
                }
                else if (btnDocumentosPapelera.BackColor == Color.FromArgb(237, 180, 141))
                {
                    documentos = Documentos.MostrarDocumentosPapelera();
                }
                else
                {
                    documentos = Documentos.MostrarDocumentos();
                }

                CargarDocumentosEnPantalla(documentos);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al recargar la lista de documentos: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CargarDocumentosEnPantalla(DataTable documentos)
        {
            try
            {
                flpDocumentos.Controls.Clear();

                if (documentos == null || documentos.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron documentos que mostrar.",
                            "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (DataRow fila in documentos.Rows)
                {
                    Panel panelDocumento = MostrarDocumentos(fila);

                    if (panelDocumento == null)
                    {
                        continue;
                    }

                    panelDocumento.DoubleClick += AbrirDocumento;

                    DataRow filaLocal = fila;
                    panelDocumento.Click += (s, e) =>
                    {
                        try
                        {
                            if (modoEliminar || modoRestaurar || modoEliminarPermanentemente)
                            {
                                object Documento = filaLocal["IdDocumentacion"];

                                if (Documento != DBNull.Value && Documento != null)
                                {
                                    idDocumentoSeleccionado = Convert.ToInt32(Documento);
                                    filaSeleccionada = filaLocal;

                                    if (modoEliminar)
                                        ConfirmarPapelera(filaLocal);
                                    else if (modoRestaurar)
                                        ConfirmarRestaurar(filaLocal);
                                    else if (modoEliminarPermanentemente)
                                        ConfirmarEliminarPermanentemente(filaLocal);
                                }
                                else
                                {
                                    MessageBox.Show("No se pudo obtener el ID del documento.",
                                            "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        catch (FormatException ex)
                        {
                            MessageBox.Show("El ID del documento no tiene el formato correcto: " + ex.Message,
                                    "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al seleccionar el documento: " + ex.Message,
                                    "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };

                    flpDocumentos.Controls.Add(panelDocumento);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar los documentos en pantalla: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel MostrarDocumentos(DataRow fila)
        {
            try
            {
                if (fila == null)
                {
                    return null;
                }

                Panel panelDocumento = new Panel();
                panelDocumento.Width = pnlContenedor.Width;
                panelDocumento.Height = pnlContenedor.Height;
                panelDocumento.BackColor = pnlContenedor.BackColor;
                panelDocumento.BorderStyle = pnlContenedor.BorderStyle;
                panelDocumento.Margin = new Padding(11);
                panelDocumento.Tag = fila["ArchivoDocumentacion"].ToString();

                PictureBox pbDocumento = new PictureBox();
                pbDocumento.Location = new Point(12, 8);
                pbDocumento.Size = pbImgDocumento.Size;
                pbDocumento.Image = pbImgDocumento.Image;
                pbDocumento.SizeMode = pbImgDocumento.SizeMode;

                Label lblTituloDocumento = new Label();
                lblTituloDocumento.Text = fila["NombreDocumentacion"].ToString();
                lblTituloDocumento.Text = AjustarTexto(lblTituloDocumento, lblTituloDocumento.Text);
                lblTituloDocumento.Location = new Point(78, 25);
                lblTituloDocumento.Font = new Font("Book Antiqua", 18, FontStyle.Bold);
                lblTituloDocumento.ForeColor = Color.FromArgb(64, 6, 6);
                lblTituloDocumento.MaximumSize = new Size(280, 28);
                lblTituloDocumento.AutoSize = true;

                Label lblFecha = new Label();
                DateTime fecha = Convert.ToDateTime(fila["FechaDocumentacion"]);
                lblFecha.Text = fecha.ToString("dd/MM/yyyy");
                lblFecha.Location = new Point(78, 59);
                lblFecha.Font = new Font("Book Antiqua", 15, FontStyle.Regular);
                lblFecha.AutoSize = true;

                panelDocumento.Controls.Add(pbDocumento);
                panelDocumento.Controls.Add(lblTituloDocumento);
                panelDocumento.Controls.Add(lblFecha);

                return panelDocumento;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Una columna esperada no existe en los datos del documento: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Un dato del documento no tiene el formato esperado: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el panel del documento: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private string AjustarTexto(Label lbl, string texto)
        {
            try
            {
                if (string.IsNullOrEmpty(texto)) return texto;

                if (texto.Length <= 3) return texto;

                while (texto.Length > 3 &&
                       TextRenderer.MeasureText(texto + "...", lbl.Font).Height > lbl.Height)
                {
                    texto = texto.Substring(0, texto.Length - 1);
                }

                return texto + "...";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al ajustar texto: " + ex.Message);
                return texto;
            }
        }

        private void AbrirDocumento(object sender, EventArgs e)
        {
            try
            {
                Control control = sender as Control;

                if (control == null)
                {
                    MessageBox.Show("No se pudo identificar el documento seleccionado.",
                            "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Panel panelDocumento;

                if (control is Panel)
                {
                    panelDocumento = (Panel)control;
                }
                else
                {
                    panelDocumento = control.Parent as Panel;
                }

                if (panelDocumento == null || panelDocumento.Tag == null)
                {
                    MessageBox.Show("El documento no tiene una ruta válida asociada.",
                            "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string rutaRelativa = panelDocumento.Tag.ToString();
                string rutaCompleta = Path.Combine(Application.StartupPath, rutaRelativa);

                if (File.Exists(rutaCompleta))
                {
                    Process.Start(new ProcessStartInfo()
                    {
                        FileName = rutaCompleta,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("No se encontró el documento en la ruta esperada.",
                            "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                MessageBox.Show("No se pudo abrir el documento con el programa predeterminado: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el documento: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog CargarArchivo = new OpenFileDialog())
                {
                    CargarArchivo.Title = "Seleccionar documento";
                    CargarArchivo.Filter = "Documentos|*.pdf;*.docx;*.xlsx|Todos los archivos|*.*";
                    CargarArchivo.Multiselect = false;

                    if (CargarArchivo.ShowDialog() != DialogResult.OK) return;

                    string archivoOrigen = CargarArchivo.FileName;

                    if (string.IsNullOrWhiteSpace(archivoOrigen) || !File.Exists(archivoOrigen))
                    {
                        MessageBox.Show("El archivo seleccionado no existe o no es válido.",
                                "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string nombreDocumento = Path.GetFileName(archivoOrigen);
                    string carpetaDocumentos = Path.Combine(Application.StartupPath, "Documentos");

                    if (!Directory.Exists(carpetaDocumentos))
                    {
                        Directory.CreateDirectory(carpetaDocumentos);
                    }

                    string rutaDestino = Path.Combine(carpetaDocumentos, nombreDocumento);
                    File.Copy(archivoOrigen, rutaDestino, true);

                    Documentos nuevoDocumento = new Documentos()
                    {
                        NombreDocumentacion = nombreDocumento,
                        FechaDocumentacion = DateTime.Now.Date,
                        ArchivoDocumentacion = rutaDestino
                    };

                    Documentos.AgregarDocumentos(nuevoDocumento);

                    EventosGloblales.EnDocumentosAgregados();

                    MessageBox.Show("Documento subido y registrado correctamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("No se pudo copiar el archivo. Puede estar en uso o sin permisos: " + ex.Message,
                        "ERROR-ARCHIVO-160", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("No tiene permisos para escribir en la carpeta de documentos: " + ex.Message,
                        "ERROR-PERMISO-161", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error de base de datos al registrar el documento: " + ex.Message,
                        "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al subir el documento:\n" + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDocumentosFavoritos_Click(object sender, EventArgs e)
        {
            try
            {
                ActivarBoton(btnDocumentosFavoritos);
                DataTable documentos = Documentos.MostrarDocumentosFavoritos();
                CargarDocumentosEnPantalla(documentos);
                btnEliminarSiempre.Hide();
                btnRestaurarDocumento.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los documentos favoritos: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDocumentosPapelera_Click(object sender, EventArgs e)
        {
            try
            {
                ActivarBoton(btnDocumentosPapelera);
                DataTable documentos = Documentos.MostrarDocumentosPapelera();
                CargarDocumentosEnPantalla(documentos);
                btnEliminarSiempre.Show();
                btnRestaurarDocumento.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los documentos de la papelera: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMisDocumentos_Click(object sender, EventArgs e)
        {
            try
            {
                ActivarBoton(btnMisDocumentos);
                DataTable documentos = Documentos.MostrarDocumentos();
                CargarDocumentosEnPantalla(documentos);
                btnEliminarSiempre.Hide();
                btnRestaurarDocumento.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar mis documentos: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmDocumentos_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                EventosGloblales.DocumentosAgregados -= RegarcarPanelDocumentos;
                EventosGloblales.DocumentosPapelera -= RegarcarPanelDocumentos;
                EventosGloblales.DocumentosEliminados -= RegarcarPanelDocumentos;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al desuscribir eventos: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                modoEliminar = true;
                modoRestaurar = false;
                modoEliminarPermanentemente = false;

                SombrearPanelEliminar();

                MessageBox.Show("Selecciona el documento que deseas eliminar.", "INFO-DOCUMENTO-PAPELERA",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al activar el modo eliminar: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SombrearPanelEliminar()
        {
            try
            {
                foreach (Panel panel in flpDocumentos.Controls.OfType<Panel>())
                {
                    panel.BackColor = Color.Salmon;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al sombrear paneles: " + ex.Message);
            }
        }

        private void QuitarSombreadoTodos()
        {
            try
            {
                foreach (Panel panel in flpDocumentos.Controls.OfType<Panel>())
                {
                    panel.BackColor = Color.FromArgb(255, 251, 234);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al quitar sombreado: " + ex.Message);
            }
        }

        private void ConfirmarPapelera(DataRow fila)
        {
            try
            {
                filaSeleccionada = fila;
                string nombreDocumento = fila["NombreDocumentacion"].ToString();

                DialogResult resultado = MessageBox.Show($"¿Estás seguro de eliminar el documento \"{nombreDocumento}\"?\n\n" + "El documento se moverá a la papelera.",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                if (resultado == DialogResult.Yes)
                {
                    EliminarDocumento();
                }
                else
                {
                    modoEliminar = false;
                    idDocumentoSeleccionado = 0;
                    filaSeleccionada = null;
                    QuitarSombreadoTodos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al confirmar la eliminación: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void EliminarDocumento()
        {
            try
            {
                bool exito = Documentos.PapeleraDocumentos(idDocumentoSeleccionado);
                if (exito)
                {
                    MessageBox.Show("Documento movido a la papelera correctamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RecargarDocumentos();
                }
                else
                {
                    MessageBox.Show("No se pudo mover el documento a la papelera.",
                            "ERROR-ACTUALIZAR-009", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error de base de datos al eliminar el documento: " + ex.Message,
                        "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el documento: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                modoEliminar = false;
                idDocumentoSeleccionado = 0;
                filaSeleccionada = null;
                QuitarSombreadoTodos();
            }
        }

        private void SombrearPanelRestaurar()
        {
            try
            {
                foreach (Panel panel in flpDocumentos.Controls.OfType<Panel>())
                {
                    panel.BackColor = Color.FromArgb(100, 85, 198);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al sombrear paneles: " + ex.Message);
            }
        }

        private void SombrearPanelEliminarPermanentemente()
        {
            try
            {
                foreach (Panel panel in flpDocumentos.Controls.OfType<Panel>())
                {
                    panel.BackColor = Color.FromArgb(120, 25, 14);
                    panel.ForeColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al sombrear paneles: " + ex.Message);
            }
        }

        private void btnRestaurarDocumento_Click(object sender, EventArgs e)
        {
            try
            {
                modoRestaurar = true;
                modoEliminar = false;
                modoEliminarPermanentemente = false;

                SombrearPanelRestaurar();

                MessageBox.Show("Selecciona el documento que desea Restaurar.", "INFO-DOCUMENTO-RESTAURAR",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al activar el modo restaurar: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfirmarRestaurar(DataRow fila)
        {
            try
            {
                filaSeleccionada = fila;
                string nombreDocumento = fila["NombreDocumentacion"].ToString();

                DialogResult resultado = MessageBox.Show(
                    $"¿Estás seguro de restaurar el documento \"{nombreDocumento}\"?\n\n" + "El documento volverá a tus documentos.",
                    "Confirmar restauración", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);

                if (resultado == DialogResult.Yes)
                {
                    RestaurarDocumento();
                }
                else
                {
                    modoRestaurar = false;
                    idDocumentoSeleccionado = 0;
                    filaSeleccionada = null;
                    QuitarSombreadoTodos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al confirmar la restauración: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RestaurarDocumento()
        {
            try
            {
                bool exito = Documentos.DocumentosRestaurar(idDocumentoSeleccionado);
                if (exito)
                {
                    MessageBox.Show("Documento restaurado correctamente.", "PROCEDIMIENTO-EXITOSO",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RecargarDocumentos();
                }
                else
                {
                    MessageBox.Show("No se pudo restaurar el documento de la papelera.",
                            "ERROR-ACTUALIZAR-009", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error de base de datos al restaurar el documento: " + ex.Message,
                        "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al restaurar el documento: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                modoRestaurar = false;
                idDocumentoSeleccionado = 0;
                filaSeleccionada = null;
                QuitarSombreadoTodos();
            }
        }

        private void btnEliminarSiempre_Click(object sender, EventArgs e)
        {
            try
            {
                modoEliminarPermanentemente = true;
                modoEliminar = false;
                modoRestaurar = false;

                SombrearPanelEliminarPermanentemente();

                MessageBox.Show( "Selecciona el documento que deseas eliminar permanentemente.\n\n" + " Esta acción NO se puede deshacer.",
                    "INFO-DOCUMENTO-ELIMINAR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al activar el modo eliminar permanentemente: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfirmarEliminarPermanentemente(DataRow fila)
        {
            try
            {
                filaSeleccionada = fila;
                string nombreDocumento = fila["NombreDocumentacion"].ToString();

                DialogResult confirmacion = MessageBox.Show(
                    $"¿Estás seguro de eliminar PERMANENTEMENTE el documento?\n\n" + $"Documento: \"{nombreDocumento}\"\n\n" +
                    "Esta acción NO se puede deshacer.", "Eliminación Permanente", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                if (confirmacion == DialogResult.Yes)
                {
                    EliminarPermanentemente();
                }
                else
                {
                    modoEliminarPermanentemente = false;
                    idDocumentoSeleccionado = 0;
                    filaSeleccionada = null;
                    QuitarSombreadoTodos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al confirmar la eliminación permanente: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarPermanentemente()
        {
            try
            {
                string rutaRelativa = filaSeleccionada != null
                    ? filaSeleccionada["ArchivoDocumentacion"].ToString()
                    : string.Empty;

                bool exito = Documentos.EliminarDocumentoPermanente(idDocumentoSeleccionado);

                if (!exito)
                {
                    MessageBox.Show("No se pudo eliminar el documento de la base de datos.",
                            "ERROR-ELIMINAR-015", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!string.IsNullOrWhiteSpace(rutaRelativa))
                {
                    string rutaCompleta = Path.IsPathRooted(rutaRelativa)
                        ? rutaRelativa
                        : Path.Combine(Application.StartupPath, rutaRelativa);

                    if (File.Exists(rutaCompleta))
                    {
                        try
                        {
                            File.Delete(rutaCompleta);
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show(
                                "El registro se eliminó, pero no se pudo borrar el archivo físico:\n" + ex.Message,
                                "ERROR-ARCHIVO-160", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            MessageBox.Show(
                                "El registro se eliminó, pero no tiene permisos para borrar el archivo:\n" + ex.Message,
                                "ERROR-PERMISO-161", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }

                MessageBox.Show("Documento eliminado permanentemente.", "PROCEDIMIENTO-EXITOSO",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                RecargarDocumentos();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error de base de datos al eliminar el documento: " + ex.Message,
                        "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el documento: " + ex.Message,
                        "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                modoEliminarPermanentemente = false;
                idDocumentoSeleccionado = 0;
                filaSeleccionada = null;
                QuitarSombreadoTodos();
            }
        }

        private void txtBarraBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string busqueda = txtBarraBuscar.Text.Trim();

                if (string.IsNullOrWhiteSpace(busqueda))
                {
                    errorProvider.SetError(txtBarraBuscar, "");
                    RecargarDocumentos();
                    return;
                }

                Documentos documentos = new Documentos();
                DataTable documentosFiltrados = documentos.BuscarDocumentos(busqueda);
                CargarDocumentosEnPantalla(documentosFiltrados);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(txtBarraBuscar, "Error en la búsqueda.");
                MessageBox.Show("Error al buscar documentos: " + ex.Message,
                        "ERROR-NODATO-007", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
