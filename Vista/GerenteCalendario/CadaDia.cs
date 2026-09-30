using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista.GerenteCalendario
{
    public partial class CadaDia : UserControl
    {
        public DateTime Fecha { get; private set; }
        public int TotalEventos { get; private set; } = 0;
        public bool EsVacio { get; private set; } = false;

        public event EventHandler<DateTime> DiaClick;

        public CadaDia()
        {
            InitializeComponent();
            EsVacio = true;
            lblDia.Text = "";
            this.BackColor = Color.Transparent;
            panel1.BackColor = Color.FromArgb(230, 220, 205);
            checkBox1.Hide();
        }

        public CadaDia(DateTime fecha) : this()
        {
            EsVacio = false;
            Fecha = fecha;
            lblDia.Text = fecha.Day.ToString();

            if (fecha.DayOfWeek == DayOfWeek.Sunday)
                lblDia.ForeColor = Color.FromArgb(180, 40, 40);
        }

        public void SetEventos(int total)
        {
            TotalEventos = total;

            if (EsVacio)
            {
                panel1.BackColor = Color.FromArgb(223, 196, 174);
                return;
            }

            if (total == 0)
                panel1.BackColor = Color.FromArgb(223, 196, 174);   
            else if (total <= 5)
                panel1.BackColor = Color.FromArgb(149, 235, 112);     
            else if (total <= 12)
                panel1.BackColor = Color.FromArgb(230, 180, 60);    
            else
                panel1.BackColor = Color.FromArgb(173, 55, 16);    
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            if (EsVacio) return;
            DiaClick?.Invoke(this, Fecha);
        }

        private void lblDia_Click(object sender, EventArgs e)
        {
            if (EsVacio) return;
            DiaClick?.Invoke(this, Fecha);
        }
    }
}
