namespace StudentBlogAPI.Features.Users.DTOs;

public class UserDTO
{
    public Guid Id { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime Created { get; init; }
    public DateTime? Updated { get; init; }
}