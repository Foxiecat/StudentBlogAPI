using System.Linq.Expressions;
using StudentBlogAPI.Common.Interfaces;
using StudentBlogAPI.Features.Posts;
using StudentBlogAPI.Features.Posts.DTOs;
using StudentBlogAPI.Features.Users.DTOs;
using StudentBlogAPI.Features.Users.Interfaces;

namespace StudentBlogAPI.Features.Users;

public class UserService
    (
    ILogger<UserService> logger,
    IMapper<User, UserResponse> userMapper,
    IMapper<User, UserRequest> registrationMapper,
    IMapper<Post, PostResponse> postMapper,
    IUserRepository userRepository,
    IHttpContextAccessor httpContextAccessor
    )
    : IUserService
{
    // Create
    public async Task<UserResponse?> RegisterAsync(UserRequest userRequest)
    {
        User user = registrationMapper.MapToModel(userRequest);
        
        user.Id = Guid.NewGuid();
        user.Created = DateTime.UtcNow;
        user.IsAdminUser = false;
        
        user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);
        
        // Add user to database
        User? addedUser = await userRepository.AddAsync(user);

        return addedUser is null
            ? null 
            : userMapper.MapToDTO(addedUser);
    }
    
    // Read
    public async Task<UserResponse?> GetByIdAsync(Guid id)
    {
        User? user = await userRepository.GetByIdAsync(id);
        
        return user is null
            ? null
            : userMapper.MapToDTO(user);
    }

    public async Task<IEnumerable<UserResponse>> GetPagedAsync(int pageNumber, int pageSize)
    {
        IEnumerable<User> users = await userRepository.GetPagedAsync(pageNumber, pageSize);
        
        return users.Select(userMapper.MapToDTO).ToList();
    }

    public async Task<IEnumerable<PostResponse>> GetUserPostsAsync(Guid userId)
    {
        IEnumerable<Post> posts = await userRepository.GetUserPostsAsync(userId);
        
        return posts.Select(postMapper.MapToDTO).ToList();
    }
    
    // Update
    public async Task<UserResponse?> UpdateAsync(UserResponse userResponse)
    {
        User user = userMapper.MapToModel(userResponse);
        
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
    
    
    public async Task<UserResponse?> AddAsync(UserResponse userResponse)
    {
        User model = userMapper.MapToModel(userResponse);
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
    
    public async Task<IEnumerable<UserResponse>> FindAsync(UserSearchRequest userSearchRequest)
    {
        Expression<Func<User, bool>> predicate = user => 
            (string.IsNullOrEmpty(userSearchRequest.UserName) || user.UserName.Contains(userSearchRequest.UserName)) &&
            (string.IsNullOrEmpty(userSearchRequest.FirstName) || user.FirstName.Contains(userSearchRequest.FirstName)) &&
            (string.IsNullOrEmpty(userSearchRequest.LastName) || user.LastName.Contains(userSearchRequest.LastName)) &&
            (string.IsNullOrEmpty(userSearchRequest.Email) || user.Email.Contains(userSearchRequest.Email));
        
        IEnumerable<User> users = await userRepository.FindAsync(predicate);
        
        return users.Select(userMapper.MapToDTO);
    }
}