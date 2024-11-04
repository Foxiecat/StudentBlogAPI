using System.ComponentModel.DataAnnotations;

namespace StudentBlogAPI.Features.Posts.DTOs;

public class PostDTO
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    public Guid? UserId { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Content { get; set; }
    public DateTime DatePosted { get; set; }
}