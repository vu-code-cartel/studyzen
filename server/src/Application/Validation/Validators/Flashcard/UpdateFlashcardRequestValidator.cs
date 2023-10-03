using FluentValidation;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;
using StudyZen.Application.Validation.Validators;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Validators;

public class UpdateFlashcardRequestValidator : BaseValidator<UpdateFlashcardDto>
{
    public UpdateFlashcardRequestValidator()
    {
        RuleFor(flashcard => flashcard.Question)
        .Must(NullOrNotWhiteSpace)
        .WithMessage("Question must not be empty or whitespace!")
        .MaximumLength(50)
        .WithMessage("Question must not exceed 50 symbols!");
        RuleFor(flashcard => flashcard.Answer)
        .Must(NullEmptyOrNotWhitespace)
        .WithMessage("Answer must not be whitespace!")
        .MaximumLength(50)
        .WithMessage("Answer must not exceed 50 symbols!");
    }
}