using Modelos.Conexion_DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
            SQLiteConnection conection = ConexionDB.Conectar();
            string comando = "SELECT * FROM VistaReporteGerente;";
            SQLiteDataAdapter ad = new SQLiteDataAdapter(comando, conection);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            return dt;
        }

        //Agregar Reportes
        public static void IngresarReporte(Reportes nuevoReporte)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql = @"
                    INSERT INTO Reporte
                        (NombreReporte, DescripcionReporte, FechaReporte, IdGerente)
                    VALUES
                        (@NombreReporte, @DescripcionReporte, @FechaReporte, 1);";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@NombreReporte", nuevoReporte.NombreReporte1);
                    command.Parameters.AddWithValue("@DescripcionReporte", nuevoReporte.DescripciónReporte1);
                    command.Parameters.AddWithValue("@FechaReporte", nuevoReporte.FechaReporte1.ToString("yyyy-MM-dd"));

                    command.ExecuteNonQuery();
                }
            }
        }

        //Eliminar Reporte
        public static bool EliminarReporte(int idReporte)
        {
            try
            {
                using (SQLiteConnection connection = ConexionDB.Conectar())
                {
                    string sql = "DELETE FROM Reporte WHERE IdReporte = @IdReporte;";

                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.Add("@IdReporte", DbType.Int32).Value = idReporte;
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SQLiteException)
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
            busqueda = (busqueda ?? "").Trim();

            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql;
                if (string.IsNullOrEmpty(busqueda))
                {
                    sql = "SELECT * FROM VistaReporteGerente;";
                }
                else
                {
                    sql = @"SELECT IdReporte, NombreReporte, DescripcionReporte, FechaReporte
                            FROM Reporte
                            WHERE NombreReporte LIKE @q;";
                }

                using (var command = new SQLiteCommand(sql, connection))
                {
                    if (!string.IsNullOrEmpty(busqueda))
                    {
                        command.Parameters.Add("@q", DbType.String).Value = "%" + busqueda + "%";
                    }

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
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
                using (SQLiteConnection conexion = ConexionDB.Conectar())
                {
                    using (SQLiteCommand cmdTotal = new SQLiteCommand(
                        "SELECT COUNT(*) FROM Reporte;", conexion))
                    {
                        totalRegistros = Convert.ToInt32(cmdTotal.ExecuteScalar());
                    }

                    int offset = (pagina - 1) * registrosPorPagina;

                    string sql = @"
                        SELECT IdReporte, NombreReporte, DescripcionReporte, FechaReporte
                        FROM Reporte
                        ORDER BY IdReporte
                        LIMIT @limit OFFSET @offset;";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conexion))
                    {
                        cmd.Parameters.Add("@limit", DbType.Int32).Value = registrosPorPagina;
                        cmd.Parameters.Add("@offset", DbType.Int32).Value = offset;

                        using (SQLiteDataAdapter ad = new SQLiteDataAdapter(cmd))
                        {
                            ad.Fill(dt);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new Exception("Error al obtener los reportes paginados: " + ex.Message, ex);
            }

            return dt;
        }
    }
}
