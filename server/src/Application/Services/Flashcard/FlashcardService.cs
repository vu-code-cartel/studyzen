using StudyZen.Application.Dtos;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Services;

public sealed class FlashcardService : IFlashcardService
{
    private readonly IFlashcardRepository _flashcards;

    public FlashcardService(IFlashcardRepository flashcards)
    {
        _flashcards = flashcards;
    }

    public FlashcardDto CreateFlashcard(CreateFlashcardDto dto)
    {
        var newFlashcard = new Flashcard(dto.FlashcardSetId, dto.Question, dto.Answer);
        _flashcards.Add(newFlashcard);
        
        return new FlashcardDto(newFlashcard);
    }

    public FlashcardDto? GetFlashcardById(int flashcardId)
    {
       var flashcard = _flashcards.GetById(flashcardId);
       
       return flashcard != null ? new FlashcardDto(flashcard) : null;
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

        flashcard.Question = dto.Question ?? flashcard.Question;
        flashcard.Answer = dto.Answer ?? flashcard.Answer;
        _flashcards.Update(flashcard);

        return true;
    }

    public bool DeleteFlashcard(int flashcardId)
    {
        return _flashcards.Delete(flashcardId);
    }
}