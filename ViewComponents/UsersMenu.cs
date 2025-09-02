using System;
using BlogApp.Data.Abstract;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.ViewComponents;

public class UsersMenu:ViewComponent
{
    private IUserRepository _userRepository;
    public UsersMenu(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {

         var users = await _userRepository.Users
            .Include(u => u.UserPosts)
            .Include(u => u.UserComments)
            .Select(u => new UserListItemViewModel
            {
                UserName = u.UserName,
                Name = u.Name,
                PostCount = u.UserPosts.Count,
                CommentCount = u.UserComments.Count
            })
            .ToListAsync();
        return View(users);
    }
}
