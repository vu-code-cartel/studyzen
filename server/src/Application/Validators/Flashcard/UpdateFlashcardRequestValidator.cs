using FluentValidation;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Validators;

public class UpdateFlashcardRequestValidator : AbstractValidator<UpdateFlashcardDto>
{
    public UpdateFlashcardRequestValidator()
    {
        RuleFor(flashcard => flashcard.Question).MaximumLength(50).WithMessage("Question must not exceed 50 symbols!");
        RuleFor(flashcard => flashcard.Answer).MaximumLength(50).WithMessage("Answer must not exceed 50 symbols!");
    }
}