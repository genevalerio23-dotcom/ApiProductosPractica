namespace ApiProductos.Negocio.Excepciones
{
    public class ReglaDeNegocioException : Exception
    {
        public ReglaDeNegocioException(string mensaje) : base(mensaje)
        {
        }
    }
}
