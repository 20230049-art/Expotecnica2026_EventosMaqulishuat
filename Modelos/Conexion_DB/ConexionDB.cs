    using System;
    using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    namespace Modelos.Conexion_DB
    {
        internal class ConexionDB
        {
        private static string archivoBaseDatos = "OFICIAL.db3";
        private static string archivoScript = "OFICIAL_SQLite.sql";

        private static string RutaDB => Path.Combine(Application.StartupPath, archivoBaseDatos);

        public static SQLiteConnection Conectar()
        {
            try
            {
                // Si el .db3 no existe, lo creamos ejecutando el script
                if (!File.Exists(RutaDB))
                {
                    InicializarBaseDatos();
                }

                string cadena = $"Data Source={RutaDB};Version=3;Foreign Keys=True;";
                SQLiteConnection conexion = new SQLiteConnection(cadena);
                conexion.Open();
                return conexion;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo conectar a la base de datos local.\n\n" +
                    $"Ruta del archivo: {RutaDB}\n\n" +
                    $"Detalles: {ex.Message}",
                    "Error de conexión",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private static void InicializarBaseDatos()
        {
            // 1) Crear el archivo .db3 vacío
            SQLiteConnection.CreateFile(RutaDB);

            // 2) Leer el script SQL
            string rutaScript = Path.Combine(Application.StartupPath, archivoScript);
            if (!File.Exists(rutaScript))
            {
                throw new FileNotFoundException(
                    $"No se encontró el script de inicialización:\n{rutaScript}\n\n" +
                    "Asegúrate de que el archivo esté marcado como 'Content' y 'Copy if newer' en el proyecto.");
            }

            string script = File.ReadAllText(rutaScript);

            // 3) Ejecutar el script
            using (SQLiteConnection conexion = new SQLiteConnection($"Data Source={RutaDB};Version=3;"))
            {
                conexion.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(script, conexion))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        //private static string servidor = "DESKTOP-S8LD4U5\\SQLEXPRESS";
        //private static string baseDatos = "OFICIAL";

        //public static SQLiteConnection Conectar()
        //{
        //    try
        //    {
        //        string cadena = $"Data Source={servidor}; Initial Catalog={baseDatos}; Integrated Security=True";
        //        SQLiteConnection conection = new SQLiteConnection(cadena);
        //        conection.Open();

        //        return conection;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"No se pudo conectar al servidor{servidor}", "Error:" + ex, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return null;
        //    }
        //}
    }
}
