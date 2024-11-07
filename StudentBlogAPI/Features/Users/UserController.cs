using Microsoft.AspNetCore.Mvc;
using StudentBlogAPI.Features.Posts.DTOs;
using StudentBlogAPI.Features.Users.DTOs;
using StudentBlogAPI.Features.Users.Interfaces;

namespace StudentBlogAPI.Features.Users;

[ApiController]
[Route("api/v1/users")]
public class UserController(ILogger<UserController> logger, IUserService userService) : ControllerBase
{
    [HttpPost("register", Name = "RegisterUserAsync")]
    public async Task<ActionResult> RegisterUserAsync(UserRequest userRequest)
    {
        UserResponse? user = await userService.RegisterAsync(userRequest);
        
        return user is null
            ? BadRequest("Failed to register user")
            : Ok(user);
    }

    
    [HttpGet(Name = "GetUserAsync")]
    public async Task<ActionResult> GetAllUserAsync(
        [FromQuery] UserSearchRequest? searchParameters,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (searchParameters?.FirstName is not null || 
            searchParameters?.LastName is not null ||
            searchParameters?.UserName is not null || 
            searchParameters?.Email is not null)
        {
            return Ok(await userService.FindAsync(searchParameters));
        }
        
        IEnumerable<UserResponse> userDTOs = await userService.GetPagedAsync(pageNumber, pageSize);
        return Ok(userDTOs);

    }

    
    [HttpGet("{id:guid}", Name = "GetUserByIdAsync")]
    public async Task<ActionResult> GetUserByIdAsync(Guid id)
    {
        UserResponse? userDTO = await userService.GetByIdAsync(id);
        
        return userDTO is null
            ? NotFound("User not found")
            : Ok(userDTO);
    }


    [HttpGet("{id:guid}/posts", Name = "GetUserPostsAsync")]
    public async Task<ActionResult> GetUserPostsAsync(Guid id)
    {
        IEnumerable<PostResponse> postDTO = await userService.GetUserPostsAsync(id);
        
        return Ok(postDTO);
    }
    

    
    [HttpPut("{id:guid}", Name = "UpdateUserAsync")]
    public async Task<ActionResult> UpdateUserAsync(Guid id, 
                                                    [FromBody] UserUpdateRequest? updatedDetails)
    {
        if (updatedDetails is null)
            return BadRequest("Please provide updated details");
        
        UserResponse? userDTO = await userService.GetByIdAsync(id);
        
        if (userDTO is null)
        {
            return NotFound("User not found");
        }
        
        
        if (updatedDetails.FirstName is not null)
            userDTO.FirstName = updatedDetails.FirstName;
        
        if (updatedDetails.LastName is not null)
            userDTO.LastName = updatedDetails.LastName;
        
        if (updatedDetails.UserName is not null)
            userDTO.UserName = updatedDetails.UserName;
        
        if (updatedDetails.Email is not null)
            userDTO.Email = updatedDetails.Email;
        
        logger.LogDebug("Updating user: {UserId}", id);
        UserResponse? updatedUser = await userService.UpdateAsync(userDTO);
        
        return Ok(updatedUser);
    }

    
    [HttpDelete("{id:guid}", Name = "DeleteUserAsync")]
    public async Task<ActionResult> DeleteUserAsync(Guid id)
    {
        logger.LogDebug("Deleting user: {UserId}", id);
        bool result = await userService.DeleteByIdAsync(id);
        
        return result
            ? Ok(result)
            : BadRequest($"Failed to delete user: {id}");
    }
}