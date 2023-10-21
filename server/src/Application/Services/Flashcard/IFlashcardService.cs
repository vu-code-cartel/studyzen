using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public interface IFlashcardService
{
    Task<FlashcardDto> CreateFlashcard(CreateFlashcardDto dto);
    Task<IReadOnlyCollection<FlashcardDto>> CreateFlashcards(IReadOnlyCollection<CreateFlashcardDto> dtos);
    Task<FlashcardDto> GetFlashcardById(int flashcardId);
    Task<IReadOnlyCollection<FlashcardDto>> GetFlashcardsBySetId(int flashcardSetId);
    Task UpdateFlashcard(int flashcardId, UpdateFlashcardDto dto);
    Task DeleteFlashcard(int flashcardId);
}