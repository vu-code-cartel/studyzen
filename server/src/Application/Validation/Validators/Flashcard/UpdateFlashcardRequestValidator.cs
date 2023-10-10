using FluentValidation;
using StudyZen.Application.Dtos;

namespace StudyZen.Application.Validation;

public class UpdateFlashcardRequestValidator : AbstractValidator<UpdateFlashcardDto>
{
    public UpdateFlashcardRequestValidator()
    {
        RuleFor(f => f.Front)
            .FlashcardFront()
            .Unless(f => f.Front is null);
        RuleFor(f => f.Back)
            .FlashcardBack()
            .Unless(f => f.Back is null);
    }
}