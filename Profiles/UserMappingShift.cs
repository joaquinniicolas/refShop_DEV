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
                .ForMember(dest => dest.HoraInicio, opt => opt.MapFrom(src => src.HoraInicio))
                .ForMember(dest => dest.HoraFin, opt => opt.MapFrom(src => src.HoraFin))
                .ForMember(dest => dest.CreadoPor, opt => opt.MapFrom(src => src.CreadoPor))
                .ForMember(dest => dest.CargaHoraria, opt => opt.MapFrom(src => src.CargaHoraria));

        }

    }
}
