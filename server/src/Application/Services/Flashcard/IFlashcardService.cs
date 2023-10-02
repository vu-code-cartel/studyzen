using StudyZen.Application.Dtos;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Services;

public interface IFlashcardService
{
    Flashcard CreateFlashcard(CreateFlashcardDto dto);
    Flashcard? GetFlashcardById(int flashcardId);
    IReadOnlyCollection<Flashcard> GetFlashcardsBySetId(int flashcardSetId);
    bool UpdateFlashcard(int flashcardId, UpdateFlashcardDto dto);
    void DeleteFlashcard(int flashcardId);
}