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
            SqlConnection conection = ConexionDB.Conectar();
            {
                string comando = "SELECT * FROM VistaDocumentacionGerente;";
                using (SqlDataAdapter adapter = new SqlDataAdapter(comando, conection))
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
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (SqlCommand command = new SqlCommand("AgregarDocumento", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@NombreDocumentacion", SqlDbType.VarChar).Value = nuevoDocumento.NombreDocumentacion;
                    command.Parameters.Add("@FechaDocumentacion", SqlDbType.DateTime).Value = nuevoDocumento.FechaDocumentacion;
                    command.Parameters.Add("@ArchivoDocumentacion", SqlDbType.VarChar).Value = nuevoDocumento.ArchivoDocumentacion;

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        //Mostrar Documentos Favoritos
        public static DataTable MostrarDocumentosFavoritos()
        {
            SqlConnection conection = ConexionDB.Conectar();
            {
                string comando = "SELECT * FROM VistaDocumentosFavoritos;";
                using (SqlDataAdapter adapter = new SqlDataAdapter(comando, conection))
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
            SqlConnection conection = ConexionDB.Conectar();
            {
                string comando = "SELECT * FROM VistaDocumentosPapelera;";
                using (SqlDataAdapter adapter = new SqlDataAdapter(comando, conection))
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
            using (SqlConnection connection = ConexionDB.Conectar())

            using (SqlCommand command = new SqlCommand("DocumentoPapelera", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@IdDocumentacion", SqlDbType.Int).Value = idDocumentacion;

                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
        }

        //Restaurar Documento
        public static bool DocumentosRestaurar(int idDocumentacion)
        {
            using (SqlConnection connection = ConexionDB.Conectar())

            using (SqlCommand command = new SqlCommand("RestaurarDocumento", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@IdDocumentacion", SqlDbType.Int).Value = idDocumentacion;

                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
        }

        //Eliminar Documento
        public static bool DocumentosEliminar(int idDocumentacion)
        {
            using (SqlConnection connection = ConexionDB.Conectar())

            using (SqlCommand command = new SqlCommand("EliminarDocumento", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@IdDocumentacion", SqlDbType.Int).Value = idDocumentacion;

                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
        }

        //Buscar Documento

        public DataTable BuscarDocumentos(string busqueda)
        {

            DataTable tabla = new DataTable();
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (var command = new SqlCommand("BuscarDocumentos", connection))
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

        public static DataTable BuscarDocumentosPagina(string busqueda, int pagina, int registrosPorPagina, int tipoFiltro, out int totalRegistros)
        {
            totalRegistros = 0;
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection cn = ConexionDB.Conectar())
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("BuscarDocumentosPagina", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Busqueda", SqlDbType.VarChar, 180).Value = busqueda ?? "";
                        cmd.Parameters.Add("@Pagina", SqlDbType.Int).Value = pagina;
                        cmd.Parameters.Add("@RegistrosPorPagina", SqlDbType.Int).Value = registrosPorPagina;
                        cmd.Parameters.Add("@TipoFiltro", SqlDbType.Int).Value = tipoFiltro;

                        SqlParameter totalParam = new SqlParameter("@TotalRegistros", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(totalParam);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }

                        if (totalParam.Value != DBNull.Value)
                            totalRegistros = Convert.ToInt32(totalParam.Value);
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
                using (SqlConnection conexion = ConexionDB.Conectar())
                {
                    using (SqlCommand cmd = new SqlCommand("EliminarPermanenteDocumento", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IdDocumentacion", SqlDbType.Int).Value = idDocumentacion;

                        int filasAfectadas = cmd.ExecuteNonQuery();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch (SqlException ex)
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
                using (SqlConnection conexion = ConexionDB.Conectar())
                using (SqlCommand cmd = new SqlCommand("CambiarFavoritoDocumento", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdDocumentacion", SqlDbType.Int).Value = idDocumentacion;
                    cmd.Parameters.Add("@DocumentacionFavorita", SqlDbType.Bit).Value = documentacionFavorita;

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    return filasAfectadas > 0;
                }
            }
            catch (SqlException ex) { throw new Exception("No se pudo actualizar el estado del documento.", ex); 
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
                using (SqlConnection connection = ConexionDB.Conectar())
                {
                    using (SqlCommand cmd = new SqlCommand("MostrarDocumentosPagina", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Pagina", SqlDbType.Int).Value = pagina;
                        cmd.Parameters.Add("@RegistrosPorPagina", SqlDbType.Int).Value = registrosPorPagina;
                        cmd.Parameters.Add("@TipoFiltro", SqlDbType.Int).Value = tipoFiltro;

                        SqlParameter totalParam = new SqlParameter("@TotalRegistros", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(totalParam);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }

                        if (totalParam.Value != DBNull.Value)
                            totalRegistros = Convert.ToInt32(totalParam.Value);

                    }

                    connection.Close();
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
