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
            using (SQLiteConnection conexion = ConexionDB.Conectar())
            {
                string sql = @"
                    INSERT INTO Venta
                        (IdEmpleado, IdCliente, FechaVenta, FechaUso,
                         SubtotalVenta, IVA, DescuentoVenta, TotalVenta,
                         IdEstadoVenta, IdTipoPago)
                    VALUES
                        (@IdEmpleado, @IdCliente, date('now','localtime'), @FechaUso,
                         0, 0, 0, 0,
                         1, @IdTipoPago);
                    SELECT last_insert_rowid();";

                using (SQLiteCommand command = new SQLiteCommand(sql, conexion))
                {
                    command.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                    command.Parameters.AddWithValue("@IdCliente", idCliente);
                    command.Parameters.AddWithValue("@FechaUso", fechaUso.ToString("yyyy-MM-dd"));
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
            using (SQLiteConnection conexion = ConexionDB.Conectar())
            {
                string sql = @"
                    SELECT V.SubtotalVenta, V.IVA, V.DescuentoVenta, V.TotalVenta,
                           TP.TipoPago, EV.EstadoVenta
                    FROM Venta V
                    INNER JOIN TipoPago   TP ON V.IdTipoPago   = TP.IdTipoPago
                    INNER JOIN EstadoVenta EV ON V.IdEstadoVenta = EV.IdEstadoVenta
                    WHERE V.IdVenta = @IdVenta;";

                using (SQLiteCommand command = new SQLiteCommand(sql, conexion))
                {
                    command.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (
                                Subtotal: (decimal)reader["SubtotalVenta"],
                                Iva: (decimal)reader["IVA"],
                                Descuento: reader["DescuentoVenta"] != DBNull.Value ? (decimal)reader["DescuentoVenta"] : 0,
                                Total: (decimal)reader["TotalVenta"],
                                TipoPago: reader["TipoPago"].ToString(),
                                EstadoVenta: reader["EstadoVenta"].ToString()
                            );
                        }
                    }
                }
            }

            return (0, 0, 0, 0, "", "");
        }

        //Descuento en la Venta
        public void AplicarDescuentoVenta(int idVenta, decimal descuento)
        {
            using (SQLiteConnection conexion = ConexionDB.Conectar())
            {
                decimal subtotal;
                using (SQLiteCommand cmdSub = new SQLiteCommand(
                    "SELECT SubtotalVenta FROM Venta WHERE IdVenta = @IdVenta;", conexion))
                {
                    cmdSub.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;
                    object result = cmdSub.ExecuteScalar();
                    subtotal = result == null || result == DBNull.Value ? 0 : Convert.ToDecimal(result);
                }

                if (descuento > subtotal)
                {
                    throw new Exception("El descuento no puede ser mayor al subtotal.");
                }

                string sql = "UPDATE Venta SET DescuentoVenta = @Descuento WHERE IdVenta = @IdVenta;";
                using (SQLiteCommand command = new SQLiteCommand(sql, conexion))
                {
                    command.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;
                    command.Parameters.Add("@Descuento", DbType.Decimal).Value = descuento;
                    command.ExecuteNonQuery();
                }
            }
        }

        //Obtener ventas por la fecha de uso
        public List<Venta> VentasPorFechaUso(DateTime fechaVenta, DateTime fechaUso)
        {
            List<Venta> ventas = new List<Venta>();
            using (SQLiteConnection conexion = ConexionDB.Conectar())
            {
                string sql = @"
                    SELECT V.IdVenta, V.FechaVenta, V.FechaUso, V.TotalVenta
                    FROM Venta V
                    WHERE V.FechaUso BETWEEN @FechaInicio AND @FechaFin
                      AND V.IdEstadoVenta = 2
                    ORDER BY V.FechaUso ASC;";

                using (SQLiteCommand command = new SQLiteCommand(sql, conexion))
                {
                    command.Parameters.Add("@FechaInicio", DbType.String).Value = fechaVenta.ToString("yyyy-MM-dd");
                    command.Parameters.Add("@FechaFin", DbType.String).Value = fechaUso.ToString("yyyy-MM-dd");

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ventas.Add(new Venta
                            {
                                IdVenta = (int)reader["IdVenta"],
                                FechaVenta = Convert.ToDateTime(reader["FechaVenta"]),
                                FechaUso = Convert.ToDateTime(reader["FechaUso"]),
                                TotalVenta = (decimal)reader["TotalVenta"]
                            });
                        }
                    }
                }
            }
            return ventas;
        }

        //Obtener venta por el idventa
        public static DataTable ObtenerVentaPorId(int idVenta)
        {
            DataTable dt = new DataTable();

            using (SQLiteConnection conexion = ConexionDB.Conectar())
            {
                string sql = @"
                    SELECT V.IdVenta, V.FechaVenta, V.FechaUso,
                           V.SubtotalVenta, V.IVA, V.DescuentoVenta, V.TotalVenta,
                           V.IdEstadoVenta, V.IdTipoPago,
                           C.IdCliente, C.NombreCliente, C.ApellidoCliente,
                           C.DUICliente, C.NITCliente, C.TelefonoCliente,
                           C.CorreoCliente, C.EstadoCliente,
                           TC.TipoCliente, TP.TipoPago, EV.EstadoVenta,
                           E.NombreEmpleado, E.ApellidoEmpleado
                    FROM Venta V
                    INNER JOIN Cliente     C  ON V.IdCliente      = C.IdCliente
                    INNER JOIN TipoCliente TC ON C.IdTipoCliente  = TC.IdTipoCliente
                    INNER JOIN TipoPago    TP ON V.IdTipoPago     = TP.IdTipoPago
                    INNER JOIN EstadoVenta EV ON V.IdEstadoVenta  = EV.IdEstadoVenta
                    INNER JOIN Empleado    E  ON V.IdEmpleado     = E.IdEmpleado
                    WHERE V.IdVenta = @IdVenta;";

                using (SQLiteCommand command = new SQLiteCommand(sql, conexion))
                {
                    command.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        //Fecha de Uso / En venta
        public void SubirFechaUsoVenta(int idVenta, DateTime fechaUso)
        {
            using (SQLiteConnection conexion = ConexionDB.Conectar())
            {
                string sql = "UPDATE Venta SET FechaUso = @FechaUso WHERE IdVenta = @IdVenta;";

                using (SQLiteCommand command = new SQLiteCommand(sql, conexion))
                {
                    command.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;
                    command.Parameters.Add("@FechaUso", DbType.String).Value = fechaUso.ToString("yyyy-MM-dd");
                    command.ExecuteNonQuery();
                }
            }
        }

        //Tipo de Pago / En venta
        public void SubirTipoPagoVenta(int idVenta, int idTipoPago)
        {
            using (SQLiteConnection conexion = ConexionDB.Conectar())
            {
                string sql = "UPDATE Venta SET IdTipoPago = @IdTipoPago WHERE IdVenta = @IdVenta;";

                using (SQLiteCommand command = new SQLiteCommand(sql, conexion))
                {
                    command.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;
                    command.Parameters.Add("@IdTipoPago", DbType.Int32).Value = idTipoPago;
                    command.ExecuteNonQuery();
                }
            }
        }

        //Cancelar Venta
        public void CancelarVenta(int idVenta)
        {
            try
            {
                using (SQLiteConnection connection = ConexionDB.Conectar())
                {
                    using (SQLiteTransaction tx = connection.BeginTransaction())
                    {
                        try
                        {
                            string sqlDevolver = @"
                                UPDATE Producto
                                SET CantidadProducto = CantidadProducto + (
                                    SELECT DV.Cantidad FROM DetalleVenta DV
                                    WHERE DV.IdProducto = Producto.IdProducto
                                      AND DV.IdVenta = @IdVenta
                                )
                                WHERE IdProducto IN (
                                    SELECT IdProducto FROM DetalleVenta WHERE IdVenta = @IdVenta
                                );";
                            using (SQLiteCommand cmd = new SQLiteCommand(sqlDevolver, connection, tx))
                            {
                                cmd.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;
                                cmd.ExecuteNonQuery();
                            }

                            using (SQLiteCommand cmd = new SQLiteCommand(
                                "DELETE FROM DetalleVenta WHERE IdVenta = @IdVenta;", connection, tx))
                            {
                                cmd.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;
                                cmd.ExecuteNonQuery();
                            }

                            using (SQLiteCommand cmd = new SQLiteCommand(
                                "DELETE FROM Factura WHERE IdVenta = @IdVenta;", connection, tx))
                            {
                                cmd.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;
                                cmd.ExecuteNonQuery();
                            }

                            using (SQLiteCommand cmd = new SQLiteCommand(
                                "DELETE FROM Venta WHERE IdVenta = @IdVenta;", connection, tx))
                            {
                                cmd.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;
                                cmd.ExecuteNonQuery();
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
            catch (Exception ex)
            {
                throw new Exception("Error al cancelar la venta.", ex);
            }
        }

        //Paginas de 20
        public static DataTable MostrarVentasPagina(int pagina, int registrosPorPagina, out int totalRegistros)
        {
            DataTable dt = new DataTable();
            totalRegistros = 0;

            using (SQLiteConnection cn = ConexionDB.Conectar())
            {
                using (SQLiteCommand cmdTotal = new SQLiteCommand(
                    "SELECT COUNT(*) FROM VistaRegistroVentaGerente;", cn))
                {
                    totalRegistros = Convert.ToInt32(cmdTotal.ExecuteScalar());
                }

                int offset = (pagina - 1) * registrosPorPagina;

                string sql = @"
                    SELECT * FROM VistaRegistroVentaGerente
                    ORDER BY IdVenta DESC
                    LIMIT @limit OFFSET @offset;";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.Add("@limit", DbType.Int32).Value = registrosPorPagina;
                    cmd.Parameters.Add("@offset", DbType.Int32).Value = offset;

                    using (SQLiteDataAdapter ad = new SQLiteDataAdapter(cmd))
                    {
                        ad.Fill(dt);
                    }
                }
            }

            return dt;
        }

        //Buuscar Venta
        public DataTable BuscarVenta(string busqueda)
        {
            DataTable dt = new DataTable();
            busqueda = (busqueda ?? "").Trim();

            using (SQLiteConnection cn = ConexionDB.Conectar())
            {
                string sql;
                if (string.IsNullOrEmpty(busqueda))
                {
                    sql = "SELECT * FROM VistaRegistroVentaGerente ORDER BY IdVenta DESC;";
                }
                else
                {
                    sql = @"SELECT * FROM VistaRegistroVentaGerente
                            WHERE IdVenta         LIKE @q
                               OR NombreCliente   LIKE @q
                               OR ApellidoCliente LIKE @q
                               OR NombreEmpleado  LIKE @q
                               OR EstadoVenta     LIKE @q
                            ORDER BY IdVenta DESC;";
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
    }
}
