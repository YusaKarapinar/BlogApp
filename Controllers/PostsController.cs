using System;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Models;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        
        return View(await _postRepository.Posts.Include(p => p.PostTags).FirstOrDefaultAsync(p => p.PostUrl == url));
    }
}
