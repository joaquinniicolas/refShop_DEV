using refShop_DEV.Models.MyDbContext;
using refShop_DEV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using refShop_DEV.Models.Login;
using Microsoft.EntityFrameworkCore;

namespace refShop_DEV.Services
{
    public class UserService : IUserService
    {
        private readonly MyDbContext _context;

        public UserService(MyDbContext context)
        {
            _context = context;
        }

       
        public IEnumerable<User> GetAll()
        {
            return _context.Users.Select(u => new User());
        }

        public User GetById(int id)
        {
            var user = _context.Users.Include(u => u.Categorias)
                    .SingleOrDefault(u => u.Id == id);
            return new User();
        }


    }
}
