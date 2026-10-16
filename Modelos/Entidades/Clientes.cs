using Modelos.Conexion_DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modelos.Entidades
{
    public class Clientes
    {
        private int idCliente;
        private string nombreCliente;
        private string apellidoCliente;
        private string telefonoCliente;
        private string NCRCliente;
        private string DUICliente;
        private string NITCliente;
        private int idTipoCliente;
        private string correoCliente;
        private bool estadoCliente;

        public int IdCliente { get => idCliente; set => idCliente = value; }
        public string NombreCliente { get => nombreCliente; set => nombreCliente = value; }
        public string ApellidoCliente { get => apellidoCliente; set => apellidoCliente = value; }
        public string TelefonoCliente { get => telefonoCliente; set => telefonoCliente = value; }
        public string NCRCliente1 { get => NCRCliente; set => NCRCliente = value; }
        public string DUICliente1 { get => DUICliente; set => DUICliente = value; }
        public string NITCliente1 { get => NITCliente; set => NITCliente = value; }
        public int IdTipoCliente { get => idTipoCliente; set => idTipoCliente = value; }
        public string CorreoCliente { get => correoCliente; set => correoCliente = value; }
        public bool EstadoCliente { get => estadoCliente; set => estadoCliente = value; }


        //Mostrar clientes Gerente y Empleado
        public static DataTable MostrarClientes()
        {
            using (SQLiteConnection conection = ConexionDB.Conectar())
            {
                string comando = "SELECT * FROM VistaDatosCliente WHERE EliminadoCliente = 0;";
                SQLiteDataAdapter ad = new SQLiteDataAdapter(comando, conection);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                return dt;
            }
        }

        //Mostrar Clientes Activos
        public static DataTable MostrarClientesActivos()
        {
            using (SQLiteConnection conection = ConexionDB.Conectar())
            {
                string comando = "SELECT * FROM VistaDatosCliente WHERE EstadoCliente = 1;";
                SQLiteDataAdapter ad = new SQLiteDataAdapter(comando, conection);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                return dt;
            }
        }

        //Mostrar Clientes Inactivos
        public static DataTable MostrarClienteInactivos()
        {
            using (SQLiteConnection conection = ConexionDB.Conectar())
            {
                string comando = "SELECT * FROM VistaDatosCliente WHERE EstadoCliente = 0;";
                SQLiteDataAdapter ad = new SQLiteDataAdapter(comando, conection);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                return dt;
            }
        }

        //Agregar Clientes a la base de Datos
        public static (string resultado, int idCliente) AgregarCliente(Clientes nuevoCliente)
        {
            try
            {
                using (SQLiteConnection connection = ConexionDB.Conectar())
                {
                    string sqlBuscar = @"
                        SELECT IdCliente, EliminadoCliente 
                        FROM Cliente
                        WHERE (@DUICliente IS NOT NULL AND DUICliente = @DUICliente)
                           OR (@NITCliente IS NOT NULL AND NITCliente = @NITCliente)
                           OR (@NCRCliente IS NOT NULL AND NCRCliente = @NCRCliente)
                           OR CorreoCliente = @CorreoCliente
                        LIMIT 1;";

                    using (SQLiteCommand cmdBuscar = new SQLiteCommand(sqlBuscar, connection))
                    {
                        cmdBuscar.Parameters.Add("@DUICliente", DbType.String).Value =
                            string.IsNullOrWhiteSpace(nuevoCliente.DUICliente) ? (object)DBNull.Value : nuevoCliente.DUICliente;
                        cmdBuscar.Parameters.Add("@NITCliente", DbType.String).Value =
                            string.IsNullOrWhiteSpace(nuevoCliente.NITCliente) ? (object)DBNull.Value : nuevoCliente.NITCliente;
                        cmdBuscar.Parameters.Add("@NCRCliente", DbType.String).Value =
                            string.IsNullOrWhiteSpace(nuevoCliente.NCRCliente) ? (object)DBNull.Value : nuevoCliente.NCRCliente;
                        cmdBuscar.Parameters.Add("@CorreoCliente", DbType.String).Value = nuevoCliente.CorreoCliente;

                        using (SQLiteDataReader reader = cmdBuscar.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int idExistente = Convert.ToInt32(reader["IdCliente"]);
                                bool eliminado = Convert.ToInt32(reader["EliminadoCliente"]) == 1;
                                return (eliminado ? "ELIMINADO" : "ACTIVO", idExistente);
                            }
                        }
                    }

                    string sqlInsert = @"
                        INSERT INTO Cliente
                            (NombreCliente, ApellidoCliente, NCRCliente, DUICliente, NITCliente,
                             TelefonoCliente, CorreoCliente, IdTipoCliente, EstadoCliente, EliminadoCliente)
                        VALUES
                            (@NombreCliente, @ApellidoCliente, @NCRCliente, @DUICliente, @NITCliente,
                             @TelefonoCliente, @CorreoCliente, @IdTipoCliente, 0, 0);
                        SELECT last_insert_rowid();";

                    using (SQLiteCommand cmdInsert = new SQLiteCommand(sqlInsert, connection))
                    {
                        cmdInsert.Parameters.Add("@NombreCliente", DbType.String).Value = nuevoCliente.NombreCliente;
                        cmdInsert.Parameters.Add("@ApellidoCliente", DbType.String).Value = nuevoCliente.ApellidoCliente;
                        cmdInsert.Parameters.Add("@NCRCliente", DbType.String).Value =
                            string.IsNullOrWhiteSpace(nuevoCliente.NCRCliente) ? (object)DBNull.Value : nuevoCliente.NCRCliente;
                        cmdInsert.Parameters.Add("@DUICliente", DbType.String).Value =
                            string.IsNullOrWhiteSpace(nuevoCliente.DUICliente) ? (object)DBNull.Value : nuevoCliente.DUICliente;
                        cmdInsert.Parameters.Add("@NITCliente", DbType.String).Value =
                            string.IsNullOrWhiteSpace(nuevoCliente.NITCliente) ? (object)DBNull.Value : nuevoCliente.NITCliente;
                        cmdInsert.Parameters.Add("@TelefonoCliente", DbType.String).Value = nuevoCliente.TelefonoCliente;
                        cmdInsert.Parameters.Add("@CorreoCliente", DbType.String).Value = nuevoCliente.CorreoCliente;
                        cmdInsert.Parameters.Add("@IdTipoCliente", DbType.Int32).Value = nuevoCliente.IdTipoCliente;

                        int idNuevo = Convert.ToInt32(cmdInsert.ExecuteScalar());
                        return ("NUEVO", idNuevo);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR en AgregarCliente:\n\n" + ex.Message,
                                "DEBUG-AGREGARCLIENTE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        //Actualizar Cliente de la base de Datos
        public static void ActualizarCliente(Clientes actualizarcliente)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql = @"
                    UPDATE Cliente
                    SET NombreCliente = @NombreCliente,
                        ApellidoCliente = @ApellidoCliente,
                        NCRCliente = @NCRCliente,
                        DUICliente = @DUICliente,
                        NITCliente = @NITCliente,
                        TelefonoCliente = @TelefonoCliente,
                        CorreoCliente = @CorreoCliente,
                        IdTipoCliente = @IdTipoCliente
                    WHERE IdCliente = @IdCliente;";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.Add("@IdCliente", DbType.Int32).Value = actualizarcliente.IdCliente;
                    command.Parameters.Add("@NombreCliente", DbType.String).Value = actualizarcliente.NombreCliente;
                    command.Parameters.Add("@ApellidoCliente", DbType.String).Value = actualizarcliente.ApellidoCliente;

                    command.Parameters.Add("@NCRCliente", DbType.String);
                    if (string.IsNullOrWhiteSpace(actualizarcliente.NCRCliente))
                    {
                        command.Parameters["@NCRCliente"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters["@NCRCliente"].Value = actualizarcliente.NCRCliente;
                    }

                    command.Parameters.Add("@DUICliente", DbType.String);
                    if (string.IsNullOrWhiteSpace(actualizarcliente.DUICliente))
                    {
                        command.Parameters["@DUICliente"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters["@DUICliente"].Value = actualizarcliente.DUICliente;
                    }

                    command.Parameters.Add("@NITCliente", DbType.String);
                    if (string.IsNullOrWhiteSpace(actualizarcliente.NITCliente))
                    {
                        command.Parameters["@NITCliente"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters["@NITCliente"].Value = actualizarcliente.NITCliente;
                    }

                    command.Parameters.Add("@TelefonoCliente", DbType.String).Value = actualizarcliente.TelefonoCliente;
                    command.Parameters.Add("@CorreoCliente", DbType.String).Value = actualizarcliente.CorreoCliente;
                    command.Parameters.Add("@IdTipoCliente", DbType.Int32).Value = actualizarcliente.IdTipoCliente;

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        //Buscar por Id-Cliente
        public static DataTable ObtenerClienteId(int idCliente)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql = "SELECT * FROM Cliente WHERE IdCliente = @IdCliente;";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.Add("@IdCliente", DbType.Int32).Value = idCliente;

                    DataTable dt = new DataTable();
                    using (SQLiteDataAdapter ad = new SQLiteDataAdapter(command))
                    {
                        ad.Fill(dt);
                    }
                    return dt;
                }
            }
        }

        //Eliminar Cliente de la base de Datos
        public static int EliminarCliente(int idCliente)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                using (SQLiteCommand cmdCheckExistencia = new SQLiteCommand(
            "SELECT COUNT(*) FROM Cliente WHERE IdCliente = @Id AND EliminadoCliente = 0;",
            connection))
                {
                    cmdCheckExistencia.Parameters.Add("@Id", DbType.Int32).Value = idCliente;
                    if (Convert.ToInt32(cmdCheckExistencia.ExecuteScalar()) == 0)
                        return 0;
                }

                using (SQLiteCommand cmdCheckVentas = new SQLiteCommand(
                    @"SELECT COUNT(*) FROM Venta
              WHERE IdCliente = @Id
                AND IdEstadoVenta IN (1, 2);",
                    connection))
                {
                    cmdCheckVentas.Parameters.Add("@Id", DbType.Int32).Value = idCliente;
                    if (Convert.ToInt32(cmdCheckVentas.ExecuteScalar()) > 0)
                        return -1;
                }

                using (SQLiteCommand cmdCheckEventos = new SQLiteCommand(
                    @"SELECT COUNT(*) FROM Fecha
              WHERE IdCliente = @Id
                AND DiaFecha >= date('now','localtime');",
                    connection))
                {
                    cmdCheckEventos.Parameters.Add("@Id", DbType.Int32).Value = idCliente;
                    if (Convert.ToInt32(cmdCheckEventos.ExecuteScalar()) > 0)
                        return -2;
                }

                using (SQLiteCommand cmdCheckPagos = new SQLiteCommand(
                    @"SELECT COUNT(*) FROM RegistoPago
              WHERE IdCliente = @Id
                AND EstadoPago = 'Pendiente';",
                    connection))
                {
                    cmdCheckPagos.Parameters.Add("@Id", DbType.Int32).Value = idCliente;
                    if (Convert.ToInt32(cmdCheckPagos.ExecuteScalar()) > 0)
                        return -3;
                }

                using (SQLiteCommand cmdDel = new SQLiteCommand(
                    "UPDATE Cliente SET EliminadoCliente = 1 WHERE IdCliente = @Id;",
                    connection))
                {
                    cmdDel.Parameters.Add("@Id", DbType.Int32).Value = idCliente;
                    cmdDel.ExecuteNonQuery();
                    return 1;
                }
            }
        }

        //Busqueda Cliente 
        public DataTable BuscarCliente(string busqueda)
        {

            DataTable tablaCliente = new DataTable();
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                busqueda = (busqueda ?? "").Trim();

                string sql;
                if (string.IsNullOrEmpty(busqueda))
                {
                    sql = "SELECT * FROM VistaDatosCliente;";
                }
                else
                {
                    sql = @"SELECT * FROM VistaDatosCliente
                            WHERE NombreCliente   LIKE @q
                               OR ApellidoCliente LIKE @q
                               OR DUICliente      LIKE @q
                               OR NITCliente      LIKE @q;";
                }

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    if (!string.IsNullOrEmpty(busqueda))
                    {
                        command.Parameters.Add("@q", DbType.String).Value = "%" + busqueda + "%";
                    }

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(tablaCliente);
                    }
                }
            }
            return tablaCliente;

        }

        //Restaurar cliente
        public static void RestaurarCliente(int idCliente)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql = "UPDATE Cliente SET EliminadoCliente = 0 WHERE IdCliente = @IdCliente;";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.Add("@IdCliente", DbType.Int32).Value = idCliente;
                    command.ExecuteNonQuery();
                }
            }
        }

        //Paginas cliente :[
        public static DataTable MostrarClientesPagina(int pagina, int registrosPorPagina, int tipoFiltro, out int totalRegistros)
        {
            DataTable dt = new DataTable();
            totalRegistros = 0;

            try
            {
                using (SQLiteConnection conexion = ConexionDB.Conectar())
                {
                    string whereFiltro;
                    switch (tipoFiltro)
                    {
                        case 2:  // Clientes Activos (con ventas en proceso/pendientes)
                            whereFiltro = @"WHERE EXISTS (
                                        SELECT 1 FROM Venta V
                                        WHERE V.IdCliente = VistaDatosCliente.IdCliente
                                          AND V.IdEstadoVenta IN (1, 2)
                                    )";
                            break;

                        case 3:  // Clientes No Activos (sin ventas activas)
                            whereFiltro = @"WHERE NOT EXISTS (
                                        SELECT 1 FROM Venta V
                                        WHERE V.IdCliente = VistaDatosCliente.IdCliente
                                          AND V.IdEstadoVenta IN (1, 2)
                                    )";
                            break;

                        default: // Todos
                            whereFiltro = "";
                            break;
                    }

                    string sqlCount = "SELECT COUNT(*) FROM VistaDatosCliente " + whereFiltro + ";";

                    using (SQLiteCommand cmdCount = new SQLiteCommand(sqlCount, conexion))
                    {
                        totalRegistros = Convert.ToInt32(cmdCount.ExecuteScalar());
                    }

                    int offset = (pagina - 1) * registrosPorPagina;

                    string sqlDatos = @"
                SELECT IdCliente, NombreCliente, ApellidoCliente, DUICliente,
                       TelefonoCliente, CorreoCliente, EstadoCliente, TipoCliente,
                       EsClienteActivo AS ClienteActivo
                FROM VistaDatosCliente
                " + whereFiltro + @"
                ORDER BY IdCliente
                LIMIT @limit OFFSET @offset;";

                    using (SQLiteCommand cmd = new SQLiteCommand(sqlDatos, conexion))
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
                throw new Exception("Error al obtener los clientes paginados: " + ex.Message, ex);
            }

            return dt;
        }
    }
}
