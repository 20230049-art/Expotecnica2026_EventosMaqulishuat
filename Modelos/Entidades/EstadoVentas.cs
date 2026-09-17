using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class EstadoVentas
    {
        private int idEstadoVenta;
        private string estadoVenta;

        public int IdEstadoVenta { get => idEstadoVenta; set => idEstadoVenta = value; }
        public string EstadoVenta { get => estadoVenta; set => estadoVenta = value; }
    }
}
