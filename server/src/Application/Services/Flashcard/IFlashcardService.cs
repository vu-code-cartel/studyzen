using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public interface IFlashcardService
{
    FlashcardDto CreateFlashcard(CreateFlashcardDto dto);
    public void CreateFlashcardsCollection(List<CreateFlashcardDto> flashcardsToCreate);
    FlashcardDto? GetFlashcardById(int flashcardId);
    IReadOnlyCollection<FlashcardDto> GetFlashcardsBySetId(int flashcardSetId);
    bool UpdateFlashcard(int flashcardId, UpdateFlashcardDto dto);
    bool DeleteFlashcard(int flashcardId);
}