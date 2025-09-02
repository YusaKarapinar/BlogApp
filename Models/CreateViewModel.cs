using System;
using System.ComponentModel.DataAnnotations;
using BlogApp.Entity;

namespace BlogApp.Models;

public class CreateViewModel
{

    [Required]
    [Display(Name = "Post Title")]
    public string? PostName { get; set; }

    [Required]
    [Display(Name = "Post Text")]
    public string? PostText { get; set; }

    [Display(Name = "Post Image")]
    public string? PostImage { get; set; }  // Resim adı veya URL

    [Display(Name = "Select Tags")]
    public List<int> SelectedTagIds { get; set; } = new List<int>(); // Seçilen taglerin Id’leri

    public List<Tag> PostTags { get; set; } = new List<Tag>(); // Tüm tagler, dropdown/checkbox için

}
