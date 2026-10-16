using Modelos.Conexion_DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Servicios
    {
        private int idServicio;
        private string NombreServicio;

        public int IdServicio { get => idServicio; set => idServicio = value; }
        public string NombreServicio1 { get => NombreServicio; set => NombreServicio = value; }


        //Para aregar productos
        public static DataTable ObtenerServicios()
        {
            SQLiteConnection conexion = ConexionDB.Conectar();
            string consulta = "SELECT idServicio, NombreServicio FROM Servicio";
            SQLiteDataAdapter da = new SQLiteDataAdapter(consulta, conexion);
            DataTable tabla = new DataTable();
            da.Fill(tabla);
            conexion.Close();
            return tabla;
        }
    }
}
