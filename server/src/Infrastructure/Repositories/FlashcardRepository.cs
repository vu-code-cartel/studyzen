using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using StudyZen.Infrastructure.Services;

namespace StudyZen.Infrastructure.Repositories;

public sealed class FlashcardRepository : Repository<Flashcard>, IFlashcardRepository
{
    public FlashcardRepository(IFileService fileService) : base("flashcards", fileService)
    {
    }
}