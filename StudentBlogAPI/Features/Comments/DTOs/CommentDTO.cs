using StudentBlogAPI.Features.Posts;
using StudentBlogAPI.Features.Users;

namespace StudentBlogAPI.Features.Comments.DTOs;

public class CommentDTO
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = string.Empty;
}