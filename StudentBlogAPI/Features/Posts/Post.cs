using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudentBlogAPI.Features.Comments;
using StudentBlogAPI.Features.Users;

namespace StudentBlogAPI.Features.Posts;

public class Post
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    
    [Required]
    [MinLength(3) , MaxLength(50)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    [Required]
    public DateTime DatePosted { get; set; }
    
    // Navigation properties
    public virtual User? User { get; set; }
    public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
}