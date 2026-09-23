namespace ApiProductos.Dto
{
    public class RespuestaApi<T>
    {
        public bool Exito { get; set; }
        public string? Mensaje { get; set; }
        public T? Datos { get; set; }

        public static RespuestaApi<T> Ok(T datos, string? mensaje = null) =>
            new()
            {
                Exito = true,
                Datos = datos,
                Mensaje = mensaje
            };

        public static RespuestaApi<T> Error(string mensaje) =>
            new()
            {
                Exito = false,
                Mensaje = mensaje,
                Datos = default
            };
    }
}
