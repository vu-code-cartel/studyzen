using StudyZen.Domain.Entities;

namespace StudyZen.Application.Repositories;

public interface IFlashcardSetRepository : IRepository<FlashcardSet>
{
    Task<List<Flashcard>> GetFlashcardsBySet(int flashcardSetId);
}