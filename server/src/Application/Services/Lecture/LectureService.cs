using StudyZen.Application.Dtos;
using StudyZen.Application.Extensions;
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

    public async Task<LectureDto> CreateLecture(CreateLectureDto dto)
    {
        await _validationHandler.ValidateAsync(dto);

        var newLecture = new Lecture(dto.CourseId, dto.Name, dto.Content);

        _unitOfWork.Lectures.Add(newLecture);
        await _unitOfWork.SaveChanges();

        return newLecture.ToDto();
    }

    public async Task<LectureDto?> GetLectureById(int lectureId)
    {
        var lecture = await _unitOfWork.Lectures.GetById(lectureId);
        return lecture?.ToDto();
    }

    public async Task<IReadOnlyCollection<LectureDto>> GetLecturesByCourseId(int courseId)
    {
        var course = await _unitOfWork.Courses.GetById(courseId, c => c.Lectures);
        if (course is null)
        {
            return new List<LectureDto>(); // TODO: return error
        }

        return course.Lectures.ToDtos();
    }

    public async Task<bool> UpdateLecture(int lectureId, UpdateLectureDto dto)
    {
        await _validationHandler.ValidateAsync(dto);

        var lecture = await _unitOfWork.Lectures.GetById(lectureId);
        if (lecture is null)
        {
            return false;
        }

        lecture.Name = dto.Name ?? lecture.Name;
        lecture.Content = dto.Content ?? lecture.Content;

        _unitOfWork.Lectures.Update(lecture);
        await _unitOfWork.SaveChanges();

        return true;
    }

    public async Task<bool> DeleteLecture(int lectureId)
    {
        var lecture = await _unitOfWork.Lectures.GetById(lectureId);
        if (lecture is null)
        {
            return false;
        }

        _unitOfWork.Lectures.Delete(lecture);
        await _unitOfWork.SaveChanges();

        return true;
    }
}