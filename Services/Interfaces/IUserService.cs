using refShop_DEV.Models.Login;
using System.Collections.Generic;


namespace refShop_DEV.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}
