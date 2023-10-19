using StudyZen.Application.Dtos;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using StudyZen.Application.Validation;

namespace StudyZen.Application.Services;

public sealed class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ValidationHandler _validationHandler;

    public CourseService(IUnitOfWork unitOfWork, ValidationHandler validationHandler)
    {
        _unitOfWork = unitOfWork;
        _validationHandler = validationHandler;
    }

    public async Task<CourseDto> CreateCourse(CreateCourseDto dto)
    {
        await _validationHandler.ValidateAsync(dto);

        var newCourse = new Course(dto.Name, dto.Description);

        _unitOfWork.Courses.Add(newCourse);
        await _unitOfWork.SaveChanges();

        return new CourseDto(newCourse);
    }

    public async Task<CourseDto?> GetCourseById(int id)
    {
        var course = await _unitOfWork.Courses.GetById(id);
        return course is null ? null : new CourseDto(course);
    }

    public async Task<bool> UpdateCourse(int id, UpdateCourseDto dto)
    {
        var course = await _unitOfWork.Courses.GetById(id);
        if (course is null)
        {
            return false;
        }

        await _validationHandler.ValidateAsync(dto);

        course.Name = dto.Name ?? course.Name;
        course.Description = dto.Description ?? course.Description;

        await _unitOfWork.SaveChanges();

        return true;
    }

    public async Task<bool> DeleteCourse(int id)
    {
        var isSuccess = await _unitOfWork.Courses.Delete(id);
        if (isSuccess)
        {
            await _unitOfWork.SaveChanges();
        }

        return isSuccess;
    }

    public async Task<IReadOnlyCollection<CourseDto>> GetAllCourses()
    {
        var allCourses = await _unitOfWork.Courses.Get();
        return allCourses.Select(course => new CourseDto(course)).ToList();
    }
}