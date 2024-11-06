namespace StudentBlogAPI.Features.Users.DTOs;

public class UserSearchRequest
{
    public string? UserName { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
}