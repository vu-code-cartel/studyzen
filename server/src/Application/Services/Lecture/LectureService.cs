using System.Security.Claims;
using Microsoft.AspNetCore.Http;
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
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LectureService(IUnitOfWork unitOfWork, ValidationHandler validationHandler, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _validationHandler = validationHandler;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<LectureDto> CreateLecture(CreateLectureDto dto)
    {
        await _validationHandler.ValidateAsync(dto);

        var newLecture = new Lecture(dto.CourseId, dto.Name, dto.Content);

        var course = await _unitOfWork.Courses.GetByIdChecked(newLecture.CourseId);

        var applicationUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                    ?? throw new InvalidOperationException("Unable to retrieve user identity.");

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

    public async Task UpdateLecture(int lectureId, UpdateLectureDto dto)
    {
        await _validationHandler.ValidateAsync(dto);

        var lecture = await _unitOfWork.Lectures.GetByIdChecked(lectureId);

        var applicationUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                    ?? throw new InvalidOperationException("Unable to retrieve user identity.");

        if (!lecture.CreatedBy.User.Equals(applicationUserId))
        {
            throw new AccessDeniedException();
        }

        lecture.Name = dto.Name ?? lecture.Name;
        lecture.Content = dto.Content ?? lecture.Content;

        await _unitOfWork.SaveChanges();
    }

    public async Task DeleteLecture(int lectureId)
    {
        var lecture = await _unitOfWork.Lectures.GetByIdChecked(lectureId);

        var applicationUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                    ?? throw new InvalidOperationException("Unable to retrieve user identity.");

        if (!lecture.CreatedBy.User.Equals(applicationUserId))
        {
            throw new AccessDeniedException();
        }

        _unitOfWork.Lectures.Delete(lecture);
        await _unitOfWork.SaveChanges();
    }
}