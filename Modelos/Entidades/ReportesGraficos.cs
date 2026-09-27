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
    public class ReportesGraficos
    {
        public static DataTable VentasMensuales()
        {
            return EjecutarSP("GraficoVentasMensuales");
        }

        public static DataTable ProductosMasVendidos()
        {
            return EjecutarSP("GraficoProductosMasVendidos");
        }

        public static DataTable EmpleadosMasVentas()
        {
            return EjecutarSP("GraficoEmpleadosMasVentas");
        }

        public static DataSet ReporteVentasTotales()
        {
            return EjecutarSPMulti("ReporteVentasTotales");
        }

        public static DataSet ReportePagos()
        {
            return EjecutarSPMulti("ReportePagos");
        }

        public static DataSet ReporteTopProductos()
        {
            return EjecutarSPMulti("ReporteTopProductos");
        }

        private static DataTable EjecutarSP(string nombreSP)
        {
            using (SqlConnection cn = ConexionDB.Conectar())
            using (SqlCommand cmd = new SqlCommand(nombreSP, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                return dt;
            }
        }

        private static DataSet EjecutarSPMulti(string nombreSP)
        {
            using (SqlConnection cn = ConexionDB.Conectar())
            using (SqlCommand cmd = new SqlCommand(nombreSP, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                return ds;
            }
        }
    }
}
