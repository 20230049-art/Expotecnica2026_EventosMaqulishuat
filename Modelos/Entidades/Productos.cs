using Modelos.Conexion_DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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


        //Mostrar productos en la vista empleaods
        public static DataTable MostrarProductosEmpleado()
        {
            SQLiteConnection conection = ConexionDB.Conectar();
            string comando = "SELECT * FROM VistaEmpleadoProductos WHERE EliminadoProducto = 0;";
            SQLiteDataAdapter ad = new SQLiteDataAdapter(comando, conection);
            DataTable dt = new DataTable();
            ad.Fill(dt);

            return dt;
        }

        //Mostrar productos en la vista empleados-Servicios
        public static DataTable VistaEmpleadoServicios(int idServicio)
        {
            using (SQLiteConnection conection = ConexionDB.Conectar())
            {
                string comando = @" SELECT * FROM VistaEmpleadoServicios 
                WHERE IdServicio = @idServicio
                 AND IdProducto NOT IN (SELECT IdProducto FROM Producto WHERE EliminadoProducto = 1);";

                using (SQLiteCommand cmd = new SQLiteCommand(comando, conection))
                {
                    cmd.Parameters.AddWithValue("@idServicio", idServicio);

                    SQLiteDataAdapter ad = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    return dt;
                }
            }
        }

        //Agregar Productos a la base de Datos
        public static void AgregarProducto(Productos nuevoProducto)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql = @"
                    INSERT INTO Producto
                        (NombreProducto, CantidadProducto, PrecioCostoProducto,
                         PrecioALquilerProducto, PrecioPerdidaProducto,
                         IdServicio, IdProveedor, ImagenProducto)
                    VALUES
                        (@NombreProducto, @CantidadProducto, @PrecioCostoProducto,
                         @PrecioAlquilerProducto, @PrecioPerdidaProducto,
                         @IdServicio, @IdProveedor, @ImagenProducto);";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {

                    command.Parameters.Add("@NombreProducto", DbType.String).Value = nuevoProducto.NombreProducto;
                    command.Parameters.Add("@CantidadProducto", DbType.Int32).Value = nuevoProducto.CantidadProducto;
                    command.Parameters.Add("@PrecioCostoProducto", DbType.Double).Value = (double)nuevoProducto.PrecioCostoProducto;
                    command.Parameters.Add("@PrecioAlquilerProducto", DbType.Double).Value = nuevoProducto.PrecioAlquilerProducto;
                    command.Parameters.Add("@PrecioPerdidaProducto", DbType.Double).Value = nuevoProducto.PrecioPerdidaProducto;
                    command.Parameters.Add("@IdServicio", DbType.Int32).Value = nuevoProducto.IdServicio;
                    command.Parameters.Add("@IdProveedor", DbType.Int32).Value = nuevoProducto.IdProveedor;
                    command.Parameters.Add("@ImagenProducto", DbType.String).Value = nuevoProducto.ImagenProducto;

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        //Filtro de Producto por cantidad :( 22:29 PM
        public List<Productos> ObtenerProductosVenta()
        {
            var productos = new List<Productos>();
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql = "SELECT * FROM Producto WHERE CantidadProducto > 0;";

                using (SQLiteCommand commandd = new SQLiteCommand(sql, connection))
                using (var reader = commandd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productos.Add(new Productos
                        {
                            IdProducto = (int)reader["IdProducto"],
                            NombreProducto = reader["NombreProducto"].ToString(),
                            CantidadProducto = Convert.ToInt32(reader["CantidadProducto"]),
                            PrecioCostoProducto = Convert.ToDecimal(reader["PrecioCostoProducto"]),
                            PrecioAlquilerProducto = Convert.ToDecimal(reader["PrecioALquilerProducto"]),
                            PrecioPerdidaProducto = Convert.ToDecimal(reader["PrecioPerdidaProducto"])
                        });
                    }
                }

                return productos;
            }
        }

        //Agregar Producto a la venta
        public void AgregarProductoVenta(int idVenta, int producto, int cantidad, decimal descuento = 0)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            using (SQLiteTransaction tx = connection.BeginTransaction())
            {
                try
                {

                    decimal precioAlquiler;
                    int inventarioActual;
                    using (SQLiteCommand cmdSel = new SQLiteCommand(
                        "SELECT PrecioALquilerProducto, CantidadProducto FROM Producto WHERE IdProducto = @Id;",
                        connection, tx))
                    {
                        cmdSel.Parameters.Add("@Id", DbType.Int32).Value = producto;
                        using (var reader = cmdSel.ExecuteReader())
                        {
                            if (!reader.Read())
                                throw new Exception("Producto no encontrado.");

                            precioAlquiler = Convert.ToDecimal(reader["PrecioALquilerProducto"]);
                            inventarioActual = Convert.ToInt32(reader["CantidadProducto"]);
                        }
                    }

                    if (inventarioActual < cantidad)
                        throw new Exception("Producto insuficiente.");

                    decimal totalDetalle = (precioAlquiler * cantidad) - descuento;
                    if (totalDetalle < 0)
                        throw new Exception("El descuento no puede ser mayor al subtotal.");

                    using (SQLiteCommand cmdIns = new SQLiteCommand(@"
                        INSERT INTO DetalleVenta
                            (IdVenta, IdProducto, Cantidad, PrecioUnitario, Descuento, TotalDetalleventa)
                        VALUES
                            (@IdVenta, @IdProducto, @Cantidad, @PrecioUnitario, @Descuento, @TotalDetalle);",
                        connection, tx))
                    {
                        cmdIns.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;
                        cmdIns.Parameters.Add("@IdProducto", DbType.Int32).Value = producto;
                        cmdIns.Parameters.Add("@Cantidad", DbType.Int32).Value = cantidad;
                        cmdIns.Parameters.Add("@PrecioUnitario", DbType.Double).Value = precioAlquiler;
                        cmdIns.Parameters.Add("@Descuento", DbType.Double).Value = descuento;
                        cmdIns.Parameters.Add("@TotalDetalle", DbType.Double).Value = totalDetalle;
                        cmdIns.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmdUpd = new SQLiteCommand(
                        "UPDATE Producto SET CantidadProducto = CantidadProducto - @Cantidad WHERE IdProducto = @Id;",
                        connection, tx))
                    {
                        cmdUpd.Parameters.Add("@Cantidad", DbType.Int32).Value = cantidad;
                        cmdUpd.Parameters.Add("@Id", DbType.Int32).Value = producto;
                        cmdUpd.ExecuteNonQuery();
                    }

                    tx.Commit();
                    ActualizarTotalesVenta(idVenta);
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        //Buscar Producto por Id para ventas
        public static DataTable ObtenerProductoId(int idProducto)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql = @"
                    SELECT IdProducto, NombreProducto, CantidadProducto,
                           PrecioCostoProducto, PrecioPerdidaProducto, PrecioALquilerProducto,
                           ImagenProducto, IdServicio, IdProveedor
                    FROM Producto
                    WHERE IdProducto = @Id;";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.Add("@Id", DbType.Int32).Value = idProducto;

                    DataTable dt = new DataTable();
                    using (SQLiteDataAdapter ad = new SQLiteDataAdapter(command))
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

            DataTable tabla = new DataTable();
            busqueda = (busqueda ?? "").Trim();

            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql;
                if (string.IsNullOrEmpty(busqueda))
                {
                    sql = @"SELECT * FROM VistaEmpleadoProductos 
                            WHERE IdProducto NOT IN (SELECT IdProducto FROM Producto WHERE EliminadoProducto = 1);";
                }
                else
                {
                    sql = @"SELECT * FROM VistaEmpleadoProductos
                            WHERE NombreProducto LIKE @q
                              AND IdProducto NOT IN (SELECT IdProducto FROM Producto WHERE EliminadoProducto = 1);";
                }

                using (var command = new SQLiteCommand(sql, connection))
                {
                    if (!string.IsNullOrEmpty(busqueda))
                    {
                        command.Parameters.Add("@q", DbType.String).Value = "%" + busqueda + "%";
                    }

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(tabla);
                    }
                }
            }
            return tabla;

        }

        //Actualizar Producto
        public static void ActualizarProducto(Productos actualizarproducto)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                string sql = @"
                    UPDATE Producto
                    SET NombreProducto         = @NombreProducto,
                        CantidadProducto       = @CantidadProducto,
                        PrecioCostoProducto    = @PrecioCostoProducto,
                        PrecioALquilerProducto = @PrecioALquilerProducto,
                        PrecioPerdidaProducto  = @PrecioPerdidaProducto,
                        IdServicio             = @IdServicio,
                        IdProveedor            = @IdProveedor,
                        ImagenProducto         = @ImagenProducto
                    WHERE IdProducto = @IdProducto;";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.Add("@IdProducto", DbType.Int32).Value = actualizarproducto.IdProducto;
                    command.Parameters.Add("@NombreProducto", DbType.String).Value = actualizarproducto.NombreProducto;
                    command.Parameters.Add("@CantidadProducto", DbType.Int32).Value = actualizarproducto.CantidadProducto;
                    command.Parameters.Add("@PrecioCostoProducto", DbType.Double).Value = actualizarproducto.PrecioCostoProducto;
                    command.Parameters.Add("@PrecioALquilerProducto", DbType.Double).Value = actualizarproducto.PrecioAlquilerProducto;
                    command.Parameters.Add("@PrecioPerdidaProducto", DbType.Double).Value = actualizarproducto.PrecioPerdidaProducto;
                    command.Parameters.Add("@IdProveedor", DbType.Int32).Value = actualizarproducto.IdProveedor;
                    command.Parameters.Add("@IdServicio", DbType.Int32).Value = actualizarproducto.IdServicio;
                    command.Parameters.Add("@ImagenProducto", DbType.String).Value = actualizarproducto.ImagenProducto ?? "";

                    command.ExecuteNonQuery();
                }
            }
        }


        //Buscar productos barra busqueda en servicios
        public static DataTable BuscarProductosPorServicio(int idServicio, string busqueda)
        {
            DataTable dt = new DataTable();
            busqueda = (busqueda ?? "").Trim();

            try
            {
                using (SQLiteConnection conexion = ConexionDB.Conectar())
                {
                    string sql;
                    if (string.IsNullOrEmpty(busqueda))
                    {
                        sql = @"SELECT IdServicio, ImagenProducto, NombreProducto, CantidadProducto,
                                       PrecioALquilerProducto, NombreServicio
                                FROM VistaEmpleadoServicios
                                WHERE IdServicio = @IdServicio
                                  AND IdServicio NOT IN (SELECT IdServicio FROM Producto WHERE EliminadoProducto = 1);";
                    }
                    else
                    {
                        sql = @"SELECT IdServicio, ImagenProducto, NombreProducto, CantidadProducto,
                                       PrecioALquilerProducto, NombreServicio
                                FROM VistaEmpleadoServicios
                                WHERE IdServicio = @IdServicio
                                  AND NombreProducto LIKE @q
                                  AND IdServicio NOT IN (SELECT IdServicio FROM Producto WHERE EliminadoProducto = 1);";
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdServicio", idServicio);
                        if (!string.IsNullOrEmpty(busqueda))
                        {
                            cmd.Parameters.AddWithValue("@q", "%" + busqueda + "%");
                        }

                        using (SQLiteDataAdapter ad = new SQLiteDataAdapter(cmd))
                        {
                            ad.Fill(dt);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
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

        //MostrarProductos Pagina
        public static DataTable MostrarProductosPagina(int pagina, int registrosPorPagina, int tipoFiltro, out int totalRegistros)
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
                        case 2: // Productos Activos = en alguna venta con estado 1 (En Proceso) o 2 (Pendiente)
                            whereFiltro = @"WHERE P.EliminadoProducto = 0
                                    AND EXISTS (SELECT 1 FROM DetalleVenta DV
                                                INNER JOIN Venta V ON DV.IdVenta = V.IdVenta
                                                WHERE DV.IdProducto = P.IdProducto
                                                  AND V.IdEstadoVenta IN (1, 2))";
                            break;

                        case 3: // Productos No Activos = sin ventas activas
                            whereFiltro = "WHERE P.EliminadoProducto = 0 AND P.CantidadProducto = 0";
                            break;


                        default: // Todos = no eliminados
                            whereFiltro = "WHERE P.EliminadoProducto = 0";
                            break;
                    }

                    string sqlCount = "SELECT COUNT(*) FROM Producto P " + whereFiltro + ";";

                    using (SQLiteCommand cmdTotal = new SQLiteCommand(sqlCount, conexion))
                    {
                        totalRegistros = Convert.ToInt32(cmdTotal.ExecuteScalar());
                    }

                    int offset = (pagina - 1) * registrosPorPagina;

                    string sqlDatos = @"
                SELECT 
                    P.IdProducto,
                    P.NombreProducto,
                    P.CantidadProducto,
                    P.PrecioCostoProducto,
                    P.PrecioAlquilerProducto,
                    P.PrecioPerdidaProducto,
                    S.NombreServicio,
                    PR.NombreProveedor,
                    P.ImagenProducto
                FROM Producto P
                INNER JOIN Servicio  S  ON P.IdServicio  = S.IdServicio
                INNER JOIN Proveedor PR ON P.IdProveedor = PR.IdProveedor
                " + whereFiltro + @"
                ORDER BY P.IdProducto
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
                MessageBox.Show("Error al obtener los productos paginados: " + ex.Message,
                                "ERROR-SQL-100", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dt;
        }

        //Eliminar producto
        public static int EliminarProducto(int idProducto)
        {
            try
            {
                using (SQLiteConnection conexion = ConexionDB.Conectar())
                {
                    using (SQLiteCommand cmdCheck = new SQLiteCommand(
                        "SELECT COUNT(*) FROM Producto WHERE IdProducto = @Id AND EliminadoProducto = 0;", conexion))
                    {
                        cmdCheck.Parameters.Add("@Id", DbType.Int32).Value = idProducto;
                        if (Convert.ToInt32(cmdCheck.ExecuteScalar()) == 0)
                            return 0;
                    }

                    using (SQLiteCommand cmdCheck = new SQLiteCommand(@"
                        SELECT COUNT(*) FROM DetalleVenta DV
                        INNER JOIN Venta V ON DV.IdVenta = V.IdVenta
                        WHERE DV.IdProducto = @Id
                          AND V.IdEstadoVenta IN (1, 2);", conexion))
                    {
                        cmdCheck.Parameters.Add("@Id", DbType.Int32).Value = idProducto;
                        if (Convert.ToInt32(cmdCheck.ExecuteScalar()) > 0)
                            return -1;
                    }

                    using (SQLiteCommand cmdDel = new SQLiteCommand(
                        "UPDATE Producto SET EliminadoProducto = 1 WHERE IdProducto = @Id;", conexion))
                    {
                        cmdDel.Parameters.Add("@Id", DbType.Int32).Value = idProducto;
                        cmdDel.ExecuteNonQuery();
                        return 1;
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new Exception("Error al eliminar el producto: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inesperado al eliminar el producto: " + ex.Message, ex);
            }
        }

        //Restaurar producto
        public static bool RestaurarProducto(int idProducto)
        {
            try
            {
                using (SQLiteConnection conexion = ConexionDB.Conectar())
                {
                    string sql = "UPDATE Producto SET EliminadoProducto = 0 WHERE IdProducto = @Id;";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conexion))
                    {
                        cmd.Parameters.Add("@Id", DbType.Int32).Value = idProducto;
                        int filasAfectadas = cmd.ExecuteNonQuery();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new Exception("Error al restaurar el producto: " + ex.Message, ex);
            }
        }

        public static void ActualizarTotalesVenta(int idVenta)
        {
            using (SQLiteConnection connection = ConexionDB.Conectar())
            {
                // 1) Calcular subtotal y descuento sumando los detalles
                decimal subtotal = 0;
                decimal descuentoTotal = 0;

                using (SQLiteCommand cmdSum = new SQLiteCommand(
                    @"SELECT 
                IFNULL(SUM(TotalDetalleventa), 0) AS Subtotal,
                IFNULL(SUM(Descuento), 0)         AS DescuentoTotal
              FROM DetalleVenta
              WHERE IdVenta = @IdVenta;", connection))
                {
                    cmdSum.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;

                    using (var reader = cmdSum.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            subtotal = Convert.ToDecimal(reader["Subtotal"]);
                            descuentoTotal = Convert.ToDecimal(reader["DescuentoTotal"]);
                        }
                    }
                }

                // 2) Calcular IVA (13%) y total
                // El subtotal YA tiene los descuentos aplicados por línea
                decimal iva = subtotal * 0.13m;
                decimal total = subtotal + iva - descuentoTotal;

                // 3) Actualizar la venta
                using (SQLiteCommand cmdUpd = new SQLiteCommand(
                    @"UPDATE Venta
              SET SubtotalVenta   = @Subtotal,
                  IVA             = @IVA,
                  DescuentoVenta  = @Descuento,
                  TotalVenta      = @Total
              WHERE IdVenta = @IdVenta;", connection))
                {
                    cmdUpd.Parameters.Add("@IdVenta", DbType.Int32).Value = idVenta;
                    cmdUpd.Parameters.Add("@Subtotal", DbType.Double).Value = (double)subtotal;
                    cmdUpd.Parameters.Add("@IVA", DbType.Double).Value = (double)iva;
                    cmdUpd.Parameters.Add("@Descuento", DbType.Double).Value = (double)descuentoTotal;
                    cmdUpd.Parameters.Add("@Total", DbType.Double).Value = (double)total;

                    cmdUpd.ExecuteNonQuery();
                }
            }
        }
    }
}
