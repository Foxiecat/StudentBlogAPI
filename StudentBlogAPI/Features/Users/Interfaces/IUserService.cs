using StudentBlogAPI.Common.Interfaces;
using StudentBlogAPI.Features.Posts.DTOs;
using StudentBlogAPI.Features.Users.DTOs;
using StudentBlogAPI.Features.Users.Extras;

namespace StudentBlogAPI.Features.Users.Interfaces;

public interface IUserService : IService<UserDTO>
{
    Task<UserDTO?> RegisterAsync(RegistrationDTO registrationDTO);
    Task<IEnumerable<PostDTO>> GetUserPostsAsync(Guid userId);
    Task<Guid> AuthenticateUserAsync(string userName, string password);
    Task<IEnumerable<UserDTO>> FindAsync(SearchParameters searchParameters);
}