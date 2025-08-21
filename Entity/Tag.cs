using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Entity;

public class Tag
{[Key]
    public int TagId { get; set; }
    public string? TagName { get; set; }
    public string? TagUrl { get; set; }
    public ICollection<Post> TaggedPosts { get; set; } = new List<Post>();

}
