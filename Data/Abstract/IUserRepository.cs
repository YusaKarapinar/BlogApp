using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entity;
using BlogApp.Models;

namespace BlogApp.Data.Abstract
{
    public interface IUserRepository
    {
        public IQueryable<User> Users { get; }

        public Task AddUserAsync(SignupViewModel model);


        public Task UpdateUserAsync(User user, EditProfileViewModel model);
        public Task DeleteUserAsync(User user);

    }
}