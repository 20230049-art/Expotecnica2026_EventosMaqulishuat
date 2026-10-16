using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista.Utilidades
{
    public class RediseñoImagen : TableLayoutPanel
    {
        public RediseñoImagen()
        {
            // Activar doble búfer
            this.DoubleBuffered = true;

            // Estilos para evitar el parpadeo con imágenes de fondo
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
        }

    }
}
