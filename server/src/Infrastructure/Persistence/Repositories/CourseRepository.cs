using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence;

public sealed class CourseRepository : Repository<Course>, ICourseRepository
{
    public CourseRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}