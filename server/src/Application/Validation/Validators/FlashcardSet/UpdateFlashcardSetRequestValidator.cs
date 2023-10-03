using FluentValidation;
using StudyZen.Application.Dtos;
using StudyZen.Application.Validation.Validators;

namespace StudyZen.Application.Validators;

public class UpdateFlashcardSetRequestValidator : BaseValidator<UpdateFlashcardSetDto>
{
    public UpdateFlashcardSetRequestValidator()
    {
        RuleFor(flashcardSet => flashcardSet.Name)
        .Must(NullOrNotWhiteSpace)
        .WithMessage("Name must not be whitespace!")
        .MaximumLength(50)
        .WithMessage("Name must not exceed 50 symbols!");
    }
}