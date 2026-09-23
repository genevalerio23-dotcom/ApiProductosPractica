using ApiProductos.Datos.Conexion;
using ApiProductos.Entidades;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ApiProductos.Datos.Repositorios
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly IConexion _conexion;

        public CategoriaRepositorio(IConexion conexion)
        {
            _conexion = conexion;
        }

        public async Task<List<Categoria>> ListarTodosAsync()
        {
            var lista = new List<Categoria>();

            using (SqlConnection conn = _conexion.CrearConexion())
            using (SqlCommand cmd = new SqlCommand("sp_Categorias_ListarTodos", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                await conn.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(MapearCategoria(reader));
                    }
                }
            }

            return lista;
        }

        public async Task<Categoria?> ObtenerPorIdAsync(int idCategoria)
        {
            Categoria? categoria = null;

            using (SqlConnection conn = _conexion.CrearConexion())
            using (SqlCommand cmd = new SqlCommand("sp_Categorias_ObtenerPorId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    new SqlParameter("@IdCategoria", SqlDbType.Int)
                    {
                        Value = idCategoria
                    });

                await conn.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        categoria = MapearCategoria(reader);
                    }
                }
            }

            return categoria;
        }

        public async Task<int> InsertarAsync(Categoria categoria)
        {
            using (SqlConnection conn = _conexion.CrearConexion())
            using (SqlCommand cmd = new SqlCommand("sp_Categorias_Insertar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    new SqlParameter("@Nombre", SqlDbType.VarChar, 100)
                    {
                        Value = categoria.Nombre
                    });

                var parametroSalida =
                    new SqlParameter("@IdCategoriaNueva", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                cmd.Parameters.Add(parametroSalida);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return (int)parametroSalida.Value;
            }
        }

        public async Task<bool> ActualizarAsync(Categoria categoria)
        {
            using (SqlConnection conn = _conexion.CrearConexion())
            using (SqlCommand cmd = new SqlCommand("sp_Categorias_Actualizar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    new SqlParameter("@IdCategoria", SqlDbType.Int)
                    {
                        Value = categoria.IdCategoria
                    });

                cmd.Parameters.Add(
                    new SqlParameter("@Nombre", SqlDbType.VarChar, 100)
                    {
                        Value = categoria.Nombre
                    });

                await conn.OpenAsync();

                int filasAfectadas = await cmd.ExecuteNonQueryAsync();

                return filasAfectadas > 0;
            }
        }

        public async Task<bool> EliminarAsync(int idCategoria)
        {
            using (SqlConnection conn = _conexion.CrearConexion())
            using (SqlCommand cmd = new SqlCommand("sp_Categorias_Eliminar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    new SqlParameter("@IdCategoria", SqlDbType.Int)
                    {
                        Value = idCategoria
                    });

                await conn.OpenAsync();

                int filasAfectadas = await cmd.ExecuteNonQueryAsync();

                return filasAfectadas > 0;
            }
        }

        private static Categoria MapearCategoria(SqlDataReader reader)
        {
            return new Categoria
            {
                IdCategoria = reader.GetInt32(
                    reader.GetOrdinal("IdCategoria")),

                Nombre = reader.GetString(
                    reader.GetOrdinal("Nombre")),

                Activo = reader.GetBoolean(
                    reader.GetOrdinal("Activo"))
            };
        }
    }
}