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

            using (SqlConnection connection = ConexionDB.Conectar())
            {
                SqlCommand command = new SqlCommand("FinalizarVenta", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@IdVenta", SqlDbType.Int).Value = idVenta;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        factura.NumeroFactura = reader["NumeroFactura"]?.ToString() ?? "N/A";
                        factura.NumeroControlFactura = reader["NumeroControlFactura"] != DBNull.Value
                            ? Convert.ToInt32(reader["NumeroControlFactura"])
                            : (int?)null;
                        factura.SelloRecepcion = reader["SelloRecepcion"]?.ToString() ?? "";
                        factura.FechaFactura = reader["FechaFactura"] != DBNull.Value
                            ? Convert.ToDateTime(reader["FechaFactura"])
                            : DateTime.Now;
                        factura.HoraFactura = reader["HoraFactura"] != DBNull.Value
                            ? TimeSpan.Parse(reader["HoraFactura"].ToString())
                            : (TimeSpan?)null;
                        factura.TipoDocumento = reader["TipoDocumento"]?.ToString() ?? "";
                        factura.Sucursal = reader["Sucursal"]?.ToString() ?? "";

                        factura.IdVenta = reader["IdVenta"] != DBNull.Value
                            ? Convert.ToInt32(reader["IdVenta"])
                            : 0;
                        factura.FechaVenta = reader["FechaVenta"] != DBNull.Value
                            ? Convert.ToDateTime(reader["FechaVenta"])
                            : DateTime.Now;
                        factura.FechaUso = reader["FechaUso"] != DBNull.Value
                            ? Convert.ToDateTime(reader["FechaUso"])
                            : DateTime.Now;
                        factura.EstadoVenta = reader["EstadoVenta"]?.ToString() ?? "";

                        factura.NombreCliente = reader["NombreCliente"]?.ToString() ?? "";
                        factura.ApellidoCliente = reader["ApellidoCliente"]?.ToString() ?? "";
                        factura.DuiCliente = reader["DUICliente"]?.ToString() ?? "";
                        factura.NitCliente = reader["NITCliente"]?.ToString() ?? "";
                        factura.CorreoCliente = reader["CorreoCliente"]?.ToString() ?? "";
                        factura.NombreEmpleado = reader["NombreEmpleado"]?.ToString() ?? "";

                        factura.SubtotalVenta = reader["SubtotalVenta"] != DBNull.Value
                            ? Convert.ToDecimal(reader["SubtotalVenta"])
                            : 0;
                        factura.IVA = reader["IVA"] != DBNull.Value
                            ? Convert.ToDecimal(reader["IVA"])
                            : 0;
                        factura.DescuentoVenta = reader["DescuentoVenta"] != DBNull.Value
                            ? Convert.ToDecimal(reader["DescuentoVenta"])
                            : 0;
                        factura.TotalVenta = reader["TotalVenta"] != DBNull.Value
                            ? Convert.ToDecimal(reader["TotalVenta"])
                            : 0;

                        factura.TipoPago = reader["TipoPago"]?.ToString() ?? "";
                        factura.EstadoVenta = reader["EstadoVenta"]?.ToString() ?? "";

                        factura.IdFactura = reader["IdFactura"] != DBNull.Value
                            ? Convert.ToInt32(reader["IdFactura"])
                            : 0;
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            factura.Detalles.Add(new DetalledeVenta
                            {
                                NombreProducto = reader["NombreProducto"]?.ToString() ?? "",
                                Cantidad = reader["Cantidad"] != DBNull.Value
                                    ? Convert.ToInt32(reader["Cantidad"])
                                    : 0,
                                PrecioUnitario = reader["PrecioUnitario"] != DBNull.Value
                                    ? Convert.ToDecimal(reader["PrecioUnitario"])
                                    : 0,
                                Descuento = reader["Descuento"] != DBNull.Value
                                    ? Convert.ToDecimal(reader["Descuento"])
                                    : 0,
                                TotalDetalleVenta = reader["SubTotal"] != DBNull.Value
                                    ? Convert.ToDecimal(reader["SubTotal"])
                                    : 0
                            });
                        }
                    }

                }
            }
            return factura;
        }

        //Mostrar Vista de la Factura Completa
        public FacturaCompleta MostrarFacturaCompleta(int idVenta)
        {
            FacturaCompleta factura = new FacturaCompleta();
            factura.Detalles = new List<DetalledeVenta>();

            using (SqlConnection connection = ConexionDB.Conectar())
            {
                SqlCommand command = new SqlCommand("SELECT * FROM VistaFactura WHERE IdVenta = @IdVenta;", connection);

                command.Parameters.Add("@IdVenta", SqlDbType.Int).Value = idVenta;

                //connection.Open();
                using (SqlDataReader leer = command.ExecuteReader())
                {
                    if (leer.Read())
                    {
                        factura.IdFactura = (int)leer["IdFactura"];
                        factura.NumeroFactura = leer["NumeroFactura"].ToString();
                        factura.NumeroControlFactura = (int)(leer["NumeroControlFactura"] != DBNull.Value ? (int)leer["NumeroControlFactura"] : (int?)null);
                        factura.SelloRecepcion = leer["SelleRecepcion"].ToString();
                        factura.FechaFactura = (DateTime)leer["FechaFactura"];
                        factura.HoraFactura = factura.HoraFactura = (TimeSpan)(leer.IsDBNull(leer.GetOrdinal("HoraFactura")) ? (TimeSpan?)null : leer.GetTimeSpan(leer.GetOrdinal("HoraFactura")));
                        factura.TipoDocumento = leer["TipoDocumento"].ToString();
                        factura.Sucursal = leer["Sucursal"].ToString();

                        factura.IdVenta = (int)leer["IdVenta"];
                        factura.FechaVenta = (DateTime)leer["FechaVenta"];
                        factura.FechaUso = (DateTime)leer["FechaUso"];
                        factura.EstadoVenta = leer["EstadoVenta"].ToString();

                        factura.NombreCliente = leer["NombreCliente"].ToString();
                        factura.ApellidoCliente = leer["ApellidoCliente"].ToString();
                        factura.DuiCliente = leer["DUICliente"] != DBNull.Value ? leer["DUICliente"].ToString() : "";
                        factura.NitCliente = leer["NITCliente"] != DBNull.Value ? leer["DUICliente"].ToString() : "";
                        factura.CorreoCliente = leer["CorreoCliente"].ToString();

                        factura.NombreEmpleado = leer["NombreEmpleado"].ToString();

                        factura.SubtotalVenta = (decimal)leer["SubtotalVenta"];
                        factura.IVA = (decimal)leer["IVA"];
                        factura.DescuentoVenta = leer["DescuentoVenta"] != DBNull.Value ? (decimal)leer["DescuentoVenta"] : 0;
                        factura.TotalVenta = (decimal)leer["TotalVenta"];

                        factura.TipoPago = leer["TipoPago"].ToString();
                        factura.EstadoVenta = leer["EstadoVenta"].ToString();
                    }
                }

                SqlCommand cmdDetallesDeventa = new SqlCommand("ObtenerDetalleVenta", connection);
                cmdDetallesDeventa.CommandType = CommandType.StoredProcedure;

                cmdDetallesDeventa.Parameters.Add("@IdVenta", SqlDbType.Int).Value = IdVenta;

                using (SqlDataReader leer = command.ExecuteReader())
                {
                    while (leer.Read())
                    {
                        factura.Detalles.Add(new DetalledeVenta
                        {
                            IdDetalleVenta = (int)leer["IdDetalleVenta"],
                            NombreProducto = leer["NombreProducto"].ToString(),
                            Cantidad = (int)leer["Cantidad"],
                            PrecioUnitario = (decimal)leer["PrecioUnitario"],
                            Descuento = leer["Descuento"] != DBNull.Value ? (decimal)leer["Descuento"] : 0,
                            TotalDetalleVenta = (decimal)leer["TotalDetalleVenta"]
                        });
                    }
                }
            }
            return factura;
        }
    }
}
