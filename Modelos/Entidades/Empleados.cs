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
    public class Empleados
    {
        private int idEmpleado;
        private string dUIEmpleado;
        private string nombreEmpleado;
        private string apellidoEmpleado;
        private string telefonoEmpleado;
        private string cargoEmpleado;
        private string correoEmpleado;
        private string cuentaBancariaEmpleado;
        private int idEstadoEmpleado;
        private string fotoEmpleado;
        private int idCliente;


        public int IdEmpleado { get => idEmpleado; set => idEmpleado = value; }
        public string DUIEmpleado { get => dUIEmpleado; set => dUIEmpleado = value; }
        public string NombreEmpleado { get => nombreEmpleado; set => nombreEmpleado = value; }
        public string ApellidoEmpleado { get => apellidoEmpleado; set => apellidoEmpleado = value; }
        public string TelefonoEmpleado { get => telefonoEmpleado; set => telefonoEmpleado = value; }
        public string CargoEmpleado { get => cargoEmpleado; set => cargoEmpleado = value; }
        public string CorreoEmpleado { get => correoEmpleado; set => correoEmpleado = value; }
        public string CuentaBancariaEmpleado { get => cuentaBancariaEmpleado; set => cuentaBancariaEmpleado = value; }
        public int IdEstadoEmpleado { get => idEstadoEmpleado; set => idEstadoEmpleado = value; }
        public string FotoEmpleado { get => fotoEmpleado; set => fotoEmpleado = value; }
        public int IdCliente { get => idCliente; set => idCliente = value; }

        public static DataTable MostrarEmpleado()
        {
            SqlConnection conection = ConexionDB.Conectar();
            string comando = "SELECT * FROM VistaEmpleadoGerente;";
            SqlDataAdapter ad = new SqlDataAdapter(comando, conection);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            return dt;
        }

        //Agregar Empleados     
        public static void AgregarEmpleado(Empleados nuevoEmpleado)
        {
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (SqlCommand command = new SqlCommand("AgregarEmpleado", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@NombreEmpleado", SqlDbType.VarChar).Value = nuevoEmpleado.NombreEmpleado;
                    command.Parameters.Add("@ApellidoEmpleado", SqlDbType.VarChar).Value = nuevoEmpleado.ApellidoEmpleado;
                    command.Parameters.Add("@TelefonoEmpleado", SqlDbType.VarChar).Value = nuevoEmpleado.TelefonoEmpleado;
                    command.Parameters.Add("@CargoEmpleado", SqlDbType.VarChar).Value = nuevoEmpleado.CargoEmpleado;
                    command.Parameters.Add("@DUIEmpleado", SqlDbType.VarChar).Value = nuevoEmpleado.DUIEmpleado;
                    command.Parameters.Add("@CorreoEmpleado", SqlDbType.VarChar).Value = nuevoEmpleado.CorreoEmpleado;
                    command.Parameters.Add("@CuentaBancariaEmpleado", SqlDbType.VarChar).Value = nuevoEmpleado.CuentaBancariaEmpleado;
                    command.Parameters.Add("@FotoEmpleado", SqlDbType.VarChar).Value = nuevoEmpleado.FotoEmpleado;

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

        }

        //Actualizar Empleados
        public static void ActualizarEmpleado(Empleados actualizarEmpleado)
        {
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (SqlCommand command = new SqlCommand("ActualizarEmpleado", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@IdEmpleado", SqlDbType.Int).Value = actualizarEmpleado.IdEmpleado;
                    command.Parameters.Add("@NombreEmpleado", SqlDbType.VarChar).Value = actualizarEmpleado.NombreEmpleado;
                    command.Parameters.Add("@ApellidoEmpleado", SqlDbType.VarChar).Value = actualizarEmpleado.ApellidoEmpleado;
                    command.Parameters.Add("@DUIEmpleado", SqlDbType.VarChar).Value = actualizarEmpleado.DUIEmpleado;
                    command.Parameters.Add("@TelefonoEmpleado", SqlDbType.VarChar).Value = actualizarEmpleado.TelefonoEmpleado;
                    command.Parameters.Add("@CorreoEmpleado", SqlDbType.VarChar).Value = actualizarEmpleado.CorreoEmpleado;
                    command.Parameters.Add("@CargoEmpleado", SqlDbType.VarChar).Value = actualizarEmpleado.CargoEmpleado;
                    command.Parameters.Add("@CuentaBancariaEmpleado", SqlDbType.VarChar).Value = actualizarEmpleado.CuentaBancariaEmpleado;
                    command.Parameters.Add("@FotoEmpleado", SqlDbType.VarChar).Value = actualizarEmpleado.FotoEmpleado;

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        //Buscar por Id-Empleado
        public static DataTable ObtenerEmpleadoId(int idCliente)
        {
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (SqlCommand command = new SqlCommand("EmpleadoId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@IdEmpleado", SqlDbType.Int).Value = idCliente;
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

        //BuscarEmpleado
        public DataTable BuscarEmpleado(string busqueda)
        {

            DataTable tablaEmpleado = new DataTable();
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (var command = new SqlCommand("BuscarEmpleado", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@busqueda", SqlDbType.VarChar).Value = busqueda ?? (object)DBNull.Value;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(tablaEmpleado);
                    }
                }
            }
            return tablaEmpleado;
        }
    }
}
