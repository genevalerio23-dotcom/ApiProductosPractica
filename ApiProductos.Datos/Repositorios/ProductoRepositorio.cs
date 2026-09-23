using ApiProductos.Datos.Conexion;
using ApiProductos.Entidades;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace ApiProductos.Datos.Repositorios
{
    public class ProductoRepositorio : IProductoRepositorio
    {
        private readonly IConexion _conexion;
        private readonly ILogger<ProductoRepositorio> _logger;

        public ProductoRepositorio(
            IConexion conexion,
            ILogger<ProductoRepositorio> logger)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<List<Producto>> ListarTodosAsync()
        {
            var lista = new List<Producto>();

            using (SqlConnection conn = _conexion.CrearConexion())
            using (SqlCommand cmd =
                new SqlCommand("sp_Productos_ListarTodos", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                await conn.OpenAsync();

                using (SqlDataReader reader =
                    await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(MapearProducto(reader));
                    }
                }
            }

            return lista;
        }

        public async Task<List<Producto>> ListarPaginadoAsync(
            int pagina,
            int tamanioPagina)
        {
            var lista = new List<Producto>();

            using (SqlConnection conn = _conexion.CrearConexion())
            using (SqlCommand cmd =
                new SqlCommand("sp_Productos_ListarPaginado", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    new SqlParameter("@Pagina", SqlDbType.Int)
                    {
                        Value = pagina
                    });

                cmd.Parameters.Add(
                    new SqlParameter("@TamanioPagina", SqlDbType.Int)
                    {
                        Value = tamanioPagina
                    });

                await conn.OpenAsync();

                using (SqlDataReader reader =
                    await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(MapearProducto(reader));
                    }
                }
            }

            return lista;
        }

        public async Task<List<Producto>> ListarPorCategoriaAsync(
            int idCategoria)
        {
            var lista = new List<Producto>();

            using (SqlConnection conn = _conexion.CrearConexion())
            using (SqlCommand cmd =
                new SqlCommand("sp_Productos_ListarPorCategoria", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    new SqlParameter("@IdCategoria", SqlDbType.Int)
                    {
                        Value = idCategoria
                    });

                await conn.OpenAsync();

                using (SqlDataReader reader =
                    await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(MapearProducto(reader));
                    }
                }
            }

            return lista;
        }

        public async Task<int> ContarActivosAsync()
        {
            const string sql =
                "SELECT COUNT(*) FROM Productos WHERE Activo = 1;";

            using (SqlConnection conn = _conexion.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;

                await conn.OpenAsync();

                object? resultado =
                    await cmd.ExecuteScalarAsync();

                return Convert.ToInt32(resultado);
            }
        }

        public async Task<Producto?> ObtenerPorIdAsync(
            int idProducto)
        {
            Producto? producto = null;

            using (SqlConnection conn = _conexion.CrearConexion())
            using (SqlCommand cmd =
                new SqlCommand("sp_Productos_ObtenerPorId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    new SqlParameter("@IdProducto", SqlDbType.Int)
                    {
                        Value = idProducto
                    });

                await conn.OpenAsync();

                using (SqlDataReader reader =
                    await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        producto = MapearProducto(reader);
                    }
                }
            }

            return producto;
        }

        public async Task<int> InsertarAsync(Producto producto)
        {
            using (SqlConnection conn = _conexion.CrearConexion())
            using (SqlCommand cmd =
                new SqlCommand("sp_Productos_Insertar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    new SqlParameter("@Nombre", SqlDbType.VarChar, 150)
                    {
                        Value = producto.Nombre
                    });

                cmd.Parameters.Add(
                    new SqlParameter("@Descripcion", SqlDbType.VarChar, 500)
                    {
                        Value =
                            (object?)producto.Descripcion ?? DBNull.Value
                    });

                cmd.Parameters.Add(
                    new SqlParameter("@Precio", SqlDbType.Decimal)
                    {
                        Value = producto.Precio
                    });

                cmd.Parameters.Add(
                    new SqlParameter("@Stock", SqlDbType.Int)
                    {
                        Value = producto.Stock
                    });

                cmd.Parameters.Add(
                    new SqlParameter("@IdCategoria", SqlDbType.Int)
                    {
                        Value = producto.IdCategoria
                    });

                var paramSalida =
                    new SqlParameter(
                        "@IdProductoNuevo",
                        SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                cmd.Parameters.Add(paramSalida);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                int nuevoId =
                    Convert.ToInt32(paramSalida.Value);

                _logger.LogInformation(
                    "Producto creado con Id {IdProducto}",
                    nuevoId);

                return nuevoId;
            }
        }

        public async Task<bool> ActualizarAsync(
            Producto producto)
        {
            using (SqlConnection conn = _conexion.CrearConexion())
            using (SqlCommand cmd =
                new SqlCommand("sp_Productos_Actualizar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    new SqlParameter("@IdProducto", SqlDbType.Int)
                    {
                        Value = producto.IdProducto
                    });

                cmd.Parameters.Add(
                    new SqlParameter("@Nombre", SqlDbType.VarChar, 150)
                    {
                        Value = producto.Nombre
                    });

                cmd.Parameters.Add(
                    new SqlParameter("@Descripcion", SqlDbType.VarChar, 500)
                    {
                        Value =
                            (object?)producto.Descripcion ?? DBNull.Value
                    });

                cmd.Parameters.Add(
                    new SqlParameter("@Precio", SqlDbType.Decimal)
                    {
                        Value = producto.Precio
                    });

                cmd.Parameters.Add(
                    new SqlParameter("@Stock", SqlDbType.Int)
                    {
                        Value = producto.Stock
                    });

                cmd.Parameters.Add(
                    new SqlParameter("@IdCategoria", SqlDbType.Int)
                    {
                        Value = producto.IdCategoria
                    });

                await conn.OpenAsync();

                int filasAfectadas =
                    await cmd.ExecuteNonQueryAsync();

                if (filasAfectadas > 0)
                {
                    _logger.LogInformation(
                        "Producto actualizado con Id {IdProducto}",
                        producto.IdProducto);
                }

                return filasAfectadas > 0;
            }
        }

        public async Task<bool> EliminarAsync(
            int idProducto)
        {
            using (SqlConnection conn = _conexion.CrearConexion())
            using (SqlCommand cmd =
                new SqlCommand("sp_Productos_Eliminar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    new SqlParameter("@IdProducto", SqlDbType.Int)
                    {
                        Value = idProducto
                    });

                await conn.OpenAsync();

                int filasAfectadas =
                    await cmd.ExecuteNonQueryAsync();

                if (filasAfectadas > 0)
                {
                    _logger.LogInformation(
                        "Producto eliminado lógicamente con Id {IdProducto}",
                        idProducto);
                }

                return filasAfectadas > 0;
            }
        }

        private static Producto MapearProducto(
            SqlDataReader reader)
        {
            return new Producto
            {
                IdProducto =
                    reader.GetInt32(
                        reader.GetOrdinal("IdProducto")),

                Nombre =
                    reader.GetString(
                        reader.GetOrdinal("Nombre")),

                Descripcion =
                    reader.IsDBNull(
                        reader.GetOrdinal("Descripcion"))
                        ? null
                        : reader.GetString(
                            reader.GetOrdinal("Descripcion")),

                Precio =
                    reader.GetDecimal(
                        reader.GetOrdinal("Precio")),

                Stock =
                    reader.GetInt32(
                        reader.GetOrdinal("Stock")),

                IdCategoria =
                    reader.GetInt32(
                        reader.GetOrdinal("IdCategoria")),

                NombreCategoria =
                    reader.IsDBNull(
                        reader.GetOrdinal("NombreCategoria"))
                        ? null
                        : reader.GetString(
                            reader.GetOrdinal("NombreCategoria")),

                FechaCreacion =
                    reader.GetDateTime(
                        reader.GetOrdinal("FechaCreacion")),

                Activo =
                    reader.GetBoolean(
                        reader.GetOrdinal("Activo"))
            };
        }
    }
}
