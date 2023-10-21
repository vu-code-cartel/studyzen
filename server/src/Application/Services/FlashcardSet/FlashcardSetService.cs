using StudyZen.Application.Dtos;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using StudyZen.Application.Validation;

namespace StudyZen.Application.Services;

public sealed class FlashcardSetService : IFlashcardSetService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ValidationHandler _validationHandler;

    public FlashcardSetService(IUnitOfWork unitOfWork, ValidationHandler validationHandler)
    {
        _unitOfWork = unitOfWork;
        _validationHandler = validationHandler;
    }

    public async Task<FlashcardSetDto> CreateFlashcardSet(CreateFlashcardSetDto dto)
    {
        await _validationHandler.ValidateAsync(dto);

        var newFlashcardSet = new FlashcardSet(dto.LectureId, dto.Name, dto.Color);

        _unitOfWork.FlashcardSets.Add(newFlashcardSet);
        await _unitOfWork.SaveChanges();

        return new FlashcardSetDto(newFlashcardSet);
    }

    public async Task<FlashcardSetDto> GetFlashcardSetById(int flashcardSetId)
    {
        var flashcardSet = await _unitOfWork.FlashcardSets.GetByIdOrThrow(flashcardSetId);
        return new FlashcardSetDto(flashcardSet);
    }

    public async Task<IReadOnlyCollection<FlashcardSetDto>> GetFlashcardSets(int? lectureId)
    {
        var flashcardSets = await _unitOfWork.FlashcardSets.Get();

        if (lectureId.HasValue)
        {
            flashcardSets = flashcardSets.Where(fs => fs.LectureId == lectureId).ToList();
        }

        return flashcardSets.Select(flashcardSet => new FlashcardSetDto(flashcardSet)).ToList();
    }

    public async Task UpdateFlashcardSet(int flashCardSetId, UpdateFlashcardSetDto dto)
    {
        await _validationHandler.ValidateAsync(dto);

        var flashcardSet = await _unitOfWork.FlashcardSets.GetByIdOrThrow(flashCardSetId);

        flashcardSet.Name = dto.Name ?? flashcardSet.Name;
        flashcardSet.Color = dto.Color ?? flashcardSet.Color;

        await _unitOfWork.SaveChanges();
    }

    public async Task DeleteFlashcardSet(int flashcardSetId)
    {
        await _unitOfWork.FlashcardSets.DeleteByIdOrThrow(flashcardSetId);
        await _unitOfWork.SaveChanges();
    }
}