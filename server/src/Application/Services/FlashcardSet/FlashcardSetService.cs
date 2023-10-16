using StudyZen.Application.Dtos;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using StudyZen.Application.Validation;

namespace StudyZen.Application.Services;

public sealed class FlashcardSetService : IFlashcardSetService
{
    private readonly IFlashcardSetRepository _flashcardSets;
    private readonly ValidationHandler _validationHandler;

    public FlashcardSetService(IFlashcardSetRepository flashcardSets, ValidationHandler validationHandler)
    {
        _flashcardSets = flashcardSets;
        _validationHandler = validationHandler;
    }

    public async Task<FlashcardSetDto> CreateFlashcardSet(CreateFlashcardSetDto dto)
    {
        await _validationHandler.ValidateAsync(dto);
        var newFlashcardSet = new FlashcardSet(dto.LectureId, dto.Name, dto.Color);
        await _flashcardSets.Add(newFlashcardSet);
        return new FlashcardSetDto(newFlashcardSet);
    }

    public async Task<FlashcardSetDto?> GetFlashcardSetById(int flashcardSetId)
    {
        var flashcardSet = await _flashcardSets.GetById(flashcardSetId);

        return flashcardSet is null ? null : new FlashcardSetDto(flashcardSet);
    }

    public async Task<IReadOnlyCollection<FlashcardSetDto>> GetFlashcardSets(int? lectureId)
    {
        var flashcardSets = await _flashcardSets.GetAll();

        if (lectureId.HasValue)
        {
            flashcardSets = flashcardSets.Where(fs => fs.LectureId == lectureId).ToList();
        }

        return flashcardSets.Select(flashcardSet => new FlashcardSetDto(flashcardSet)).ToList();
    }

    public async Task<bool> UpdateFlashcardSet(int flashCardSetId, UpdateFlashcardSetDto dto)
    {
        var flashcardSet = await _flashcardSets.GetById(flashCardSetId);
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

    public async Task<bool> DeleteFlashcardSet(int flashcardSetId)
    {
        return await _flashcardSets.Delete(flashcardSetId);
    }

}