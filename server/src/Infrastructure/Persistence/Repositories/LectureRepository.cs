using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using StudyZen.Infrastructure.Services;

namespace StudyZen.Infrastructure.Persistence;

public sealed class LectureRepository : Repository<Lecture>, ILectureRepository
{
    public LectureRepository(IFileService fileService, ApplicationDbContext dbContext) : base("lectures", fileService, dbContext)
    {
    }
}