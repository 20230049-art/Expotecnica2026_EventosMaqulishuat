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
    public class Productos
    {
        private int idProducto;
        private string nombreProducto;
        private int cantidadProducto;
        private decimal precioCostoProducto;
        private decimal precioAlquilerProducto;
        private decimal precioPerdidaProducto;
        private int idServicio;
        private int idProveedor;
        private string imagenProducto;

        public int IdProducto { get => idProducto; set => idProducto = value; }
        public string NombreProducto { get => nombreProducto; set => nombreProducto = value; }
        public int CantidadProducto { get => cantidadProducto; set => cantidadProducto = value; }
        public decimal PrecioCostoProducto { get => precioCostoProducto; set => precioCostoProducto = value; }
        public decimal PrecioAlquilerProducto { get => precioAlquilerProducto; set => precioAlquilerProducto = value; }
        public decimal PrecioPerdidaProducto { get => precioPerdidaProducto; set => precioPerdidaProducto = value; }
        public int IdServicio { get => idServicio; set => idServicio = value; }
        public int IdProveedor { get => idProveedor; set => idProveedor = value; }
        public string ImagenProducto { get => imagenProducto; set => imagenProducto = value; }


        //Mostrar Datos en el DataGridView - Par la vista Gerente
        public static DataTable MostrarProducto()
        {
            SqlConnection conection = ConexionDB.Conectar();
            string comando = "SELECT * FROM VistaProductosGerente;";
            SqlDataAdapter ad = new SqlDataAdapter(comando, conection);
            DataTable dt = new DataTable();
            ad.Fill(dt);

            return dt;
        }

        //Mostrar productos en la vista empleaods
        public static DataTable MostrarProductosEmpleado()
        {
            SqlConnection conection = ConexionDB.Conectar();
            string comando = "SELECT * FROM VistaEmpleadoProductos;";
            SqlDataAdapter ad = new SqlDataAdapter(comando, conection);
            DataTable dt = new DataTable();
            ad.Fill(dt);

            return dt;
        }

        //Mostrar productos en la vista empleados-Servicios
        public static DataTable VistaEmpleadoServicios(int idServicio)
        {
            SqlConnection conection = ConexionDB.Conectar();
            string comando = "SELECT * FROM VistaEmpleadoServicios WHERE IdServicio = @idServicio;";

            SqlCommand cmd = new SqlCommand(comando, conection);
            cmd.Parameters.AddWithValue("@idServicio", idServicio);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            ad.Fill(dt);

            conection.Close();

            return dt;
        }

        //Agregar Productos a la base de Datos
        public static void AgregarProducto(Productos nuevoProducto)
        {
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (SqlCommand command = new SqlCommand("IngresarProducto", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@NombreProducto", SqlDbType.VarChar).Value = nuevoProducto.NombreProducto;
                    command.Parameters.Add("@CantidadProducto", SqlDbType.Int).Value = nuevoProducto.CantidadProducto;
                    command.Parameters.Add("@PrecioCostoProducto", SqlDbType.Decimal).Value = nuevoProducto.PrecioCostoProducto;
                    command.Parameters.Add("@PrecioAlquilerProducto", SqlDbType.Decimal).Value = nuevoProducto.PrecioAlquilerProducto;
                    command.Parameters.Add("@PrecioPerdidaProducto", SqlDbType.Decimal).Value = nuevoProducto.PrecioPerdidaProducto;
                    command.Parameters.Add("@IdServicio", SqlDbType.Int).Value = nuevoProducto.IdServicio;
                    command.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = nuevoProducto.IdProveedor;
                    command.Parameters.Add("@ImagenProducto", SqlDbType.VarChar).Value = nuevoProducto.ImagenProducto;

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        //Filtro de Producto por cantidad :( 22:29 PM
        public List<Productos> ObtenerProductosVenta()
        {
            var productos = new List<Productos>();
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                SqlCommand commandd = new SqlCommand("ObtenerProductosDisponibles", connection);
                commandd.CommandType = CommandType.StoredProcedure;
                using (var reader = commandd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productos.Add(new Productos
                        {
                            IdProducto = (int)reader["IdProducto"],
                            NombreProducto = reader["NombreProducto"].ToString(),
                            CantidadProducto = (int)reader["CantidadProducto"],
                            PrecioCostoProducto = (decimal)reader["PrecioCostoProducto"],
                            PrecioAlquilerProducto = (decimal)reader["PrecioAlquilerProducto"],
                            PrecioPerdidaProducto = (decimal)reader["PrecioPerdidaProducto"]
                        });
                    }
                }

                return productos;
            }
        }

        //Agregar Producto a la venta
        public void AgregarProductoVenta(int idVenta, int producto, int cantidad, decimal descuento = 0)
        {
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                SqlCommand commandd = new SqlCommand("AgregarProductosVenta", connection);
                commandd.CommandType = CommandType.StoredProcedure;

                commandd.Parameters.Add("@IdVenta", SqlDbType.Int).Value = idVenta;
                commandd.Parameters.Add("@IdProducto", SqlDbType.Int).Value = producto;
                commandd.Parameters.Add("@Cantidad", SqlDbType.Int).Value = cantidad;
                commandd.Parameters.Add("@Descuento", SqlDbType.Decimal).Value = descuento;

                commandd.ExecuteNonQuery();
            }
        }

        //Buscar Producto por Id para ventas
        public static DataTable ObtenerProductoId(int idProducto)
        {
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (SqlCommand command = new SqlCommand("ProductoId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@IdProducto", SqlDbType.Int).Value = idProducto;
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    using (SqlDataAdapter ad = new SqlDataAdapter(command))
                    {
                        ad.Fill(dt);
                    }

                    return dt;
                }
            }
        }

        //Buscar Producto para el Empleado
        public DataTable BuscarProductoEmpleado(string busqueda)
        {

            DataTable tablaCliente = new DataTable();
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (var command = new SqlCommand("BuscarProductoEmpleado", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@busqueda", SqlDbType.VarChar).Value = busqueda ?? (object)DBNull.Value;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(tablaCliente);
                    }
                }
            }
            return tablaCliente;

        }

        //Actualizar Producto
        public static void ActualizarProducto(Productos actualizarproducto)
        {
            using (SqlConnection connection = ConexionDB.Conectar())
            {
                using (SqlCommand command = new SqlCommand("ActualizarProducto", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@IdProducto", SqlDbType.Int).Value = actualizarproducto.IdProducto;
                    command.Parameters.Add("@NombreProducto", SqlDbType.VarChar).Value = actualizarproducto.NombreProducto;
                    command.Parameters.Add("@CantidadProducto", SqlDbType.Int).Value = actualizarproducto.CantidadProducto;
                    command.Parameters.Add("@PrecioCostoProducto", SqlDbType.Int).Value = actualizarproducto.PrecioCostoProducto;
                    command.Parameters.Add("@PrecioALquilerProducto", SqlDbType.Int).Value = actualizarproducto.PrecioAlquilerProducto;
                    command.Parameters.Add("@PrecioPerdidaProducto", SqlDbType.Int).Value = actualizarproducto.PrecioPerdidaProducto;
                    command.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = actualizarproducto.IdProveedor;
                    command.Parameters.Add("@IdServicio", SqlDbType.Int).Value = actualizarproducto.IdServicio;
                    command.Parameters.Add("@ImagenProducto", SqlDbType.VarChar).Value = actualizarproducto.ImagenProducto ?? "";

                    command.ExecuteNonQuery();
                }
            }
        }


        //Buscar productos barra busqueda en servicios
        public static DataTable BuscarProductosPorServicio(int idServicio, string busqueda)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conexion = ConexionDB.Conectar())
                {
                    using (SqlCommand cmd = new SqlCommand("BuscarProductoServicio", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdServicio", idServicio);
                        cmd.Parameters.AddWithValue("@Busqueda", busqueda);

                        using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                        {
                            ad.Fill(dt);
                        }
                    }
                } 
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ocurrió un error en la base de datos: " + ex.Message,
                                "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error inesperado: " + ex.Message,
                                "ERROR-EXCEPCION-102", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dt;
        }
    }
}
