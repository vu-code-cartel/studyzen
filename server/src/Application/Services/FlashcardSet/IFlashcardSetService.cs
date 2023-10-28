using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public interface IFlashcardSetService
{
    Task<FlashcardSetDto> CreateFlashcardSet(CreateFlashcardSetDto dto);
    Task<FlashcardSetDto> GetFlashcardSetById(int flashcardSetId);
    Task<IReadOnlyCollection<FlashcardSetDto>> GetFlashcardSets(int? lectureId = null);
    Task UpdateFlashcardSet(int flashCardSetId, UpdateFlashcardSetDto dto);
    Task DeleteFlashcardSet(int flashcardSetId);
}