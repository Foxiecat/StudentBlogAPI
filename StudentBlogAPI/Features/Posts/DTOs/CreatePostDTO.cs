using System.ComponentModel.DataAnnotations;

namespace StudentBlogAPI.Features.Posts.DTOs;

public class CreatePostDTO
{
    public string Title { get; set; }
    public string Content { get; set; }
}