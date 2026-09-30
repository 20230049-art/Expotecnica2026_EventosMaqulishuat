using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vista.GerenteClientes;
using Vista.Utilidades; 

namespace Vista.GerenteCalendario
{
    public partial class frmCalendario : Form
    {
        private int anioActual;
        private int mesActual;

        public frmCalendario()
        {
            InitializeComponent();
        }

        private void frmCalendario_Load(object sender, EventArgs e)
        {
            anioActual = DateTime.Now.Year;
            mesActual = DateTime.Now.Month;
            showDias(mesActual, anioActual);
        }

        private void btnPaginaSiguiente_Click(object sender, EventArgs e)
        {
            mesActual++;
            if (mesActual > 12) { mesActual = 1; anioActual++; }
            showDias(mesActual, anioActual);
        }

        private void btnPaginaAnterior_Click(object sender, EventArgs e)
        {
            mesActual--;
            if (mesActual < 1) { mesActual = 12; anioActual--; }
            showDias(mesActual, anioActual);
        }

        private void showDias(int mes, int anio)
        {
            try
            {
                flpCalendario.Controls.Clear();

                string nombreDelMes = new DateTimeFormatInfo().GetMonthName(mes);
                lblMes.Text = nombreDelMes.ToUpper() + " " + anio;

                DateTime primerDiaDelMes = new DateTime(anio, mes, 1);
                int totalDias = DateTime.DaysInMonth(anio, mes);
                int offset = (int)primerDiaDelMes.DayOfWeek;

                for (int i = 0; i < offset; i++)
                {
                    CadaDia vacio = new CadaDia();
                    vacio.SetEventos(0);
                    flpCalendario.Controls.Add(vacio);
                }

                DataTable conteos = Fechas.ContarPorMes(anio, mes);
                var dic = new System.Collections.Generic.Dictionary<int, int>();
                foreach (DataRow fila in conteos.Rows)
                {
                    DateTime dia = Convert.ToDateTime(fila["Dia"]);
                    dic[dia.Day] = Convert.ToInt32(fila["TotalEventos"]);
                }

                for (int i = 1; i <= totalDias; i++)
                {
                    DateTime fechaDia = new DateTime(anio, mes, i);
                    CadaDia cadaDia = new CadaDia(fechaDia);

                    int total = dic.ContainsKey(i) ? dic[i] : 0;
                    cadaDia.SetEventos(total);

                    cadaDia.DiaClick += CadaDia_DiaClick;
                    flpCalendario.Controls.Add(cadaDia);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar el calendario: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CadaDia_DiaClick(object sender, DateTime fecha)
        {
            try
            {
                frmFondoNegro fondo = new frmFondoNegro();
                fondo.StartPosition = FormStartPosition.CenterParent;
                fondo.WindowState = FormWindowState.Maximized;
                fondo.Bounds        = Screen.FromControl(this).Bounds;

                using (var frm = new frmCalendarioRecordatorios(fecha))
                {
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.Owner = this;

                    fondo.Show();
                    frm.ShowDialog();

                    fondo.Close();
                    fondo.Dispose();
                }

                showDias(mesActual, anioActual);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir los recordatorios: " + ex.Message,
                        "ERROR-CAMBIO-103", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
