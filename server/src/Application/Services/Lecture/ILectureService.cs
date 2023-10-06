using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public interface ILectureService
{
    LectureDto CreateLecture(CreateLectureDto dto);
    LectureDto? GetLectureById(int lectureId);
    IReadOnlyCollection<LectureDto> GetLecturesByCourseId(int courseId);
    bool UpdateLecture(int lectureId, UpdateLectureDto dto);
    bool DeleteLecture(int lectureId);
}