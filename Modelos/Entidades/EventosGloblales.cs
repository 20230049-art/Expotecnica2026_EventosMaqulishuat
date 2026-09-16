using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class EventosGloblales
    {
        public static event EventHandler ClienteAgregado;
        public static event EventHandler ClienteActualizado;
        public static event EventHandler ClienteEliminado;

        public static void EnClienteAgregado()
        {
            ClienteAgregado?.Invoke(null, EventArgs.Empty);
        }

        public static void EnClienteActualizado()
        {
            ClienteActualizado?.Invoke(null, EventArgs.Empty);
        }

        public static void EnClienteEliminado()
        {
            ClienteEliminado?.Invoke(null, EventArgs.Empty);
        }

        public static event EventHandler DocumentosAgregados;
        public static event EventHandler DocumentosEliminados;

        public static void EnDocumentosEliminados()
        {
            DocumentosEliminados?.Invoke(null, EventArgs.Empty);
        }

        public static void EnDocumentosAgregados()
        {
            DocumentosAgregados?.Invoke(null, EventArgs.Empty);
        }
    }
}
