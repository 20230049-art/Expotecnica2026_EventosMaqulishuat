using Modelos.Conexion_DB;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Fechas
    {
        private int idFecha;
        private DateTime diaFecha;
        private string lugarFecha;
        private TimeSpan horaFecha;
        private string asusntoFecha;
        private int idCliente;
        private int idUsuario;

        public int IdFecha { get => idFecha; set => idFecha = value; }
        public DateTime DiaFecha { get => diaFecha; set => diaFecha = value; }
        public string LugarFecha { get => lugarFecha; set => lugarFecha = value; }
        public TimeSpan HoraFecha { get => horaFecha; set => horaFecha = value; }
        public string AsusntoFecha { get => asusntoFecha; set => asusntoFecha = value; }
        public int IdCliente { get => idCliente; set => idCliente = value; }
        public int IdUsuario { get => idUsuario; set => idUsuario = value; }

        public static DataTable ContarPorMes(int anio, int mes)
        {
            using (SqlConnection cn = ConexionDB.Conectar())
            using (SqlCommand cmd = new SqlCommand("ContarEventosPorDiaMes", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Anio", SqlDbType.Int).Value = anio;
                cmd.Parameters.Add("@Mes", SqlDbType.Int).Value = mes;

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                return dt;
            }
        }

        // Eventos de un día específico
        public static DataTable MostrarPorDia(DateTime dia)
        {
            using (SqlConnection cn = ConexionDB.Conectar())
            using (SqlCommand cmd = new SqlCommand("MostrarFechasPorDia", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Dia", SqlDbType.Date).Value = dia.Date;

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                return dt;
            }
        }

        // Obtener por Id
        public static DataTable ObtenerPorId(int idFecha)
        {
            using (SqlConnection cn = ConexionDB.Conectar())
            using (SqlCommand cmd = new SqlCommand("ObtenerFechaPorId", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdFecha", SqlDbType.Int).Value = idFecha;

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                return dt;
            }
        }

        // Agregar
        public static bool Agregar(Fechas fecha)
        {
            try
            {
                using (SqlConnection cn = ConexionDB.Conectar())
                using (SqlCommand cmd = new SqlCommand("AgregarFecha", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@DiaFecha", SqlDbType.Date).Value = fecha.DiaFecha.Date;
                    cmd.Parameters.Add("@HoraFecha", SqlDbType.Time).Value = fecha.HoraFecha;
                    cmd.Parameters.Add("@LugarFecha", SqlDbType.VarChar).Value = fecha.LugarFecha;
                    cmd.Parameters.Add("@AsuntoFecha", SqlDbType.VarChar).Value = fecha.AsusntoFecha;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = fecha.IdCliente;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = fecha.IdUsuario;


                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message,
                        "No se puede guardar", System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }
        }

        // Actualizar
        public static bool Actualizar(Fechas fecha)
        {
            try
            {
                using (SqlConnection cn = ConexionDB.Conectar())
                using (SqlCommand cmd = new SqlCommand("ActualizarFecha", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdFecha", SqlDbType.Int).Value = fecha.IdFecha;
                    cmd.Parameters.Add("@DiaFecha", SqlDbType.Date).Value = fecha.DiaFecha.Date;
                    cmd.Parameters.Add("@HoraFecha", SqlDbType.Time).Value = fecha.HoraFecha;
                    cmd.Parameters.Add("@LugarFecha", SqlDbType.VarChar).Value = fecha.LugarFecha;
                    cmd.Parameters.Add("@AsuntoFecha", SqlDbType.VarChar).Value = fecha.AsusntoFecha;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = fecha.IdCliente;

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message,
                        "No se puede actualizar", System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }
        }

        // Eliminar
        public static bool Eliminar(int idFecha)
        {
            try
            {
                using (SqlConnection cn = ConexionDB.Conectar())
                using (SqlCommand cmd = new SqlCommand("EliminarFecha", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdFecha", SqlDbType.Int).Value = idFecha;

                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (SqlException)
            {
                return false;
            }
        }
    }
}
