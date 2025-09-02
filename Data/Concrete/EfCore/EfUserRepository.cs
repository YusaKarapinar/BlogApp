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
            string? imageName = null;

            if (model.UserImage != null && model.UserImage.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
                imageName = Guid.NewGuid() + Path.GetExtension(model.UserImage.FileName);
                var filePath = Path.Combine(uploads, imageName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.UserImage.CopyToAsync(fileStream);
                }
            }
            else
            {
                imageName = "defaultProfilePicture.jpg";
            }
            var user = new User
            {
                Email = model.Email,
                Password = model.Password,
                UserName = model.UserName,
                Name = model.Name,
                Surname = model.Surname,
                UserImage = imageName,
                UserUrl = model.UserName?.ToLower().Replace(" ", "-")
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

    }
}