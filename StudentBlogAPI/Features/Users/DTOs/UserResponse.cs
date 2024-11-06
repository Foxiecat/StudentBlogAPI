namespace StudentBlogAPI.Features.Users.DTOs;

public class UserResponse
{
    public Guid Id { get; init; }
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime Created { get; init; }
    public DateTime? Updated { get; init; }
}