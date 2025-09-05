using System;
using System.ComponentModel.DataAnnotations;
using BlogApp.Entity;

namespace BlogApp.Models;

public class EditProfileViewModel
{



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
