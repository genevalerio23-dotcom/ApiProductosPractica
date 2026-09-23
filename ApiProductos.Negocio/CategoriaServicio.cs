using ApiProductos.Datos.Repositorios;
using ApiProductos.Dto;
using ApiProductos.Entidades;
using ApiProductos.Negocio.Excepciones;
using AutoMapper;

namespace ApiProductos.Negocio
{
    public class CategoriaServicio : ICategoriaServicio
    {
        private readonly ICategoriaRepositorio _repositorio;
        private readonly IMapper _mapper;

        public CategoriaServicio(
            ICategoriaRepositorio repositorio,
            IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<List<CategoriaDto>> ListarTodosAsync()
        {
            var categorias =
                await _repositorio.ListarTodosAsync();

            return _mapper.Map<List<CategoriaDto>>(categorias);
        }

        public async Task<CategoriaDto?> ObtenerPorIdAsync(
            int idCategoria)
        {
            var categoria =
                await _repositorio.ObtenerPorIdAsync(
                    idCategoria);

            if (categoria == null)
            {
                return null;
            }

            return _mapper.Map<CategoriaDto>(categoria);
        }

        public async Task<CategoriaDto> CrearAsync(
            CategoriaCrearDto dto)
        {
            var entidad =
                _mapper.Map<Categoria>(dto);

            int nuevoId =
                await _repositorio.InsertarAsync(entidad);

            var creada =
                await _repositorio.ObtenerPorIdAsync(
                    nuevoId);

            return _mapper.Map<CategoriaDto>(creada!);
        }

        public async Task ActualizarAsync(
            CategoriaActualizarDto dto)
        {
            var existente =
                await _repositorio.ObtenerPorIdAsync(
                    dto.IdCategoria);

            if (existente == null)
            {
                throw new CategoriaNoEncontradaException(
                    dto.IdCategoria);
            }

            var entidad =
                _mapper.Map<Categoria>(dto);

            await _repositorio.ActualizarAsync(entidad);
        }

        public async Task EliminarAsync(
            int idCategoria)
        {
            var existente =
                await _repositorio.ObtenerPorIdAsync(
                    idCategoria);

            if (existente == null)
            {
                throw new CategoriaNoEncontradaException(
                    idCategoria);
            }

            await _repositorio.EliminarAsync(
                idCategoria);
        }
    }
}