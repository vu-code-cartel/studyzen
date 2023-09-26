using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using StudyZen.Infrastructure.Services;

namespace StudyZen.Infrastructure.Repositories;

public sealed class FlashcardSetRepository : Repository<FlashcardSet>, IFlashcardSetRepository
{
    public FlashcardSetRepository(IFileService fileService) : base("flashcardSets", fileService)
    {
    }
}