using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudentBlogAPI.Features.Comments;
using StudentBlogAPI.Features.Comments.Models;
using StudentBlogAPI.Features.Users;

namespace StudentBlogAPI.Features.Posts;

public class Post
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime DatePosted { get; set; }
    
    // Navigation properties
    public virtual User? User { get; init; }
    public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
}