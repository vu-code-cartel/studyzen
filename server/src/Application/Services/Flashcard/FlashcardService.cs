using StudyZen.Application.Dtos;
using StudyZen.Application.Extensions;
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

        return newFlashcard.ToDto();
    }

    public async Task<IReadOnlyCollection<FlashcardDto>> CreateFlashcards(IReadOnlyCollection<CreateFlashcardDto> dtos)
    {
        foreach (var dto in dtos)
        {
            await _validationHandler.ValidateAsync(dto);
        }

        var flashcards = dtos.Select(dto => new Flashcard(dto.FlashcardSetId, dto.Front, dto.Back)).ToArray();

        _unitOfWork.Flashcards.Add(flashcards);
        await _unitOfWork.SaveChanges();

        return flashcards.ToDtos();
    }

    public async Task<FlashcardDto?> GetFlashcardById(int flashcardId)
    {
        var flashcard = await _unitOfWork.Flashcards.GetById(flashcardId);
        return flashcard?.ToDto();
    }

    public async Task<IReadOnlyCollection<FlashcardDto>> GetFlashcardsBySetId(int flashcardSetId)
    {
        var flashcardSet = await _unitOfWork.FlashcardSets.GetById(flashcardSetId, fs => fs.Flashcards);
        if (flashcardSet is null)
        {
            return new List<FlashcardDto>(); // TODO: change this to error
        }

        return flashcardSet.Flashcards.ToDtos();
    }

    public async Task<bool> UpdateFlashcard(int flashcardId, UpdateFlashcardDto dto)
    {
        await _validationHandler.ValidateAsync(dto);

        var flashcard = await _unitOfWork.Flashcards.GetById(flashcardId);
        if (flashcard is null)
        {
            return false;
        }

        flashcard.Front = dto.Front ?? flashcard.Front;
        flashcard.Back = dto.Back ?? flashcard.Back;

        _unitOfWork.Flashcards.Update(flashcard);
        await _unitOfWork.SaveChanges();

        return true;
    }

    public async Task<bool> DeleteFlashcard(int flashcardId)
    {
        var flashcard = await _unitOfWork.Flashcards.GetById(flashcardId);
        if (flashcard is null)
        {
            return false;
        }

        _unitOfWork.Flashcards.Delete(flashcard);
        await _unitOfWork.SaveChanges();

        return true;
    }
}