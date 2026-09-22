using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class VentaActiva
    {
        private static int idVenta = 0;
        private static string nombreCliente = "";

        public static int IdVenta
        {
            get { return idVenta; }
            set { idVenta = value; }
        }

        public static string NombreCliente
        {
            get { return nombreCliente; }
            set { nombreCliente = value; }
        }

        public static bool HayVentaActiva
        {
            get { return IdVenta > 0; }
        }

        public static void Iniciar(int idVenta, string nombreCliente)
        {
            IdVenta = idVenta;
            NombreCliente = nombreCliente;
        }

        public static void Limpiar()
        {
            IdVenta = 0;
            NombreCliente = "";
        }
    }
}
