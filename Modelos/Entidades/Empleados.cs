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
    public class Empleados
    {
        private int idEmpleado;
        private string dUIEmpleado;
        private string nombreEmpleado;
        private string apellidoEmpleado;
        private string telefonoEmpleado;
        private string cargoEmpleado;
        private string correoEmpleado;
        private string cuentaBancariaEmpleado;
        private int idEstadoEmpleado;
        private string fotoEmpleado;
        private int idCliente;


        public int IdEmpleado { get => idEmpleado; set => idEmpleado = value; }
        public string DUIEmpleado { get => dUIEmpleado; set => dUIEmpleado = value; }
        public string NombreEmpleado { get => nombreEmpleado; set => nombreEmpleado = value; }
        public string ApellidoEmpleado { get => apellidoEmpleado; set => apellidoEmpleado = value; }
        public string TelefonoEmpleado { get => telefonoEmpleado; set => telefonoEmpleado = value; }
        public string CargoEmpleado { get => cargoEmpleado; set => cargoEmpleado = value; }
        public string CorreoEmpleado { get => correoEmpleado; set => correoEmpleado = value; }
        public string CuentaBancariaEmpleado { get => cuentaBancariaEmpleado; set => cuentaBancariaEmpleado = value; }
        public int IdEstadoEmpleado { get => idEstadoEmpleado; set => idEstadoEmpleado = value; }
        public string FotoEmpleado { get => fotoEmpleado; set => fotoEmpleado = value; }
        public int IdCliente { get => idCliente; set => idCliente = value; }

        public static DataTable MostrarEmpleado()
        {
            using (SQLiteConnection conection = ConexionDB.Conectar())
            {
                string comando = "SELECT * FROM VistaEmpleadoGerente WHERE EliminadoEmpleado = 0;";
                SQLiteDataAdapter ad = new SQLiteDataAdapter(comando, conection);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                return dt;
            }
        }

        //Agregar Empleados     
        public static (string resultado, int idEmpleado) AgregarEmpleado(Empleados nuevoEmpleado)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                int idExistente = -1;
                int eliminado = 0;

                string sqlBuscar = @"
                    SELECT IdEmpleado, EliminadoEmpleado
                    FROM Empleado
                    WHERE DUIEmpleado = @DUI
                       OR CorreoEmpleado = @Correo
                    LIMIT 1;";

                using (SQLiteCommand cmdBuscar = new SQLiteCommand(sqlBuscar, connection))
                {
                    cmdBuscar.Parameters.Add("@DUI", DbType.String).Value = nuevoEmpleado.DUIEmpleado;
                    cmdBuscar.Parameters.Add("@Correo", DbType.String).Value = nuevoEmpleado.CorreoEmpleado;

                    using (SQLiteDataReader reader = cmdBuscar.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idExistente = Convert.ToInt32(reader["IdEmpleado"]);
                            eliminado = Convert.ToInt32(reader["EliminadoEmpleado"]);
                        }
                    }
                }

                if (idExistente != -1 && eliminado == 0)
                    return ("ACTIVO", idExistente);

                if (idExistente != -1 && eliminado == 1)
                    return ("ELIMINADO", idExistente);

                string sqlInsert = @"
                    INSERT INTO Empleado
                        (DUIEmpleado, NombreEmpleado, ApellidoEmpleado, TelefonoEmpleado,
                         CargoEmpleado, CorreoEmpleado, CuentaBancariaEmpleado,
                         FotoEmpleado, IdEstadoEmpleado, EliminadoEmpleado)
                    VALUES
                        (@DUI, @Nombre, @Apellido, @Telefono,
                         @Cargo, @Correo, @Cuenta,
                         @Foto, 1, 0);
                    SELECT last_insert_rowid();";

                using (SQLiteCommand cmdInsert = new SQLiteCommand(sqlInsert, connection))
                {
                    cmdInsert.Parameters.Add("@DUI", DbType.String).Value = nuevoEmpleado.DUIEmpleado;
                    cmdInsert.Parameters.Add("@Nombre", DbType.String).Value = nuevoEmpleado.NombreEmpleado;
                    cmdInsert.Parameters.Add("@Apellido", DbType.String).Value = nuevoEmpleado.ApellidoEmpleado;
                    cmdInsert.Parameters.Add("@Telefono", DbType.String).Value = nuevoEmpleado.TelefonoEmpleado;
                    cmdInsert.Parameters.Add("@Cargo", DbType.String).Value = nuevoEmpleado.CargoEmpleado;
                    cmdInsert.Parameters.Add("@Correo", DbType.String).Value = nuevoEmpleado.CorreoEmpleado;
                    cmdInsert.Parameters.Add("@Cuenta", DbType.String).Value = nuevoEmpleado.CuentaBancariaEmpleado;
                    cmdInsert.Parameters.Add("@Foto", DbType.String).Value =
                        string.IsNullOrWhiteSpace(nuevoEmpleado.FotoEmpleado) ? (object)DBNull.Value : nuevoEmpleado.FotoEmpleado;

                    int idNuevo = Convert.ToInt32(cmdInsert.ExecuteScalar());
                    return ("NUEVO", idNuevo);
                }
            }
        }

        //Actualizar Empleados
        public static void ActualizarEmpleado(Empleados actualizarEmpleado)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql = @"
                     UPDATE Empleado
            SET DUIEmpleado            = @DUIEmpleado,
                NombreEmpleado         = @NombreEmpleado,
                ApellidoEmpleado       = @ApellidoEmpleado,
                TelefonoEmpleado       = @TelefonoEmpleado,
                CargoEmpleado          = @CargoEmpleado,
                CorreoEmpleado         = @CorreoEmpleado,
                CuentaBancariaEmpleado = @CuentaBancariaEmpleado,
                FotoEmpleado           = @FotoEmpleado
            WHERE IdEmpleado = @IdEmpleado;";


                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.Add("@IdEmpleado", DbType.Int32).Value = actualizarEmpleado.IdEmpleado;
                    command.Parameters.Add("@NombreEmpleado", DbType.String).Value = actualizarEmpleado.NombreEmpleado;
                    command.Parameters.Add("@ApellidoEmpleado", DbType.String).Value = actualizarEmpleado.ApellidoEmpleado;
                    command.Parameters.Add("@DUIEmpleado", DbType.String).Value = actualizarEmpleado.DUIEmpleado;
                    command.Parameters.Add("@TelefonoEmpleado", DbType.String).Value = actualizarEmpleado.TelefonoEmpleado;
                    command.Parameters.Add("@CorreoEmpleado", DbType.String).Value = actualizarEmpleado.CorreoEmpleado;
                    command.Parameters.Add("@CargoEmpleado", DbType.String).Value = actualizarEmpleado.CargoEmpleado;
                    command.Parameters.Add("@CuentaBancariaEmpleado", DbType.String).Value = actualizarEmpleado.CuentaBancariaEmpleado;
                    command.Parameters.Add("@FotoEmpleado", DbType.String).Value = string.IsNullOrWhiteSpace(actualizarEmpleado.FotoEmpleado) ? (object)DBNull.Value : actualizarEmpleado.FotoEmpleado;

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        //Buscar por Id-Empleado
        public static DataTable ObtenerEmpleadoId(int idEmpleado)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql = "SELECT * FROM Empleado WHERE IdEmpleado = @IdEmpleado;";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.Add("@IdEmpleado", DbType.Int32).Value = idEmpleado;

                    DataTable dt = new DataTable();
                    using (SQLiteDataAdapter ad = new SQLiteDataAdapter(command))
                    {
                        ad.Fill(dt);
                    }
                    return dt;
                }
            }
        }

        //BuscarEmpleado
        public DataTable BuscarEmpleado(string busqueda)
        {

            DataTable tablaEmpleado = new DataTable();
            busqueda = (busqueda ?? "").Trim();

            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql;
                if (string.IsNullOrEmpty(busqueda))
                {
                    sql = "SELECT * FROM VistaEmpleadoGerente WHERE EliminadoEmpleado = 0;";
                }
                else
                {
                    sql = @"SELECT * FROM VistaEmpleadoGerente
                            WHERE EliminadoEmpleado = 0
                              AND (NombreEmpleado   LIKE @q
                                OR ApellidoEmpleado LIKE @q
                                OR DUIEmpleado      LIKE @q);";
                }

                using (var command = new SQLiteCommand(sql, connection))
                {
                    if (!string.IsNullOrEmpty(busqueda))
                    {
                        command.Parameters.Add("@q", DbType.String).Value = "%" + busqueda + "%";
                    }

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(tablaEmpleado);
                    }
                }
            }
            return tablaEmpleado;
        }

        //Cambiar cmb Estado
        public static void ActualizarEstadoEmpleado(int idEstadoEmpleado, int idEmpleado)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql = @"
                    UPDATE Empleado
                    SET IdEstadoEmpleado = @IdEstadoEmpleado
                    WHERE IdEmpleado = @IdEmpleado;";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.Add("@IdEstadoEmpleado", DbType.Int32).Value = idEstadoEmpleado;
                    command.Parameters.Add("@IdEmpleado", DbType.Int32).Value = idEmpleado;
                    command.ExecuteNonQuery();
                }
            }
        }


        //Mostrar empleado por pagina 20

        public static DataTable MostrarEmpleadoPagina(int pagina, int registrosPorPagina, out int totalRegistros)
        {
            DataTable dt = new DataTable();
            totalRegistros = 0;

            try
            {
                using (SQLiteConnection conexion = ConexionDB.Conectar())
                {
                    using (SQLiteCommand cmdTotal = new SQLiteCommand(
                        "SELECT COUNT(*) FROM Empleado WHERE EliminadoEmpleado = 0;", conexion))
                    {
                        totalRegistros = Convert.ToInt32(cmdTotal.ExecuteScalar());
                    }

                    int offset = (pagina - 1) * registrosPorPagina;

                    string sql = @"
                        SELECT IdEmpleado, DUIEmpleado, NombreEmpleado, ApellidoEmpleado,
                               TelefonoEmpleado, CargoEmpleado, CorreoEmpleado,
                               CuentaBancariaEmpleado, FotoEmpleado, IdEstadoEmpleado
                        FROM Empleado
                        WHERE EliminadoEmpleado = 0
                        ORDER BY IdEmpleado
                        LIMIT @limit OFFSET @offset;";

                    using (SQLiteCommand command = new SQLiteCommand(sql, conexion))
                    {
                        command.Parameters.Add("@limit", DbType.Int32).Value = registrosPorPagina;
                        command.Parameters.Add("@offset", DbType.Int32).Value = offset;

                        using (SQLiteDataAdapter ad = new SQLiteDataAdapter(command))
                        {
                            ad.Fill(dt);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new Exception("Error al obtener los empleados por pagina: " + ex.Message, ex);
            }

            return dt;
        }

        //Restaurar empleado
        public static void RestaurarEmpleado(int idEmpleado)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql = "UPDATE Empleado SET EliminadoEmpleado = 0 WHERE IdEmpleado = @IdEmpleado;";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.Add("@IdEmpleado", DbType.Int32).Value = idEmpleado;
                    command.ExecuteNonQuery();
                }
            }
        }

        //Eliminar Empleado
        public static bool EliminarEmpleado(int idEmpleado)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sqlCheck = @"
                    SELECT COUNT(*) FROM Venta
                    WHERE IdEmpleado = @IdEmpleado
                      AND IdEstadoVenta IN (1, 2);";

                using (SQLiteCommand cmdCheck = new SQLiteCommand(sqlCheck, connection))
                {
                    cmdCheck.Parameters.Add("@IdEmpleado", DbType.Int32).Value = idEmpleado;
                    if (Convert.ToInt32(cmdCheck.ExecuteScalar()) > 0)
                    {
                        return false;
                    }
                }

                string sql = "UPDATE Empleado SET EliminadoEmpleado = 1 WHERE IdEmpleado = @IdEmpleado;";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.Add("@IdEmpleado", DbType.Int32).Value = idEmpleado;
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }
    }
}
