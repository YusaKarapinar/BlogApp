using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface IUserRepository
    {
        public IQueryable<User> Users { get; }

        public void CreateUser(User user);


    }
}