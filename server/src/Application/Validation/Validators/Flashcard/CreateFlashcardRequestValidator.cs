using FluentValidation;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;

namespace StudyZen.Application.Validation;

public class CreateFlashcardRequestValidator : AbstractValidator<CreateFlashcardDto>
{
    private readonly IFlashcardSetService _flashcardSetService;

    public CreateFlashcardRequestValidator(IFlashcardSetService flashcardSetService)
    {
        _flashcardSetService = flashcardSetService;
        RuleFor(flashcard => flashcard.Front)
            .NotEmpty()
            .WithMessage("Front must not be empty!")
            .MaximumLength(50)
            .WithMessage("Front must not exceed 50 symbols!");
        RuleFor(flashcard => flashcard.Back)
            .NotEmpty()
            .WithMessage("Back cannot be null!")
            .MaximumLength(50)
            .WithMessage("Back must not exceed 50 symbols!");
        RuleFor(flashcard => flashcard.FlashcardSetId)
            .Must(IsValidFlashcardSetId)
            .WithMessage("FlashcardSet with the given id does not exist!");
    }

    protected bool IsValidFlashcardSetId(int flashcardSetId)
    {
        return _flashcardSetService.GetFlashcardSetById(flashcardSetId) is not null;
    }
}