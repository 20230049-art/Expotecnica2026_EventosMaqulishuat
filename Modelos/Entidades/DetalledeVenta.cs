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
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql = @"
                    SELECT DV.IdDetalleVenta,
                           P.NombreProducto,
                           DV.Cantidad,
                           DV.PrecioUnitario,
                           DV.Descuento,
                           DV.TotalDetalleventa
                    FROM DetalleVenta DV
                    INNER JOIN Producto P ON DV.IdProducto = P.IdProducto
                    WHERE DV.IdVenta = @IdVenta;";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DetalledeVenta detalle = new DetalledeVenta
                            {
                                IdDetalleVenta = reader.GetInt32(reader.GetOrdinal("IdDetalleVenta")),
                                NombreProducto = reader.GetString(reader.GetOrdinal("NombreProducto")),
                                Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                                PrecioUnitario = Convert.ToDecimal(reader["PrecioUnitario"]),
                                Descuento = reader["Descuento"] != DBNull.Value ? Convert.ToDecimal(reader["Descuento"]) : 0,
                                TotalDetalleVenta = Convert.ToDecimal(reader["TotalDetalleventa"])
                            };
                            detalles.Add(detalle);
                        }
                    }
                }
                return detalles;
            }
        }

        //Agregar Producto a la Venta
        public void AgregarProductosVenta(int idVenta, int idProducto, int cantidad)
        {
            using (SQLiteConnection conexion = ConexionDB.Conectar())
            using (SQLiteTransaction tx = conexion.BeginTransaction())
            {
                try
                {
                    decimal precioAlquiler;
                    int inventarioActual;

                    using (SQLiteCommand cmdSel = new SQLiteCommand(
                        "SELECT PrecioALquilerProducto, CantidadProducto FROM Producto WHERE IdProducto = @Id;",
                        conexion, tx))
                    {
                        cmdSel.Parameters.Add("@Id", DbType.Int32).Value = idProducto;
                        using (var reader = cmdSel.ExecuteReader())
                        {
                            if (!reader.Read())
                                throw new Exception("Producto no encontrado.");

                            precioAlquiler = Convert.ToDecimal(reader["PrecioALquilerProducto"]);
                            inventarioActual = Convert.ToInt32(reader["CantidadProducto"]);

                        }
                    }

                    if (inventarioActual < cantidad)
                        throw new Exception("Producto insuficiente.");

                    decimal totalDetalle = precioAlquiler * cantidad;

                    using (SQLiteCommand cmdIns = new SQLiteCommand(@"
                        INSERT INTO DetalleVenta
                            (IdVenta, IdProducto, Cantidad, PrecioUnitario, Descuento, TotalDetalleventa)
                        VALUES
                            (@IdVenta, @IdProducto, @Cantidad, @PrecioUnitario, 0, @TotalDetalle);",
                        conexion, tx))
                    {
                        cmdIns.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;
                        cmdIns.Parameters.Add("@IdProducto", DbType.Int32).Value = idProducto;
                        cmdIns.Parameters.Add("@Cantidad", DbType.Int32).Value = cantidad;
                        cmdIns.Parameters.Add("@PrecioUnitario", DbType.Double).Value = precioAlquiler;
                        cmdIns.Parameters.Add("@TotalDetalle", DbType.Double).Value = totalDetalle;
                        cmdIns.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmdUpd = new SQLiteCommand(
                        "UPDATE Producto SET CantidadProducto = CantidadProducto - @Cantidad WHERE IdProducto = @Id;",
                        conexion, tx))
                    {
                        cmdUpd.Parameters.Add("@Cantidad", DbType.Int32).Value = cantidad;
                        cmdUpd.Parameters.Add("@Id", DbType.Int32).Value = idProducto;
                        cmdUpd.ExecuteNonQuery();
                    }

                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
    }
}
