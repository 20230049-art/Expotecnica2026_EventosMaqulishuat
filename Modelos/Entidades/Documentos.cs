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
    public class Documentos
    {
        private int idDocumentacion;
        private string nombreDocumentacion;
        private DateTime fechaDocumentacion;
        private DateTime fechaUltimoAcceso;
        private bool tipoDocumentos;
        private string archivoDocumentacion;
        private bool documentacionFavorita;

        public int IdDocumentacion { get => idDocumentacion; set => idDocumentacion = value; }
        public string NombreDocumentacion { get => nombreDocumentacion; set => nombreDocumentacion = value; }
        public DateTime FechaDocumentacion { get => fechaDocumentacion; set => fechaDocumentacion = value; }
        public DateTime FechaUltimoAcceso { get => fechaUltimoAcceso; set => fechaUltimoAcceso = value; }
        public bool TipoDocumentos { get => tipoDocumentos; set => tipoDocumentos = value; }
        public string ArchivoDocumentacion { get => archivoDocumentacion; set => archivoDocumentacion = value; }
        public bool DocumentacionFavorita { get => documentacionFavorita; set => documentacionFavorita = value; }

        public static DataTable MostrarDocumentos()
        {
            SQLiteConnection conection = ConexionDB.Conectar();
            {
                string comando = "SELECT * FROM VistaDocumentacionGerente;";
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(comando, conection))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        //AgregarDocumentos
        public static void AgregarDocumentos(Documentos nuevoDocumento)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql = @"
                    INSERT INTO Documentacion
                        (FechaDocumentacion, NombreDocumentacion, ArchivoDocumentacion, IdGerente)
                    VALUES
                        (@FechaDocumentacion, @NombreDocumentacion, @ArchivoDocumentacion, 1);";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {

                    command.Parameters.Add("@NombreDocumentacion", DbType.String).Value = nuevoDocumento.NombreDocumentacion;
                    command.Parameters.Add("@FechaDocumentacion", DbType.DateTime).Value = nuevoDocumento.FechaDocumentacion;
                    command.Parameters.Add("@ArchivoDocumentacion", DbType.String).Value = nuevoDocumento.ArchivoDocumentacion;

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        //Mostrar Documentos Favoritos
        public static DataTable MostrarDocumentosFavoritos()
        {
            SQLiteConnection conection = ConexionDB.Conectar();
            {
                string comando = "SELECT * FROM VistaDocumentosFavoritos;";
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(comando, conection))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }


        //Mostrar Documentos en Papelera
        public static DataTable MostrarDocumentosPapelera()
        {
            SQLiteConnection conection = ConexionDB.Conectar();
            {
                string comando = "SELECT * FROM VistaDocumentosPapelera;";
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(comando, conection))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        //Mandar Documento a Papelera
        public static bool PapeleraDocumentos(int idDocumentacion)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql = "UPDATE Documentacion SET DocumentacionPapelera = 1 WHERE IdDocumentacion = @Id;";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.Add("@Id", DbType.Int32).Value = idDocumentacion;
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }

        //Restaurar Documento
        public static bool DocumentosRestaurar(int idDocumentacion)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql = "UPDATE Documentacion SET DocumentacionPapelera = 0 WHERE IdDocumentacion = @Id;";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.Add("@Id", DbType.Int32).Value = idDocumentacion;
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }

        //Eliminar Documento
        public static bool DocumentosEliminar(int idDocumentacion)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql = "UPDATE Documentacion SET DocumentacionPapelera = 0 WHERE IdDocumentacion = @Id;";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.Add("@Id", DbType.Int32).Value = idDocumentacion;
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }

        //Buscar Documento

        public DataTable BuscarDocumentos(string busqueda)
        {

            DataTable tabla = new DataTable();
            busqueda = (busqueda ?? "").Trim();

            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql;
                if (string.IsNullOrEmpty(busqueda))
                {
                    sql = "SELECT * FROM VistaDocumentacionGerente;";
                }
                else
                {
                    sql = @"SELECT * FROM VistaDocumentacionGerente
                            WHERE NombreDocumentacion LIKE @q;";
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

        public static DataTable BuscarDocumentosPagina(string busqueda, int pagina, int registrosPorPagina, int tipoFiltro, out int totalRegistros)
        {
            totalRegistros = 0;
            DataTable dt = new DataTable();
            busqueda = (busqueda ?? "").Trim();

            try
            {
                using (SQLiteConnection cn = ConexionDB.Conectar())
                {
                    bool conBusqueda = !string.IsNullOrEmpty(busqueda);

                    string filtroTipo;
                    switch (tipoFiltro)
                    {
                        case 2:
                            filtroTipo = "AND DocumentacionFavorita = 1";
                            break;
                        case 3:
                            filtroTipo = "AND DocumentacionPapelera = 1";
                            break;
                        default:
                            filtroTipo = "AND DocumentacionPapelera = 0";
                            break;
                    }

                    string whereBase = "WHERE 1=1 " + filtroTipo;
                    if (conBusqueda) whereBase += " AND NombreDocumentacion LIKE @q";

                    string sqlCount = "SELECT COUNT(*) FROM Documentacion " + whereBase;

                    using (SQLiteCommand cmdTotal = new SQLiteCommand(sqlCount, cn))
                    {
                        if (conBusqueda)
                            cmdTotal.Parameters.Add("@q", DbType.String).Value = "%" + busqueda + "%";

                        totalRegistros = Convert.ToInt32(cmdTotal.ExecuteScalar());
                    }

                    int offset = (pagina - 1) * registrosPorPagina;

                    string sqlDatos = @"
                SELECT IdDocumentacion, NombreDocumentacion, FechaDocumentacion,
                       FechaUltimoAcceso, ArchivoDocumentacion,
                       DocumentacionFavorita, DocumentacionPapelera
                FROM Documentacion " + whereBase + @"
                ORDER BY IdDocumentacion DESC
                LIMIT @limit OFFSET @offset;";

                    using (SQLiteCommand cmd = new SQLiteCommand(sqlDatos, cn))
                    {
                        if (conBusqueda)
                            cmd.Parameters.Add("@q", DbType.String).Value = "%" + busqueda + "%";

                        cmd.Parameters.Add("@limit", DbType.Int32).Value = registrosPorPagina;
                        cmd.Parameters.Add("@offset", DbType.Int32).Value = offset;

                        using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar documentos paginados: " + ex.Message,
                    "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dt;
        }

        //Eliminar Documento permanentemente
        public static bool EliminarDocumentoPermanente(int idDocumentacion)
        {
            try
            {
                using (SQLiteConnection conexion = ConexionDB.Conectar())
                {
                    string sql = "DELETE FROM Documentacion WHERE IdDocumentacion = @Id;";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conexion))
                    {
                        cmd.Parameters.Add("@Id", DbType.Int32).Value = idDocumentacion;
                        int filasAfectadas = cmd.ExecuteNonQuery();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new Exception("Error al eliminar el documento permanentemente: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inesperado al eliminar el documento: " + ex.Message, ex);
            }
        }

        //Documento a favorito
        public static bool CambiarFavorito(int idDocumentacion, bool documentacionFavorita)
        {
            try
            {
                using (SQLiteConnection conexion = ConexionDB.Conectar())
                {
                    string sql = "UPDATE Documentacion SET DocumentacionFavorita = @Fav WHERE IdDocumentacion = @Id;";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conexion))
                    {
                        cmd.Parameters.Add("@Id", DbType.Int32).Value = idDocumentacion;
                        cmd.Parameters.Add("@Fav", DbType.Boolean).Value = documentacionFavorita ? 1 : 0;

                        int filasAfectadas = cmd.ExecuteNonQuery();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch (SQLiteException ex) { throw new Exception("No se pudo actualizar el estado del documento.", ex); 
            }
            catch (Exception ex) { throw new Exception("Ocurrió un error al cambiar el estado del documento.", ex);
            }
        }


        //Paginas 20 x 20
        public static DataTable MostrarDocumentosPagina(int pagina, int registrosPorPagina, int tipoFiltro, out int totalRegistros)
        {
            totalRegistros = 0;
            DataTable dt = new DataTable();

            try
            {
                using (SQLiteConnection connection = ConexionDB.Conectar())
                {
                    string sqlCount;
                    switch (tipoFiltro)
                    {
                        case 2:
                            sqlCount = "SELECT COUNT(*) FROM Documentacion WHERE DocumentacionPapelera = 0 AND DocumentacionFavorita = 1;";
                            break;
                        case 3:
                            sqlCount = "SELECT COUNT(*) FROM Documentacion WHERE DocumentacionPapelera = 1;";
                            break;
                        default:
                            sqlCount = "SELECT COUNT(*) FROM Documentacion WHERE DocumentacionPapelera = 0;";
                            break;
                    }

                    using (SQLiteCommand cmdTotal = new SQLiteCommand(sqlCount, connection))
                    {
                        totalRegistros = Convert.ToInt32(cmdTotal.ExecuteScalar());
                    }

                    int offset = (pagina - 1) * registrosPorPagina;

                    string sqlDatos = @"
                        SELECT IdDocumentacion, NombreDocumentacion, FechaDocumentacion,
                               FechaUltimoAcceso, ArchivoDocumentacion,
                               DocumentacionFavorita, DocumentacionPapelera
                        FROM Documentacion
                        WHERE (
                                (@TipoFiltro = 1 AND DocumentacionPapelera = 0)
                                OR (@TipoFiltro = 2 AND DocumentacionPapelera = 0 AND DocumentacionFavorita = 1)
                                OR (@TipoFiltro = 3 AND DocumentacionPapelera = 1)
                                OR (@TipoFiltro NOT IN (1,2,3) AND DocumentacionPapelera = 0)
                              )
                        ORDER BY IdDocumentacion DESC
                        LIMIT @limit OFFSET @offset;";

                    using (SQLiteCommand cmd = new SQLiteCommand(sqlDatos, connection))
                    {
                        cmd.Parameters.Add("@TipoFiltro", DbType.Int32).Value = tipoFiltro;
                        cmd.Parameters.Add("@limit", DbType.Int32).Value = registrosPorPagina;
                        cmd.Parameters.Add("@offset", DbType.Int32).Value = offset;

                        using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al paginar documentos: " + ex.Message,
                    "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            return dt;
        }
    }
}
