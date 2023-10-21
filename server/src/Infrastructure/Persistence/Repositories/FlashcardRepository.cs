using Microsoft.EntityFrameworkCore;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence;

public sealed class FlashcardRepository : Repository<Flashcard>, IFlashcardRepository
{
    public FlashcardRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}