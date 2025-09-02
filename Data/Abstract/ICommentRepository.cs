using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface ICommentRepository
    {
        public IQueryable<Comment> Comments { get; }

        public Task AddCommentAsync(int PostId, Comment comment);
    }
}