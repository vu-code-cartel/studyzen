using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public interface ILectureService
{
    Task<LectureDto> CreateLecture(CreateLectureDto dto);
    Task<LectureDto?> GetLectureById(int lectureId);
    Task<IReadOnlyCollection<LectureDto>> GetLecturesByCourseId(int courseId);
    Task<bool> UpdateLecture(int lectureId, UpdateLectureDto dto);
    Task<bool> DeleteLecture(int lectureId);
}