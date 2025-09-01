using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Entity;

public class Post
{
    [Key]
    public int PostId { get; set; }
    public string? PostName { get; set; }
    public string? PostText { get; set; }
    public string? PostImage { get; set; }
    public DateTime PostPublishDate { get; set; }
    public bool PostIsActive { get; set; }
    public string PostUrl { get; set; } = null!;

    public ICollection<Comment> PostComments { get; set; } = new List<Comment>();
    public ICollection<Tag> PostTags { get; set; } = new List<Tag>();


    public int UserId { get; set; }
    public User User { get; set; } = null!;


}
