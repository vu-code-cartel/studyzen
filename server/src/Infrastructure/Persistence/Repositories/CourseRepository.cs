using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using StudyZen.Infrastructure.Services;

namespace StudyZen.Infrastructure.Persistence;

public sealed class CourseRepository : Repository<Course>, ICourseRepository
{
    public CourseRepository(IFileService fileService, ApplicationDbContext dbContext) : base("courses", fileService, dbContext)
    {
    }
}