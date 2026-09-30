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

namespace Vista.GerenteCalendario
{
    public partial class frmCalendarioRecordatorios : Form
    {
        private DateTime _fecha;

        public frmCalendarioRecordatorios()
        {
            InitializeComponent();
        }

        public frmCalendarioRecordatorios(DateTime fecha) : this()
        {
            _fecha = fecha;
        }

        private void frmCalendarioRecordatorios_Load(object sender, EventArgs e)
        {
            try
            {
                lblDia.Text = _fecha.ToString("d 'de' MMMM").ToUpper();
                CargarRecordatorios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar recordatorios: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarRecordatorios()
        {
            try
            {
                flpRecordatorios.Controls.Clear();

                DataTable dt = Fechas.MostrarPorDia(_fecha);

                if (dt.Rows.Count == 0)
                {
                    Label lblVacio = new Label();
                    lblVacio.Text = "No hay recordatorios para este día.";
                    lblVacio.Font = new Font("Book Antiqua", 12, FontStyle.Italic);
                    lblVacio.ForeColor = Color.Gray;
                    lblVacio.AutoSize = true;
                    lblVacio.Margin = new Padding(10);
                    flpRecordatorios.Controls.Add(lblVacio);
                    return;
                }

                foreach (DataRow fila in dt.Rows)
                {
                    Panel tarjeta = CrearTarjetaRecordatorio(fila);
                    if (tarjeta != null) flpRecordatorios.Controls.Add(tarjeta);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
            }
        }

        private Panel CrearTarjetaRecordatorio(DataRow fila)
        {
            try
            {
                Panel panel = new Panel();
                panel.Width = flpRecordatorios.Width - 40;
                panel.Height = 90;
                panel.BackColor = Color.FromArgb(253, 241, 217);
                panel.BorderStyle = BorderStyle.FixedSingle;
                panel.Margin = new Padding(8);
                panel.Tag = fila["IdFecha"];
                panel.Cursor = Cursors.Hand;
                Redondeo.RedondearFig(panel, 10);

                TimeSpan hora = (TimeSpan)fila["HoraFecha"];

                Label lblHora = new Label();
                lblHora.Text = "Hora: " + DateTime.Today.Add(hora).ToString("hh:mm tt");
                lblHora.Font = new Font("Bookman Old Style", 10, FontStyle.Bold);
                lblHora.ForeColor = Color.FromArgb(64, 6, 6);
                lblHora.Location = new Point(15, 10);
                lblHora.AutoSize = true;

                Label lblCliente = new Label();
                lblCliente.Text = "Cliente: " + fila["NombreCliente"];
                lblCliente.Font = new Font("Bookman Old Style", 11, FontStyle.Bold);
                lblCliente.ForeColor = Color.FromArgb(64, 6, 6);
                lblCliente.Location = new Point(15, 35);
                lblCliente.AutoSize = true;

                Label lblLugar = new Label();
                lblLugar.Text = "Ubicación: " + fila["LugarFecha"];
                lblLugar.Font = new Font("Bookman Old Style", 10, FontStyle.Regular);
                lblLugar.ForeColor = Color.FromArgb(100, 50, 30);
                lblLugar.Location = new Point(15, 60);
                lblLugar.AutoSize = true;

                Label lblDiaFecha = new Label();
                lblDiaFecha.Text = "Día: " + Convert.ToDateTime(fila["DiaFecha"]).ToString("dd/MM/yyyy");
                lblDiaFecha.Font = new Font("Bookman Old Style", 10, FontStyle.Regular);
                lblDiaFecha.ForeColor = Color.FromArgb(100, 50, 30);
                lblDiaFecha.Location = new Point(350, 10);
                lblDiaFecha.AutoSize = true;

                EventHandler clickTarjeta = (s, ev) =>
                {
                    int idFecha = Convert.ToInt32(panel.Tag);
                    using (var frm = new frmCalendarioActualizarEliminar(idFecha))
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                            CargarRecordatorios();
                    }
                };

                panel.Click += clickTarjeta;
                lblHora.Click += clickTarjeta;
                lblCliente.Click += clickTarjeta;
                lblLugar.Click += clickTarjeta;
                lblDiaFecha.Click += clickTarjeta;

                panel.Controls.Add(lblHora);
                panel.Controls.Add(lblCliente);
                panel.Controls.Add(lblLugar);
                panel.Controls.Add(lblDiaFecha);

                return panel;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al crear tarjeta: " + ex.Message);
                return null;
            }
        }

        private void btnAnadirRecordatorio_Click(object sender, EventArgs e)
        {
            using (var frm = new frmCalendarioAgregarCitas(_fecha))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    CargarRecordatorios();
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
