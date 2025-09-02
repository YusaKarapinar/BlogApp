using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using BlogApp.Models;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfUserRepository : IUserRepository
    {
        private BlogContext _context;
        public EfUserRepository(BlogContext context)
        {
            _context = context;
        }




        public IQueryable<User> Users => _context.Users;

        public async Task AddUserAsync(SignupViewModel model)
        {
            var user = new User
            {
                Email = model.Email,
                Password = model.Password,
                UserName = model.UserName,
                Name = model.Name,
                Surname = model.Surname
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

    }
}