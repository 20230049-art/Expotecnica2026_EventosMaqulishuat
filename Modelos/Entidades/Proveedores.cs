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
    public class Proveedores
    {
        private int idProveedor;
        private string nombreProveedor;
        private string correoProveedor;
        private string telefonoProveedor;
        private string fotoProveedor;
        private int idGerente;

        public int IdProveedor { get => idProveedor; set => idProveedor = value; }
        public string NombreProveedor { get => nombreProveedor; set => nombreProveedor = value; }
        public string CorreoProveedor { get => correoProveedor; set => correoProveedor = value; }
        public string TelefonoProveedor { get => telefonoProveedor; set => telefonoProveedor = value; }
        public string FotoProveedor { get => fotoProveedor; set => fotoProveedor = value; }
        public int IdGerente { get => idGerente; set => idGerente = value; }

        //Mostrar Proveedores
        public static DataTable MostrarProvedores()
        {
            SQLiteConnection conection = ConexionDB.Conectar();
            string comando = "SELECT * FROM VistaProveedorGerente;";
            SQLiteDataAdapter ad = new SQLiteDataAdapter(comando, conection);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            return dt;
        }

        //Mostrar Proveedores en un cmbox
        public static DataTable ObtnerProveedor()
        {
            SQLiteConnection conection = ConexionDB.Conectar();
            string comando = "SELECT idProveedor, NombreProveedor FROM Proveedor;";
            SQLiteDataAdapter ad = new SQLiteDataAdapter(comando, conection);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            return dt;
        }

        //Añadir Proveedor
        public static int IngresarProveedor(Proveedores nuevoProveedor)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                int idExistente = -1;
                int eliminado = 0;

                string sqlBuscar = @"
                    SELECT IdProveedor, EliminadoProveedor
                    FROM Proveedor
                    WHERE CorreoProveedor   = @Correo
                       OR NombreProveedor   = @Nombre
                       OR TelefonoProveedor = @Telefono
                    LIMIT 1;";

                using (SQLiteCommand cmdBuscar = new SQLiteCommand(sqlBuscar, connection))
                {
                    cmdBuscar.Parameters.Add("@Correo", DbType.String).Value = nuevoProveedor.CorreoProveedor;
                    cmdBuscar.Parameters.Add("@Nombre", DbType.String).Value = nuevoProveedor.NombreProveedor;
                    cmdBuscar.Parameters.Add("@Telefono", DbType.String).Value = nuevoProveedor.TelefonoProveedor;

                    using (SQLiteDataReader reader = cmdBuscar.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idExistente = Convert.ToInt32(reader["IdProveedor"]);
                            eliminado = Convert.ToInt32(reader["EliminadoProveedor"]);
                        }
                    }
                }

                if (idExistente != -1 && eliminado == 0)
                    return 2;

                if (idExistente != -1 && eliminado == 1)
                {
                    string sqlUpdate = @"
                        UPDATE Proveedor
                        SET NombreProveedor   = @Nombre,
                            CorreoProveedor   = @Correo,
                            TelefonoProveedor = @Telefono,
                            FotoProveedor     = @Foto,
                            EliminadoProveedor = 0
                        WHERE IdProveedor = @Id;";

                    using (SQLiteCommand cmdUpdate = new SQLiteCommand(sqlUpdate, connection))
                    {
                        cmdUpdate.Parameters.Add("@Id", DbType.Int32).Value = idExistente;
                        cmdUpdate.Parameters.Add("@Nombre", DbType.String).Value = nuevoProveedor.NombreProveedor;
                        cmdUpdate.Parameters.Add("@Correo", DbType.String).Value = nuevoProveedor.CorreoProveedor;
                        cmdUpdate.Parameters.Add("@Telefono", DbType.String).Value = nuevoProveedor.TelefonoProveedor;
                        cmdUpdate.Parameters.Add("@Foto", DbType.String).Value =
                            string.IsNullOrWhiteSpace(nuevoProveedor.FotoProveedor) ? (object)DBNull.Value : nuevoProveedor.FotoProveedor;

                        cmdUpdate.ExecuteNonQuery();
                    }
                    return 1;
                }

                string sqlInsert = @"
                    INSERT INTO Proveedor
                        (NombreProveedor, CorreoProveedor, TelefonoProveedor, FotoProveedor, IdGerente, EliminadoProveedor)
                    VALUES
                        (@Nombre, @Correo, @Telefono, @Foto, 1, 0);";

                using (SQLiteCommand cmdInsert = new SQLiteCommand(sqlInsert, connection))
                {
                    cmdInsert.Parameters.Add("@Nombre", DbType.String).Value = nuevoProveedor.NombreProveedor;
                    cmdInsert.Parameters.Add("@Correo", DbType.String).Value = nuevoProveedor.CorreoProveedor;
                    cmdInsert.Parameters.Add("@Telefono", DbType.String).Value = nuevoProveedor.TelefonoProveedor;
                    cmdInsert.Parameters.Add("@Foto", DbType.String).Value =
                        string.IsNullOrWhiteSpace(nuevoProveedor.FotoProveedor) ? (object)DBNull.Value : nuevoProveedor.FotoProveedor;

                    cmdInsert.ExecuteNonQuery();
                }
                return 0;
            }
        }

        //Actualizar Proveedor
        public static void ActualizarProveedor(Proveedores actualizarProveedor)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql = @"
                    UPDATE Proveedor
                    SET NombreProveedor   = @Nombre,
                        CorreoProveedor   = @Correo,
                        TelefonoProveedor = @Telefono,
                        FotoProveedor     = @Foto
                    WHERE IdProveedor = @Id;";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.Add("@Id", DbType.Int32).Value = actualizarProveedor.IdProveedor;
                    command.Parameters.Add("@Nombre", DbType.String).Value = actualizarProveedor.NombreProveedor;
                    command.Parameters.Add("@Correo", DbType.String).Value = actualizarProveedor.CorreoProveedor;
                    command.Parameters.Add("@Telefono", DbType.String).Value = actualizarProveedor.TelefonoProveedor;
                    command.Parameters.Add("@Foto", DbType.String).Value =
                        string.IsNullOrWhiteSpace(actualizarProveedor.FotoProveedor) ? (object)DBNull.Value : actualizarProveedor.FotoProveedor;

                    command.ExecuteNonQuery();
                }
            }
        }

        //Buscar por Id-Proveedor
        public static DataTable ObtenerProveedorId(int idProveedor)
        {

            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql = "SELECT * FROM Proveedor WHERE IdProveedor = @Id;";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.Add("@Id", DbType.Int32).Value = idProveedor;

                    DataTable dt = new DataTable();
                    using (SQLiteDataAdapter ad = new SQLiteDataAdapter(command))
                    {
                        ad.Fill(dt);
                    }
                    return dt;
                }
            }
        }


        //Proveedorer nombre
        public static DataTable ObtenerNombreProveedor()
        {
            SQLiteConnection conexion = ConexionDB.Conectar();
            string consulta = "SELECT IdProveedor, NombreProveedor FROM Proveedor";
            SQLiteDataAdapter da = new SQLiteDataAdapter(consulta, conexion);
            DataTable tabla = new DataTable();
            da.Fill(tabla);
            conexion.Close();
            return tabla;
        }

        //Buscar Proveedor
        public DataTable BuscarProveedor(string busqueda)
        {

            DataTable tablaProveedor = new DataTable();
            busqueda = (busqueda ?? "").Trim();

            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql;
                if (string.IsNullOrEmpty(busqueda))
                {
                    sql = "SELECT * FROM VistaProveedorGerente;";
                }
                else
                {
                    sql = @"SELECT * FROM VistaProveedorGerente
                            WHERE NombreProveedor   LIKE @q
                               OR CorreoProveedor   LIKE @q
                               OR TelefonoProveedor LIKE @q;";
                }

                using (var command = new SQLiteCommand(sql, connection))
                {
                    if (!string.IsNullOrEmpty(busqueda))
                    {
                        command.Parameters.Add("@q", DbType.String).Value = "%" + busqueda + "%";
                    }

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(tablaProveedor);
                    }
                }
            }
            return tablaProveedor;
        }

        //Mostrar proveedores por pagina
        public static DataTable MostrarProveedoresPaginado(int pagina, int registrosPorPagina, out int totalRegistros)
        {
            DataTable dt = new DataTable();
            totalRegistros = 0;

            try
            {
                using (SQLiteConnection conexion = ConexionDB.Conectar())
                {
                    // Total
                    using (SQLiteCommand cmdTotal = new SQLiteCommand(
                        "SELECT COUNT(*) FROM Proveedor WHERE EliminadoProveedor = 0;", conexion))
                    {
                        totalRegistros = Convert.ToInt32(cmdTotal.ExecuteScalar());
                    }

                    int offset = (pagina - 1) * registrosPorPagina;

                    string sql = @"
                        SELECT IdProveedor, NombreProveedor, CorreoProveedor, TelefonoProveedor, FotoProveedor
                        FROM Proveedor
                        WHERE EliminadoProveedor = 0
                        ORDER BY IdProveedor
                        LIMIT @limit OFFSET @offset;";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conexion))
                    {
                        cmd.Parameters.Add("@limit", DbType.Int32).Value = registrosPorPagina;
                        cmd.Parameters.Add("@offset", DbType.Int32).Value = offset;

                        using (SQLiteDataAdapter ad = new SQLiteDataAdapter(cmd))
                        {
                            ad.Fill(dt);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new Exception("Error al obtener los proveedores paginados: " + ex.Message, ex);
            }

            return dt;
        }

        //Eliminar producto
        public static bool EliminarProveedor(int idProveedor)
        {
            try
            {
                using (SQLiteConnection connection = ConexionDB.Conectar())
                {
                    string sql = "UPDATE Proveedor SET EliminadoProveedor = 1 WHERE IdProveedor = @Id;";

                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.Add("@Id", DbType.Int32).Value = idProveedor;
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SQLiteException)
            {
                return false;
            }
        }

        // Restaurar
        public static bool RestaurarProveedor(int idProveedor)
        {
            try
            {
                using (SQLiteConnection connection = ConexionDB.Conectar())
                {
                    string sql = "UPDATE Proveedor SET EliminadoProveedor = 0 WHERE IdProveedor = @Id;";

                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.Add("@Id", DbType.Int32).Value = idProveedor;
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SQLiteException)
            {
                return false;
            }
        }
    }
}
