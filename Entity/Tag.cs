using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Entity;

public enum TagColors
{
    primary,
    secondary,
    success,
    danger,
    warning,
    info,
    light,
    dark
}

public class Tag
{
    [Key]
    public int TagId { get; set; }
    public string? TagName { get; set; }
    public string? TagUrl { get; set; }
    public TagColors? TagColor { get; set; } = TagColors.secondary;
    public ICollection<Post> TaggedPosts { get; set; } = new List<Post>();

}
