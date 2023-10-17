using StudyZen.Application.Dtos;
using StudyZen.Application.Extensions;
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

        return newCourse.ToDto();
    }

    public async Task<CourseDto?> GetCourseById(int id)
    {
        var course = await _unitOfWork.Courses.GetById(id);
        return course?.ToDto();
    }

    public async Task<bool> UpdateCourse(int id, UpdateCourseDto dto)
    {
        await _validationHandler.ValidateAsync(dto);

        var course = await _unitOfWork.Courses.GetById(id);
        if (course is null)
        {
            return false;
        }

        course.Name = dto.Name ?? course.Name;
        course.Description = dto.Description ?? course.Description;

        _unitOfWork.Courses.Update(course);
        await _unitOfWork.SaveChanges();

        return true;
    }

    public async Task<bool> DeleteCourse(int id)
    {
        var course = await _unitOfWork.Courses.GetById(id);
        if (course is null)
        {
            return false;
        }

        _unitOfWork.Courses.Delete(course);
        await _unitOfWork.SaveChanges();

        return true;
    }

    public async Task<IReadOnlyCollection<CourseDto>> GetAllCourses()
    {
        var allCourses = await _unitOfWork.Courses.GetAll();
        return allCourses.ToDtos();
    }
}