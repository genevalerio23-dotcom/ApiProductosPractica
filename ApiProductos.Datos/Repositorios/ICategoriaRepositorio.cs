using ApiProductos.Entidades;

namespace ApiProductos.Datos.Repositorios
{
    public interface ICategoriaRepositorio
    {
        Task<List<Categoria>> ListarTodosAsync();
        Task<Categoria?> ObtenerPorIdAsync(int idCategoria);
        Task<int> InsertarAsync(Categoria categoria);
        Task<bool> ActualizarAsync(Categoria categoria);
        Task<bool> EliminarAsync(int idCategoria);
    }
}