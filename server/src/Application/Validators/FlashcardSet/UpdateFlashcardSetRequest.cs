using FluentValidation;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Validators;

public class UpdateFlashcardSetRequestValidator : AbstractValidator<UpdateFlashcardSetDto>
{
    public UpdateFlashcardSetRequestValidator()
    {
        RuleFor(flashcardSet => flashcardSet.Name).NotEmpty().WithMessage("Name must not be empty!").MaximumLength(50).WithMessage("Name must not exceed 50 symbols!");
    }
}