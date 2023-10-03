using StudyZen.Application.Dtos;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using FluentValidation;
using StudyZen.Application.Validation;

namespace StudyZen.Application.Services;

public sealed class FlashcardSetService : IFlashcardSetService
{
    private readonly IFlashcardSetRepository _flashcardSets;
    private readonly IFlashcardRepository _flashcards;
    private readonly ValidationHandler _validationHandler;

    public FlashcardSetService(IFlashcardSetRepository flashcardSets, IFlashcardRepository flashcards, ValidationHandler validationHandler)
    {
        _flashcardSets = flashcardSets;
        _flashcards = flashcards;
        _validationHandler = validationHandler;
    }

    public FlashcardSet CreateFlashcardSet(CreateFlashcardSetDto dto)
    {
        _validationHandler.Validate(dto);
        var newFlashcardSet = new FlashcardSet(dto.LectureId, dto.Name, dto.Color);
        _flashcardSets.Add(newFlashcardSet);
        return newFlashcardSet;
    }

    public FlashcardSet? GetFlashcardSetById(int flashcardSetId)
    {
        var flashcardSet = _flashcardSets.GetById(flashcardSetId);
        return flashcardSet;
    }

    public IReadOnlyCollection<FlashcardSet> GetAllFlashcardSets()
    {
        var allFlashcardSets = _flashcardSets.GetAll();
        return allFlashcardSets;
    }

    public IReadOnlyCollection<FlashcardSet> GetFlashcardSetsByLectureId(int? lectureId)
    {
        var allFlashcardSets = _flashcardSets.GetAll();
        var lectureFlashcardSets = allFlashcardSets.Where(fs => fs.LectureId == lectureId).ToList();
        return lectureFlashcardSets;
    }

    public FlashcardSet? UpdateFlashcardSet(int flashCardSetId, UpdateFlashcardSetDto dto)
    {
        var flashcardSet = _flashcardSets.GetById(flashCardSetId);
        if (flashcardSet is null)
        {
            return null;
        }

        _validationHandler.Validate(dto);
        flashcardSet.Name = dto.Name ?? flashcardSet.Name;
        flashcardSet.Color = dto.Color ?? flashcardSet.Color;
        _flashcardSets.Update(flashcardSet);

        return flashcardSet;
    }

    public void DeleteFlashcardSet(int flashcardSetId)
    {
        DeleteFlashcardsBySetId(flashcardSetId);
        _flashcardSets.Delete(flashcardSetId);
    }

    private void DeleteFlashcardsBySetId(int flashcardSetId)
    {
        var allFlashcards = _flashcards.GetAll();
        var setFlashcards = allFlashcards.Where(f => f.FlashcardSetId == flashcardSetId);

        foreach (var setFlashcard in setFlashcards)
        {
            _flashcards.Delete(setFlashcard.Id);
        }
    }
}