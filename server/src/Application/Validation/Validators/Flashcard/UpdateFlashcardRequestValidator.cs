using FluentValidation;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;
using StudyZen.Application.Validation.Validators;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Validators;

public class UpdateFlashcardRequestValidator : AbstractValidator<UpdateFlashcardDto>
{
    public UpdateFlashcardRequestValidator()
    {
        RuleFor(f => f.Question)
        .NotEmpty()
        .Unless(f => f.Question is null)
        .WithMessage("Name must not be empty or whitespace!")
        .WithMessage("Question must not be empty or whitespace!")
        .MaximumLength(50)
        .WithMessage("Question must not exceed 50 symbols!");
        RuleFor(f => f.Answer)
        .NotEmpty()
        .Unless(f => f.Answer is null || f.Answer.Equals(""))
        .WithMessage("Answer must not be whitespace!")
        .MaximumLength(50)
        .WithMessage("Answer must not exceed 50 symbols!");
    }
}