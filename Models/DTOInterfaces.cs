using refShop_DEV.Services;
using refShop_DEV.Services.Interfaces;

namespace refShop_DEV.Models
{
    public class DTOInterfaces : IDTOInterfaces
    {
        public IUserRole UserRole { get; set; }

        public List<IPermissionsDTO> Permissions { get; set; }

    }
}
