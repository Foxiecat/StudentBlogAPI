using Microsoft.AspNetCore.Mvc;
using StudentBlogAPI.Features.Users.DTOs;
using StudentBlogAPI.Features.Users.Interfaces;

namespace StudentBlogAPI.Features.Users;

[ApiController]
[Route("api/v1/users")]
public class UserController(ILogger<UserController> logger, IUserService userService) : ControllerBase
{
    [HttpPost("register", Name = "RegisterUserAsync")]
    public async Task<ActionResult> RegisterUserAsync(RegistrationDTO registrationDTO)
    {
        UserDTO? user = await userService.RegisterAsync(registrationDTO);
        
        return user is null
            ? BadRequest("Failed to register user")
            : Ok(user);
    }

    [HttpGet(Name = "GetUserAsync")]
    public async Task<ActionResult> GetAllUserAsync(
        [FromQuery] SearchParameters? searchParameters,
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
        
        IEnumerable<UserDTO> userDTOs = await userService.GetPagedAsync(pageNumber, pageSize);
        return Ok(userDTOs);

    }

    [HttpGet("{id:guid}", Name = "GetUserByIdAsync")]
    public async Task<ActionResult> GetUserByIdAsync(Guid id)
    {
        UserDTO? userDTO = await userService.GetByIdAsync(id);
        return userDTO is null
            ? BadRequest("User not found")
            : Ok(userDTO);
    }

    [HttpPut("{id:guid}", Name = "UpdateUserAsync")]
    public async Task<ActionResult> UpdateUserAsync(Guid id, 
                                                    [FromQuery] SearchParameters? updatedDetails)
    {
        UserDTO? userDTO = await userService.GetByIdAsync(id);
        
        if (updatedDetails is null)
            return NotFound("Please provide updated details");

        
        if (updatedDetails.FirstName is not null)
            userDTO.FirstName = updatedDetails.FirstName;
        
        if (updatedDetails.LastName is not null)
            userDTO.LastName = updatedDetails.LastName;
        
        if (updatedDetails.UserName is not null)
            userDTO.UserName = updatedDetails.UserName;
        
        if (updatedDetails.Email is not null)
            userDTO.Email = updatedDetails.Email;
        
        logger.LogDebug("Updating user: {UserId}", id);
        UserDTO? updatedUser = await userService.UpdateAsync(userDTO);
        
        return updatedUser is null
            ? NotFound("User not found")
            : Ok(updatedUser);
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