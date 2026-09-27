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

            string consulta = @"UPDATE RegistoPago
                        SET EstadoPago=@EstadoPago
                        WHERE IdRegistroPago=@IdRegistroPago";

            using (SqlCommand cmd = new SqlCommand(consulta, con))
            {
                cmd.Parameters.AddWithValue("@EstadoPago", estado);
                cmd.Parameters.AddWithValue("@IdRegistroPago", idpago);

                cmd.ExecuteNonQuery();
            }
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
        public DataTable BuscarRegistroPago(string busqueda)
        {
            DataTable dt = new DataTable();

            using (SqlConnection cn = ConexionDB.Conectar())
            using (SqlCommand cmd = new SqlCommand("BuscarRegistroPago", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@busqueda", SqlDbType.VarChar).Value =
                    string.IsNullOrWhiteSpace(busqueda) ? (object)DBNull.Value : busqueda;

                using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                {
                    ad.Fill(dt);
                }
            }

            return dt;
        }

        //Regitros por paginas--
        public static DataTable MostrarRegistroPagosPagina(int pagina, int registrosPorPagina, string estado, out int totalRegistros)
        {
            DataTable dt = new DataTable();
            totalRegistros = 0;

            try
            {
                using (SqlConnection cn = ConexionDB.Conectar())
                using (SqlCommand cmd = new SqlCommand("MostrarRegistroPagosPagina", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Pagina", SqlDbType.Int).Value = pagina;
                    cmd.Parameters.Add("@RegistrosPorPagina", SqlDbType.Int).Value = registrosPorPagina;
                    cmd.Parameters.Add("@Estado", SqlDbType.VarChar, 20).Value =
                        string.IsNullOrWhiteSpace(estado) ? (object)DBNull.Value : estado;

                    SqlParameter outTotal = new SqlParameter("@TotalRegistros", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outTotal);

                    using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                    {
                        ad.Fill(dt);
                    }

                    totalRegistros = outTotal.Value == DBNull.Value ? 0 : Convert.ToInt32(outTotal.Value);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener los pagos por paginas: " + ex.Message, ex);
            }

            return dt;
        }
    }
}
