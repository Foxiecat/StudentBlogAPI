using FluentValidation;
using StudentBlogAPI.Features.Users.DTOs;

namespace StudentBlogAPI.Features.Users.Validators;

public class UserDTOValidator : AbstractValidator<UserDTO>
{
    public UserDTOValidator()
    {
        RuleFor(userDTO => userDTO.Id)
            .NotEmpty();
        
        RuleFor(userDTO => userDTO.UserName)
            .NotEmpty().WithMessage("Username is required")
            .Length(3, 20).WithMessage("Username must be between 3 and 20 characters");
        
        RuleFor(userDTO => userDTO.FirstName)
            .NotEmpty().WithMessage("Firstname is required")
            .Length(2, 100).WithMessage("Firstname must be between 2 and 100 characters");
        
        RuleFor(userDTO => userDTO.LastName)
            .NotEmpty().WithMessage("Lastname is required")
            .Length(2, 100).WithMessage("Lastname must be between 2 and 100 characters");

        RuleFor(userDTO => userDTO.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid Email Address");
    }
}