using System;
using System.ComponentModel.DataAnnotations;
using BlogApp.Entity;

namespace BlogApp.Models;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email Address")]
    public string? Email { get; set; }
    [Required]
    [StringLength(20, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string? Password { get; set; }
}
