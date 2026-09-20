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
    public class Venta
    {
        private int idVenta;
        private DateTime fechaVenta;
        private DateTime fechaUso;
        private int idEmpleado;
        private int idCliente;
        private decimal subtotalVenta;
        private decimal iva;
        private decimal descuentoVenta;
        private decimal totalVenta;
        private int idEstadoVenta;
        private string estadoVenta;
        private int idTipoPagoVenta;
        private string tipoPagoVenta;
        private int idFactura;

        public int IdVenta { get => idVenta; set => idVenta = value; }
        public DateTime FechaVenta { get => fechaVenta; set => fechaVenta = value; }
        public DateTime FechaUso { get => fechaUso; set => fechaUso = value; }
        public int IdEmpleado { get => idEmpleado; set => idEmpleado = value; }
        public int IdCliente { get => idCliente; set => idCliente = value; }
        public decimal SubtotalVenta { get => subtotalVenta; set => subtotalVenta = value; }
        public decimal Iva { get => iva; set => iva = value; }
        public decimal DescuentoVenta { get => descuentoVenta; set => descuentoVenta = value; }
        public decimal TotalVenta { get => totalVenta; set => totalVenta = value; }
        public int IdEstadoVenta { get => idEstadoVenta; set => idEstadoVenta = value; }
        public string EstadoVenta { get => estadoVenta; set => estadoVenta = value; }
        public int IdTipoPagoVenta { get => idTipoPagoVenta; set => idTipoPagoVenta = value; }
        public string TipoPagoVenta { get => tipoPagoVenta; set => tipoPagoVenta = value; }
        public int IdFactura { get => idFactura; set => idFactura = value; }



        //Ingresar Venta
        public int IngresarVenta(int idEmpleado, int idCliente, DateTime fechaUso, int idTipoPago)
        {
            using (SqlConnection conexion = ConexionDB.Conectar())
            {
                using (SqlCommand command = new SqlCommand("IngresarVenta", conexion))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                    command.Parameters.AddWithValue("@IdCliente", idCliente);
                    command.Parameters.AddWithValue("@FechaUso", fechaUso.Date);
                    command.Parameters.AddWithValue("@IdTipoPago", idTipoPago);

                    var result = command.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }

        //Obtener totales de Venta
        public (decimal Subtotal, decimal Iva, decimal Descuento, decimal Total, string TipoPago, string EstadoVenta)
    ObtenerTotalesVenta(int idVenta)
        {
            using (SqlConnection conexion = ConexionDB.Conectar())
            {
                SqlCommand command = new SqlCommand("ObtenerTotalesVenta", conexion);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@IdVenta", SqlDbType.Int).Value = idVenta;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return (
                            Subtotal: (decimal)reader["SubtotalVenta"],
                            Iva: (decimal)reader["Iva"],
                            Descuento: reader["DescuentoVenta"] != DBNull.Value ? (decimal)reader["DescuentoVenta"] : 0,
                            Total: (decimal)reader["TotalVenta"],
                            TipoPago: reader["TipoPago"].ToString(),
                            EstadoVenta: reader["EstadoVenta"].ToString()
                        );
                    }
                }
            }

            return (0, 0, 0, 0, "", "");
        }

        //Descuento en la Venta
        public void AplicarDescuentoVenta(int idVenta, decimal descuento)
        {
            using (SqlConnection conexion = ConexionDB.Conectar())
            {
                SqlCommand command = new SqlCommand("AplicarDescuentoVenta", conexion);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@IdVenta", SqlDbType.Int).Value = idVenta;
                command.Parameters.Add("@Descuento", SqlDbType.Decimal).Value = descuento;

                command.ExecuteNonQuery();
                conexion.Close();
            }
        }

        //Obtener ventas por la fecha de uso
        public List<Venta> VentasPorFechaUso(DateTime fechaVenta, DateTime fechaUso)
        {
            List<Venta> ventas = new List<Venta>();
            using (SqlConnection conexion = ConexionDB.Conectar())
            {
                SqlCommand command = new SqlCommand("ObtenerVentasFechaUso", conexion);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("FechaVenta", SqlDbType.Date).Value = fechaVenta;
                command.Parameters.Add("FechaUso", SqlDbType.Date).Value = fechaUso;

                //connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ventas.Add(new Venta
                        {
                            IdVenta = (int)reader["IdVenta"],
                            FechaVenta = (DateTime)reader["FechaVenta"],
                            FechaUso = (DateTime)reader["FechaUso"],
                            TotalVenta = (decimal)reader["TotalVenta"]
                        });
                    }
                }
            }
            return ventas;
        }

        //Obtener venta por el idventa
        public static DataTable ObtenerVentaPorId(int idVenta)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conexion = ConexionDB.Conectar())
            {
                SqlCommand command = new SqlCommand("VentaPorId", conexion);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@IdVenta", SqlDbType.Int).Value = idVenta;

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        //Fecha de Uso / En venta
        public void SubirFechaUsoVenta(int idVenta, DateTime fechaUso)
        {
            using (SqlConnection conexion = ConexionDB.Conectar())
            {
                SqlCommand command = new SqlCommand("SubirFechaUso", conexion);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@IdVenta", SqlDbType.Int).Value = idVenta;
                command.Parameters.Add("@FechaUso", SqlDbType.DateTime).Value = fechaUso.Date;

                command.ExecuteNonQuery();
                conexion.Close();
            }
        }

        //Tipo de Pago / En venta
        public void SubirTipoPagoVenta(int idVenta, int idTipoPago)
        {
            using (SqlConnection conexion = ConexionDB.Conectar())
            {
                SqlCommand command = new SqlCommand("SubirTipoPago", conexion);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@IdVenta", SqlDbType.Int).Value = idVenta;
                command.Parameters.Add("@IdTipoPago", SqlDbType.Decimal).Value = idTipoPago;

                command.ExecuteNonQuery();
                conexion.Close();
            }
        }

        //Cancelar Venta
        public void CancelarVenta(int idVenta)
        {
            try
            {
                using (SqlConnection connection = ConexionDB.Conectar())
                {
                    SqlCommand command = new SqlCommand("CancelarVenta", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@IdVenta", SqlDbType.Int).Value = idVenta;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cancelar la venta.", ex);
            }
        }
    }
}
