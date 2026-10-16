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
    public class Usuario
    {
        private int idUsuario;
        private string nombreUsuario;
        private string contrasenaUsuario;
        private int idTipoUsuario;
        private int? idGerente;
        private int? idEmpleado;
        private string nombreGerente;
        private string apellidoGerente;
        private string correoGerente;
        private int nuevaContrasena;

        public int IdUsuario { get => idUsuario; set => idUsuario = value; }
        public string NombreUsuario { get => nombreUsuario; set => nombreUsuario = value; }
        public string ContrasenaUsuario { get => contrasenaUsuario; set => contrasenaUsuario = value; }
        public int IdTipoUsuario { get => idTipoUsuario; set => idTipoUsuario = value; }
        public int? IdGerente { get => idGerente; set => idGerente = value; }
        public int? IdEmpleado { get => idEmpleado; set => idEmpleado = value; }
        public string NombreGerente { get => nombreGerente; set => nombreGerente = value; }
        public string ApellidoGerente { get => apellidoGerente; set => apellidoGerente = value; }
        public string CorreoGerente { get => correoGerente; set => correoGerente = value; }

        public static Usuario Datos { get; set; }
        public int NuevaContrasena { get => nuevaContrasena; set => nuevaContrasena = value; }

        //Crear cuenta Gerente
        public bool RegistrarGerente()
        {
            try
            {
                using (SQLiteConnection connection = ConexionDB.Conectar())
                using (SQLiteTransaction tx = connection.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmdCheck = new SQLiteCommand(
                            "SELECT COUNT(*) FROM Gerente WHERE CorreoGerente = @Correo;",
                            connection, tx))
                        {
                            cmdCheck.Parameters.Add("@Correo", DbType.String).Value = correoGerente;
                            if (Convert.ToInt32(cmdCheck.ExecuteScalar()) > 0)
                            {
                                MessageBox.Show("Ya existe un gerente registrado con este correo electrónico.");
                                tx.Rollback();
                                return false;
                            }
                        }

                        using (SQLiteCommand cmdCheck = new SQLiteCommand(
                            "SELECT COUNT(*) FROM Usuario WHERE NombreUsuario = @NombreUsuario;",
                            connection, tx))
                        {
                            cmdCheck.Parameters.Add("@NombreUsuario", DbType.String).Value = nombreUsuario;
                            if (Convert.ToInt32(cmdCheck.ExecuteScalar()) > 0)
                            {
                                MessageBox.Show("El nombre de usuario ya está registrado.");
                                tx.Rollback();
                                return false;
                            }
                        }

                        using (SQLiteCommand cmdIns = new SQLiteCommand(@"
                            INSERT INTO Gerente (NombreGerente, ApellidoGerente, CorreoGerente)
                            VALUES (@NombreGerente, @ApellidoGerente, @CorreoGerente);
                            SELECT last_insert_rowid();", connection, tx))
                        {
                            cmdIns.Parameters.Add("@NombreGerente", DbType.String).Value = nombreGerente;
                            cmdIns.Parameters.Add("@ApellidoGerente", DbType.String).Value = apellidoGerente;
                            cmdIns.Parameters.Add("@CorreoGerente", DbType.String).Value = correoGerente;
                            IdGerente = Convert.ToInt32(cmdIns.ExecuteScalar());
                        }

                        using (SQLiteCommand cmdIns = new SQLiteCommand(@"
                            INSERT INTO Usuario
                                (NombreUsuario, ContrasenaUsuario, IdTipoUsuario, IdEmpleado, IdGerente)
                            VALUES
                                (@NombreUsuario, @ContrasenaUsuario, 1, NULL, @IdGerente);
                            SELECT last_insert_rowid();", connection, tx))
                        {
                            cmdIns.Parameters.Add("@NombreUsuario", DbType.String).Value = nombreUsuario;
                            cmdIns.Parameters.Add("@ContrasenaUsuario", DbType.String).Value = contrasenaUsuario;
                            cmdIns.Parameters.Add("@IdGerente", DbType.Int32).Value = IdGerente.Value;
                            IdUsuario = Convert.ToInt32(cmdIns.ExecuteScalar());
                        }

                        tx.Commit();
                        return true;
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        //Crear Cuenta empleado
        public int RegistrarEmpleado()
        {
            try
            {
                using (SQLiteConnection connection = ConexionDB.Conectar())
                {
                    int idEmpleado;
                    using (SQLiteCommand cmdBuscar = new SQLiteCommand(
                        "SELECT IdEmpleado FROM Empleado WHERE CorreoEmpleado = @Correo AND EliminadoEmpleado = 0;",
                        connection))
                    {
                        cmdBuscar.Parameters.Add("@Correo", DbType.String).Value = nombreUsuario;
                        object result = cmdBuscar.ExecuteScalar();
                        if (result == null || result == DBNull.Value)
                        {
                            MessageBox.Show("No existe un empleado registrado con este correo electrónico.");
                            return 0;
                        }
                        idEmpleado = Convert.ToInt32(result);
                    }

                    using (SQLiteCommand cmdCheck = new SQLiteCommand(
                        "SELECT COUNT(*) FROM Usuario WHERE NombreUsuario = @NombreUsuario;", connection))
                    {
                        cmdCheck.Parameters.Add("@NombreUsuario", DbType.String).Value = nombreUsuario;
                        if (Convert.ToInt32(cmdCheck.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("El nombre de usuario ya está registrado.");
                            return 0;
                        }
                    }

                    using (SQLiteCommand cmdIns = new SQLiteCommand(@"
                        INSERT INTO Usuario
                            (NombreUsuario, ContrasenaUsuario, IdTipoUsuario, IdEmpleado, IdGerente)
                        VALUES
                            (@NombreUsuario, @ContrasenaUsuario, 2, @IdEmpleado, NULL);
                        SELECT last_insert_rowid();", connection))
                    {
                        cmdIns.Parameters.Add("@NombreUsuario", DbType.String).Value = nombreUsuario;
                        cmdIns.Parameters.Add("@ContrasenaUsuario", DbType.String).Value = contrasenaUsuario;
                        cmdIns.Parameters.Add("@IdEmpleado", DbType.Int32).Value = idEmpleado;
                        return Convert.ToInt32(cmdIns.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        //Validar login Gerente
        public Usuario ValidarLoginGerente(string nombreUsuario, string contrasenaUsuario)
        {
            try
            {
                using (SQLiteConnection connection = ConexionDB.Conectar())
                {
                    string sql = @"
                        SELECT IdUsuario, NombreUsuario, ContrasenaUsuario,
                               IdTipoUsuario, IdEmpleado, IdGerente
                        FROM Usuario
                        WHERE NombreUsuario = @Correo
                          AND EliminadoUsuario = 0;";

                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.Add("@Correo", DbType.String).Value = nombreUsuario;

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string hash = reader["ContrasenaUsuario"].ToString();
                                bool contrasenaCorrecta = BCrypt.Net.BCrypt.Verify(contrasenaUsuario, hash);

                                if (!contrasenaCorrecta) return null;

                                Usuario usuario = new Usuario();
                                usuario.IdUsuario = Convert.ToInt32(reader["IdUsuario"]);
                                usuario.NombreUsuario = reader["NombreUsuario"].ToString();
                                usuario.IdTipoUsuario = Convert.ToInt32(reader["IdTipoUsuario"]);

                                if (reader["IdEmpleado"] != DBNull.Value)
                                    usuario.IdEmpleado = Convert.ToInt32(reader["IdEmpleado"]);
                                if (reader["IdGerente"] != DBNull.Value)
                                    usuario.IdGerente = Convert.ToInt32(reader["IdGerente"]);

                                return usuario;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al validar login: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            ;
        }


        //Validar login empleado
        public Usuario ValidarLoginEmpleado(string nombreUsuario, string contrasenaUsuario)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql = @"
                    SELECT U.IdUsuario, U.NombreUsuario, U.ContrasenaUsuario,
                           U.IdTipoUsuario, U.IdEmpleado, U.IdGerente,
                           E.NombreEmpleado, E.ApellidoEmpleado
                    FROM Usuario U
                    INNER JOIN Empleado E ON U.IdEmpleado = E.IdEmpleado
                    WHERE E.CorreoEmpleado = @Correo
                      AND U.EliminadoUsuario = 0;";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.Add("@Correo", DbType.String).Value = nombreUsuario;

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string hash = reader["ContrasenaUsuario"].ToString();
                            bool contrasenaCorrecta = BCrypt.Net.BCrypt.Verify(contrasenaUsuario, hash);

                            if (!contrasenaCorrecta) return null;

                            Usuario usuario = new Usuario();
                            usuario.IdUsuario = Convert.ToInt32(reader["IdUsuario"]);
                            usuario.NombreUsuario = reader["NombreUsuario"].ToString();
                            usuario.IdTipoUsuario = Convert.ToInt32(reader["IdTipoUsuario"]);

                            if (reader["IdEmpleado"] != DBNull.Value)
                                usuario.IdEmpleado = Convert.ToInt32(reader["IdEmpleado"]);
                            if (reader["IdGerente"] != DBNull.Value)
                                usuario.IdGerente = Convert.ToInt32(reader["IdGerente"]);

                            return usuario;
                        }
                    }
                }
            }

            return null;
        }

        //Validar que el correo del usuario/Empleado - Nombre Usuario
        public bool ValidarCorreoUsuario(string correo)
        {
            try
            {
                using (SQLiteConnection connection = ConexionDB.Conectar())
                {
                    string sql = @"
                        SELECT COUNT(*)
                        FROM Empleado E
                        LEFT JOIN Usuario U ON E.IdEmpleado = U.IdEmpleado AND U.EliminadoUsuario = 0
                        WHERE E.CorreoEmpleado = @Correo
                          AND U.IdUsuario IS NULL;";

                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.Add("@Correo", DbType.String).Value = correo;
                        int cantidad = Convert.ToInt32(command.ExecuteScalar());
                        return cantidad > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        //Validar que el correo del usuario/Gerente - Nombre Usuario
        public bool ValidarCorreoGerente(string correo)
        {
            try
            {
                using (SQLiteConnection connection = ConexionDB.Conectar())
                {
                    string sql = @"
                        SELECT COUNT(*)
                        FROM Gerente G
                        LEFT JOIN Usuario U ON G.IdGerente = U.IdGerente AND U.EliminadoUsuario = 0
                        WHERE G.CorreoGerente = @Correo
                          AND U.IdUsuario IS NULL;";

                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.Add("@Correo", DbType.String).Value = correo;
                        int cantidad = Convert.ToInt32(command.ExecuteScalar());
                        return cantidad > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static DataTable tipoUsuario()
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string querycargar = "SELECT IdTipoUsuario, NombreTipoUsuario FROM TipoUsuario;";
                SQLiteDataAdapter dt = new SQLiteDataAdapter(querycargar, connection);
                DataTable tabla = new DataTable();
                dt.Fill(tabla);
                return tabla;
            }

        }

        //Paginas
        public static DataTable MostrarUsuariosPagina(int pagina, int registrosPorPagina, out int totalRegistros)
        {
            DataTable dt = new DataTable();
            totalRegistros = 0;

            using (SQLiteConnection cn = ConexionDB.Conectar())
            {
                using (SQLiteCommand cmdTotal = new SQLiteCommand(
                    "SELECT COUNT(*) FROM Usuario WHERE EliminadoUsuario = 0;", cn))
                {
                    totalRegistros = Convert.ToInt32(cmdTotal.ExecuteScalar());
                }

                int offset = (pagina - 1) * registrosPorPagina;

                string sql = @"
                    SELECT 
                        U.IdUsuario,
                        TU.NombreTipoUsuario AS Rol,
                        U.NombreUsuario,
                        IFNULL(G.NombreGerente || ' ' || G.ApellidoGerente,
                               E.NombreEmpleado || ' ' || E.ApellidoEmpleado) AS NombreCompleto,
                        IFNULL(G.CorreoGerente, E.CorreoEmpleado) AS Correo
                    FROM Usuario U
                    INNER JOIN TipoUsuario TU ON U.IdTipoUsuario = TU.IdTipoUsuario
                    LEFT JOIN Gerente  G ON U.IdGerente  = G.IdGerente
                    LEFT JOIN Empleado E ON U.IdEmpleado = E.IdEmpleado
                    WHERE U.EliminadoUsuario = 0
                    ORDER BY U.IdUsuario
                    LIMIT @limit OFFSET @offset;";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
                {
                    cmd.Parameters.Add("@limit", DbType.Int32).Value = registrosPorPagina;
                    cmd.Parameters.Add("@offset", DbType.Int32).Value = offset;

                    using (SQLiteDataAdapter ad = new SQLiteDataAdapter(cmd))
                    {
                        ad.Fill(dt);
                    }
                }
            }

            return dt;
        }

        //Elimianr Usuario
        public static int EliminarUsuario(int idUsuario)
        {
            using (SQLiteConnection cn = ConexionDB.Conectar())
            {
                if (idUsuario == 1)
                    return 0;   

                using (SQLiteCommand cmdCheck = new SQLiteCommand(
                    "SELECT COUNT(*) FROM Usuario WHERE IdUsuario = @Id AND EliminadoUsuario = 0;", cn))
                {
                    cmdCheck.Parameters.Add("@Id", DbType.Int32).Value = idUsuario;
                    if (Convert.ToInt32(cmdCheck.ExecuteScalar()) == 0)
                        return -1;   
                }

                using (SQLiteCommand cmdDel = new SQLiteCommand(
                    "UPDATE Usuario SET EliminadoUsuario = 1 WHERE IdUsuario = @Id;", cn))
                {
                    cmdDel.Parameters.Add("@Id", DbType.Int32).Value = idUsuario;
                    cmdDel.ExecuteNonQuery();
                    return 1;   
                }
            }
        }

        //Restaurar Usuario
        public static bool RestaurarUsuario(int idUsuario)
        {
            try
            {
                using (SQLiteConnection cn = ConexionDB.Conectar())
                using (SQLiteCommand cmd = new SQLiteCommand("RestaurarUsuario", cn))
                {
                    cmd.Parameters.Add("@IdUsuario", DbType.Int32).Value = idUsuario;

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (SQLiteException)
            {
                return false;
            }
        }

        //Verifica si el correo existe y guarda codigo
        public bool GuardarCodigoRecuperacion(string NombreUsuario, string codigo)
        {
            try
            {
                using (SQLiteConnection connection = ConexionDB.Conectar())
                {
                    using (SQLiteCommand cmdCheck = new SQLiteCommand(
                        "SELECT COUNT(*) FROM Usuario WHERE NombreUsuario = @Usuario AND EliminadoUsuario = 0;",
                        connection))
                    {
                        cmdCheck.Parameters.Add("@Usuario", DbType.String).Value = NombreUsuario;
                        if (Convert.ToInt32(cmdCheck.ExecuteScalar()) == 0)
                            return false;
                    }

                    string sql = @"
                        UPDATE Usuario
                        SET CodigoRecuperacion = @Codigo,
                            ExpiracionCodigo   = datetime('now','localtime', '+15 minutes')
                        WHERE NombreUsuario = @Usuario
                          AND EliminadoUsuario = 0;";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                    {
                        cmd.Parameters.Add("@Usuario", DbType.String).Value = NombreUsuario;
                        cmd.Parameters.Add("@Codigo", DbType.String).Value = codigo;
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar código: " + ex.Message);
                return false;
            }
        }

        public bool ValidarCodigoRecuperacion(string NombreUsuario, string codigo)
        {
            try
            {
                using (SQLiteConnection connection = ConexionDB.Conectar())
                {
                    string sql = @"
                        SELECT COUNT(*)
                        FROM Usuario
                        WHERE NombreUsuario = @Usuario
                          AND CodigoRecuperacion = @Codigo
                          AND ExpiracionCodigo >= datetime('now','localtime')
                          AND EliminadoUsuario = 0;";

                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.Add("@Usuario", DbType.String).Value = NombreUsuario;
                        command.Parameters.Add("@Codigo", DbType.String).Value = codigo;

                        int cantidad = Convert.ToInt32(command.ExecuteScalar());
                        return cantidad > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public bool ActualizarContrasenaRecuperacion(string NombreUsuario, string nuevaContrasena)
        {
            try
            {
                string hash = BCrypt.Net.BCrypt.HashPassword(nuevaContrasena);

                using (SQLiteConnection connection = ConexionDB.Conectar())
                {
                    string sql = @"
                        UPDATE Usuario
                        SET ContrasenaUsuario  = @NuevaContrasena,
                            CodigoRecuperacion = NULL,
                            ExpiracionCodigo   = NULL
                        WHERE NombreUsuario = @Usuario
                          AND EliminadoUsuario = 0;";

                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.Add("@Usuario", DbType.String).Value = NombreUsuario;
                        command.Parameters.Add("@NuevaContrasena", DbType.String).Value = hash;

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar contraseña: " + ex.Message);
                return false;
            }
        }
    }
}
