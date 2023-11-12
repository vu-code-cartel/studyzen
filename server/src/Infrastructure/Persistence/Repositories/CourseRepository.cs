using StudyZen.Application.Exceptions;
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
        var course = await GetByIdChecked(
            courseId,
            c => c.Lectures);

        return course.Lectures;
    }
    public async Task DeleteByIdChecked(int courseId, string applicationUserId)
    {
        var course = await GetByIdChecked(courseId);
        if (!course.CreatedBy.User.Equals(applicationUserId))
        {
            throw new AccessDeniedException();
        }
        Delete(course);
    }
}