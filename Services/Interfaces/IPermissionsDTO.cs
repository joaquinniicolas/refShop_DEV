using refShop_DEV.Models;
using refShop_DEV.Models.Permission;
using System.ComponentModel.DataAnnotations;

namespace refShop_DEV.Services.Interfaces
{
    public interface IPermissionsDTO
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }

}
