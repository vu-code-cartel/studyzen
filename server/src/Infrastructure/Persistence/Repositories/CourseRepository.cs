using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence;

public sealed class CourseRepository : Repository<Course>, ICourseRepository
{
    public CourseRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Lecture>> GetLecturesByCourse(int courseId)
    {
        var course = await GetByIdOrThrow(
            courseId,
            c => c.Lectures);

        return course.Lectures;
    }
}