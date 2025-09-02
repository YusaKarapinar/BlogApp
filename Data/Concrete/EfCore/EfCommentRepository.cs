using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlogApp.Data.Abstract;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfCommentRepository : ICommentRepository
    {
        private BlogContext _context;
        public EfCommentRepository(BlogContext context)
        {
            _context = context;
        }




        public IQueryable<Comment> Comments => _context.Comments;

       

      
        public async Task AddCommentAsync(int PostId, Comment comment)
        {
            var post = await _context.Posts.Include(p => p.PostComments).FirstOrDefaultAsync(p => p.PostId == PostId);
            if (post != null)
            {
                  comment.PostId = post.PostId;
                post.PostComments.Add(comment);
                await _context.SaveChangesAsync();   
            }
        }
    }
}