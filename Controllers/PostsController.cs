using System;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogApp.Controllers;

public class PostsController : Controller
{
    private IPostRepository _postRepository;
    private ITagRepository _tagRepository;
    public PostsController(IPostRepository postRepository, ITagRepository tagRepository)
    {
        _postRepository = postRepository;
        _tagRepository = tagRepository;
    }



    public IActionResult Index(string? tag)
    {
        var posts = _postRepository.Posts;


        if (!string.IsNullOrEmpty(tag))
        {
            posts = posts.Where(p => p.PostTags.Any(t => t.TagUrl == tag));
        }




        var tags = _tagRepository.Tags.ToList();
        PostViewModel postViewModel = new PostViewModel
        {
            Posts = posts.Include(p => p.PostTags).ToList(),
            Tags = tags
        };


        return View(postViewModel);
    }
    public async Task<IActionResult> Details(string? url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return NotFound();
        }

        return View(await _postRepository
        .Posts
        .Include(p => p.PostTags)
        .Include(p => p.PostComments)
        .ThenInclude(c => c.User)
        .FirstOrDefaultAsync(p => p.PostUrl == url)

        );
        
        
    }
    [HttpPost]
    public async Task<IActionResult> Comment(int PostId, string url, [Bind("CommentText")]Comment comment)
    {
        if (comment.CommentText != null)
        {
            comment.CommentDate = DateTime.UtcNow;
            comment.UserId = 1; // TODO: Giriş yapan kullanıcı ID'si gelecek
            await _postRepository.AddCommentAsync(PostId, comment);
            return RedirectToAction("Details", new { url = url });
        }
        return NotFound();
    }
}
