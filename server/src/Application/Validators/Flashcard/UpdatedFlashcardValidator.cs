using FluentValidation;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Validators;

public class UpdatedFlashcardValidator : AbstractValidator<Flashcard>
{

    public UpdatedFlashcardValidator()
    {
        RuleFor(flashcard => flashcard.Question).NotEmpty().WithMessage("Name must not be empty!");
        RuleFor(flashcard => flashcard.Answer).NotEmpty().WithMessage("Content cannot be null!");
    }
}