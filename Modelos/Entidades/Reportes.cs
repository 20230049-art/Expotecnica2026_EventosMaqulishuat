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
        public static bool EliminarReporte(int idReporte)
        {
            try
            {
                using (SqlConnection connection = ConexionDB.Conectar())
                {
                    using (SqlCommand command = new SqlCommand("EliminarReporte", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@IdReporte", SqlDbType.Int).Value = idReporte;

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
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
        //Reportes pagina
        public static DataTable MostrarReportesPagina(int pagina, int registrosPorPagina, out int totalRegistros)
        {
            DataTable dt = new DataTable();
            totalRegistros = 0;

            try
            {
                using (SqlConnection conexion = ConexionDB.Conectar())
                {
                    using (SqlCommand cmd = new SqlCommand("MostrarReportesPagina", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Pagina", SqlDbType.Int).Value = pagina;
                        cmd.Parameters.Add("@RegistrosPorPagina", SqlDbType.Int).Value = registrosPorPagina;

                        SqlParameter outputTotal = new SqlParameter("@TotalRegistros", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputTotal);

                        using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                        {
                            ad.Fill(dt);
                        }

                        totalRegistros = outputTotal.Value == DBNull.Value
                            ? 0
                            : Convert.ToInt32(outputTotal.Value);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener los reportes paginados: " + ex.Message, ex);
            }

            return dt;
        }
    }
}
