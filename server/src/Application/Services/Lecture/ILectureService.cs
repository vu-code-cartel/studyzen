using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public interface ILectureService
{
    Task<LectureDto> CreateLecture(CreateLectureDto dto);
    Task<LectureDto> GetLectureById(int lectureId);
    Task<IReadOnlyCollection<LectureDto>> GetLecturesByCourseId(int courseId);
    Task UpdateLecture(int lectureId, UpdateLectureDto dto);
    Task DeleteLecture(int lectureId);
}