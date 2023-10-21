using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence;

public sealed class FlashcardSetRepository : Repository<FlashcardSet>, IFlashcardSetRepository
{
    public FlashcardSetRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Flashcard>> GetFlashcardsBySet(int flashcardSetId)
    {
        var flashcardSet = await GetByIdOrThrow(
            flashcardSetId,
            fs => fs.Flashcards);

        return flashcardSet.Flashcards;
    }
}