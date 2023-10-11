using FluentValidation;
using StudyZen.Domain.Constraints;

namespace StudyZen.Application.Validation;

public static class FlashcardValidationExtensions
{
    public static IRuleBuilderOptions<T, string?> FlashcardFront<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MustNotBeNullOrWhitespace()
            .MustNotExceedLength(FlashcardConstraints.FrontMaxLength);
    }

    public static IRuleBuilderOptions<T, string?> FlashcardBack<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MustNotBeNullOrWhitespace()
            .MustNotExceedLength(FlashcardConstraints.BackMaxLength);
    }
}