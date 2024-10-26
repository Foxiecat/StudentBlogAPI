using StudentBlogAPI.Features.Users.Interfaces;
using StudentBlogAPI.Features.Users.Models;

namespace StudentBlogAPI.Features.Users;

public class UserService : IUserService
{
    public Task<UserDTO?> AddAsync(UserDTO model)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO?> UpdateAsync(UserDTO model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserDTO>> GetPagedAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO?> RegisterAsync(UserRegistrationDTO registrationDTO)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> AuthenticateUserAsync(string userName, string password)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserDTO>> FindAsync(UserSearchParameters searchParameters)
    {
        throw new NotImplementedException();
    }
}