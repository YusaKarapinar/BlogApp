using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Entity;

public class User
{
    [Key]
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public string? UserImage { get; set; }= "default.png";


    public ICollection<Post> UserPosts { get; set; } = new List<Post>();
    public ICollection<Comment> UserComments { get; set; } = new List<Comment>();
    



}
