using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using StudentBlogAPI.Features.Comments;
using StudentBlogAPI.Features.Users;

namespace StudentBlogAPI.Features.Posts.DTOs;

public class PostDTO
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}