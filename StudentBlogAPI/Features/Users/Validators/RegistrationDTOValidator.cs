using FluentValidation;
using StudentBlogAPI.Features.Users.DTOs;

namespace StudentBlogAPI.Features.Users.Validators;

public class RegistrationDTOValidator : AbstractValidator<RegistrationDTO>
{
    public RegistrationDTOValidator()
    {
        RuleFor(rDTO => rDTO.UserName)
            .NotEmpty().WithMessage("Username is required")
            .Length(3, 20).WithMessage("Username must be between 3 and 20 characters");
        
        RuleFor(rDTO => rDTO.FirstName)
            .NotEmpty().WithMessage("Firstname is required")
            .Length(2, 100).WithMessage("Firstname must be between 2 and 100 characters");
        
        RuleFor(rDTO => rDTO.LastName)
            .NotEmpty().WithMessage("Lastname is required")
            .Length(2, 100).WithMessage("Lastname must be between 2 and 100 characters");
        
        RuleFor(rDTO => rDTO.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid Email: Must be a valid email address");

        RuleFor(rDTO => rDTO.Password)
            .NotEmpty().WithMessage("Password is required")
            .Length(6, 50).WithMessage("Invalid Password: Must be between 6 and 50 characters")
            .Matches("[0-9]+").WithMessage("Invalid Password: Must contain at least 1 number")
            .Matches("[A-Z]+").WithMessage("Invalid Password: Must contain at least 1 upper-case letter")
            .Matches("[a-z]+").WithMessage("Invalid Password: Must contain at least 1 lower-case letter")
            .Matches("[^æøåÆØÅ]+").WithMessage("Invalid Password: Cannot contain (æÆ, øØ, åÅ) characters");
    }
}