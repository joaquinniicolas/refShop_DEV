using refShop_DEV.Services.Interfaces;

namespace refShop_DEV.Services
{
    public interface IDTOInterfaces
    {
        IUserRole UserRole { get; set; }
        List<IPermissionsDTO> Permissions { get; set; }
    }
}
