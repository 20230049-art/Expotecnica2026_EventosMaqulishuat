using Modelos.Conexion_DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
            SQLiteConnection conection = ConexionDB.Conectar();
            string comando = "SELECT * FROM VistaRegistroPagoGerente;";
            SQLiteDataAdapter ad = new SQLiteDataAdapter(comando, conection);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            return dt;
        }


        public static void ActualizaEstado(int idpago, string estado)
        {
            SQLiteConnection con = ConexionDB.Conectar();

            string consulta = @"UPDATE RegistoPago
                        SET EstadoPago=@EstadoPago
                        WHERE IdRegistroPago=@IdRegistroPago";

            using (SQLiteCommand cmd = new SQLiteCommand(consulta, con))
            {
                cmd.Parameters.AddWithValue("@EstadoPago", estado);
                cmd.Parameters.AddWithValue("@IdRegistroPago", idpago);

                cmd.ExecuteNonQuery();
            }
        }

        //Mostrar Registros de pagos proximos
        public static DataTable MostrarRegistroPagoNoPagados()
        {
            using (SQLiteConnection conection = ConexionDB.Conectar())
            {
                string comando = "SELECT * FROM VistaRegistroPagoGerente WHERE EstadoPago = 'No Pagado';";
                SQLiteDataAdapter ad = new SQLiteDataAdapter(comando, conection);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                return dt;
            }
        }

        //Mostrar Registros de pagos pendientes
        public static DataTable MostrarRegistroPagosPendientes()
        {
            using (SQLiteConnection conection = ConexionDB.Conectar())
            {
                string comando = "SELECT * FROM VistaRegistroPagoGerente WHERE EstadoPago = 'Pendiente';";
                SQLiteDataAdapter ad = new SQLiteDataAdapter(comando, conection);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                return dt;
            }
        }

        //Mostrar Registros de pagos realizados
        public static DataTable MostrarRegistroPagosRealizados()
        {
            using (SQLiteConnection conection = ConexionDB.Conectar())
            {
                string comando = "SELECT * FROM VistaRegistroPagoGerente WHERE EstadoPago = 'Pagado';";
                SQLiteDataAdapter ad = new SQLiteDataAdapter(comando, conection);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                return dt;
            }
        }

        //Buscar registros
        public DataTable BuscarRegistroPago(string busqueda)
        {
            DataTable dt = new DataTable();
            busqueda = (busqueda ?? "").Trim();

            using (SQLiteConnection cn = ConexionDB.Conectar())
            {
                string sql;
                if (string.IsNullOrEmpty(busqueda))
                {
                    sql = "SELECT * FROM VistaRegistroPagoGerente ORDER BY IdRegistroPago;";
                }
                else
                {
                    sql = @"SELECT * FROM VistaRegistroPagoGerente
                            WHERE NombreCliente   LIKE @q
                               OR ApellidoCliente LIKE @q
                               OR NombreServicio  LIKE @q
                               OR CorreoCliente   LIKE @q
                               OR TelefonoCliente LIKE @q
                            ORDER BY IdRegistroPago;";
                }

                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    if (!string.IsNullOrEmpty(busqueda))
                    {
                        cmd.Parameters.Add("@q", DbType.String).Value = "%" + busqueda + "%";
                    }

                    using (SQLiteDataAdapter ad = new SQLiteDataAdapter(cmd))
                    {
                        ad.Fill(dt);
                    }
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
                using (SQLiteConnection cn = ConexionDB.Conectar())
                {
                    bool conFiltro = !string.IsNullOrWhiteSpace(estado);

                    string sqlCount = conFiltro
                        ? "SELECT COUNT(*) FROM VistaRegistroPagoGerente WHERE EstadoPago = @Estado;"
                        : "SELECT COUNT(*) FROM VistaRegistroPagoGerente;";

                    using (SQLiteCommand cmdTotal = new SQLiteCommand(sqlCount, cn))
                    {
                        if (conFiltro)
                            cmdTotal.Parameters.Add("@Estado", DbType.String).Value = estado;

                        totalRegistros = Convert.ToInt32(cmdTotal.ExecuteScalar());
                    }

                    int offset = (pagina - 1) * registrosPorPagina;

                    string sql = conFiltro
                        ? @"SELECT * FROM VistaRegistroPagoGerente
                            WHERE EstadoPago = @Estado
                            ORDER BY IdRegistroPago
                            LIMIT @limit OFFSET @offset;"
                        : @"SELECT * FROM VistaRegistroPagoGerente
                            ORDER BY IdRegistroPago
                            LIMIT @limit OFFSET @offset;";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                    {
                        if (conFiltro)
                            cmd.Parameters.Add("@Estado", DbType.String).Value = estado;

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
                throw new Exception("Error al obtener los pagos por paginas: " + ex.Message, ex);
            }

            return dt;
        }
    }
}
