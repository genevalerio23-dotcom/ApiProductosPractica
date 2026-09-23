using ApiProductos.Dto;

namespace ApiProductos.Negocio
{
    public interface ICategoriaServicio
    {
        Task<List<CategoriaDto>> ListarTodosAsync();
        Task<CategoriaDto?> ObtenerPorIdAsync(int idCategoria);
        Task<CategoriaDto> CrearAsync(CategoriaCrearDto dto);
        Task ActualizarAsync(CategoriaActualizarDto dto);
        Task EliminarAsync(int idCategoria);
    }
}