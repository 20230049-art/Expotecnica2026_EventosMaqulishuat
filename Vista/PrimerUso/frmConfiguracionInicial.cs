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

namespace Vista.PrimerUso
{
    public partial class frmConfiguracionInicial : Form
    {
        private string rutaLogoRelativa = "";
        public frmConfiguracionInicial()
        {
            InitializeComponent();
        }

        private void btnFinalizarConfiguracion_Click(object sender, EventArgs e)
        {
            try
            {
                // ===== Validaciones básicas =====
                if (string.IsNullOrWhiteSpace(txtNombreEmpresa.Text) ||
                    string.IsNullOrWhiteSpace(txtCorreoEmpresa.Text) ||
                    string.IsNullOrWhiteSpace(txtTelefonoEmpresa.Text) ||
                    string.IsNullOrWhiteSpace(txtDireccionEmpresa.Text))
                {
                    MessageBox.Show("Completa todos los datos de la empresa.", "INFO",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNombreGerente.Text) ||
                    string.IsNullOrWhiteSpace(txtApellidoGerente.Text) ||
                    string.IsNullOrWhiteSpace(txtCorreoGerente.Text) ||
                    string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                    string.IsNullOrWhiteSpace(txtContrasena.Text))
                {
                    MessageBox.Show("Completa todos los datos del gerente.", "INFO",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (txtContrasena.Text.Length < 6)
                {
                    MessageBox.Show("La contraseña debe tener al menos 6 caracteres.", "INFO",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ===== 1) Guardar la configuración de la empresa =====
                var config = new DatosEmpresa
                {
                    NombreEmpresa = txtNombreEmpresa.Text.Trim(),
                    CorreoEmpresa = txtCorreoEmpresa.Text.Trim(),
                    TelefonoEmpresa = txtTelefonoEmpresa.Text.Trim(),
                    DireccionEmpresa = txtDireccionEmpresa.Text.Trim(),
                    LogoEmpresa = rutaLogoRelativa
                };

                if (!DatosEmpresa.Guardar(config))
                {
                    MessageBox.Show("No se pudo guardar la configuración de la empresa.", "ERROR",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ===== 2) Crear el gerente + usuario =====
                string hash = BCrypt.Net.BCrypt.HashPassword(txtContrasena.Text);

                var gerente = new Usuario
                {
                    NombreGerente = txtNombreGerente.Text.Trim(),
                    ApellidoGerente = txtApellidoGerente.Text.Trim(),
                    CorreoGerente = txtCorreoGerente.Text.Trim(),
                    NombreUsuario = txtUsuario.Text.Trim(),
                    ContrasenaUsuario = hash
                };

                if (!gerente.RegistrarGerente())
                {
                    MessageBox.Show("No se pudo crear el gerente. Verifica que el correo y usuario no existan.", "ERROR",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ===== 3) Pasar al siguiente form =====
                // Guardamos datos para que frmInicioCompleto los muestre
                DatosInicioCompleto.UsuarioGerente = txtUsuario.Text.Trim();
                DatosInicioCompleto.ContrasenaGerente = txtContrasena.Text;
                DatosInicioCompleto.LogoRuta = rutaLogoRelativa;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al finalizar la configuración: " + ex.Message, "ERROR",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSubirlogo_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp";
                    ofd.Title = "Seleccionar logo de la empresa";
                    if (ofd.ShowDialog() != DialogResult.OK) return;

                    // Validar tamaño (máx 5 MB)
                    var info = new System.IO.FileInfo(ofd.FileName);
                    if (info.Length > 5 * 1024 * 1024)
                    {
                        MessageBox.Show("La imagen no debe superar los 5 MB.", "ERROR-LONGITUD",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Carpeta destino: bin\Debug\Empresa
                    string carpeta = System.IO.Path.Combine(Application.StartupPath, "Empresa");
                    if (!System.IO.Directory.Exists(carpeta))
                        System.IO.Directory.CreateDirectory(carpeta);

                    string extension = System.IO.Path.GetExtension(ofd.FileName);
                    string nombreUnico = $"logo{extension}";
                    string rutaDestino = System.IO.Path.Combine(carpeta, nombreUnico);

                    System.IO.File.Copy(ofd.FileName, rutaDestino, true);

                    rutaLogoRelativa = System.IO.Path.Combine("Empresa", nombreUnico);

                    // Mostrar preview
                    using (var stream = new System.IO.FileStream(rutaDestino,
                           System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        pbLogo.Image = System.Drawing.Image.FromStream(stream);
                    }
                    pbLogo.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al subir el logo: " + ex.Message, "ERROR",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
