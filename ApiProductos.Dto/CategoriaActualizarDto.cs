using System.ComponentModel.DataAnnotations;

namespace ApiProductos.Dto
{
    public class CategoriaActualizarDto
    {
        [Required]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }
}