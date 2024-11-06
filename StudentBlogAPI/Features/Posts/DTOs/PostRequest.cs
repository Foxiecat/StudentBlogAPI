using System.ComponentModel.DataAnnotations;

namespace StudentBlogAPI.Features.Posts.DTOs;

public class PostRequest
{
    public string Title { get; set; }
    public string Content { get; set; }
}