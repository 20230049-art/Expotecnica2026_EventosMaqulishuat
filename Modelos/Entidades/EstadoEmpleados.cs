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
    public class EstadoEmpleados
    {
        private int idEstadoEmpleado;
        private string estadoEmpleado;

        public int IdEstadoEmpleado { get => idEstadoEmpleado; set => idEstadoEmpleado = value; }
        public string EstadoEmpleado { get => estadoEmpleado; set => estadoEmpleado = value; }

        //Mostrar en Estado empleado
        public static DataTable MostrarEstadoEmpleado()
        {
            SqlConnection conexion = ConexionDB.Conectar();
            string consulta = "SELECT IdEstadoEmpleado, EstadoEmpleado FROM EstadoEmpleado";
            SqlDataAdapter da = new SqlDataAdapter(consulta, conexion);
            DataTable tabla = new DataTable();
            da.Fill(tabla);
            conexion.Close();
            return tabla;
        }

        public static void ActualizarEstadoEmpleado(int idEmpleado, int idEstadoEmpleado)
        {
            using (SqlConnection conexion = ConexionDB.Conectar())
            {
                using (SqlCommand cmd = new SqlCommand("ActualizarEstadoEmpleado", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                    cmd.Parameters.AddWithValue("@IdEstadoEmpleado", idEstadoEmpleado);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
