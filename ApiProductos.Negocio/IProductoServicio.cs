using ApiProductos.Dto;

namespace ApiProductos.Negocio
{
    public interface IProductoServicio
    {
        Task<List<ProductoDto>> ListarTodosAsync();

        Task<List<ProductoDto>> ListarPaginadoAsync(
            int pagina,
            int tamanioPagina);

        Task<List<ProductoDto>> ListarPorCategoriaAsync(
            int idCategoria);

        Task<int> ContarActivosAsync();

        Task<ProductoDto?> ObtenerPorIdAsync(
            int idProducto);

        Task<ProductoDto> CrearAsync(
            ProductoCrearDto dto);

        Task ActualizarAsync(
            ProductoActualizarDto dto);

        Task EliminarAsync(
            int idProducto);
    }
}