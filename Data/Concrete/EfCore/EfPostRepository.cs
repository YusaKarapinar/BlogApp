using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using BlogApp.Models;
using System.Security.Claims;

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

        public async Task<Post> AddPostAsync(CreateViewModel model, int userId)
        {
            var newPost = new Post
            {
                PostName = model.PostName,
                PostText = model.PostText,
                PostImage = model.PostImage,
                PostTags = _context.Tags
                .Where(t => model.SelectedTagIds.Contains(t.TagId))
                .ToList(),
                PostUrl = model.PostName?.ToLowerInvariant().Replace(" ", "-") + "-" + Guid.NewGuid(),
                PostPublishDate = DateTime.UtcNow,
                PostIsActive = true,
                UserId = userId
            };

            await _context.Posts.AddAsync(newPost);
            await _context.SaveChangesAsync();
            return newPost;
        }

        public async Task<Post> EditPostAsync(Post post, CreateViewModel model)
        {
            post.PostName = model.PostName;
            post.PostText = model.PostText;
            if (model.PostImage != null)
            {
                post.PostImage = model.PostImage;
            }
            post.PostTags = _context.Tags
                .Where(t => model.SelectedTagIds.Contains(t.TagId))
                .ToList();

            await _context.SaveChangesAsync();
            return post;
        }

        public Task DeletePostAsync(Post post)
        {
            _context.Posts.Remove(post);
            return _context.SaveChangesAsync();
        }
    }
}