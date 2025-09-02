using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Entity;

public class User
{
    [Key]
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public string? UserImage { get; set; }= "defaultProfilePicture.png";
    
    public string? UserUrl { get; set; } = null!;


    public string? Name { get; set; }
    public string? Surname { get; set; }

    public string? Email { get; set; }
    public string? Password { get; set; }


    public ICollection<Post> UserPosts { get; set; } = new List<Post>();
    public ICollection<Comment> UserComments { get; set; } = new List<Comment>();
    



}
