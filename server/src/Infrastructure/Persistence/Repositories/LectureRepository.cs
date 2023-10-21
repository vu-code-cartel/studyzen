using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence;

public sealed class LectureRepository : Repository<Lecture>, ILectureRepository
{
    public LectureRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}