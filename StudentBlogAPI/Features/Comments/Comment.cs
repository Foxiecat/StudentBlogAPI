using StudentBlogAPI.Features.Posts;
using StudentBlogAPI.Features.Users;

namespace StudentBlogAPI.Features.Comments;

public class Comment
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime DateCommented { get; set; }
    
    // Navigation properties
    public virtual Post? Post { get; init; }
    public virtual User? User { get; init; }
}