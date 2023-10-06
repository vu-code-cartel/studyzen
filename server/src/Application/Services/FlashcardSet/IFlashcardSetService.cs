using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public interface IFlashcardSetService
{
    FlashcardSetDto CreateFlashcardSet(CreateFlashcardSetDto dto);
    FlashcardSetDto? GetFlashcardSetById(int flashcardSetId);
    IReadOnlyCollection<FlashcardSetDto> GetAllFlashcardSets();
    IReadOnlyCollection<FlashcardSetDto> GetFlashcardSetsByLectureId(int? lectureId);
    bool UpdateFlashcardSet(int flashCardSetId, UpdateFlashcardSetDto dto);
    bool DeleteFlashcardSet(int flashcardSetId);
}