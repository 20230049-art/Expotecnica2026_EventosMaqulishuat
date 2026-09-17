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
    public class DetalledeVenta
    {
        private int idDetalleVenta;
        private int idVenta;
        private int idProducto;
        private string nombreProducto;
        private int cantidad;
        private decimal precioUnitario;
        private decimal descuento;
        private decimal totalDetalleVenta;

        public int IdDetalleVenta { get => idDetalleVenta; set => idDetalleVenta = value; }
        public int IdVenta { get => idVenta; set => idVenta = value; }
        public int IdProducto { get => idProducto; set => idProducto = value; }
        public string NombreProducto { get => nombreProducto; set => nombreProducto = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }
        public decimal PrecioUnitario { get => precioUnitario; set => precioUnitario = value; }
        public decimal Descuento { get => descuento; set => descuento = value; }
        public decimal TotalDetalleVenta { get => totalDetalleVenta; set => totalDetalleVenta = value; }

        //Mostrar DetalleVenta
        public List<DetalledeVenta> ObtenerDetalleVenta(int idVenta)
        {
            List<DetalledeVenta> detalles = new List<DetalledeVenta>();
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                SqlCommand command = new SqlCommand("ObtenerDetalleVenta", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@IdVenta", SqlDbType.Int).Value = idVenta;
                //command.Connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DetalledeVenta detalle = new DetalledeVenta
                        {
                            IdDetalleVenta = reader.GetInt32(reader.GetOrdinal("IdDetalleVenta")),
                            NombreProducto = reader.GetString(reader.GetOrdinal("NombreProducto")),
                            Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                            PrecioUnitario = reader.GetDecimal(reader.GetOrdinal("PrecioUnitario")),
                            Descuento = reader["Descuento"] != DBNull.Value ? reader.GetDecimal(reader.GetOrdinal("Descuento")) : 0,
                            TotalDetalleVenta = reader.GetDecimal(reader.GetOrdinal("TotalDetalleventa"))
                        };
                        detalles.Add(detalle);
                    }
                }
                return detalles;
            }
        }

        //Agregar Producto a la Venta
        public void AgregarProductosVenta(int idVenta, int idProducto, int cantidad)
        {
            using (SqlConnection conexion = ConexionDB.Conectar())
            {
                SqlCommand command = new SqlCommand("AgregarProductosVenta", conexion);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@IdVenta", SqlDbType.Int).Value = idVenta;
                command.Parameters.Add("@IdProducto", SqlDbType.Decimal).Value = idProducto;
                command.Parameters.Add("@IdCantidad", SqlDbType.Decimal).Value = cantidad;

                command.ExecuteNonQuery();
                conexion.Close();
            }
        }
    }
}
