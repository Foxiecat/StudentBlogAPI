using System.ComponentModel.DataAnnotations;
using StudentBlogAPI.Features.Comments.Models;
using StudentBlogAPI.Features.Posts.Models;

namespace StudentBlogAPI.Features.Users.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MinLength(3), MaxLength(30)]
    public string UserName { get; set; } = string.Empty;
    
    [Required]
    [MinLength(2), MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [MinLength(2), MaxLength(50)]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string HashedPassword { get; set; } = string.Empty;
    
    [Required]
    public DateTime Created { get; set; }
    
    [Required]
    public DateTime Updated { get; set; }
    
    [Required]
    public bool IsAdminUser { get; set; }
    
    
    // Navigation properties
    public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
}