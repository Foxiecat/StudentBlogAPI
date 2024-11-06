using System.ComponentModel.DataAnnotations;
using StudentBlogAPI.Features.Comments;
using StudentBlogAPI.Features.Comments.Models;
using StudentBlogAPI.Features.Posts;

namespace StudentBlogAPI.Features.Users;

public class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public bool IsAdminUser { get; set; }
    
    
    // Navigation properties
    public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
}