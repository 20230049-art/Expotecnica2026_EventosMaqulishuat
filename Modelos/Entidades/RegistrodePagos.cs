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
    public class RegistrodePagos
    {
        private int idPago;
        private int idCliente;
        private int idServicio;
        private string EstadoPago;
        private DateTime FechaPago;
        private int idVenta;

        public int IdPago { get => idPago; set => idPago = value; }
        public int IdCliente { get => idCliente; set => idCliente = value; }
        public int IdServicio { get => idServicio; set => idServicio = value; }
        public string EstadoPago1 { get => EstadoPago; set => EstadoPago = value; }
        public DateTime FechaPago1 { get => FechaPago; set => FechaPago = value; }
        public int IdVenta { get => idVenta; set => idVenta = value; }


        //Mostrar Registro de Pagos - Gerente
        public static DataTable RegistosdePagos()
        {
            SqlConnection conection = ConexionDB.Conectar();
            string comando = "SELECT * FROM VistaRegistroPagoGerente;";
            SqlDataAdapter ad = new SqlDataAdapter(comando, conection);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            return dt;
        }


        public static void ActualizaEstado(int idpago, string estado)
        {
            SqlConnection con = ConexionDB.Conectar();

            string consulta = @"UPDATE RegistroPago
                        SET EstadoPago=@Estado
                        WHERE idPago=@Id";

            SqlCommand cmd = new SqlCommand(consulta, con);

            cmd.Parameters.AddWithValue("@Estado", estado);
            cmd.Parameters.AddWithValue("@Id", idpago);
        }

        //Mostrar Registros de pagos proximos
        public static DataTable MostrarRegistroPagoNoPagados()
        {
            using (SqlConnection conection = ConexionDB.Conectar())
            {
                string comando = "SELECT * FROM VistaRegistroPagoGerente WHERE EstadoPago = 'No Pagado';";
                SqlDataAdapter ad = new SqlDataAdapter(comando, conection);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                return dt;
            }
        }

        //Mostrar Registros de pagos pendientes
        public static DataTable MostrarRegistroPagosPendientes()
        {
            using (SqlConnection conection = ConexionDB.Conectar())
            {
                string comando = "SELECT * FROM VistaRegistroPagoGerente WHERE EstadoPago = 'Pendiente';";
                SqlDataAdapter ad = new SqlDataAdapter(comando, conection);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                return dt;
            }
        }

        //Mostrar Registros de pagos realizados
        public static DataTable MostrarRegistroPagosRealizados()
        {
            using (SqlConnection conection = ConexionDB.Conectar())
            {
                string comando = "SELECT * FROM VistaRegistroPagoGerente WHERE EstadoPago = 'Pagado';";
                SqlDataAdapter ad = new SqlDataAdapter(comando, conection);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                return dt;
            }
        }

        //Buscar registros
        public DataTable BuscarReporte(string busqueda)
        {

            DataTable tabla = new DataTable();
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (var command = new SqlCommand("BucarRegistro", connection))
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
