using StudyZen.Application.Dtos;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using StudyZen.Application.Validation;

namespace StudyZen.Application.Services;

public sealed class FlashcardService : IFlashcardService
{
    private readonly IFlashcardRepository _flashcards;
    private readonly ValidationHandler _validationHandler;


    public FlashcardService(IFlashcardRepository flashcards, ValidationHandler validationHandler)
    {
        _flashcards = flashcards;
        _validationHandler = validationHandler;
    }

    public FlashcardDto CreateFlashcard(CreateFlashcardDto dto)
    {
        _validationHandler.Validate(dto);
        var newFlashcard = new Flashcard(dto.FlashcardSetId, dto.Front, dto.Back);
        _flashcards.Add(newFlashcard);

        return new FlashcardDto(newFlashcard);
    }

    public IEnumerable<FlashcardDto> CreateFlashcards(IEnumerable<CreateFlashcardDto> dtos)
    {
        var createdFlashcards = new List<FlashcardDto>();

          foreach (var dto in dtos)
        {
            var newFlashcardDto = CreateFlashcard(dto);
            createdFlashcards.Add(newFlashcardDto);
        }

        return createdFlashcards;
    }

    public FlashcardDto? GetFlashcardById(int flashcardId)
    {
        var flashcard = _flashcards.GetById(flashcardId);

        return flashcard is null ? null : new FlashcardDto(flashcard);
    }

    public IReadOnlyCollection<FlashcardDto> GetFlashcardsBySetId(int flashcardSetId)
    {
        var allFlashcards = _flashcards.GetAll();
        var setFlashcards = allFlashcards.Where(f => f.FlashcardSetId == flashcardSetId).ToList();

        return setFlashcards.Select(flashcard => new FlashcardDto(flashcard)).ToList();
    }

    public bool UpdateFlashcard(int flashcardId, UpdateFlashcardDto dto)
    {
        var flashcard = _flashcards.GetById(flashcardId);
        if (flashcard is null)
        {
            return false;
        }

        _validationHandler.Validate(dto);
        flashcard.Front = dto.Front ?? flashcard.Front;
        flashcard.Back = dto.Back ?? flashcard.Back;
        _flashcards.Update(flashcard);

        return true;
    }

    public bool DeleteFlashcard(int flashcardId)
    {
        return _flashcards.Delete(flashcardId);
    }
}