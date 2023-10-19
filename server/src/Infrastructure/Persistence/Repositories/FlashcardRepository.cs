using Microsoft.EntityFrameworkCore;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence;

public sealed class FlashcardRepository : Repository<Flashcard>, IFlashcardRepository
{
    public FlashcardRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Flashcard>> GetFlashcardsBySetId(int setId)
    {
        return await Get(f => f.FlashcardSetId == setId);
    }
}