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
    public class Documentos
    {
        private int idDocumentacion;
        private string nombreDocumentacion;
        private DateTime fechaDocumentacion;
        private DateTime fechaUltimoAcceso;
        private bool tipoDocumentos;
        private string archivoDocumentacion;

        public int IdDocumentacion { get => idDocumentacion; set => idDocumentacion = value; }
        public string NombreDocumentacion { get => nombreDocumentacion; set => nombreDocumentacion = value; }
        public DateTime FechaDocumentacion { get => fechaDocumentacion; set => fechaDocumentacion = value; }
        public DateTime FechaUltimoAcceso { get => fechaUltimoAcceso; set => fechaUltimoAcceso = value; }
        public bool TipoDocumentos { get => tipoDocumentos; set => tipoDocumentos = value; }
        public string ArchivoDocumentacion { get => archivoDocumentacion; set => archivoDocumentacion = value; }

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
    }
}
