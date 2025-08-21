using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlogApp.Data.Abstract;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfPostRepository : IPostRepository
    {
        private BlogContext _context;
        public EfPostRepository(BlogContext context)
        {
            _context = context;
        }




        public IQueryable<Post> Posts => _context.Posts;

        public void CreatePost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public async Task<List<Post>> GetSomePostsAsync(int count = 5, int skip = 0)
        {

            return await _context.Posts.OrderByDescending(p => p.PostPublishDate).Skip(skip * 5).Take(count).ToListAsync();
        }
       
        
    }
}