using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace StudyZen.Infrastructure.Persistence;

public sealed class FlashcardRepository : Repository<Flashcard>, IFlashcardRepository
{
    public FlashcardRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
    public async Task<List<Flashcard>> GetFlashcardsBySetId(int setId)
    {
        var flashcards = await _dbContext.Flashcards
                                           .Where(f => f.FlashcardSetId == setId)
                                           .ToListAsync();

        return flashcards;
    }
}