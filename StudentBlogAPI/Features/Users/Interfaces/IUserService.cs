using StudentBlogAPI.Features.Common.Interfaces;
using StudentBlogAPI.Features.Posts.DTOs;
using StudentBlogAPI.Features.Users.DTOs;

namespace StudentBlogAPI.Features.Users.Interfaces;

public interface IUserService : IService<UserResponse>
{
    Task<UserResponse?> RegisterAsync(UserRequest userRequest);
    Task<IEnumerable<PostResponse>> GetUserPostsAsync(Guid userId);
    Task<Guid> AuthenticateUserAsync(string userName, string password);
    Task<IEnumerable<UserResponse>> FindAsync(UserSearchRequest userSearchRequest);
}