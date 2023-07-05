using refShop_DEV.Models.Login;
using refShop_DEV.Models.Permission;

namespace refShop_DEV.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user, List<PermissionDTO> permissions);
        string RenewToken();
    }
}
