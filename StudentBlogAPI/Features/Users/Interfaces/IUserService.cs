using StudentBlogAPI.Features.Base.Interfaces;
using StudentBlogAPI.Features.Users.Models;

namespace StudentBlogAPI.Features.Users.Interfaces;

public interface IUserService : IService<UserDTO>
{
    Task<UserDTO?> RegisterAsync(UserRegistrationDTO registrationDTO);
    Task<Guid> AuthenticateUserAsync(string userName, string password);
    Task<IEnumerable<UserDTO>> FindAsync(UserSearchParameters searchParameters);
}