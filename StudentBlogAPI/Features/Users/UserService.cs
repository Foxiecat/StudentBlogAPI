using System.Linq.Expressions;
using StudentBlogAPI.Features.Common.Interfaces;
using StudentBlogAPI.Features.Users.DTOs;
using StudentBlogAPI.Features.Users.Interfaces;

namespace StudentBlogAPI.Features.Users;

public class UserService
    (
    ILogger<UserService> logger,
    IMapper<User, UserDTO> userMapper,
    IMapper<User, RegistrationDTO> registrationMapper,
    IUserRepository userRepository,
    IHttpContextAccessor httpContextAccessor
    )
    : IUserService
{
    // Create
    public async Task<UserDTO?> RegisterAsync(RegistrationDTO registrationDTO)
    {
        User user = registrationMapper.MapToModel(registrationDTO);
        
        user.Id = Guid.NewGuid();
        user.Created = DateTime.UtcNow;
        user.IsAdminUser = false;
        
        user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(registrationDTO.Password);
        
        // Add user to database
        User? addedUser = await userRepository.AddAsync(user);

        return addedUser is null
            ? null 
            : userMapper.MapToDTO(addedUser);
    }
    
    // Read
    public Task<UserDTO?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserDTO>> GetPagedAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }
    
    // Update
    public Task<UserDTO?> UpdateAsync(UserDTO model)
    {
        throw new NotImplementedException();
    }
    
    // Delete
    public Task<bool> DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
    
    
    public async Task<UserDTO?> AddAsync(UserDTO userDTO)
    {
        User model = userMapper.MapToModel(userDTO);
        User? modelResponse = await userRepository.AddAsync(model);
        
        return modelResponse is null
            ? null
            : userMapper.MapToDTO(modelResponse);
    }
    
    public async Task<Guid> AuthenticateUserAsync(string userName, string password)
    {
        Expression<Func<User, bool>> expression = user => user.UserName == userName;
        User? user = (await userRepository.FindAsync(expression)).FirstOrDefault();
        if (user is null)
            return Guid.Empty;

        return BCrypt.Net.BCrypt
            .Verify(password, user.HashedPassword) 
            ? user.Id 
            : Guid.Empty;
    }
    
    public async Task<IEnumerable<UserDTO>> FindAsync(UserSearchParameters searchParameters)
    {
        Expression<Func<User, bool>> predicate = user => 
            (string.IsNullOrEmpty(searchParameters.UserName) || user.UserName.Contains(searchParameters.UserName)) &&
            (string.IsNullOrEmpty(searchParameters.FirstName) || user.FirstName.Contains(searchParameters.FirstName)) &&
            (string.IsNullOrEmpty(searchParameters.LastName) || user.LastName.Contains(searchParameters.LastName)) &&
            (string.IsNullOrEmpty(searchParameters.Email) || user.Email.Contains(searchParameters.Email));
        
        IEnumerable<User> users = await userRepository.FindAsync(predicate);
        
        return users.Select(userMapper.MapToDTO);
    }
}