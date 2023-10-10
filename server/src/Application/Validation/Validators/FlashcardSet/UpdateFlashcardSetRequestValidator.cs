using FluentValidation;
using StudyZen.Application.Dtos;

namespace StudyZen.Application.Validation;

public class UpdateFlashcardSetRequestValidator : AbstractValidator<UpdateFlashcardSetDto>
{
    public UpdateFlashcardSetRequestValidator()
    {
        RuleFor(f => f.Name)
            .FlashcardSetName()
            .Unless(f => f.Name is null);
    }
}