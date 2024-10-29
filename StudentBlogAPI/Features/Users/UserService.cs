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
    public async Task<UserDTO?> GetByIdAsync(Guid id)
    {
        User? user = await userRepository.GetByIdAsync(id);
        
        return user is null
            ? null
            : userMapper.MapToDTO(user);
    }

    public async Task<IEnumerable<UserDTO>> GetPagedAsync(int pageNumber, int pageSize)
    {
        IEnumerable<User> users = await userRepository.GetPagedAsync(pageNumber, pageSize);
        
        return users.Select(userMapper.MapToDTO).ToList();
    }
    
    // Update
    public async Task<UserDTO?> UpdateAsync(UserDTO userDTO)
    {
        User user = userMapper.MapToModel(userDTO);
        
        string? loggedInUserId = httpContextAccessor.HttpContext?.Items["UserId"] as string;
        
        if (loggedInUserId is null)
        {
            logger.LogWarning("User {UserId} not found", loggedInUserId);
            return null;
        }
        
        Guid id = Guid.Parse(loggedInUserId);
        
        if (!id.ToString().Equals(user.Id.ToString()))
        {
            logger.LogWarning("Could not update User {UserId}", loggedInUserId);
            return null;
        }
        
        logger.LogDebug("Updating user {UserId}", loggedInUserId);
        User? updatedUser = await userRepository.UpdateAsync(user);

        if (updatedUser is null)
        {
            logger.LogWarning("Did not update user {UserId}", id);
            return null;
        }
        
        logger.LogDebug("User {UserId} updated", updatedUser.Id);
        return userMapper.MapToDTO(updatedUser);
    }
    
    // Delete
    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        string? loggedInUserId = httpContextAccessor.HttpContext?.Items["UserId"] as string;

        User? loggedInUser =
            (await userRepository.FindAsync(user => user.Id.ToString() == loggedInUserId)).FirstOrDefault();

        if (loggedInUserId is null)
        {
            logger.LogWarning("User {UserId} not found", loggedInUserId);
            return false;
        }

        if (!id.ToString().Equals(loggedInUser!.Id.ToString()))
        {
            logger.LogWarning("Could not delete User {UserId}", loggedInUserId);
            return false;
        }

        if (loggedInUser.IsAdminUser)
        {
            await userRepository.DeleteByIdAsync(id);
            return true;
        }
        
        logger.LogDebug("Deleting user {UserId}", loggedInUserId);
        User? deletedUser = await userRepository.DeleteByIdAsync(id);

        if (deletedUser is null)
        {
            logger.LogWarning("Did not delete user {UserId}", id);
            return false;
        }
        
        logger.LogDebug("User {UserId} deleted", deletedUser.Id);
        return true;

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
    
    public async Task<IEnumerable<UserDTO>> FindAsync(SearchParameters searchParameters)
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