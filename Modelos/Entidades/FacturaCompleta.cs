using Modelos.Conexion_DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modelos.Entidades
{
    public class FacturaCompleta
    {
        private int idFactura;
        private string numeroFactura;
        private int? numeroControlFactura;
        private string selloRecepcion;
        private DateTime fechaFactura;
        private TimeSpan? horaFactura;
        private string tipoDocumento;
        private string sucursal;
        private int idVenta;
        private DateTime fechaVenta;
        private DateTime fechaUso;
        private decimal subtotalVenta;
        private decimal iVA;
        private decimal descuentoVenta;
        private decimal totalVenta;
        private string nombreCliente;
        private string apellidoCliente;
        private string duiCliente;
        private string nitCliente;
        private string correoCliente;
        private string nombreEmpleado;
        private string tipoPago;
        private string estadoVenta;
        private List<DetalledeVenta> detalles;

        public int IdFactura { get => idFactura; set => idFactura = value; }
        public string NumeroFactura { get => numeroFactura; set => numeroFactura = value; }
        public int? NumeroControlFactura { get => numeroControlFactura; set => numeroControlFactura = value; }
        public string SelloRecepcion { get => selloRecepcion; set => selloRecepcion = value; }
        public DateTime FechaFactura { get => fechaFactura; set => fechaFactura = value; }
        public TimeSpan? HoraFactura { get => horaFactura; set => horaFactura = value; }
        public string TipoDocumento { get => tipoDocumento; set => tipoDocumento = value; }
        public string Sucursal { get => sucursal; set => sucursal = value; }
        public int IdVenta { get => idVenta; set => idVenta = value; }
        public DateTime FechaVenta { get => fechaVenta; set => fechaVenta = value; }
        public DateTime FechaUso { get => fechaUso; set => fechaUso = value; }
        public decimal SubtotalVenta { get => subtotalVenta; set => subtotalVenta = value; }
        public decimal IVA { get => iVA; set => iVA = value; }
        public decimal DescuentoVenta { get => descuentoVenta; set => descuentoVenta = value; }
        public decimal TotalVenta { get => totalVenta; set => totalVenta = value; }
        public string NombreCliente { get => nombreCliente; set => nombreCliente = value; }
        public string ApellidoCliente { get => apellidoCliente; set => apellidoCliente = value; }
        public string DuiCliente { get => duiCliente; set => duiCliente = value; }
        public string NitCliente { get => nitCliente; set => nitCliente = value; }
        public string CorreoCliente { get => correoCliente; set => correoCliente = value; }
        public string NombreEmpleado { get => nombreEmpleado; set => nombreEmpleado = value; }
        public string TipoPago { get => tipoPago; set => tipoPago = value; }
        public string EstadoVenta { get => estadoVenta; set => estadoVenta = value; }
        public List<DetalledeVenta> Detalles { get => detalles; set => detalles = value; }

        //Finalizar Venta
        public FacturaCompleta FinalizarVenta(int idVenta)
        {
            FacturaCompleta factura = new FacturaCompleta();
            factura.Detalles = new List<DetalledeVenta>();

            using (SQLiteConnection connection = ConexionDB.Conectar())
            using (SQLiteTransaction tx = connection.BeginTransaction())
            {
                try
                {
                    int idCliente;
                    int correlativo;
                    int numeroControl;

                    using (SQLiteCommand cmdCli = new SQLiteCommand(
                        "SELECT IdCliente FROM Venta WHERE IdVenta = @IdVenta;", connection, tx))
                    {
                        cmdCli.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;
                        idCliente = Convert.ToInt32(cmdCli.ExecuteScalar());
                    }

                    using (SQLiteCommand cmdCorr = new SQLiteCommand(
                        "SELECT IFNULL(MAX(IdFactura), 0) + 1 FROM Factura;", connection, tx))
                    {
                        correlativo = Convert.ToInt32(cmdCorr.ExecuteScalar());
                    }

                    using (SQLiteCommand cmdCtrl = new SQLiteCommand(
                        "SELECT IFNULL(MAX(NumeroControlFactura), 0) + 1 FROM Factura;", connection, tx))
                    {
                        numeroControl = Convert.ToInt32(cmdCtrl.ExecuteScalar());
                    }

                    string numeroFactura = "FAC" + DateTime.Now.Year + correlativo.ToString("D6");
                    Random rnd = new Random();
                    string selloRecepcion = "S" + rnd.Next(100000000, 999999999).ToString();
                    string fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
                    string horaAhora = DateTime.Now.ToString("HH:mm:ss");

                    using (SQLiteCommand cmdIns = new SQLiteCommand(@"
                        INSERT INTO Factura
                            (NumeroFactura, NumeroControlFactura, SelloRecepcion,
                             Fecha, Hora, TipoDocumento, Sucursal, IdVenta)
                        VALUES
                            (@NumeroFactura, @NumeroControl, @Sello,
                             @Fecha, @Hora, 'Factura', 'Nombre Sucursal', @IdVenta);
                        SELECT last_insert_rowid();", connection, tx))
                    {
                        cmdIns.Parameters.Add("@NumeroFactura", DbType.String).Value = numeroFactura;
                        cmdIns.Parameters.Add("@NumeroControl", DbType.Int32).Value = numeroControl;
                        cmdIns.Parameters.Add("@Sello", DbType.String).Value = selloRecepcion;
                        cmdIns.Parameters.Add("@Fecha", DbType.String).Value = fechaHoy;
                        cmdIns.Parameters.Add("@Hora", DbType.String).Value = horaAhora;
                        cmdIns.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;

                        factura.IdFactura = Convert.ToInt32(cmdIns.ExecuteScalar());
                    }

                    using (SQLiteCommand cmdUpd = new SQLiteCommand(
                        "UPDATE Venta SET IdEstadoVenta = 2 WHERE IdVenta = @IdVenta;", connection, tx))
                    {
                        cmdUpd.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;
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
            Productos.ActualizarTotalesVenta(idVenta);
            RegistrarPagoAutomatico(idVenta);
            return MostrarFacturaCompleta(idVenta);
        }

        //Mostrar Vista de la Factura Completa
        public FacturaCompleta MostrarFacturaCompleta(int idVenta)
        {
            FacturaCompleta factura = new FacturaCompleta();
            factura.Detalles = new List<DetalledeVenta>();

            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sqlFactura = "SELECT * FROM VistaFactura WHERE IdVenta = @IdVenta;";

                using (SQLiteCommand command = new SQLiteCommand(sqlFactura, connection))
                {
                    command.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;

                    using (SQLiteDataReader leer = command.ExecuteReader())
                    {
                        if (leer.Read())
                        {
                            factura.IdFactura = Convert.ToInt32(leer["IdFactura"]);
                            factura.NumeroFactura = leer["NumeroFactura"].ToString();
                            factura.NumeroControlFactura = leer["NumeroControlFactura"] != DBNull.Value
                                ? (int?)Convert.ToInt32(leer["NumeroControlFactura"])
                                : null;
                            factura.SelloRecepcion = leer["SelloRecepcion"] != DBNull.Value
                                ? leer["SelloRecepcion"].ToString() : "";
                            factura.FechaFactura = Convert.ToDateTime(leer["FechaFactura"]);

                            factura.HoraFactura = leer["HoraFactura"] != DBNull.Value
                                ? TimeSpan.Parse(leer["HoraFactura"].ToString())
                                : (TimeSpan?)null;

                            factura.TipoDocumento = leer["TipoDocumento"].ToString();
                            factura.Sucursal = leer["Sucursal"] != DBNull.Value
                                ? leer["Sucursal"].ToString() : "";

                            factura.IdVenta = Convert.ToInt32(leer["IdVenta"]);
                            factura.FechaVenta = Convert.ToDateTime(leer["FechaVenta"]);
                            factura.FechaUso = Convert.ToDateTime(leer["FechaUso"]);
                            factura.EstadoVenta = leer["EstadoVenta"].ToString();

                            factura.NombreCliente = leer["NombreCliente"].ToString();
                            factura.ApellidoCliente = leer["ApellidoCliente"].ToString();
                            factura.DuiCliente = leer["DUICliente"] != DBNull.Value ? leer["DUICliente"].ToString() : "";
                            factura.NitCliente = leer["NITCliente"] != DBNull.Value ? leer["NITCliente"].ToString() : "";
                            factura.CorreoCliente = leer["CorreoCliente"].ToString();

                            factura.NombreEmpleado = leer["NombreEmpleado"].ToString();

                            factura.SubtotalVenta = Convert.ToDecimal(leer["SubtotalVenta"]);
                            factura.IVA = Convert.ToDecimal(leer["IVA"]);
                            factura.DescuentoVenta = leer["DescuentoVenta"] != DBNull.Value
                                ? Convert.ToDecimal(leer["DescuentoVenta"]) : 0;
                            factura.TotalVenta = Convert.ToDecimal(leer["TotalVenta"]);

                            factura.TipoPago = leer["TipoPago"].ToString();
                        }
                    }
                }

                string sqlDetalles = @"
            SELECT DV.IdDetalleVenta, P.NombreProducto, DV.Cantidad,
                   DV.PrecioUnitario, DV.Descuento, DV.TotalDetalleventa
            FROM DetalleVenta DV
            INNER JOIN Producto P ON DV.IdProducto = P.IdProducto
            WHERE DV.IdVenta = @IdVenta;";

                using (SQLiteCommand cmdDetalles = new SQLiteCommand(sqlDetalles, connection))
                {
                    cmdDetalles.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;

                    using (SQLiteDataReader leer = cmdDetalles.ExecuteReader())
                    {
                        while (leer.Read())
                        {
                            factura.Detalles.Add(new DetalledeVenta
                            {

                                IdDetalleVenta = Convert.ToInt32(leer["IdDetalleVenta"]),
                                NombreProducto = leer["NombreProducto"].ToString(),
                                Cantidad = Convert.ToInt32(leer["Cantidad"]),
                                PrecioUnitario = Convert.ToDecimal(leer["PrecioUnitario"]),
                                Descuento = leer["Descuento"] != DBNull.Value
                                    ? Convert.ToDecimal(leer["Descuento"]) : 0,
                                TotalDetalleVenta = Convert.ToDecimal(leer["TotalDetalleventa"])
                            });
                        }
                    }
                }
            }
            return factura;
        }

        private void RegistrarPagoAutomatico(int idVenta)
        {
            try
            {
                using (SQLiteConnection connection = ConexionDB.Conectar())
                {
                    int idServicio = 1; 

                    using (SQLiteCommand cmdServ = new SQLiteCommand(@"
                SELECT P.IdServicio 
                FROM DetalleVenta DV
                INNER JOIN Producto P ON DV.IdProducto = P.IdProducto
                WHERE DV.IdVenta = @IdVenta
                LIMIT 1;", connection))
                    {
                        cmdServ.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;
                        object result = cmdServ.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            idServicio = Convert.ToInt32(result);
                    }

                    string sqlPago = @"
                INSERT INTO RegistoPago
                    (EstadoPago, FechaPago, IdServicio, IdVenta, IdCliente)
                SELECT 
                    'Pagado',
                    date('now','localtime'),
                    @IdServicio,
                    @IdVenta,
                    IdCliente
                FROM Venta
                WHERE IdVenta = @IdVenta;";

                    using (SQLiteCommand cmdPago = new SQLiteCommand(sqlPago, connection))
                    {
                        cmdPago.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;
                        cmdPago.Parameters.Add("@IdServicio", DbType.Int32).Value = idServicio;
                        cmdPago.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("La venta se guardó, pero no se pudo registrar el pago automáticamente:\n\n" + ex.Message,
                                "ADVERTENCIA-PAGO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}

