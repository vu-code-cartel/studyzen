using StudyZen.Application.Dtos;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using FluentValidation;
using StudyZen.Application.Validation;


namespace StudyZen.Application.Services;

public sealed class CourseService : ICourseService
{
    private readonly ICourseRepository _courses;
    private readonly ILectureRepository _lectures;
    private readonly ValidationHandler _validationHandler;

    public CourseService(ICourseRepository courses, ILectureRepository lectures, ValidationHandler validationHandler)
    {
        _courses = courses;
        _lectures = lectures;
        _validationHandler = validationHandler;
    }

    public CourseDto CreateCourse(CreateCourseDto dto)
    {
        _validationHandler.Validate(dto);
        var newCourse = new Course(dto.Name, dto.Description);
        _courses.Add(newCourse);
        return new CourseDto(newCourse);
    }

    public CourseDto? GetCourseById(int id)
    {
        var course = _courses.GetById(id);
        return course is null ? null : new CourseDto(course);
    }

    public bool UpdateCourse(int id, UpdateCourseDto dto)
    {
        var course = _courses.GetById(id);
        if (course is null)
        {
            return false;
        }

        _validationHandler.Validate(dto);
        course.Name = dto.Name ?? course.Name;
        course.Description = dto.Description ?? course.Description;
        _courses.Update(course);
        return true;
    }

    public bool DeleteCourse(int id)
    {
        DeleteLecturesByCourseId(id);
        return _courses.Delete(id);
    }

    public IReadOnlyCollection<CourseDto> GetAllCourses()
    {
        var allCourses = _courses.GetAll();
        return allCourses.Select(course => new CourseDto(course)).ToList();
    }

    private void DeleteLecturesByCourseId(int courseId)
    {
        var allLectures = _lectures.GetAll();
        var courseLectures = allLectures.Where(l => l.CourseId == courseId);

        foreach (var courseLecture in courseLectures)
        {
            _lectures.Delete(courseLecture.Id);
        }
    }
}