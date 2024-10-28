using StudentBlogAPI.Features.Common.Interfaces;
using StudentBlogAPI.Features.Users.DTOs;

namespace StudentBlogAPI.Features.Users.Mappers;

public class RegistrationMapper : IMapper<User, RegistrationDTO>
{
    public RegistrationDTO MapToDTO(User model)
    {
        return new RegistrationDTO
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserName = model.UserName,
            Email = model.Email
        };
    }

    public User MapToModel(RegistrationDTO dto)
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