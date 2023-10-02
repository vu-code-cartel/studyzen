using FluentValidation;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;

namespace StudyZen.Application.Validators;

public class CreateFlashcardRequestValidator : AbstractValidator<CreateFlashcardDto>
{
    private readonly IFlashcardSetService _flashcardSetService;
    public CreateFlashcardRequestValidator(IFlashcardSetService flashcardSetService)
    {
        _flashcardSetService = flashcardSetService;
        RuleFor(flashcard => flashcard.Question).NotEmpty().WithMessage("Name must not be empty!").MaximumLength(50).WithMessage("Question must not exceed 50 symbols!");
        RuleFor(flashcard => flashcard.Answer).NotEmpty().WithMessage("Content cannot be null!").MaximumLength(50).WithMessage("Answer must not exceed 50 symbols!");
        RuleFor(flashcard => flashcard.FlashcardSetId).Must(IsValidFlashcardSetId).WithMessage("FlashcardSet with the given id does not exist!");
    }
    protected bool IsValidFlashcardSetId(int flashcardSetId)
    {
        return _flashcardSetService.GetFlashcardSetById(flashcardSetId) is not null;
    }
}