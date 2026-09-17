using Modelos.Conexion_DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        public int IdUsuario { get => idUsuario; set => idUsuario = value; }
        public string NombreUsuario { get => nombreUsuario; set => nombreUsuario = value; }
        public string ContrasenaUsuario { get => contrasenaUsuario; set => contrasenaUsuario = value; }
        public int IdTipoUsuario { get => idTipoUsuario; set => idTipoUsuario = value; }
        public int? IdGerente { get => idGerente; set => idGerente = value; }
        public int? IdEmpleado { get => idEmpleado; set => idEmpleado = value; }
        public string NombreGerente { get => nombreGerente; set => nombreGerente = value; }
        public string ApellidoGerente { get => apellidoGerente; set => apellidoGerente = value; }
        public string CorreoGerente { get => correoGerente; set => correoGerente = value; }


        //Crear cuenta Gerente
        public bool RegistrarGerente()
        {
            try
            {
                using (SqlConnection connection = ConexionDB.Conectar())
                {
                    SqlCommand command = new SqlCommand("RegistarGerente", connection);
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("@NombreGerente", SqlDbType.VarChar).Value = nombreGerente;
                        command.Parameters.Add("@ApellidoGerente", SqlDbType.VarChar).Value = apellidoGerente;
                        command.Parameters.Add("@CorreoGerente", SqlDbType.VarChar).Value = correoGerente;
                        command.Parameters.Add("@NombreUsuario", SqlDbType.VarChar).Value = nombreUsuario;
                        command.Parameters.Add("@ContrasenaUsuario", SqlDbType.VarChar).Value = contrasenaUsuario;

                        SqlParameter GerenteId = new SqlParameter("@IdGerente", SqlDbType.Int);
                        GerenteId.Direction = ParameterDirection.Output;
                        command.Parameters.Add(GerenteId);

                        SqlParameter UsuarioId = new SqlParameter("@IdUsuario", SqlDbType.Int);
                        UsuarioId.Direction = ParameterDirection.Output;
                        command.Parameters.Add(UsuarioId);

                        command.ExecuteNonQuery();

                        IdGerente = Convert.ToInt32(GerenteId.Value);
                        IdUsuario = Convert.ToInt32(UsuarioId.Value);

                        connection.Close();
                        return true;
                    }
                }
            }
            catch (SqlException ex)
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
                using (SqlConnection connection = ConexionDB.Conectar())
                {
                    using (SqlCommand command = new SqlCommand("RegistrarEmpleado", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("@NombreUsuario", SqlDbType.VarChar).Value = nombreUsuario;
                        command.Parameters.Add("@ContrasenaUsuario", SqlDbType.VarChar).Value = contrasenaUsuario;

                        object resultado = command.ExecuteScalar();
                        return resultado != null ? Convert.ToInt32(resultado) : 0;
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


                using (SqlConnection connection = ConexionDB.Conectar())
                using (SqlCommand command = new SqlCommand("VerificarLoginGerente", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@Correo", SqlDbType.VarChar).Value = nombreUsuario;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string hash = reader["ContrasenaUsuario"].ToString();

                            bool contrasenaCorrecta = BCrypt.Net.BCrypt.Verify(contrasenaUsuario, hash);

                            if (!contrasenaCorrecta)
                            {
                                return null;
                            }

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
            using (SqlConnection connection = ConexionDB.Conectar())
            using (SqlCommand command = new SqlCommand("VerificarLoginEmpleado", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@Correo", SqlDbType.VarChar).Value = nombreUsuario;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string hash = reader["ContrasenaUsuario"].ToString();

                        bool contrasenaCorrecta = BCrypt.Net.BCrypt.Verify(contrasenaUsuario, hash);

                        if (!contrasenaCorrecta)
                        {
                            return null;
                        }

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

            return null;
        }

        //Validar que el correo del usuario/Empleado - Nombre Usuario
        public bool ValidarCorreoUsuario(string correo)
        {
            try
            {
                using (SqlConnection connection = ConexionDB.Conectar())
                using (SqlCommand command = new SqlCommand("CorreoVerificarEmpleado", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@Correo", SqlDbType.VarChar).Value = correo;
                    int cantidad = Convert.ToInt32(command.ExecuteScalar());
                    return cantidad > 0;
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
                using (SqlConnection connection = ConexionDB.Conectar())
                using (SqlCommand command = new SqlCommand("CorreoVerificarGerente", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Correo", SqlDbType.VarChar).Value = correo;
                    int cantidad = Convert.ToInt32(command.ExecuteScalar());
                    return cantidad > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public static DataTable tipoUsuario()
        {
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                string querycargar = "select IdTipoUsuario,NombreTipoUsuario from TipoUsuario;";
                SqlDataAdapter dt = new SqlDataAdapter(querycargar, connection);
                DataTable tabla = new DataTable();
                dt.Fill(tabla);
                return tabla;
            }

        }
    }
}
