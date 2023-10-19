using StudyZen.Application.Dtos;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using StudyZen.Application.Validation;

namespace StudyZen.Application.Services;

public sealed class FlashcardService : IFlashcardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ValidationHandler _validationHandler;

    public FlashcardService(IUnitOfWork unitOfWork, ValidationHandler validationHandler)
    {
        _unitOfWork = unitOfWork;
        _validationHandler = validationHandler;
    }

    public async Task<FlashcardDto> CreateFlashcard(CreateFlashcardDto dto)
    {
        await _validationHandler.ValidateAsync(dto);

        var newFlashcard = new Flashcard(dto.FlashcardSetId, dto.Front, dto.Back);

        _unitOfWork.Flashcards.Add(newFlashcard);
        await _unitOfWork.SaveChanges();

        return new FlashcardDto(newFlashcard);
    }

    public async Task<IReadOnlyCollection<FlashcardDto>> CreateFlashcards(IEnumerable<CreateFlashcardDto> dtos)
    {
        var results = new List<FlashcardDto>();

        foreach (var dto in dtos)
        {
            var result = await CreateFlashcard(dto);
            results.Add(result);
        }

        return results;
    }

    public async Task<FlashcardDto?> GetFlashcardById(int flashcardId)
    {
        var flashcard = await _unitOfWork.Flashcards.GetById(flashcardId);

        return flashcard is null ? null : new FlashcardDto(flashcard);
    }

    public async Task<IReadOnlyCollection<FlashcardDto>> GetFlashcardsBySetId(int flashcardSetId)
    {
        var setFlashcards = await _unitOfWork.Flashcards.GetFlashcardsBySetId(flashcardSetId);

        return setFlashcards.Select(flashcard => new FlashcardDto(flashcard)).ToList();
    }

    public async Task<bool> UpdateFlashcard(int flashcardId, UpdateFlashcardDto dto)
    {
        var flashcard = await _unitOfWork.Flashcards.GetById(flashcardId);
        if (flashcard is null)
        {
            return false;
        }

        await _validationHandler.ValidateAsync(dto);

        flashcard.Front = dto.Front ?? flashcard.Front;
        flashcard.Back = dto.Back ?? flashcard.Back;

        await _unitOfWork.SaveChanges();

        return true;
    }

    public async Task<bool> DeleteFlashcard(int flashcardId)
    {
        var isSuccess = await _unitOfWork.Flashcards.Delete(flashcardId);
        if (isSuccess)
        {
            await _unitOfWork.SaveChanges();
        }

        return isSuccess;
    }
}