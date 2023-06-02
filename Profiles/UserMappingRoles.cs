using AutoMapper;
using refShop_DEV.Models.Permission;

namespace refShop_DEV.Profiles
{
    public class UserMappingRoles : Profile
    {

        public UserMappingRoles()
        {
            CreateMap<UserRole, UserRoleDto>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));


            CreateMap<RolePermissions, RolePermissionsDTO>()
        .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
        .ForMember(dest => dest.PermissionId, opt => opt.MapFrom(src => src.PermissionId));

            CreateMap<ActivityPermission, PermissionDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

        }

    }


}



