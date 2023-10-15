using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public interface IFlashcardService
{
    Task<FlashcardDto> CreateFlashcard(CreateFlashcardDto dto);
    Task<FlashcardDto?> GetFlashcardById(int flashcardId);
    Task<IReadOnlyCollection<FlashcardDto>> GetFlashcardsBySetId(int flashcardSetId);
    Task<bool> UpdateFlashcard(int flashcardId, UpdateFlashcardDto dto);
    Task<bool> DeleteFlashcard(int flashcardId);
}