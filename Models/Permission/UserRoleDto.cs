using refShop_DEV.Services.Interfaces;

namespace refShop_DEV.Models.Permission
{
    public class UserRoleDto : IUserRole
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int Level { get; set; }
    }
}
