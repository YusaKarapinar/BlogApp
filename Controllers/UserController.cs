using System;
using System.Security.Claims;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogApp.Controllers;


public class UserController : Controller
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public IActionResult Login()
    {

        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {

            return View(model);
        }

        var isUser = await _userRepository.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
        if (isUser != null)
        {
            var userClaims = new List<Claim>();
            userClaims.Add(new Claim(ClaimTypes.NameIdentifier, isUser.UserId.ToString()));
            userClaims.Add(new Claim(ClaimTypes.Name, isUser.UserName ?? ""));
            userClaims.Add(new Claim("UserImage", isUser.UserImage ?? ""));
            var userIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(60),
                IsPersistent = true
            };
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(userIdentity), authProperties);
            return RedirectToAction("Index", "Posts");

        }
        else
        {
            ModelState.AddModelError("", "Geçersiz giriş");
        }

        return View(model);
    }


    public IActionResult Profile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return RedirectToAction("Login");
        }

        var user = _userRepository.Users.Include(u => u.UserPosts).Include(u => u.UserComments).ThenInclude(c => c.Post).FirstOrDefault(u => u.UserId.ToString() == userId);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Posts");
    }






}
