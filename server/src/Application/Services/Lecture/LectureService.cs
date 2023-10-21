using StudyZen.Application.Dtos;
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

        return new LectureDto(newLecture);
    }

    public async Task<LectureDto?> GetLectureById(int lectureId)
    {
        var lecture = await _unitOfWork.Lectures.GetById(lectureId);
        return lecture is null ? null : new LectureDto(lecture);
    }

    public async Task<IReadOnlyCollection<LectureDto>> GetLecturesByCourseId(int courseId)
    {
        var courseLectures = await _unitOfWork.Lectures.GetLecturesByCourseId(courseId);
        return courseLectures.Select(lecture => new LectureDto(lecture)).ToList();
    }

    public async Task<bool> UpdateLecture(int lectureId, UpdateLectureDto dto)
    {
        var lecture = await _unitOfWork.Lectures.GetById(lectureId);
        if (lecture is null)
        {
            return false;
        }

        await _validationHandler.ValidateAsync(dto);

        lecture.Name = dto.Name ?? lecture.Name;
        lecture.Content = dto.Content ?? lecture.Content;

        await _unitOfWork.SaveChanges();

        return true;
    }

    public async Task<bool> DeleteLecture(int lectureId)
    {
        var isSuccess = await _unitOfWork.Lectures.Delete(lectureId);
        if (isSuccess)
        {
            await _unitOfWork.SaveChanges();
        }

        return isSuccess;
    }
}