using StudentBlogAPI.Features.Common.Interfaces;
using StudentBlogAPI.Features.Users.DTOs;

namespace StudentBlogAPI.Features.Users.Interfaces;

public interface IUserService : IService<UserDTO>
{
    Task<UserDTO?> RegisterAsync(RegistrationDTO registrationDTO);
    Task<Guid> AuthenticateUserAsync(string userName, string password);
    Task<IEnumerable<UserDTO>> FindAsync(UserSearchParameters searchParameters);
}