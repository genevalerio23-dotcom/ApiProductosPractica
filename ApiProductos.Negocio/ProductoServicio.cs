using ApiProductos.Datos.Repositorios;
using ApiProductos.Dto;
using ApiProductos.Entidades;
using ApiProductos.Negocio.Excepciones;
using AutoMapper;

namespace ApiProductos.Negocio
{
    public class ProductoServicio : IProductoServicio
    {
        private readonly IProductoRepositorio _repositorio;
        private readonly IMapper _mapper;

        public ProductoServicio(
            IProductoRepositorio repositorio,
            IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<List<ProductoDto>> ListarTodosAsync()
        {
            var productos =
                await _repositorio.ListarTodosAsync();

            return _mapper.Map<List<ProductoDto>>(productos);
        }

        public async Task<List<ProductoDto>> ListarPaginadoAsync(
            int pagina,
            int tamanioPagina)
        {
            if (pagina < 1)
            {
                throw new ReglaDeNegocioException(
                    "La página debe ser mayor o igual a 1.");
            }

            if (tamanioPagina < 1)
            {
                throw new ReglaDeNegocioException(
                    "El tamaño de página debe ser mayor o igual a 1.");
            }

            var productos =
                await _repositorio.ListarPaginadoAsync(
                    pagina,
                    tamanioPagina);

            return _mapper.Map<List<ProductoDto>>(productos);
        }

        public async Task<List<ProductoDto>> ListarPorCategoriaAsync(
            int idCategoria)
        {
            var productos =
                await _repositorio.ListarPorCategoriaAsync(
                    idCategoria);

            return _mapper.Map<List<ProductoDto>>(productos);
        }

        public async Task<int> ContarActivosAsync()
        {
            return await _repositorio.ContarActivosAsync();
        }

        public async Task<ProductoDto?> ObtenerPorIdAsync(
            int idProducto)
        {
            var producto =
                await _repositorio.ObtenerPorIdAsync(
                    idProducto);

            if (producto == null)
            {
                return null;
            }

            return _mapper.Map<ProductoDto>(producto);
        }

        public async Task<ProductoDto> CrearAsync(
            ProductoCrearDto dto)
        {
            if (dto.Precio <= 0)
            {
                throw new ReglaDeNegocioException(
                    "El precio debe ser mayor a cero.");
            }

            var entidad =
                _mapper.Map<Producto>(dto);

            int nuevoId =
                await _repositorio.InsertarAsync(entidad);

            var creado =
                await _repositorio.ObtenerPorIdAsync(
                    nuevoId);

            return _mapper.Map<ProductoDto>(creado!);
        }

        public async Task ActualizarAsync(
            ProductoActualizarDto dto)
        {
            var existente =
                await _repositorio.ObtenerPorIdAsync(
                    dto.IdProducto);

            if (existente == null)
            {
                throw new ProductoNoEncontradoException(
                    dto.IdProducto);
            }

            var entidad =
                _mapper.Map<Producto>(dto);

            await _repositorio.ActualizarAsync(entidad);
        }

        public async Task EliminarAsync(
            int idProducto)
        {
            var existente =
                await _repositorio.ObtenerPorIdAsync(
                    idProducto);

            if (existente == null)
            {
                throw new ProductoNoEncontradoException(
                    idProducto);
            }

            await _repositorio.EliminarAsync(
                idProducto);
        }
    }
}