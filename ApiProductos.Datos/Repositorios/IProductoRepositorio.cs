using ApiProductos.Entidades;

namespace ApiProductos.Datos.Repositorios
{
    public interface IProductoRepositorio
    {
        Task<List<Producto>> ListarTodosAsync();
        Task<List<Producto>> ListarPaginadoAsync(int pagina, int tamanioPagina);
        Task<List<Producto>> ListarPorCategoriaAsync(int idCategoria);
        Task<int> ContarActivosAsync();
        Task<Producto?> ObtenerPorIdAsync(int idProducto);
        Task<int> InsertarAsync(Producto producto);
        Task<bool> ActualizarAsync(Producto producto);
        Task<bool> EliminarAsync(int idProducto);
    }
}
