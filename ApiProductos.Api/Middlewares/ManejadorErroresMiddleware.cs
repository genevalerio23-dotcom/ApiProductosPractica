using ApiProductos.Dto;
using ApiProductos.Negocio.Excepciones;
using System.Net;
using System.Text.Json;

namespace ApiProductos.Api.Middlewares
{
    public class ManejadorErroresMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ManejadorErroresMiddleware> _logger;

        public ManejadorErroresMiddleware(
            RequestDelegate next,
            ILogger<ManejadorErroresMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ProductoNoEncontradoException ex)
            {
                await EscribirRespuesta(
                    context,
                    HttpStatusCode.NotFound,
                    ex.Message);
            }
            catch (CategoriaNoEncontradaException ex)
            {
                await EscribirRespuesta(
                    context,
                    HttpStatusCode.NotFound,
                    ex.Message);
            }
            catch (ReglaDeNegocioException ex)
            {
                await EscribirRespuesta(
                    context,
                    HttpStatusCode.BadRequest,
                    ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado");

                await EscribirRespuesta(
                    context,
                    HttpStatusCode.InternalServerError,
                    "Ocurrió un error interno. Intente nuevamente más tarde.");
            }
        }

        private static async Task EscribirRespuesta(
            HttpContext context,
            HttpStatusCode statusCode,
            string mensaje)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var respuesta =
                RespuestaApi<object>.Error(mensaje);

            var json =
                JsonSerializer.Serialize(respuesta);

            await context.Response.WriteAsync(json);
        }
    }
}