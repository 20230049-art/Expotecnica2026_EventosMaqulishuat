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
            using (SqlConnection cn = ConexionDB.Conectar())
            using (SqlCommand cmd = new SqlCommand("ExisteConfiguracionInicial", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bool usuarios = Convert.ToInt32(reader["TieneUsuarios"]) == 1;
                        bool configuracion = Convert.ToInt32(reader["TieneConfiguracion"]) == 1;
                        return (usuarios, configuracion);
                    }
                }
            }

            return (false, false);
        }

        //Guardar info empresa
        public static bool Guardar(DatosEmpresa config)
        {
            using (SqlConnection cn = ConexionDB.Conectar())
            using (SqlCommand cmd = new SqlCommand("GuardarConfiguracionEmpresa", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@NombreEmpresa", SqlDbType.VarChar).Value = config.NombreEmpresa;
                cmd.Parameters.Add("@CorreoEmpresa", SqlDbType.VarChar).Value = config.CorreoEmpresa;
                cmd.Parameters.Add("@TelefonoEmpresa", SqlDbType.VarChar).Value = config.TelefonoEmpresa;
                cmd.Parameters.Add("@DireccionEmpresa", SqlDbType.VarChar).Value = config.DireccionEmpresa;

                cmd.Parameters.Add("@LogoEmpresa", SqlDbType.VarChar).Value =
                    string.IsNullOrWhiteSpace(config.LogoEmpresa) ? (object)DBNull.Value : config.LogoEmpresa;

                cmd.ExecuteNonQuery();
                return true;
            }
        }

        //Guardar configuracion datso empresa
        public static DatosEmpresa Obtener()
        {
            using (SqlConnection cn = ConexionDB.Conectar())
            using (SqlCommand cmd = new SqlCommand("MostrarConfiguracionEmpresa", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = cmd.ExecuteReader())
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
