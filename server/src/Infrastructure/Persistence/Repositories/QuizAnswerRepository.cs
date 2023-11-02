using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence;

public sealed class QuizAnswerRepository : Repository<QuizAnswer>, IQuizAnswerRepository
{
    public QuizAnswerRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
