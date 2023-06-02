using AutoMapper;
using refShop_DEV.Models.Login;
using refShop_DEV.Models.Permission;

namespace refShop_DEV.Profiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username));
            //.ForMember(dest => dest.MesasCreadas, opt => opt.Ignore())
            //.ForMember(dest => dest.MesasModificadas, opt => opt.Ignore())
            //.ForMember(dest => dest.MesasAsociadas, opt => opt.Ignore())
            //.ForMember(dest => dest.MesasMozo, opt => opt.Ignore());


        }
    }
}
