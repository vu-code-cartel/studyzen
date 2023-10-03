using StudyZen.Application.Dtos;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Services;

public interface ILectureService
{
    Lecture CreateLecture(CreateLectureDto dto);
    Lecture? GetLectureById(int lectureId);
    IReadOnlyCollection<Lecture> GetLecturesByCourseId(int courseId);
    bool UpdateLecture(int lectureId, UpdateLectureDto dto);
    bool DeleteLecture(int lectureId);
}