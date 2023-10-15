using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace StudyZen.Infrastructure.Persistence;

public sealed class LectureRepository : Repository<Lecture>, ILectureRepository
{
    public LectureRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
    public async Task<List<Lecture>> GetLecturesByCourseId(int courseId)
    {
        var lectures = await _dbContext.Lectures
                                           .Where(l => l.CourseId == courseId)
                                           .ToListAsync();

        return lectures;
    }
}