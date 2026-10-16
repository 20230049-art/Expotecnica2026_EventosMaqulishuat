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
    public class DatosEmpresa
    {
        private int idConfiguracion;
        private string nombreEmpresa;
        private string correoEmpresa;
        private string telefonoEmpresa;
        private string direccionEmpresa;
        private string logoEmpresa;

        public int IdConfiguracion { get => idConfiguracion; set => idConfiguracion = value; }
        public string NombreEmpresa { get => nombreEmpresa; set => nombreEmpresa = value; }
        public string CorreoEmpresa { get => correoEmpresa; set => correoEmpresa = value; }
        public string TelefonoEmpresa { get => telefonoEmpresa; set => telefonoEmpresa = value; }
        public string DireccionEmpresa { get => direccionEmpresa; set => direccionEmpresa = value; }
        public string LogoEmpresa { get => logoEmpresa; set => logoEmpresa = value; }

        //Hay usuarios o una configuracion previa
        public static (bool existenteUsuarios, bool existenDatosEmpresa) ExisteConfiguracion()
        {
            using (SQLiteConnection cn = ConexionDB.Conectar())
            {
                string sql = @"SELECT 
                (SELECT COUNT(*) FROM Usuario WHERE EliminadoUsuario = 0)       AS TieneUsuarios,
                (SELECT COUNT(*) FROM DatosEmpresa)                             AS TieneConfiguracion;";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bool usuarios = Convert.ToInt32(reader["TieneUsuarios"]) > 0;
                            bool configuracion = Convert.ToInt32(reader["TieneConfiguracion"]) > 0;
                            return (usuarios, configuracion);
                        }
                    }
                }
            }

            return (false, false);
        }

        //Guardar info empresa
        public static bool Guardar(DatosEmpresa config)
        {
            using (SQLiteConnection cn = ConexionDB.Conectar())
            {
                string sqlExiste = "SELECT COUNT(*) FROM DatosEmpresa;";

                using (SQLiteCommand cmdExiste = new SQLiteCommand(sqlExiste, cn))
                {
                    int existe = Convert.ToInt32(cmdExiste.ExecuteScalar());

                    string sql;
                    if (existe > 0)
                    {
                        sql = @"UPDATE DatosEmpresa
                                SET NombreEmpresa      = @NombreEmpresa,
                                    CorreoEmpresa      = @CorreoEmpresa,
                                    TelefonoEmpresa    = @TelefonoEmpresa,
                                    DireccionEmpresa   = @DireccionEmpresa,
                                    LogoEmpresa        = @LogoEmpresa,
                                    FechaConfiguracion = datetime('now','localtime')
                                WHERE IdDatosEmpresa = (SELECT IdDatosEmpresa FROM DatosEmpresa ORDER BY IdDatosEmpresa LIMIT 1);";
                    }
                    else
                    {
                        sql = @"INSERT INTO DatosEmpresa 
                                    (NombreEmpresa, CorreoEmpresa, TelefonoEmpresa, DireccionEmpresa, LogoEmpresa)
                                VALUES 
                                    (@NombreEmpresa, @CorreoEmpresa, @TelefonoEmpresa, @DireccionEmpresa, @LogoEmpresa);";
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                    {
                        cmd.Parameters.Add("@NombreEmpresa", DbType.String).Value = config.NombreEmpresa;
                        cmd.Parameters.Add("@CorreoEmpresa", DbType.String).Value = config.CorreoEmpresa;
                        cmd.Parameters.Add("@TelefonoEmpresa", DbType.String).Value = config.TelefonoEmpresa;
                        cmd.Parameters.Add("@DireccionEmpresa", DbType.String).Value = config.DireccionEmpresa;
                        cmd.Parameters.Add("@LogoEmpresa", DbType.String).Value =
                            string.IsNullOrWhiteSpace(config.LogoEmpresa) ? (object)DBNull.Value : config.LogoEmpresa;

                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
        }

        //Guardar configuracion datso empresa
        public static DatosEmpresa Obtener()
        {
            using (SQLiteConnection cn = ConexionDB.Conectar())
            {
                string sql = @"SELECT IdDatosEmpresa, NombreEmpresa, CorreoEmpresa, TelefonoEmpresa,
                                      DireccionEmpresa, LogoEmpresa, FechaConfiguracion
                               FROM DatosEmpresa
                               ORDER BY IdDatosEmpresa DESC
                               LIMIT 1;";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new DatosEmpresa
                        {
                            IdConfiguracion = Convert.ToInt32(reader["IdDatosEmpresa"]),
                            NombreEmpresa = reader["NombreEmpresa"].ToString(),
                            CorreoEmpresa = reader["CorreoEmpresa"].ToString(),
                            TelefonoEmpresa = reader["TelefonoEmpresa"].ToString(),
                            DireccionEmpresa = reader["DireccionEmpresa"].ToString(),
                            LogoEmpresa = reader["LogoEmpresa"] == DBNull.Value
                                                ? null
                                                : reader["LogoEmpresa"].ToString()
                        };
                    }
                }
            }

            return null;
        }

        //Imagen logo
        public static System.Drawing.Image ObtenerLogo()
        {
            try
            {
                var config = Obtener();
                if (config == null || string.IsNullOrWhiteSpace(config.LogoEmpresa)) return null;

                string rutaCompleta = System.IO.Path.IsPathRooted(config.LogoEmpresa)
                    ? config.LogoEmpresa
                    : System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, config.LogoEmpresa);

                if (!System.IO.File.Exists(rutaCompleta)) return null;

                using (var stream = new System.IO.FileStream(rutaCompleta,
                       System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    return System.Drawing.Image.FromStream(stream);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
