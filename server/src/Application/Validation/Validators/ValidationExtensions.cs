using FluentValidation;

namespace StudyZen.Application.Validation;

// might be a useful reference for future: https://github.com/FluentValidation/FluentValidation/issues/1688
public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string?> NotNullRule<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotNull()
            .WithErrorCode(ValidationErrorCodes.Null);
    }

    public static IRuleBuilderOptions<T, string?> NotNullOrWhitespaceRule<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.NullOrWhitespace);
    }

    public static IRuleBuilderOptions<T, string?> MaxLengthRule<T>(this IRuleBuilder<T, string?> ruleBuilder, int maxLength)
    {
        return ruleBuilder
            .MaximumLength(maxLength)
            .WithErrorCode(ValidationErrorCodes.MaxLength);
    }
}