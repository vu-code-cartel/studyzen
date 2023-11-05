using FluentValidation;
using Microsoft.AspNetCore.Identity;
using StudyZen.Application.Exceptions;
using StudyZen.Application.Services;
using StudyZen.Domain.Constraints;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Validation;

public static class ApplicationUserValidationExtensions
{
    public static IRuleBuilderOptions<T, string?> Email<T>(this IRuleBuilder<T, string?> ruleBuilder, UserManager<ApplicationUser> userManager)
    {
        return ruleBuilder
            .MustNotBeNullOrWhitespace()
            .LengthMustNotExceed(ApplicationUserConstraints.EmailMaxLength)
            .EmailAddress()
            .MustAsync(async (email, cancellationToken) => await IsUniqueEmail(email, userManager, cancellationToken))
            .WithErrorCode(ValidationErrorCodes.UserAlreadyExists);
    }

    public static IRuleBuilderOptions<T, string?> FirstName<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MustNotBeNullOrWhitespace()
            .LengthMustNotExceed(ApplicationUserConstraints.FirstNameMaxLength);
    }

    public static IRuleBuilderOptions<T, string?> LastName<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MustNotBeNullOrWhitespace()
            .LengthMustNotExceed(ApplicationUserConstraints.LastNameMaxLength);
    }

    private static async Task<bool> IsUniqueEmail(string email, UserManager<ApplicationUser> userManager, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user != null)
        {
            throw new UserAlreadyExistsException("email", email);
        }
        return true;
    }
}
