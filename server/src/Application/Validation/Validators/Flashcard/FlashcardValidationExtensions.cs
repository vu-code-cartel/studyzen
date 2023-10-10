using FluentValidation;
using StudyZen.Domain.Constraints;

namespace StudyZen.Application.Validation;

public static class FlashcardValidationExtensions
{
    public static IRuleBuilderOptions<T, string?> FlashcardFront<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.MustNotBeEmpty)
            .MinimumLength(FlashcardConstraints.FrontMinLength)
            .WithErrorCode(ValidationErrorCodes.TooShort)
            .MaximumLength(FlashcardConstraints.FrontMaxLength)
            .WithErrorCode(ValidationErrorCodes.TooLong);
    }

    public static IRuleBuilderOptions<T, string?> FlashcardBack<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.MustNotBeEmpty)
            .MinimumLength(FlashcardConstraints.BackMinLength)
            .WithErrorCode(ValidationErrorCodes.TooShort)
            .MaximumLength(FlashcardConstraints.BackMaxLength)
            .WithErrorCode(ValidationErrorCodes.TooLong);
    }
}