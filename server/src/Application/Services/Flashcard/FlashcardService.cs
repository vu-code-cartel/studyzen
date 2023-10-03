using StudyZen.Application.Dtos;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using FluentValidation;
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

    public Flashcard CreateFlashcard(CreateFlashcardDto dto)
    {
        _validationHandler.Validate(dto);
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

        _validationHandler.Validate(dto);
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