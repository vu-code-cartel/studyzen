using StudyZen.Application.Exceptions;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence;

public sealed class LectureRepository : Repository<Lecture>, ILectureRepository
{
    public LectureRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
    public async Task DeleteByIdChecked(int lectureId, string applicationUserId)
    {
        var lecture = await GetByIdChecked(lectureId);
        if (!lecture.CreatedBy.User.Equals(applicationUserId))
        {
            throw new AccessDeniedException();
        }
        Delete(lecture);
    }
}