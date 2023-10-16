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

    public async Task<FlashcardDto> CreateFlashcard(CreateFlashcardDto dto)
    {
        await _validationHandler.ValidateAsync(dto);
        var newFlashcard = new Flashcard(dto.FlashcardSetId, dto.Front, dto.Back);
        await _flashcards.Add(newFlashcard);

        return new FlashcardDto(newFlashcard);
    }

    public async Task<FlashcardDto?> GetFlashcardById(int flashcardId)
    {
        var flashcard = await _flashcards.GetById(flashcardId);

        return flashcard is null ? null : new FlashcardDto(flashcard);
    }

    public async Task<IReadOnlyCollection<FlashcardDto>> GetFlashcardsBySetId(int flashcardSetId)
    {
        var setFlashcards = await _flashcards.GetFlashcardsBySetId(flashcardSetId);

        return setFlashcards.Select(flashcard => new FlashcardDto(flashcard)).ToList();
    }

    public async Task<bool> UpdateFlashcard(int flashcardId, UpdateFlashcardDto dto)
    {
        var flashcard = await _flashcards.GetById(flashcardId);
        if (flashcard is null)
        {
            return false;
        }

        await _validationHandler.ValidateAsync(dto);
        flashcard.Front = dto.Front ?? flashcard.Front;
        flashcard.Back = dto.Back ?? flashcard.Back;
        await _flashcards.Update(flashcard);

        return true;
    }

    public async Task<bool> DeleteFlashcard(int flashcardId)
    {
        return await _flashcards.Delete(flashcardId);
    }
}