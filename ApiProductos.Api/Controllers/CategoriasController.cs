using ApiProductos.Dto;
using ApiProductos.Negocio;
using Microsoft.AspNetCore.Mvc;

namespace ApiProductos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaServicio _servicio;

        public CategoriasController(ICategoriaServicio servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        public async Task<ActionResult<RespuestaApi<List<CategoriaDto>>>> ListarTodos()
        {
            var categorias =
                await _servicio.ListarTodosAsync();

            return Ok(
                RespuestaApi<List<CategoriaDto>>.Ok(categorias));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RespuestaApi<CategoriaDto>>> ObtenerPorId(int id)
        {
            var categoria =
                await _servicio.ObtenerPorIdAsync(id);

            if (categoria == null)
            {
                return NotFound(
                    RespuestaApi<CategoriaDto>.Error(
                        $"No existe una categoría con Id {id}."));
            }

            return Ok(
                RespuestaApi<CategoriaDto>.Ok(categoria));
        }

        [HttpPost]
        public async Task<ActionResult<RespuestaApi<CategoriaDto>>> Crear(
            [FromBody] CategoriaCrearDto dto)
        {
            var creada =
                await _servicio.CrearAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = creada.IdCategoria },
                RespuestaApi<CategoriaDto>.Ok(
                    creada,
                    "Categoría creada correctamente."));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(
            int id,
            [FromBody] CategoriaActualizarDto dto)
        {
            if (id != dto.IdCategoria)
            {
                return BadRequest(
                    RespuestaApi<object>.Error(
                        "El Id de la ruta no coincide con el del cuerpo."));
            }

            await _servicio.ActualizarAsync(dto);

            return Ok(
                RespuestaApi<object>.Ok(
                    null!,
                    "Categoría actualizada correctamente."));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _servicio.EliminarAsync(id);

            return Ok(
                RespuestaApi<object>.Ok(
                    null!,
                    "Categoría eliminada correctamente."));
        }
    }
}