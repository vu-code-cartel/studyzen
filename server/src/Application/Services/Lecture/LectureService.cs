using StudyZen.Application.Dtos;
using StudyZen.Application.Exceptions;
using StudyZen.Application.Repositories;
using StudyZen.Application.Validation;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Services;

public sealed class LectureService : ILectureService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ValidationHandler _validationHandler;

    public LectureService(IUnitOfWork unitOfWork, ValidationHandler validationHandler)
    {
        _unitOfWork = unitOfWork;
        _validationHandler = validationHandler;
    }

    public async Task<LectureDto> CreateLecture(CreateLectureDto dto, string applicationUserId)
    {
        await _validationHandler.ValidateAsync(dto);

        var newLecture = new Lecture(dto.CourseId, dto.Name, dto.Content);

        var course = await _unitOfWork.Courses.GetByIdChecked(newLecture.CourseId);

        if (!course.CreatedBy.User.Equals(applicationUserId))
        {
            throw new AccessDeniedException();
        }

        _unitOfWork.Lectures.Add(newLecture);
        await _unitOfWork.SaveChanges();

        return new LectureDto(newLecture);
    }

    public async Task<LectureDto> GetLectureById(int lectureId)
    {
        var lecture = await _unitOfWork.Lectures.GetByIdChecked(lectureId);
        return new LectureDto(lecture);
    }

    public async Task<IReadOnlyCollection<LectureDto>> GetLecturesByCourseId(int courseId)
    {
        var lectures = await _unitOfWork.Courses.GetLecturesByCourse(courseId);
        return lectures.Select(l => new LectureDto(l)).ToList();
    }

    public async Task UpdateLecture(int lectureId, UpdateLectureDto dto, string applicationUserId)
    {
        await _validationHandler.ValidateAsync(dto);

        var lecture = await _unitOfWork.Lectures.GetByIdChecked(lectureId);

        if (!lecture.CreatedBy.User.Equals(applicationUserId))
        {
            throw new AccessDeniedException();
        }

        lecture.Name = dto.Name ?? lecture.Name;
        lecture.Content = dto.Content ?? lecture.Content;

        await _unitOfWork.SaveChanges();
    }

    public async Task DeleteLecture(int lectureId, string applicationUserId)
    {
        await _unitOfWork.Lectures.DeleteByIdChecked(lectureId, applicationUserId);
        await _unitOfWork.SaveChanges();
    }
}