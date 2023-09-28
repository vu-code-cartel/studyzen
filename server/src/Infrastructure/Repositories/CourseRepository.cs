using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using StudyZen.Infrastructure.Services;

namespace StudyZen.Infrastructure.Repositories;

public sealed class CourseRepository : Repository<Course>, ICourseRepository
{
    public CourseRepository(IFileService fileService) : base("courses", fileService)
    {
    }
}