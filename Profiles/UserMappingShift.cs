using AutoMapper;
using refShop_DEV.Models.Restaurant;

namespace refShop_DEV.Profiles
{
    public class UserMappingShift : Profile
    {
        public UserMappingShift()
        {
            CreateMap<Turno, TurnoDTO>()

                .ForMember(dest => dest.IDTurno, opt => opt.MapFrom(src => src.IDTurno))
                .ForMember(dest => dest.IDEmpleado, opt => opt.MapFrom(src => src.IDEmpleado))
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.FechaInicio))
                .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.FechaFin))
                .ForMember(dest => dest.FechaDeCreacion, opt => opt.MapFrom(src => src.FechaDeCreacion))
                .ForMember(dest => dest.CreadoPor, opt => opt.MapFrom(src => src.CreadoPor));

        }

    }
}
