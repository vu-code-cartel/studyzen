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

    public Flashcard CreateFlashcard(CreateFlashcardDto dto)
    {
        var newFlashcard = new Flashcard(dto.FlashcardSetId, dto.Question, dto.Answer);
        _flashcards.Add(newFlashcard);
        return newFlashcard;
    }

    public Flashcard? GetFlashcardById(int flashcardId)
    {
        return _flashcards.GetById(flashcardId);
    }

    public IReadOnlyCollection<Flashcard> GetFlashcardsBySetId(int flashcardSetId)
    {
        var allFlashcards = _flashcards.GetAll();
        var setFlashcards = allFlashcards.Where(f => f.FlashcardSetId == flashcardSetId).ToList();
        return setFlashcards;
    }

    public Flashcard? UpdateFlashcard(int flashcardId, UpdateFlashcardDto dto)
    {
        var flashcard = _flashcards.GetById(flashcardId);
        if (flashcard is null)
        {
            return null;
        }

        flashcard.Question = dto.Question ?? flashcard.Question;
        flashcard.Answer = dto.Answer ?? flashcard.Answer;
        _flashcards.Update(flashcard);

        return flashcard;
    }

    public void DeleteFlashcard(int flashcardId)
    {
        _flashcards.Delete(flashcardId);
    }
}