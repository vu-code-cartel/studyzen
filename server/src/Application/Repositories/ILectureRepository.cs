using StudyZen.Domain.Entities;

namespace StudyZen.Application.Repositories;

public interface ILectureRepository : IRepository<Lecture>
{
    Task DeleteByIdChecked(int lectureId, string applicationUserId);
}