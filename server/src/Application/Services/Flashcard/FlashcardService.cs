using StudyZen.Application.Dtos;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using FluentValidation;

namespace StudyZen.Application.Services;

public sealed class FlashcardService : IFlashcardService
{
    private readonly IFlashcardRepository _flashcards;
    private readonly IValidator<Flashcard> _updatedFlashcardValidator;


    public FlashcardService(IFlashcardRepository flashcards, IValidator<Flashcard> updatedFlashcardValidator)
    {
        _flashcards = flashcards;
        _updatedFlashcardValidator = updatedFlashcardValidator;
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
        _updatedFlashcardValidator.ValidateAndThrow(flashcard);
        _flashcards.Update(flashcard);

        return flashcard;
    }

    public void DeleteFlashcard(int flashcardId)
    {
        _flashcards.Delete(flashcardId);
    }
}