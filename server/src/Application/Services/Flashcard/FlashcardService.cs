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

    public async Task<IReadOnlyCollection<FlashcardDto>> CreateFlashcards(IReadOnlyCollection<CreateFlashcardDto> dtos)
    {
        var flashcards = new List<Flashcard>(dtos.Count);
        foreach (var dto in dtos)
        {
            await _validationHandler.ValidateAsync(dto);
            flashcards.Add(new Flashcard(dto.FlashcardSetId, dto.Front, dto.Back));
        }

        _unitOfWork.Flashcards.AddRange(flashcards);
        await _unitOfWork.SaveChanges();

        return flashcards.Select(f => new FlashcardDto(f)).ToList();
    }

    public async Task<FlashcardDto> GetFlashcardById(int flashcardId)
    {
        var flashcard = await _unitOfWork.Flashcards.GetByIdChecked(flashcardId);
        return new FlashcardDto(flashcard);
    }

    public async Task<IReadOnlyCollection<FlashcardDto>> GetFlashcardsBySetId(int flashcardSetId)
    {
        var flashcards = await _unitOfWork.FlashcardSets.GetFlashcardsBySet(flashcardSetId);
        return flashcards.Select(flashcard => new FlashcardDto(flashcard)).ToList();
    }

    public async Task UpdateFlashcard(int flashcardId, UpdateFlashcardDto dto)
    {
        await _validationHandler.ValidateAsync(dto);

        var flashcard = await _unitOfWork.Flashcards.GetByIdChecked(flashcardId);

        flashcard.Front = dto.Front ?? flashcard.Front;
        flashcard.Back = dto.Back ?? flashcard.Back;

        await _unitOfWork.SaveChanges();
    }

    public async Task DeleteFlashcard(int flashcardId)
    {
        await _unitOfWork.Flashcards.DeleteByIdChecked(flashcardId);
        await _unitOfWork.SaveChanges();
    }
}