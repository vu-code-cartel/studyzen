using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public interface IFlashcardService
{
    FlashcardDto CreateFlashcard(CreateFlashcardDto dto);
    public IEnumerable<FlashcardDto> CreateFlashcardsCollection(IEnumerable<CreateFlashcardDto> dtos);
    FlashcardDto? GetFlashcardById(int flashcardId);
    IReadOnlyCollection<FlashcardDto> GetFlashcardsBySetId(int flashcardSetId);
    bool UpdateFlashcard(int flashcardId, UpdateFlashcardDto dto);
    bool DeleteFlashcard(int flashcardId);
}