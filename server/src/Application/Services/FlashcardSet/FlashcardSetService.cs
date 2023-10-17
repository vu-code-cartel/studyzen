using StudyZen.Application.Dtos;
using StudyZen.Application.Extensions;
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

        return newFlashcardSet.ToDto();
    }

    public async Task<FlashcardSetDto?> GetFlashcardSetById(int flashcardSetId)
    {
        var flashcardSet = await _unitOfWork.FlashcardSets.GetById(flashcardSetId);
        return flashcardSet?.ToDto();
    }

    public async Task<IReadOnlyCollection<FlashcardSetDto>> GetFlashcardSets(int? lectureId)
    {
        if (lectureId.HasValue)
        {
            var lecture = await _unitOfWork.Lectures.GetById(lectureId.Value, l => l.FlashcardSets);
            if (lecture is null)
            {
                return new List<FlashcardSetDto>(); // TODO: return error
            }

            return lecture.FlashcardSets.ToDtos();
        }

        var allFlashcardSets = await _unitOfWork.FlashcardSets.GetAll();
        return allFlashcardSets.ToDtos();
    }

    public async Task<bool> UpdateFlashcardSet(int flashCardSetId, UpdateFlashcardSetDto dto)
    {
        await _validationHandler.ValidateAsync(dto);

        var flashcardSet = await _unitOfWork.FlashcardSets.GetById(flashCardSetId);
        if (flashcardSet is null)
        {
            return false;
        }

        flashcardSet.Name = dto.Name ?? flashcardSet.Name;
        flashcardSet.Color = dto.Color ?? flashcardSet.Color;

        _unitOfWork.FlashcardSets.Update(flashcardSet);
        await _unitOfWork.SaveChanges();

        return true;
    }

    public async Task<bool> DeleteFlashcardSet(int flashcardSetId)
    {
        var flashcardSet = await _unitOfWork.FlashcardSets.GetById(flashcardSetId);
        if (flashcardSet is null)
        {
            return false;
        }

        _unitOfWork.FlashcardSets.Delete(flashcardSet);
        await _unitOfWork.SaveChanges();

        return true;
    }
}