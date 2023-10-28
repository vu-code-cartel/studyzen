using FluentValidation;

namespace StudyZen.Application.Validation;

// might be a useful reference for future: https://github.com/FluentValidation/FluentValidation/issues/1688
public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty?> MustNotBeNull<T, TProperty>(this IRuleBuilder<T, TProperty?> ruleBuilder)
    {
        return ruleBuilder
            .NotNull()
            .WithErrorCode(ValidationErrorCodes.Null);
    }

    public static IRuleBuilderOptions<T, string?> MustNotBeNullOrWhitespace<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.NullOrWhitespace);
    }

    public static IRuleBuilderOptions<T, long> MustBeGreaterThan<T>(this IRuleBuilder<T, long> ruleBuilder, long comparedValue)
    {
        return ruleBuilder
            .GreaterThan(comparedValue)
            .WithErrorCode(ValidationErrorCodes.TooSmall);
    }

    public static IRuleBuilderOptions<T, long> MustBeLessThan<T>(this IRuleBuilder<T, long> ruleBuilder, long comparedValue)
    {
        return ruleBuilder
            .LessThan(comparedValue)
            .WithErrorCode(ValidationErrorCodes.TooLarge);
    }

    public static IRuleBuilderOptions<T, string?> LengthMustNotExceed<T>(this IRuleBuilder<T, string?> ruleBuilder, int maxLength)
    {
        return ruleBuilder
            .MaximumLength(maxLength)
            .WithErrorCode(ValidationErrorCodes.MaxLength);
    }
}