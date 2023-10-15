using StudyZen.Application.Dtos;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using StudyZen.Application.Validation;

namespace StudyZen.Application.Services;

public sealed class CourseService : ICourseService
{
    private readonly ICourseRepository _courses;
    private readonly ValidationHandler _validationHandler;

    public CourseService(ICourseRepository courses, ValidationHandler validationHandler)
    {
        _courses = courses;
        _validationHandler = validationHandler;
    }

    public async Task<CourseDto> CreateCourse(CreateCourseDto dto)
    {
        _validationHandler.Validate(dto);
        var newCourse = new Course(dto.Name, dto.Description);
        await _courses.Add(newCourse);
        return new CourseDto(newCourse);
    }

    public async Task<CourseDto?> GetCourseById(int id)
    {
        var course = await _courses.GetById(id);
        return course is null ? null : new CourseDto(course);
    }

    public async Task<bool> UpdateCourse(int id, UpdateCourseDto dto)
    {
        var course = await _courses.GetById(id);
        if (course is null)
        {
            return false;
        }

        _validationHandler.Validate(dto);
        course.Name = dto.Name ?? course.Name;
        course.Description = dto.Description ?? course.Description;
        return await _courses.Update(course);
    }

    public async Task<bool> DeleteCourse(int id)
    {
        return await _courses.Delete(id);
    }

    public async Task<IReadOnlyCollection<CourseDto>> GetAllCourses()
    {
        var allCourses = await _courses.GetAll();
        return allCourses.Select(course => new CourseDto(course)).ToList();
    }
}