using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista.Utilidades
{
    public class Redondeo
    {
        public static void RedondearFig(Control control, int radio)
        {
            GraphicsPath forma = new GraphicsPath();

            forma.AddArc(0, 0, radio, radio, 180, 90);
            forma.AddArc(control.Width - radio, 0, radio, radio, 270, 90);
            forma.AddArc(control.Width - radio, control.Height - radio, radio, radio, 0, 90);
            forma.AddArc(0, control.Height - radio, radio, radio, 90, 90);

            forma.CloseFigure();

            control.Region = new Region(forma);
        }
    }
}
