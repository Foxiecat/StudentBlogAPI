using System.Linq.Expressions;
using StudentBlogAPI.Features.Common.Interfaces;
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
        bool userExists =
            (await userRepository.FindAsync(u => u.UserName == userRequest.UserName 
                                                 || u.Email == userRequest.Email)).Any();
        if (userExists)
        {
            logger.LogWarning("User already exists");
            return null;
        }
        
        User user = registrationMapper.MapToModel(userRequest);
        user.Id = Guid.NewGuid();
        user.Created = DateTime.UtcNow;
        user.IsAdminUser = false;
        user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);
        
        User? addedUser = await userRepository.AddAsync(user);
        return addedUser is null
            ? null 
            : userMapper.MapToResponse(addedUser);
    }
    
    // Read
    public async Task<UserResponse?> GetByIdAsync(Guid id)
    {
        User? user = await userRepository.GetByIdAsync(id);
        return user is null
            ? null
            : userMapper.MapToResponse(user);
    }

    public async Task<IEnumerable<UserResponse>> GetPagedAsync(int pageNumber, int pageSize)
    {
        IEnumerable<User> users = await userRepository.GetPagedAsync(pageNumber, pageSize);
        return users.Select(userMapper.MapToResponse).ToList();
    }

    public async Task<IEnumerable<PostResponse>> GetUserPostsAsync(Guid userId)
    {
        IEnumerable<Post> posts = await userRepository.GetUserPostsAsync(userId);
        return posts.Select(postMapper.MapToResponse).ToList();
    }
    
    // Update
    public async Task<UserResponse?> UpdateAsync(UserResponse updateRequest)
    {
        User user = userMapper.MapToModel(updateRequest);
        string? loggedInUserId = httpContextAccessor.HttpContext?.Items["UserId"] as string;
        
        if (loggedInUserId is null)
        {
            logger.LogWarning("Unauthorized update attempt by user {UserId}", loggedInUserId);
            return null;
        }
        
        User? loggedInUser = await userRepository.GetByIdAsync(Guid.Parse(loggedInUserId));
        if (loggedInUser is null || (!loggedInUser.IsAdminUser && loggedInUser.Id != user.Id))
        {
            logger.LogWarning("Unauthorized update attempt by user {UserId}", loggedInUserId);
            return null;
        }
        
        logger.LogDebug("Updating user {UserId}", loggedInUserId);
        User? updatedUser = await userRepository.UpdateAsync(user);

        if (updatedUser is null)
        {
            logger.LogWarning("Did not update user {UserId}", user.Id);
            return null;
        }
        
        logger.LogDebug("User {UserId} updated", updatedUser.Id);
        return userMapper.MapToResponse(updatedUser);
    }
    
    // Delete

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        string? loggedInUserId = httpContextAccessor.HttpContext?.Items["UserId"] as string;
        if (loggedInUserId is null)
        {
            logger.LogWarning("User {UserId} not found", loggedInUserId);
            return false;
        }
        
        User? loggedInUser = await userRepository.GetByIdAsync(Guid.Parse(loggedInUserId));

        if (loggedInUser is null || (!loggedInUser.IsAdminUser && loggedInUser.Id != id))
        {
            logger.LogWarning("Could not delete User {UserId}", loggedInUserId);
            return false;
        }
        
        logger.LogDebug("Deleting user {UserId}", loggedInUserId);
        User? deletedUser = await userRepository.DeleteByIdAsync(id);

        if (deletedUser is null)
        {
            logger.LogWarning("Failed to delete user {UserId}", id);
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
            : userMapper.MapToResponse(modelResponse);
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
        
        return users.Select(userMapper.MapToResponse);
    }
}