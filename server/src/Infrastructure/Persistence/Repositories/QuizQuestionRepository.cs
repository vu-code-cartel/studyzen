using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence;

public sealed class QuizQuestionRepository : Repository<QuizQuestion>, IQuizQuestionRepository
{
    public QuizQuestionRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
