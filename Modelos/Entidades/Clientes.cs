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
    public class Clientes
    {
        private int idCliente;
        private string nombreCliente;
        private string apellidoCliente;
        private string telefonoCliente;
        private string NCRCliente;
        private string DUICliente;
        private string NITCliente;
        private int idTipoCliente;
        private string correoCliente;
        private bool estadoCliente;

        public int IdCliente { get => idCliente; set => idCliente = value; }
        public string NombreCliente { get => nombreCliente; set => nombreCliente = value; }
        public string ApellidoCliente { get => apellidoCliente; set => apellidoCliente = value; }
        public string TelefonoCliente { get => telefonoCliente; set => telefonoCliente = value; }
        public string NCRCliente1 { get => NCRCliente; set => NCRCliente = value; }
        public string DUICliente1 { get => DUICliente; set => DUICliente = value; }
        public string NITCliente1 { get => NITCliente; set => NITCliente = value; }
        public int IdTipoCliente { get => idTipoCliente; set => idTipoCliente = value; }
        public string CorreoCliente { get => correoCliente; set => correoCliente = value; }
        public bool EstadoCliente { get => estadoCliente; set => estadoCliente = value; }


        //Mostrar clientes Gerente y Empleado
        public static DataTable MostrarClientes()
        {
            using (SqlConnection conection = ConexionDB.Conectar())
            {
                string comando = "SELECT * FROM VistaDatosCliente";
                SqlDataAdapter ad = new SqlDataAdapter(comando, conection);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                return dt;
            }
        }

        //Mostrar Clientes Activos
        public static DataTable MostrarClientesActivos()
        {
            using (SqlConnection conection = ConexionDB.Conectar())
            {
                string comando = "SELECT * FROM VistaDatosCliente WHERE EstadoCliente = 1;";
                SqlDataAdapter ad = new SqlDataAdapter(comando, conection);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                return dt;
            }
        }

        //Mostrar Clientes Inactivos
        public static DataTable MostrarClienteInactivos()
        {
            using (SqlConnection conection = ConexionDB.Conectar())
            {
                string comando = "SELECT * FROM VistaDatosCliente WHERE EstadoCliente = 0;";
                SqlDataAdapter ad = new SqlDataAdapter(comando, conection);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                return dt;
            }
        }

        //Agregar Clientes a la base de Datos
        public static void AgregarCliente(Clientes nuevocliente)
        {
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (SqlCommand command = new SqlCommand("IngresarCliente", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@NombreCliente", SqlDbType.VarChar).Value = nuevocliente.NombreCliente;
                    command.Parameters.Add("@ApellidoCliente", SqlDbType.VarChar).Value = nuevocliente.ApellidoCliente;


                    command.Parameters.Add("@NCRCliente", SqlDbType.VarChar);
                    if (string.IsNullOrWhiteSpace(nuevocliente.NCRCliente))
                    {
                        command.Parameters["@NCRCliente"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters["@NCRCliente"].Value = nuevocliente.NCRCliente;
                    }

                    command.Parameters.Add("@DUICliente", SqlDbType.VarChar);
                    if (string.IsNullOrWhiteSpace(nuevocliente.DUICliente))
                    {
                        command.Parameters["@DUICliente"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters["@DUICliente"].Value = nuevocliente.DUICliente;
                    }

                    command.Parameters.Add("@NITCliente", SqlDbType.VarChar);
                    if (string.IsNullOrWhiteSpace(nuevocliente.NITCliente))
                    {
                        command.Parameters["@NITCliente"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters["@NITCliente"].Value = nuevocliente.NITCliente;
                    }

                    command.Parameters.Add("@TelefonoCliente", SqlDbType.VarChar).Value = nuevocliente.TelefonoCliente;
                    command.Parameters.Add("@CorreoCliente", SqlDbType.VarChar).Value = nuevocliente.CorreoCliente;
                    command.Parameters.Add("@IdTipoCliente", SqlDbType.Int).Value = nuevocliente.IdTipoCliente;

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        //Actualizar Cliente de la base de Datos
        public static void ActualizarCliente(Clientes actualizarcliente)
        {
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (SqlCommand command = new SqlCommand("ActualizarCliente", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@IdCliente", SqlDbType.Int).Value = actualizarcliente.IdCliente;
                    command.Parameters.Add("@NombreCliente", SqlDbType.VarChar).Value = actualizarcliente.NombreCliente;
                    command.Parameters.Add("@ApellidoCliente", SqlDbType.VarChar).Value = actualizarcliente.ApellidoCliente;

                    command.Parameters.Add("@NCRCliente", SqlDbType.VarChar);
                    if (string.IsNullOrWhiteSpace(actualizarcliente.NCRCliente))
                    {
                        command.Parameters["@NCRCliente"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters["@NCRCliente"].Value = actualizarcliente.NCRCliente;
                    }

                    command.Parameters.Add("@DUICliente", SqlDbType.VarChar);
                    if (string.IsNullOrWhiteSpace(actualizarcliente.DUICliente))
                    {
                        command.Parameters["@DUICliente"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters["@DUICliente"].Value = actualizarcliente.DUICliente;
                    }

                    command.Parameters.Add("@NITCliente", SqlDbType.VarChar);
                    if (string.IsNullOrWhiteSpace(actualizarcliente.NITCliente))
                    {
                        command.Parameters["@NITCliente"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters["@NITCliente"].Value = actualizarcliente.NITCliente;
                    }

                    command.Parameters.Add("@TelefonoCliente", SqlDbType.VarChar).Value = actualizarcliente.TelefonoCliente;
                    command.Parameters.Add("@CorreoCliente", SqlDbType.VarChar).Value = actualizarcliente.CorreoCliente;
                    command.Parameters.Add("@IdTipoCliente", SqlDbType.Int).Value = actualizarcliente.IdTipoCliente;

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        //Buscar por Id-Cliente
        public static DataTable ObtenerClienteId(int idCliente)
        {
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (SqlCommand command = new SqlCommand("ClienteId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@IdCliente", SqlDbType.Int).Value = idCliente;
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

        //Eliminar Cliente de la base de Datos
        public static void EliminarCliente(int idCliente)
        {
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (SqlCommand command = new SqlCommand("EliminarCliente", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@IdCliente", SqlDbType.Int).Value = idCliente;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

        }

        //Busqueda Cliente 
        public DataTable BuscarCliente(string busqueda)
        {

            DataTable tablaCliente = new DataTable();
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (var command = new SqlCommand("BuscarClientes", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@busqueda", SqlDbType.VarChar).Value = busqueda ?? (object)DBNull.Value;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(tablaCliente);
                    }
                }
            }
            return tablaCliente;

        }
    }
}
