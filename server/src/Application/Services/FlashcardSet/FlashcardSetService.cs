using StudyZen.Application.Dtos;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
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

    public FlashcardSetDto CreateFlashcardSet(CreateFlashcardSetDto dto)
    {
        _validationHandler.Validate(dto);
        var newFlashcardSet = new FlashcardSet(dto.LectureId, dto.Name, dto.Color);
        _flashcardSets.Add(newFlashcardSet);
        return new FlashcardSetDto(newFlashcardSet);
    }

    public FlashcardSetDto? GetFlashcardSetById(int flashcardSetId)
    {
        var flashcardSet = _flashcardSets.GetById(flashcardSetId);

        return flashcardSet is null ? null : new FlashcardSetDto(flashcardSet);
    }

    public IReadOnlyCollection<FlashcardSetDto> GetFlashcardSets(int? lectureId)
    {
        var flashcardSets = _flashcardSets.GetAll();

        if (lectureId.HasValue)
        {
            flashcardSets = flashcardSets.Where(fs => fs.LectureId == lectureId).ToList();
        }

        return flashcardSets.Select(flashcardSet => new FlashcardSetDto(flashcardSet)).ToList();
    }

    public bool UpdateFlashcardSet(int flashCardSetId, UpdateFlashcardSetDto dto)
    {
        var flashcardSet = _flashcardSets.GetById(flashCardSetId);
        if (flashcardSet is null)
        {
            return false;
        }

        _validationHandler.Validate(dto);
        flashcardSet.Name = dto.Name ?? flashcardSet.Name;
        flashcardSet.Color = dto.Color ?? flashcardSet.Color;
        _flashcardSets.Update(flashcardSet);

        return true;
    }

    public bool DeleteFlashcardSet(int flashcardSetId)
    {
        DeleteFlashcardsBySetId(flashcardSetId);
        return _flashcardSets.Delete(flashcardSetId);
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