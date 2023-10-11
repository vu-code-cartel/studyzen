using FluentValidation;
using StudyZen.Application.Services;
using StudyZen.Domain.Constraints;

namespace StudyZen.Application.Validation;

public static class FlashcardSetValidationExtensions
{
    public static IRuleBuilderOptions<T, string?> FlashcardSetName<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MustNotBeNullOrWhitespace()
            .MustNotExceedLength(FlashcardSetConstraints.NameMaxLength);
    }

    public static IRuleBuilderOptions<T, int> FlashcardSetId<T>(this IRuleBuilder<T, int> ruleBuilder, IFlashcardSetService flashcardSetService)
    {
        return ruleBuilder
            .Must(id => flashcardSetService.GetFlashcardSetById(id) is not null)
            .WithErrorCode(ValidationErrorCodes.NotFound);
    }
}