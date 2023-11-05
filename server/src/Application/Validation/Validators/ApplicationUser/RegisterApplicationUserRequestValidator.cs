using FluentValidation;
using Microsoft.AspNetCore.Identity;
using StudyZen.Application.Dtos;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Validation;

public class RegisterApplicationUserRequestValidator : AbstractValidator<RegisterApplicationUserDto>
{
    public RegisterApplicationUserRequestValidator(UserManager<ApplicationUser> userManager)
    {
        RuleFor(u => u.Email)
            .Email(userManager);
        RuleFor(u => u.FirstName)
            .FirstName();
        RuleFor(u => u.LastName)
            .LastName();
    }
}