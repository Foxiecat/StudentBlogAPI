using Microsoft.AspNetCore.Mvc;
using StudentBlogAPI.Features.Users.DTOs;
using StudentBlogAPI.Features.Users.Interfaces;

namespace StudentBlogAPI.Features.Users;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController(ILogger<UsersController> logger, IUserService userService) : ControllerBase
{
    [HttpPost("register", Name = "RegisterUserAsync")]
    public async Task<ActionResult<UserDTO>> RegisterUserAsync(RegistrationDTO registrationDTO)
    {
        UserDTO? user = await userService.RegisterAsync(registrationDTO);
        return user is null
            ? BadRequest("Failed to register user")
            : Ok(user);
    }
}