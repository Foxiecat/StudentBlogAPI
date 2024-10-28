using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StudentBlogAPI.Features.Users;
using StudentBlogAPI.Features.Users.DTOs;
using StudentBlogAPI.Features.Users.Interfaces;

namespace StudentBlogAPI.UnitTests.Features.Users;

public class UsersControllerTests
{
    private readonly UsersController _usersController;
    private readonly Mock<ILogger<UsersController>> _mockLogger = new();
    private readonly Mock<IUserService> _mockUserService = new();

    public UsersControllerTests()
    {
        _usersController = new UsersController(_mockLogger.Object, _mockUserService.Object);
    }


    [Fact]
    public async Task RegisterAsync_Should_Register_User()
    {
        // Arrange
        RegistrationDTO registrationDTO = new()
        {
            FirstName = "John",
            LastName = "Doe",
            UserName = "JohnDoe",
            Email = "johndoe@mail.com",
            Password = "johndoe1"
        };

        UserDTO expectedUserDTO = new()
        {
            Id = Guid.NewGuid(),
            FirstName = registrationDTO.FirstName,
            LastName = registrationDTO.LastName,
            UserName = registrationDTO.UserName,
            Email = registrationDTO.Email,
        };
        
        _mockUserService
            .Setup(service => service.RegisterAsync(It.IsAny<RegistrationDTO>()))
            .ReturnsAsync(expectedUserDTO);

        
        // Act
        ActionResult<UserDTO> result = await _usersController.RegisterUserAsync(registrationDTO);

        
        // Assert
        OkObjectResult returnValue = Assert.IsType<OkObjectResult>(result.Result);
        UserDTO returnValueDTO = Assert.IsType<UserDTO>(returnValue.Value);
        
        Assert.NotNull(returnValueDTO);
        Assert.Equal(expectedUserDTO.FirstName, returnValueDTO.FirstName);
        Assert.Equal(expectedUserDTO.LastName, returnValueDTO.LastName);
        Assert.Equal(expectedUserDTO.UserName, returnValueDTO.UserName);
        Assert.Equal(expectedUserDTO.Email, returnValueDTO.Email);
        
        _mockUserService.Verify(service => service.RegisterAsync(It.IsAny<RegistrationDTO>()), Times.Once);
    }
}