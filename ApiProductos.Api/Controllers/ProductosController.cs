using ApiProductos.Dto;
using ApiProductos.Negocio;
using Microsoft.AspNetCore.Mvc;

namespace ApiProductos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoServicio _servicio;

        public ProductosController(
            IProductoServicio servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        public async Task<
            ActionResult<RespuestaApi<List<ProductoDto>>>>
            ListarTodos(
                [FromQuery] int pagina = 1,
                [FromQuery] int tamanioPagina = 10)
        {
            var productos =
                await _servicio.ListarPaginadoAsync(
                    pagina,
                    tamanioPagina);

            return Ok(
                RespuestaApi<List<ProductoDto>>
                    .Ok(productos));
        }

        [HttpGet("categoria/{idCategoria:int}")]
        public async Task<
            ActionResult<RespuestaApi<List<ProductoDto>>>>
            ListarPorCategoria(int idCategoria)
        {
            var productos =
                await _servicio.ListarPorCategoriaAsync(
                    idCategoria);

            return Ok(
                RespuestaApi<List<ProductoDto>>
                    .Ok(productos));
        }

        [HttpGet("contador")]
        public async Task<
            ActionResult<RespuestaApi<int>>>
            ContarActivos()
        {
            int total =
                await _servicio.ContarActivosAsync();

            return Ok(
                RespuestaApi<int>.Ok(
                    total,
                    "Total de productos activos."));
        }

        [HttpGet("{id:int}")]
        public async Task<
            ActionResult<RespuestaApi<ProductoDto>>>
            ObtenerPorId(int id)
        {
            var producto =
                await _servicio.ObtenerPorIdAsync(id);

            if (producto == null)
            {
                return NotFound(
                    RespuestaApi<ProductoDto>.Error(
                        $"No existe un producto con Id {id}."));
            }

            return Ok(
                RespuestaApi<ProductoDto>.Ok(
                    producto));
        }

        [HttpPost]
        public async Task<
            ActionResult<RespuestaApi<ProductoDto>>>
            Crear(
                [FromBody] ProductoCrearDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var creado =
                await _servicio.CrearAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = creado.IdProducto },
                RespuestaApi<ProductoDto>.Ok(
                    creado,
                    "Producto creado correctamente."));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(
            int id,
            [FromBody] ProductoActualizarDto dto)
        {
            if (id != dto.IdProducto)
            {
                return BadRequest(
                    RespuestaApi<object>.Error(
                        "El Id de la ruta no coincide con el del cuerpo."));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _servicio.ActualizarAsync(dto);

            return Ok(
                RespuestaApi<object>.Ok(
                    null!,
                    "Producto actualizado correctamente."));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(
            int id)
        {
            await _servicio.EliminarAsync(id);

            return Ok(
                RespuestaApi<object>.Ok(
                    null!,
                    "Producto eliminado correctamente."));
        }
    }
}