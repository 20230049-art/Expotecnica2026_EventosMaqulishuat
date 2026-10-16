using Modelos.Conexion_DB;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
        private string asuntoFecha;
        private int idCliente;
        private int idUsuario;

        public int IdFecha { get => idFecha; set => idFecha = value; }
        public DateTime DiaFecha { get => diaFecha; set => diaFecha = value; }
        public string LugarFecha { get => lugarFecha; set => lugarFecha = value; }
        public TimeSpan HoraFecha { get => horaFecha; set => horaFecha = value; }
        public string AsuntoFecha { get => asuntoFecha; set => asuntoFecha = value; }
        public int IdCliente { get => idCliente; set => idCliente = value; }
        public int IdUsuario { get => idUsuario; set => idUsuario = value; }

        public static DataTable ContarPorMes(int anio, int mes)
        {
            using (SQLiteConnection cn = ConexionDB.Conectar())
            {
                // strftime('%Y', DiaFecha) = año como texto
                // strftime('%m', DiaFecha) = mes como texto de 2 dígitos
                string sql = @"
                    SELECT 
                        DiaFecha AS Dia,
                        COUNT(*) AS TotalEventos
                    FROM Fecha
                    WHERE strftime('%Y', DiaFecha) = @Anio
                      AND strftime('%m', DiaFecha) = @Mes
                    GROUP BY DiaFecha;";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.Add("@Anio", DbType.String).Value = anio.ToString("D4");
                    cmd.Parameters.Add("@Mes", DbType.String).Value = mes.ToString("D2");

                    SQLiteDataAdapter ad = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    return dt;
                }
            }
        }

        // Eventos de un día específico
        public static DataTable MostrarPorDia(DateTime dia)
        {
            using (SQLiteConnection cn = ConexionDB.Conectar())
            {
                string sql = @"
            SELECT 
                F.IdFecha,
                F.DiaFecha,
                F.HoraFecha,
                F.LugarFecha,
                F.AsuntoFecha,
                F.IdCliente,
                C.NombreCliente || ' ' || C.ApellidoCliente AS NombreCliente,
                C.CorreoCliente,
                C.TelefonoCliente
            FROM Fecha F
            LEFT JOIN Cliente C ON F.IdCliente = C.IdCliente
            WHERE date(F.DiaFecha) = @Dia
            ORDER BY F.HoraFecha;";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.Add("@Dia", DbType.String).Value = dia.ToString("yyyy-MM-dd");

                    SQLiteDataAdapter ad = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    return dt;
                }
            }
        }

        // Obtener por Id
        public static DataTable ObtenerPorId(int idFecha)
        {
            using (SQLiteConnection cn = ConexionDB.Conectar())
            {
                string sql = @"
                    SELECT IdFecha, DiaFecha, HoraFecha, LugarFecha,
                           AsuntoFecha, IdCliente, IdUsuario
                    FROM Fecha
                    WHERE IdFecha = @IdFecha;";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.Add("@IdFecha", DbType.Int32).Value = idFecha;

                    SQLiteDataAdapter ad = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    return dt;
                }
            }
        }

        public static bool Agregar(Fechas fecha)
        {
            try
            {
                using (SQLiteConnection cn = ConexionDB.Conectar())
                {
                    if (fecha.DiaFecha.Date < DateTime.Today)
                    {
                        System.Windows.Forms.MessageBox.Show("La fecha no puede ser anterior a hoy.",
                                "No se puede guardar", System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Warning);
                        return false;
                    }

                    using (SQLiteCommand cmdCheck = new SQLiteCommand(
                        "SELECT COUNT(*) FROM Fecha WHERE date(DiaFecha) = @Dia;", cn))
                    {
                        cmdCheck.Parameters.Add("@Dia", DbType.String).Value = fecha.DiaFecha.ToString("yyyy-MM-dd");
                        if (Convert.ToInt32(cmdCheck.ExecuteScalar()) >= 20)
                        {
                            System.Windows.Forms.MessageBox.Show("Máximo 20 recordatorios por día.",
                                    "No se puede guardar", System.Windows.Forms.MessageBoxButtons.OK,
                                    System.Windows.Forms.MessageBoxIcon.Warning);
                            return false;
                        }
                    }

                    string sql = @"
                INSERT INTO Fecha
                    (DiaFecha, HoraFecha, LugarFecha, AsuntoFecha, IdCliente, IdUsuario)
                VALUES
                    (@DiaFecha, @HoraFecha, @LugarFecha, @AsuntoFecha, @IdCliente, @IdUsuario);";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                    {
                        cmd.Parameters.Add("@DiaFecha", DbType.String).Value = fecha.DiaFecha.ToString("yyyy-MM-dd");
                        cmd.Parameters.Add("@HoraFecha", DbType.String).Value = fecha.HoraFecha.ToString(@"hh\:mm\:ss");
                        cmd.Parameters.Add("@LugarFecha", DbType.String).Value = fecha.LugarFecha;
                        cmd.Parameters.Add("@AsuntoFecha", DbType.String).Value = fecha.AsuntoFecha;
                        cmd.Parameters.Add("@IdCliente", DbType.Int32).Value = fecha.IdCliente;
                        cmd.Parameters.Add("@IdUsuario", DbType.Int32).Value = fecha.IdUsuario;

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (SQLiteException ex)
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
                using (SQLiteConnection cn = ConexionDB.Conectar())
                {
                    if (fecha.DiaFecha.Date < DateTime.Today)
                    {
                        System.Windows.Forms.MessageBox.Show("La fecha no puede ser anterior a hoy.",
                                "No se puede actualizar", System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Warning);
                        return false;
                    }

                    string sql = @"
                        UPDATE Fecha
                        SET DiaFecha    = @DiaFecha,
                            HoraFecha   = @HoraFecha,
                            LugarFecha  = @LugarFecha,
                            AsuntoFecha = @AsuntoFecha,
                            IdCliente   = @IdCliente
                        WHERE IdFecha = @IdFecha;";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                    {
                        cmd.Parameters.Add("@IdFecha", DbType.Int32).Value = fecha.IdFecha;
                        cmd.Parameters.Add("@DiaFecha", DbType.Date).Value = fecha.DiaFecha.ToString("yyyy-MM-dd");
                        cmd.Parameters.Add("@HoraFecha", DbType.Time).Value = fecha.HoraFecha.ToString(@"hh\:mm\:ss");
                        cmd.Parameters.Add("@LugarFecha", DbType.String).Value = fecha.LugarFecha;
                        cmd.Parameters.Add("@AsuntoFecha", DbType.String).Value = fecha.AsuntoFecha;
                        cmd.Parameters.Add("@IdCliente", DbType.Int32).Value = fecha.IdCliente;

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (SQLiteException ex)
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
                using (SQLiteConnection cn = ConexionDB.Conectar())
                {
                    string sql = "DELETE FROM Fecha WHERE IdFecha = @IdFecha;";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                    {
                        cmd.Parameters.Add("@IdFecha", DbType.Int32).Value = idFecha;
                        cmd.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (SQLiteException)
            {
                return false;
            }
        }
    }
}
