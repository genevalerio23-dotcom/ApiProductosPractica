namespace ApiProductos.Negocio.Excepciones
{
    public class CategoriaNoEncontradaException : Exception
    {
        public CategoriaNoEncontradaException(int idCategoria)
            : base($"No se encontró la categoría con Id {idCategoria}.")
        {
        }
    }
}