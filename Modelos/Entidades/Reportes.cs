using Modelos.Conexion_DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modelos.Entidades
{
    public class Reportes
    {
        private int idReporte;
        private string NombreReporte;
        private string DescripciónReporte;
        private DateTime FechaReporte;

        public int IdReporte { get => idReporte; set => idReporte = value; }
        public string NombreReporte1 { get => NombreReporte; set => NombreReporte = value; }
        public string DescripciónReporte1 { get => DescripciónReporte; set => DescripciónReporte = value; }
        public DateTime FechaReporte1 { get => FechaReporte; set => FechaReporte = value; }

        //Mostrar Reportes
        public static DataTable MostrarReportes()
        {
            SqlConnection conection = ConexionDB.Conectar();
            string comando = "SELECT * FROM VistaReporteGerente;";
            SqlDataAdapter ad = new SqlDataAdapter(comando, conection);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            return dt;
        }

        //Agregar Reportes
        public static void IngresarReporte(Reportes nuevoReporte)
        {
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (SqlCommand command = new SqlCommand("AgregarReporte", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@NombreReporte", nuevoReporte.NombreReporte1);
                    command.Parameters.AddWithValue("@DescripcionReporte", nuevoReporte.DescripciónReporte1);
                    command.Parameters.AddWithValue("@FechaReporte", nuevoReporte.FechaReporte1);

                    command.ExecuteNonQuery();
                    connection.Close();

                }
            }
        }

        //Eliminar Reporte
        public void EliminarRegistro(int idReporte)
        {
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (SqlCommand command = new SqlCommand("EliminarReporte", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Idreporte", idReporte);

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        //Buscar
        public DataTable BuscarReporte(string busqueda)
        {

            DataTable tabla = new DataTable();
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (var command = new SqlCommand("BuscarReporte", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@busqueda", SqlDbType.VarChar).Value = busqueda ?? (object)DBNull.Value;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(tabla);
                    }
                }
            }
            return tabla;
        }

        //Eliminar
        public static bool EliminarCliente(int idCliente)
        {
            try
            {
                using (SqlConnection conexion = ConexionDB.Conectar())
                {
                    using (SqlCommand comando = new SqlCommand("EliminarReporte", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdReporte", idCliente);

                        conexion.Open();
                        comando.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el cliente: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }
    }
}
