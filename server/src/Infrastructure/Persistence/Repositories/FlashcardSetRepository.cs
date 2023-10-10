using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using StudyZen.Infrastructure.Services;

namespace StudyZen.Infrastructure.Persistence;

public sealed class FlashcardSetRepository : Repository<FlashcardSet>, IFlashcardSetRepository
{
    public FlashcardSetRepository(IFileService fileService, ApplicationDbContext dbContext) : base("flashcardSets", fileService, dbContext)
    {
    }
}