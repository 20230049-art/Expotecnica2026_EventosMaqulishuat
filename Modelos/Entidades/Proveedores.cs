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
    public class Proveedores
    {
        private int idProveedor;
        private string nombreProveedor;
        private string correoProveedor;
        private string telefonoProveedor;
        private string fotoProveedor;
        private int idGerente;

        public int IdProveedor { get => idProveedor; set => idProveedor = value; }
        public string NombreProveedor { get => nombreProveedor; set => nombreProveedor = value; }
        public string CorreoProveedor { get => correoProveedor; set => correoProveedor = value; }
        public string TelefonoProveedor { get => telefonoProveedor; set => telefonoProveedor = value; }
        public string FotoProveedor { get => fotoProveedor; set => fotoProveedor = value; }
        public int IdGerente { get => idGerente; set => idGerente = value; }

        //Mostrar Proveedores
        public static DataTable MostrarProvedores()
        {
            SqlConnection conection = ConexionDB.Conectar();
            string comando = "SELECT * FROM VistaProveedorGerente;";
            SqlDataAdapter ad = new SqlDataAdapter(comando, conection);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            return dt;
        }

        //Mostrar Proveedores en un cmbox
        public static DataTable ObtnerProveedor()
        {
            SqlConnection conection = ConexionDB.Conectar();
            string comando = "SELECT idProveedor, NombreProveedor FROM Proveedor;";
            SqlDataAdapter ad = new SqlDataAdapter(comando, conection);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            return dt;
        }

        //Añadir Proveedor
        public static void IngresarProveedor(Proveedores nuevoProveedor)
        {
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (SqlCommand command = new SqlCommand("AgregarProveedor", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@NombreProveedor", SqlDbType.VarChar).Value = nuevoProveedor.NombreProveedor;
                    command.Parameters.Add("@CorreoProveedor", SqlDbType.VarChar).Value = nuevoProveedor.CorreoProveedor;
                    command.Parameters.Add("@TelefonoProveedor", SqlDbType.VarChar).Value = nuevoProveedor.TelefonoProveedor;
                    command.Parameters.Add("@FotoProveedor", SqlDbType.VarChar).Value = nuevoProveedor.FotoProveedor;

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        //Actualizar Proveedor
        public static void ActualizarProveedor(Proveedores actualizarProveedor)
        {
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (SqlCommand command = new SqlCommand("ActualizarProveedor", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = actualizarProveedor.IdProveedor;
                    command.Parameters.Add("@NombreProveedor", SqlDbType.VarChar).Value = actualizarProveedor.NombreProveedor;
                    command.Parameters.Add("@CorreoProveedor", SqlDbType.VarChar).Value = actualizarProveedor.CorreoProveedor;
                    command.Parameters.Add("@TelefonoProveedor", SqlDbType.VarChar).Value = actualizarProveedor.TelefonoProveedor;
                    command.Parameters.Add("@FotoProveedor", SqlDbType.VarChar).Value = actualizarProveedor.FotoProveedor;

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        //Buscar por Id-Proveedor
        public static DataTable ObtenerProveedorId(int idProveedor)
        {

            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (SqlCommand command = new SqlCommand("ProveedorId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = idProveedor;
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    using (SqlDataAdapter ad = new SqlDataAdapter(command))
                    {
                        ad.Fill(dt);
                    }

                    return dt;
                }
            }
        }


        //Proveedorer nombre
        public static DataTable ObtenerNombreProveedor()
        {
            SqlConnection conexion = ConexionDB.Conectar();
            string consulta = "SELECT IdProveedor, NombreProveedor FROM Proveedor";
            SqlDataAdapter da = new SqlDataAdapter(consulta, conexion);
            DataTable tabla = new DataTable();
            da.Fill(tabla);
            conexion.Close();
            return tabla;
        }

        //Buscar Proveedor
        public DataTable BuscarProveedor(string busqueda)
        {

            DataTable tablaProveedor = new DataTable();
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (var command = new SqlCommand("BuscarProveedor", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@busqueda", SqlDbType.VarChar).Value = busqueda ?? (object)DBNull.Value;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(tablaProveedor);
                    }
                }
            }
            return tablaProveedor;
        }
    }
}
