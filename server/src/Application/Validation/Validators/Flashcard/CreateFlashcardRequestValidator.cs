using FluentValidation;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;

namespace StudyZen.Application.Validation;

public class CreateFlashcardRequestValidator : AbstractValidator<CreateFlashcardDto>
{
    public CreateFlashcardRequestValidator(IFlashcardSetService flashcardSetService)
    {
        RuleFor(f => f.Front)
            .FlashcardFront();
        RuleFor(f => f.Back)
            .FlashcardBack();
        RuleFor(f => f.FlashcardSetId)
            .FlashcardSetId(flashcardSetService);
    }
}