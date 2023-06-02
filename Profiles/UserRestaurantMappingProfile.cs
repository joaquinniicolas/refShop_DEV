using AutoMapper;
using refShop_DEV.Models.Restaurant;

namespace refShop_DEV.Profiles
{
    public class UserRestaurantMappingProfile : Profile
    {
        public UserRestaurantMappingProfile()
        {
            CreateMap<Mesa, MesaDTO>()

                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => src.Numero))
                .ForMember(dest => dest.Capacidad, opt => opt.MapFrom(src => src.Capacidad))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
                .ForMember(dest => dest.CreadoEl, opt => opt.MapFrom(src => src.CreadoEl))
                .ForMember(dest => dest.CreadoPor, opt => opt.MapFrom(src => src.CreadoPor))
                .ForMember(dest => dest.ModificadoEl, opt => opt.MapFrom(src => src.ModificadoEl))
                .ForMember(dest => dest.IdUsuario, opt => opt.MapFrom(src => src.IdUsuario))
                .ForMember(dest => dest.IdMozo, opt => opt.MapFrom(src => src.IdMozo))
                .ForMember(dest => dest.Creador, opt => opt.MapFrom(src => src.CreadoPorNavigation != null ? src.CreadoPorNavigation.FirstName + ' ' + src.CreadoPorNavigation.LastName : null))
                .ForMember(dest => dest.Modificador, opt => opt.MapFrom(src => src.ModificadoPorNavigation != null ? src.ModificadoPorNavigation.FirstName + ' ' + src.ModificadoPorNavigation.LastName : null))
                .ForMember(dest => dest.MozoEncargado, opt => opt.MapFrom(src => src.IdMozoNavigation != null ? src.IdMozoNavigation.FirstName + ' ' + src.IdMozoNavigation.LastName : null));

        }
    }

}
