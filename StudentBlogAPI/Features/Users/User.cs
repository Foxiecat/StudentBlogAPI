using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using StudentBlogAPI.Features.Comments;
using StudentBlogAPI.Features.Posts;

namespace StudentBlogAPI.Features.Users;

public class User
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MinLength(3), MaxLength(30)]
    public string UserName { get; init; } = string.Empty;
    
    [Required]
    [MinLength(2), MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [MinLength(2), MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string HashedPassword { get; set; } = string.Empty;
    
    [Required]
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    
    [Required]
    public bool IsAdminUser { get; set; }
    
    
    // Navigation properties
    public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
}