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
    public class ReportesGraficos
    {
        public static DataTable VentasMensuales()
        {
            string sql = @"
                WITH RECURSIVE Meses(Mes) AS (
                    SELECT date('now','start of month','-11 months')
                    UNION ALL
                    SELECT date(Mes, '+1 month') FROM Meses
                    WHERE Mes < date('now','start of month')
                )
                SELECT 
                    strftime('%m/%Y', M.Mes) AS EtiquetaMes,
                    IFNULL(SUM(V.TotalVenta), 0) AS TotalVentas,
                    IFNULL(COUNT(V.IdVenta), 0) AS CantidadVentas
                FROM Meses M
                LEFT JOIN Venta V 
                    ON strftime('%Y-%m', V.FechaVenta) = strftime('%Y-%m', M.Mes)
                GROUP BY M.Mes
                ORDER BY M.Mes;";

            return EjecutarConsulta(sql);
        }

        public static DataTable ProductosMasVendidos()
        {
            string sql = @"
                SELECT 
                    P.NombreProducto,
                    SUM(DV.Cantidad) AS TotalVendido
                FROM DetalleVenta DV
                INNER JOIN Producto P ON DV.IdProducto = P.IdProducto
                GROUP BY P.NombreProducto
                ORDER BY TotalVendido DESC
                LIMIT 15;";

            return EjecutarConsulta(sql);
        }

        public static DataTable EmpleadosMasVentas()
        {
            string sql = @"
                SELECT 
                    E.NombreEmpleado || ' ' || E.ApellidoEmpleado AS NombreEmpleado,
                    SUM(V.TotalVenta) AS TotalVendido,
                    COUNT(V.IdVenta) AS CantidadVentas
                FROM Venta V
                INNER JOIN Empleado E ON V.IdEmpleado = E.IdEmpleado
                GROUP BY E.NombreEmpleado, E.ApellidoEmpleado
                ORDER BY TotalVendido DESC
                LIMIT 12;";

            return EjecutarConsulta(sql);
        }

        public static DataSet ReporteVentasTotales()
        {
            string sql = @"
                SELECT 
                    COUNT(*) AS TotalVentas,
                    IFNULL(SUM(TotalVenta), 0) AS TotalIngresos,
                    IFNULL(AVG(TotalVenta), 0) AS PromedioVenta,
                    IFNULL(SUM(IVA), 0) AS TotalIVA,
                    IFNULL(SUM(DescuentoVenta), 0) AS TotalDescuentos
                FROM Venta
                WHERE FechaVenta >= date('now','start of year')
                  AND FechaVenta <= date('now','localtime');

                SELECT 
                    V.IdVenta,
                    V.FechaVenta,
                    C.NombreCliente || ' ' || C.ApellidoCliente AS Cliente,
                    E.NombreEmpleado || ' ' || E.ApellidoEmpleado AS Empleado,
                    EV.EstadoVenta,
                    TP.TipoPago,
                    V.SubtotalVenta,
                    V.IVA,
                    V.DescuentoVenta,
                    V.TotalVenta
                FROM Venta V
                INNER JOIN Cliente      C  ON V.IdCliente     = C.IdCliente
                INNER JOIN Empleado     E  ON V.IdEmpleado    = E.IdEmpleado
                INNER JOIN EstadoVenta  EV ON V.IdEstadoVenta = EV.IdEstadoVenta
                INNER JOIN TipoPago     TP ON V.IdTipoPago    = TP.IdTipoPago
                WHERE V.FechaVenta >= date('now','start of year')
                  AND V.FechaVenta <= date('now','localtime')
                ORDER BY V.FechaVenta;";

            return EjecutarConsultaMulti(sql);
        }

        public static DataSet ReportePagos()
        {
            string sql = @"
                SELECT 
                    RP.EstadoPago,
                    COUNT(*) AS Cantidad,
                    IFNULL(SUM(V.TotalVenta), 0) AS MontoTotal
                FROM RegistoPago RP
                INNER JOIN Venta V ON RP.IdVenta = V.IdVenta
                GROUP BY RP.EstadoPago;

                SELECT 
                    RP.IdRegistroPago,
                    RP.FechaPago,
                    C.NombreCliente || ' ' || C.ApellidoCliente AS Cliente,
                    S.NombreServicio,
                    RP.EstadoPago,
                    V.TotalVenta
                FROM RegistoPago RP
                INNER JOIN Cliente  C ON RP.IdCliente  = C.IdCliente
                INNER JOIN Servicio S ON RP.IdServicio = S.IdServicio
                INNER JOIN Venta    V ON RP.IdVenta    = V.IdVenta
                ORDER BY RP.FechaPago DESC;";

            return EjecutarConsultaMulti(sql);
        }

        public static DataSet ReporteTopProductos()
        {
            string sql = @"
                SELECT 
                    P.NombreProducto,
                    S.NombreServicio,
                    PR.NombreProveedor,
                    SUM(DV.Cantidad) AS TotalVendido,
                    SUM(DV.TotalDetalleVenta) AS TotalGenerado,
                    AVG(DV.PrecioUnitario) AS PrecioPromedio
                FROM DetalleVenta DV
                INNER JOIN Producto  P  ON DV.IdProducto   = P.IdProducto
                INNER JOIN Servicio  S  ON P.IdServicio    = S.IdServicio
                INNER JOIN Proveedor PR ON P.IdProveedor   = PR.IdProveedor
                GROUP BY P.NombreProducto, S.NombreServicio, PR.NombreProveedor
                ORDER BY TotalVendido DESC
                LIMIT 15;";

            return EjecutarConsultaMulti(sql);
        }

        private static DataTable EjecutarConsulta(string sql)
        {
            using (SQLiteConnection cn = ConexionDB.Conectar())
            using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
            {
                SQLiteDataAdapter ad = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                return dt;
            }
        }

        private static DataSet EjecutarConsultaMulti(string sql)
        {
            using (SQLiteConnection cn = ConexionDB.Conectar())
            using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
            {
                SQLiteDataAdapter ad = new SQLiteDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                return ds;
            }
        }
    }
}
