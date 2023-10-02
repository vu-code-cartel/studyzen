using FluentValidation;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Validators;

public class UpdatedFlashcardSetValidator : AbstractValidator<FlashcardSet>
{
    public UpdatedFlashcardSetValidator()
    {
        RuleFor(flashcardSet => flashcardSet.Name).NotEmpty().WithMessage("Name must not be empty!");
    }
}