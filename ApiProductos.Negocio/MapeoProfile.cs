using ApiProductos.Dto;
using ApiProductos.Entidades;
using AutoMapper;

namespace ApiProductos.Negocio
{
    public class MapeoProfile : Profile
    {
        public MapeoProfile()
        {
            CreateMap<Producto, ProductoDto>();
            CreateMap<ProductoCrearDto, Producto>();
            CreateMap<ProductoActualizarDto, Producto>();

            CreateMap<Categoria, CategoriaDto>();
            CreateMap<CategoriaCrearDto, Categoria>();
            CreateMap<CategoriaActualizarDto, Categoria>();
        }
    }
}
