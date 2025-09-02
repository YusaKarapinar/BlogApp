using System;
using System.ComponentModel.DataAnnotations;
using BlogApp.Entity;

namespace BlogApp.Models;

public class SignupViewModel
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


    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
    public string? ConfirmPassword { get; set; }

    [Required]
    [Display(Name = "User Name")]
    public string? UserName { get; set; }

    [Required]
    [Display(Name = "Name")]
    public string? Name { get; set; }
    [Required]
    [Display(Name = "Surname")]
    public string? Surname { get; set; }

    public IFormFile? UserImage { get; set; }

}
