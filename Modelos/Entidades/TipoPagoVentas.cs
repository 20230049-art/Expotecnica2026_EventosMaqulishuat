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
    public class TipoPagoVentas
    {
        private int IdTipoPago;
        private string TipoPago;

        public int IdTipoPago1 { get => IdTipoPago; set => IdTipoPago = value; }
        public string TipoPago1 { get => TipoPago; set => TipoPago = value; }

        //Mostrar TIpo de pago en Registro Pago y Ventas
        public static DataTable MostrarTipoPago()
        {
            SQLiteConnection conexion = ConexionDB.Conectar();
            string consulta = "SELECT IdTipoPago, TipoPago FROM TipoPago";
            SQLiteDataAdapter da = new SQLiteDataAdapter(consulta, conexion);
            DataTable tabla = new DataTable();
            da.Fill(tabla);
            conexion.Close();
            return tabla;
        }
    }
}
