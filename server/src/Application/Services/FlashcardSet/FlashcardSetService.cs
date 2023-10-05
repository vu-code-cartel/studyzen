using StudyZen.Application.Dtos;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Services;

public sealed class FlashcardSetService : IFlashcardSetService
{
    private readonly IFlashcardSetRepository _flashcardSets;
    private readonly IFlashcardRepository _flashcards;

    public FlashcardSetService(IFlashcardSetRepository flashcardSets, IFlashcardRepository flashcards)
    {
        _flashcardSets = flashcardSets;
        _flashcards = flashcards;
    }

    public FlashcardSetDto CreateFlashcardSet(CreateFlashcardSetDto dto)
    {
        var newFlashcardSet = new FlashcardSet(dto.LectureId, dto.Name, dto.Color);
        _flashcardSets.Add(newFlashcardSet);
        return new FlashcardSetDto(newFlashcardSet);
    }

    public FlashcardSetDto? GetFlashcardSetById(int flashcardSetId)
    {
        var flashcardSet = _flashcardSets.GetById(flashcardSetId);

        return flashcardSet != null ? new FlashcardSetDto(flashcardSet) : null;
    }

    public IReadOnlyCollection<FlashcardSetDto> GetAllFlashcardSets()
    {
        var allFlashcardSets = _flashcardSets.GetAll();

        return allFlashcardSets.Select(flashcardSet => new FlashcardSetDto(flashcardSet)).ToList();
    }

    public IReadOnlyCollection<FlashcardSetDto> GetFlashcardSetsByLectureId(int? lectureId)
    {
        var allFlashcardSets = _flashcardSets.GetAll();
        var lectureFlashcardSets = allFlashcardSets.Where(fs => fs.LectureId == lectureId).ToList();
        return lectureFlashcardSets.Select(flashcardSet => new FlashcardSetDto(flashcardSet)).ToList();
    }

    public bool UpdateFlashcardSet(int flashCardSetId, UpdateFlashcardSetDto dto)
    {
        var flashcardSet = _flashcardSets.GetById(flashCardSetId);
        if (flashcardSet is null)
        {
            return false;
        }

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