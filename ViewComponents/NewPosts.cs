using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.ViewComponents
{
    public class NewPosts : ViewComponent
    {
        private IPostRepository _postRepository;
    public NewPosts(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {

        var posts = await _postRepository.GetSomePostsAsync();
        return View(posts);
    }
    }
}