using System;
using System.ComponentModel.DataAnnotations;
using BlogApp.Entity;

namespace BlogApp.Models;

public class UserListViewModel
{
    public string? UserName { get; set; }
    public string? Name { get; set; }
    public int PostCount { get; set; }
    public int CommentCount { get; set; }
    public string? UserUrl { get; set; }
}
