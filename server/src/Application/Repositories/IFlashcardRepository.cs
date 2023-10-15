using StudyZen.Domain.Entities;

namespace StudyZen.Application.Repositories;

public interface IFlashcardRepository : IRepository<Flashcard>
{
    Task<List<Flashcard>> GetFlashcardsBySetId(int setId);
}