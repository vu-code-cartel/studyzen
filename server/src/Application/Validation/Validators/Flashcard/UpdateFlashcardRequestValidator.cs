using FluentValidation;
using StudyZen.Application.Dtos;

namespace StudyZen.Application.Validation;

public class UpdateFlashcardRequestValidator : AbstractValidator<UpdateFlashcardDto>
{
    public UpdateFlashcardRequestValidator()
    {
        RuleFor(f => f.Front)
            .NotEmpty()
            .Unless(f => f.Front is null)
            .WithMessage("Front must not be empty or whitespace!")
            .WithMessage("Front must not be empty or whitespace!")
            .MaximumLength(50)
            .WithMessage("Front must not exceed 50 symbols!");
        RuleFor(f => f.Back)
            .NotEmpty()
            .Unless(f => f.Back is null || f.Back.Equals(""))
            .WithMessage("Back must not be whitespace!")
            .MaximumLength(50)
            .WithMessage("Back must not exceed 50 symbols!");
    }
}