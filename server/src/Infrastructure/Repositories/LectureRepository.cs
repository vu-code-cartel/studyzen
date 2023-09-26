using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using StudyZen.Infrastructure.Services;

namespace StudyZen.Infrastructure.Repositories;

public sealed class LectureRepository : Repository<Lecture>, ILectureRepository
{
    public LectureRepository(IFileService fileService) : base("lectures", fileService)
    {
    }
}