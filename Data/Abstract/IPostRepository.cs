using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface IPostRepository
    {
        public IQueryable<Post> Posts { get; }

        void CreatePost(Post post);
        public Task<List<Post>> GetSomePostsAsync(int count = 5, int skip = 0);
    }
}