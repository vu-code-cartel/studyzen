using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using StudyZen.Infrastructure.Persistence;
using StudyZen.Infrastructure.Services;

namespace StudyZen.Infrastructure.Repositories;

public sealed class CourseRepository : Repository<Course>, ICourseRepository
{
    public CourseRepository(IFileService fileService, ApplicationDbContext dbContext) : base("courses", fileService, dbContext)
    {
    }
}