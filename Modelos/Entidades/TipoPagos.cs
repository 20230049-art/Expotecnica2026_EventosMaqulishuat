using Modelos.Conexion_DB;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class TipoPagos
    {
        private int idTipoPago;
        private string tipoPago;

        public int IdTipoPago { get => idTipoPago; set => idTipoPago = value; }
        public string TipoPago { get => tipoPago; set => tipoPago = value; }

        //Obtner tipo de pago
        public List<TipoPagos> ObtenerTipoPago()
        {
            var tipoPago = new List<TipoPagos>();
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                var commandd = new SqlCommand("SELECT * FROM TipoPago ORDER BY IdTipoPago", connection);
                //connection.Open();
                using (var reader = commandd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tipoPago.Add(new TipoPagos
                        {
                            IdTipoPago = (int)reader["IdTipoPago"],
                            TipoPago = reader["TipoPago"].ToString()
                        });
                    }
                }

                return tipoPago;
            }

        }
    }
}
