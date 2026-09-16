using Modelos.Conexion_DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class TipoClientes
    {
        private int idTipoCliente;
        private string tipoCliente;

        public int IdTipoCliente { get => idTipoCliente; set => idTipoCliente = value; }
        public string TipoCliente { get => tipoCliente; set => tipoCliente = value; }

        //Para mostrar tipo cliente
        public static DataTable ObtenerTipoCliente()
        {
            SqlConnection conexion = ConexionDB.Conectar();
            string consulta = "SELECT IdTipoCliente, TipoCliente FROM TipoCliente";
            SqlDataAdapter da = new SqlDataAdapter(consulta, conexion);
            DataTable tabla = new DataTable();
            da.Fill(tabla);
            conexion.Close();
            return tabla;
        }
    }
}
