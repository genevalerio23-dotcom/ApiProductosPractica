namespace ApiProductos.Negocio.Excepciones
{
    public class ProductoNoEncontradoException : Exception
    {
        public ProductoNoEncontradoException(int idProducto)
            : base($"No se encontró el producto con Id {idProducto}.")
        {
        }
    }
}