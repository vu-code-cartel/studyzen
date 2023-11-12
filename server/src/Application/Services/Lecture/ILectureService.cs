using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public interface ILectureService
{
    Task<LectureDto> CreateLecture(CreateLectureDto dto, string applicationUserId);
    Task<LectureDto> GetLectureById(int lectureId);
    Task<IReadOnlyCollection<LectureDto>> GetLecturesByCourseId(int courseId);
    Task UpdateLecture(int lectureId, UpdateLectureDto dto, string applicationUserId);
    Task DeleteLecture(int lectureId, string applicationUserId);
}