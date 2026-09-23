namespace ApiProductos.Dto
{
    public class ProductoDto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int IdCategoria { get; set; }
        public string? NombreCategoria { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
