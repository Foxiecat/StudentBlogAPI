namespace StudentBlogAPI.Features.Comments.DTO;

public class CommentResponse
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = string.Empty;
}