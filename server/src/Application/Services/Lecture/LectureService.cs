using StudyZen.Application.Dtos;
using StudyZen.Application.Repositories;
using StudyZen.Application.Validation;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Services;

public sealed class LectureService : ILectureService
{
    private readonly ILectureRepository _lectures;
    private readonly ValidationHandler _validationHandler;

    public LectureService(ILectureRepository lectures, ValidationHandler validationHandler)
    {
        _lectures = lectures;
        _validationHandler = validationHandler;
    }

    public async Task<LectureDto> CreateLecture(CreateLectureDto dto)
    {
        await _validationHandler.ValidateAsync(dto);
        var newLecture = new Lecture(dto.CourseId, dto.Name, dto.Content);
        await _lectures.Add(newLecture);
        return new LectureDto(newLecture);
    }

    public async Task<LectureDto?> GetLectureById(int lectureId)
    {
        var lecture = await _lectures.GetById(lectureId);
        return lecture is null ? null : new LectureDto(lecture);
    }

    public async Task<IReadOnlyCollection<LectureDto>> GetLecturesByCourseId(int courseId)
    {
        var courseLectures = await _lectures.GetLecturesByCourseId(courseId);
        return courseLectures.Select(lecture => new LectureDto(lecture)).ToList();

    }

    public async Task<bool> UpdateLecture(int lectureId, UpdateLectureDto dto)
    {
        var lecture = await _lectures.GetById(lectureId);
        if (lecture is null)
        {
            return false;
        }

        _validationHandler.Validate(dto);
        lecture.Name = dto.Name ?? lecture.Name;
        lecture.Content = dto.Content ?? lecture.Content;
        _lectures.Update(lecture);

        return true;
    }

    public async Task<bool> DeleteLecture(int lectureId)
    {
        return await _lectures.Delete(lectureId);
    }
}