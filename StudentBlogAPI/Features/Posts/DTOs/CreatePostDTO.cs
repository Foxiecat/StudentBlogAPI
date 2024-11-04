using System.ComponentModel.DataAnnotations;

namespace StudentBlogAPI.Features.Posts.DTOs;

public class CreatePostDTO
{
    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Content { get; set; }
}