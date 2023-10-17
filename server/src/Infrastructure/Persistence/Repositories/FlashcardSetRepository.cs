using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence;

public sealed class FlashcardSetRepository : Repository<FlashcardSet>, IFlashcardSetRepository
{
    public FlashcardSetRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}