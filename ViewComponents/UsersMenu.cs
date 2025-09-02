using System;
using BlogApp.Data.Abstract;
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

        var users = await _userRepository.Users.ToListAsync();
        return View(users);
    }
}
