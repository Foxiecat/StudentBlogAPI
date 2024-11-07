using StudentBlogAPI.Features.Common.Interfaces;
using StudentBlogAPI.Features.Users.DTOs;

namespace StudentBlogAPI.Features.Users.Mappers;

public class UserMapper : IMapper<User, UserResponse>
{
    public UserResponse MapToResponse(User model)
    {
        return new UserResponse
        {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserName = model.UserName,
            Email = model.Email,
            Created = model.Created,
            Updated = model.Updated
        };
    }

    public User MapToModel(UserResponse response)
    {
        return new User
        {
            Id = response.Id,
            FirstName = response.FirstName,
            LastName = response.LastName,
            UserName = response.UserName,
            Email = response.Email,
            Created = response.Created,
            Updated = response.Updated
        };
    }
}