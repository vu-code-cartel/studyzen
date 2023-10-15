using StudyZen.Domain.Entities;

namespace StudyZen.Application.Repositories;

public interface ILectureRepository : IRepository<Lecture>
{
    Task<List<Lecture>> GetLecturesByCourseId(int courseId);
}