using StudentBlogAPI.Common.Interfaces;
using StudentBlogAPI.Features.Users.DTOs;

namespace StudentBlogAPI.Features.Users.Mappers;

public class RegistrationMapper : IMapper<User, UserRequest>
{
    public UserRequest MapToDTO(User model)
    {
        return new UserRequest
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserName = model.UserName,
            Email = model.Email
        };
    }

    public User MapToModel(UserRequest dto)
    {
        return new User
        {
            FirstName = dto.FirstName!,
            LastName = dto.LastName!,
            UserName = dto.UserName!,
            Email = dto.Email ?? string.Empty
        };
    }
}