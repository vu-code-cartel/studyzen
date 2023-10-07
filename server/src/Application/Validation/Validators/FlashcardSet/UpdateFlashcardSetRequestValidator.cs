using FluentValidation;
using StudyZen.Application.Dtos;

namespace StudyZen.Application.Validation;

public class UpdateFlashcardSetRequestValidator : AbstractValidator<UpdateFlashcardSetDto>
{
    public UpdateFlashcardSetRequestValidator()
    {
        RuleFor(f => f.Name)
            .NotEmpty()
            .Unless(f => f.Name is null)
            .WithMessage("Name must not be empty or whitespace!")
            .MaximumLength(50)
            .WithMessage("Name must not exceed 50 symbols!");
    }
}