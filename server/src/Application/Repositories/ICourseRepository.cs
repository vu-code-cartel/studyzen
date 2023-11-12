using StudyZen.Domain.Entities;

namespace StudyZen.Application.Repositories;

public interface ICourseRepository : IRepository<Course>
{
    Task<List<Lecture>> GetLecturesByCourse(int courseId);
    Task DeleteByIdChecked(int courseId, string applicationUserId);
}